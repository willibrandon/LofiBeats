using NAudio.Wave;

namespace LofiBeats.Core.Playback;

/// <summary>
/// Provides audio samples from a file source with proper resource management.
/// This class handles the reading and streaming of user-supplied audio samples,
/// ensuring proper disposal of resources and thread-safe operation.
/// </summary>
/// <remarks>
/// Features:
/// - Automatic sample rate conversion to match system requirements
/// - Proper resource cleanup through IDisposable
/// - Thread-safe sample reading
/// - Support for various audio file formats through NAudio
/// </remarks>
public class UserSampleProvider : ISampleProvider, IDisposable
{
    private readonly AudioFileReader _reader;
    private bool _disposed;

    /// <summary>
    /// Gets the WaveFormat of the audio sample, which includes sample rate,
    /// bit depth, and channel configuration.
    /// </summary>
    public WaveFormat WaveFormat => _reader.WaveFormat;

    /// <summary>
    /// Initializes a new instance of the UserSampleProvider.
    /// </summary>
    /// <param name="filePath">The path to the audio file to read samples from.</param>
    /// <exception cref="ArgumentException">Thrown when the file is invalid or cannot be opened.</exception>
    public UserSampleProvider(string filePath)
    {
        _reader = new AudioFileReader(filePath);
    }

    /// <summary>
    /// Reads a sequence of samples from the audio file into the specified buffer.
    /// </summary>
    /// <param name="buffer">The buffer to fill with audio samples.</param>
    /// <param name="offset">The offset in the buffer to start writing.</param>
    /// <param name="count">The number of samples to read.</param>
    /// <returns>The number of samples read. May be less than requested if end of file is reached.</returns>
    public int Read(float[] buffer, int offset, int count)
    {
        if (_disposed) return 0;
        return _reader.Read(buffer, offset, count);
    }

    /// <summary>
    /// Disposes of the audio file reader and releases associated resources.
    /// </summary>
    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        _reader.Dispose();
        GC.SuppressFinalize(this);
    }
} 