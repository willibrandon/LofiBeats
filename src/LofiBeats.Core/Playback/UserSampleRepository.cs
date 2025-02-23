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
    private readonly string _samplesDirectory;
    private bool _disposed;

    private static readonly Action<ILogger, int, string, Exception?> _logSamplesLoaded =
        LoggerMessage.Define<int, string>(LogLevel.Debug, new EventId(0, "SamplesLoaded"),
            "Loaded {Count} samples from {Directory}");

    private static readonly Action<ILogger, string, Exception> _logLoadError =
        LoggerMessage.Define<string>(LogLevel.Error, new EventId(1, "LoadError"),
            "Failed to load existing samples from {Directory}");

    private static readonly Action<ILogger, string, string?, string, Exception?> _logSampleRegistered =
        LoggerMessage.Define<string, string?, string>(LogLevel.Debug, new EventId(2, "SampleRegistered"),
            "Successfully registered sample {Name} (velocity: {Velocity}) from {FilePath}");

    private static readonly Action<ILogger, string, string, Exception> _logRegistrationError =
        LoggerMessage.Define<string, string>(LogLevel.Error, new EventId(3, "RegistrationError"),
            "Failed to register sample {Name} from {FilePath}");

    private static readonly Action<ILogger, string, string?, Exception?> _logSampleOverwrite =
        LoggerMessage.Define<string, string?>(LogLevel.Warning, new EventId(4, "SampleOverwrite"),
            "Sample with name {Name} and velocity {Velocity} already exists and will be overwritten");

    private static readonly Action<ILogger, string, Exception?> _logSampleUnregistered =
        LoggerMessage.Define<string>(LogLevel.Debug, new EventId(5, "SampleUnregistered"),
            "Successfully unregistered sample {Name}");

    private static readonly Action<ILogger, string, Exception?> _logSampleNotFound =
        LoggerMessage.Define<string>(LogLevel.Warning, new EventId(6, "SampleNotFound"),
            "Sample {Name} not found in repository");

    private static readonly Action<ILogger, string, Exception> _logDeleteError =
        LoggerMessage.Define<string>(LogLevel.Error, new EventId(7, "DeleteError"),
            "Failed to delete sample file for {Name}");

    public UserSampleRepository(ILogger<UserSampleRepository> logger, bool preloadSamples = false)
    {
        _logger = logger;
        _preloadSamples = preloadSamples;
        
        // Create samples directory in user's app data
        _samplesDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "LofiBeats",
            "Samples"
        );
        Directory.CreateDirectory(_samplesDirectory);
        LoadExistingSamples();
    }

    private void LoadExistingSamples()
    {
        try
        {
            foreach (var file in Directory.GetFiles(_samplesDirectory, "*.wav"))
            {
                var name = Path.GetFileNameWithoutExtension(file);
                var parts = name.Split('_');
                if (parts.Length > 1 && int.TryParse(parts[1], out var velocity))
                {
                    // Sample with velocity layer
                    _samples[GetSampleKey(parts[0], velocity)] = new UserSampleInfo(file, GetWaveFormat(file));
                }
                else
                {
                    // Sample without velocity layer
                    _samples[name] = new UserSampleInfo(file, GetWaveFormat(file));
                }
            }
            _logSamplesLoaded(_logger, _samples.Count, _samplesDirectory, null);
        }
        catch (Exception ex)
        {
            _logLoadError(_logger, _samplesDirectory, ex);
        }
    }

    private static WaveFormat GetWaveFormat(string filePath)
    {
        using var reader = new WaveFileReader(filePath);
        return reader.WaveFormat;
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
        ObjectDisposedException.ThrowIf(_disposed, this);
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Sample name cannot be empty", nameof(name));
        if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("File path cannot be empty", nameof(filePath));
        if (!File.Exists(filePath)) throw new ArgumentException("File does not exist", nameof(filePath));
        if (velocityLayer.HasValue && (velocityLayer.Value < 0 || velocityLayer.Value > 127))
            throw new ArgumentException("Velocity layer must be between 0 and 127", nameof(velocityLayer));
        if (!filePath.EndsWith(".wav", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("Only WAV files are supported", nameof(filePath));

        try
        {
            // Validate the audio file and get its format
            using var reader = new WaveFileReader(filePath);
            var format = reader.WaveFormat;

            // Create destination path
            string destinationFileName = velocityLayer.HasValue ? $"{name}_{velocityLayer}.wav" : $"{name}.wav";
            string destinationPath = Path.Combine(_samplesDirectory, destinationFileName);

            // Copy file to samples directory
            File.Copy(filePath, destinationPath, true);

            string key = GetSampleKey(name, velocityLayer);
            var sampleInfo = new UserSampleInfo(destinationPath, format);

            if (_preloadSamples)
            {
                PreloadSample(key, destinationPath);
            }

            if (!_samples.TryAdd(key, sampleInfo))
            {
                _logSampleOverwrite(_logger, name, velocityLayer?.ToString() ?? "default", null);
                _samples[key] = sampleInfo;
            }

            _logSampleRegistered(_logger, name, velocityLayer?.ToString() ?? "default", filePath, null);
        }
        catch (Exception ex)
        {
            _logRegistrationError(_logger, name, filePath, ex);
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
        ObjectDisposedException.ThrowIf(_disposed, this);

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

    /// <summary>
    /// Unregisters a sample and removes its associated files.
    /// </summary>
    /// <param name="name">The name of the sample to unregister.</param>
    /// <returns>True if any samples were unregistered, false otherwise.</returns>
    public bool UnregisterSample(string name)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Sample name cannot be empty", nameof(name));

        var unregisteredAny = false;
        var keysToRemove = _samples.Keys.Where(k => k == name || k.StartsWith($"{name}:")).ToList();

        foreach (var key in keysToRemove)
        {
            if (_samples.TryRemove(key, out var sampleInfo))
            {
                try
                {
                    if (File.Exists(sampleInfo.FilePath))
                    {
                        File.Delete(sampleInfo.FilePath);
                    }
                    _preloadedData.TryRemove(key, out _);
                    unregisteredAny = true;
                }
                catch (Exception ex)
                {
                    _logDeleteError(_logger, key, ex);
                }
            }
        }

        if (unregisteredAny)
        {
            _logSampleUnregistered(_logger, name, null);
        }
        else
        {
            _logSampleNotFound(_logger, name, null);
        }

        return unregisteredAny;
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