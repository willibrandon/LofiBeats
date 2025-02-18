using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using NAudio.Wave;

namespace LofiBeats.Core.Playback;

/// <summary>
/// Manages user-supplied audio samples with thread-safe operations and proper resource management.
/// </summary>
public class UserSampleRepository : IDisposable
{
    private readonly ILogger<UserSampleRepository> _logger;
    private readonly ConcurrentDictionary<string, UserSampleInfo> _samples = new();
    private readonly ConcurrentDictionary<string, byte[]> _preloadedData = new();
    private readonly bool _preloadSamples;
    private bool _disposed;

    public UserSampleRepository(ILogger<UserSampleRepository> logger, bool preloadSamples = false)
    {
        _logger = logger;
        _preloadSamples = preloadSamples;
    }

    /// <summary>
    /// Registers a new sample with the repository.
    /// </summary>
    /// <param name="name">The unique name to identify the sample.</param>
    /// <param name="filePath">The full path to the audio file.</param>
    /// <param name="velocityLayer">Optional velocity layer (0-127). If not specified, registers as the default layer.</param>
    /// <exception cref="ArgumentException">Thrown when the file is invalid or unsupported.</exception>
    public void RegisterSample(string name, string filePath, int? velocityLayer = null)
    {
        if (_disposed) throw new ObjectDisposedException(nameof(UserSampleRepository));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Sample name cannot be empty", nameof(name));
        if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("File path cannot be empty", nameof(filePath));
        if (!File.Exists(filePath)) throw new ArgumentException("File does not exist", nameof(filePath));
        if (velocityLayer.HasValue && (velocityLayer.Value < 0 || velocityLayer.Value > 127))
            throw new ArgumentException("Velocity layer must be between 0 and 127", nameof(velocityLayer));

        try
        {
            // Validate the audio file and get its format
            using var reader = new AudioFileReader(filePath);
            var sampleInfo = new UserSampleInfo(filePath, reader.WaveFormat);

            string key = GetSampleKey(name, velocityLayer);
            if (_preloadSamples)
            {
                PreloadSample(key, filePath);
            }

            if (!_samples.TryAdd(key, sampleInfo))
            {
                _logger.LogWarning("Sample with name {Name} and velocity {Velocity} already exists and will be overwritten", 
                    name, velocityLayer?.ToString() ?? "default");
                _samples[key] = sampleInfo;
            }

            _logger.LogInformation("Successfully registered sample {Name} (velocity: {Velocity}) from {FilePath}", 
                name, velocityLayer?.ToString() ?? "default", filePath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to register sample {Name} from {FilePath}", name, filePath);
            throw new ArgumentException("Failed to register sample. The file may be invalid or unsupported.", nameof(filePath), ex);
        }
    }

    /// <summary>
    /// Creates a new sample provider for the specified sample, selecting the appropriate velocity layer.
    /// </summary>
    /// <param name="name">The name of the registered sample.</param>
    /// <param name="velocity">The velocity value (0-1) to use for layer selection.</param>
    /// <returns>An ISampleProvider for the requested sample.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the sample is not found.</exception>
    public ISampleProvider CreateSampleProvider(string name, float velocity = 1.0f)
    {
        if (_disposed) throw new ObjectDisposedException(nameof(UserSampleRepository));

        // Convert velocity (0-1) to MIDI velocity (0-127)
        int midiVelocity = (int)(velocity * 127);
        
        // Find the closest matching velocity layer
        var matchingLayers = _samples.Keys
            .Where(k => k.StartsWith($"{name}:"))
            .Select(k => (Key: k, Layer: GetVelocityLayer(k)))
            .OrderBy(x => x.Layer.HasValue ? Math.Abs(x.Layer.Value - midiVelocity) : 0)
            .ToList();

        string key = matchingLayers.FirstOrDefault().Key ?? name;
        if (!_samples.TryGetValue(key, out var sampleInfo))
        {
            throw new KeyNotFoundException($"Sample '{name}' not found in repository");
        }

        if (_preloadedData.TryGetValue(key, out var data))
        {
            return new PreloadedSampleProvider(data, sampleInfo.WaveFormat);
        }

        return new UserSampleProvider(sampleInfo.FilePath);
    }

    /// <summary>
    /// Checks if a sample with the specified name exists in the repository.
    /// </summary>
    public bool HasSample(string name) => _samples.Keys.Any(k => k.StartsWith($"{name}:") || k == name);

    private void PreloadSample(string key, string filePath)
    {
        using var reader = new AudioFileReader(filePath);
        using var memoryStream = new MemoryStream();
        reader.CopyTo(memoryStream);
        _preloadedData[key] = memoryStream.ToArray();
    }

    private static string GetSampleKey(string name, int? velocityLayer)
        => velocityLayer.HasValue ? $"{name}:{velocityLayer.Value}" : name;

    private static int? GetVelocityLayer(string key)
    {
        var parts = key.Split(':');
        return parts.Length > 1 && int.TryParse(parts[1], out var layer) ? layer : null;
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        _samples.Clear();
        _preloadedData.Clear();
        
        GC.SuppressFinalize(this);
    }

    private sealed record UserSampleInfo(string FilePath, WaveFormat WaveFormat);
} 