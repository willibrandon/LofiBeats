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
    /// <exception cref="ArgumentException">Thrown when the file is invalid or unsupported.</exception>
    public void RegisterSample(string name, string filePath)
    {
        if (_disposed) throw new ObjectDisposedException(nameof(UserSampleRepository));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Sample name cannot be empty", nameof(name));
        if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("File path cannot be empty", nameof(filePath));
        if (!File.Exists(filePath)) throw new ArgumentException("File does not exist", nameof(filePath));

        try
        {
            // Validate the audio file and get its format
            using var reader = new AudioFileReader(filePath);
            var sampleInfo = new UserSampleInfo(filePath, reader.WaveFormat);

            if (_preloadSamples)
            {
                PreloadSample(name, filePath);
            }

            if (!_samples.TryAdd(name, sampleInfo))
            {
                _logger.LogWarning("Sample with name {Name} already exists and will be overwritten", name);
                _samples[name] = sampleInfo;
            }

            _logger.LogInformation("Successfully registered sample {Name} from {FilePath}", name, filePath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to register sample {Name} from {FilePath}", name, filePath);
            throw new ArgumentException("Failed to register sample. The file may be invalid or unsupported.", nameof(filePath), ex);
        }
    }

    /// <summary>
    /// Creates a new sample provider for the specified sample.
    /// </summary>
    /// <param name="name">The name of the registered sample.</param>
    /// <returns>An ISampleProvider for the requested sample.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the sample is not found.</exception>
    public ISampleProvider CreateSampleProvider(string name)
    {
        if (_disposed) throw new ObjectDisposedException(nameof(UserSampleRepository));
        if (!_samples.TryGetValue(name, out var sampleInfo))
        {
            throw new KeyNotFoundException($"Sample '{name}' not found in repository");
        }

        if (_preloadedData.TryGetValue(name, out var data))
        {
            return new PreloadedSampleProvider(data, sampleInfo.WaveFormat);
        }

        return new UserSampleProvider(sampleInfo.FilePath);
    }

    /// <summary>
    /// Checks if a sample with the specified name exists in the repository.
    /// </summary>
    public bool HasSample(string name) => _samples.ContainsKey(name);

    private void PreloadSample(string name, string filePath)
    {
        using var reader = new AudioFileReader(filePath);
        using var memoryStream = new MemoryStream();
        reader.CopyTo(memoryStream);
        _preloadedData[name] = memoryStream.ToArray();
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