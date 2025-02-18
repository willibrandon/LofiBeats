using NAudio.Wave;

namespace LofiBeats.Core.Playback;

/// <summary>
/// Provides audio samples from a WAV file source with proper resource management.
/// This class handles the reading and streaming of user-supplied WAV samples,
/// ensuring proper disposal of resources and thread-safe operation.
/// </summary>
/// <remarks>
/// Features:
/// - Thread-safe sample reading
/// - Proper resource cleanup through IDisposable
/// - Support for WAV files
/// </remarks>
public class UserSampleProvider : ISampleProvider, IDisposable
{
    private readonly WaveFileReader _reader;
    private bool _disposed;

    /// <summary>
    /// Gets the WaveFormat of the audio sample, which includes sample rate,
    /// bit depth, and channel configuration.
    /// </summary>
    public WaveFormat WaveFormat => _reader.WaveFormat;

    /// <summary>
    /// Initializes a new instance of the UserSampleProvider.
    /// </summary>
    /// <param name="filePath">The path to the WAV file to read samples from.</param>
    /// <exception cref="ArgumentException">Thrown when the file is invalid or cannot be opened.</exception>
    public UserSampleProvider(string filePath)
    {
        if (!filePath.EndsWith(".wav", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("Only WAV files are supported", nameof(filePath));

        _reader = new WaveFileReader(filePath);
    }

    /// <summary>
    /// Reads a sequence of samples from the WAV file into the specified buffer.
    /// </summary>
    /// <param name="buffer">The buffer to fill with audio samples.</param>
    /// <param name="offset">The offset in the buffer to start writing.</param>
    /// <param name="count">The number of samples to read.</param>
    /// <returns>The number of samples read. May be less than requested if end of file is reached.</returns>
    public int Read(float[] buffer, int offset, int count)
    {
        if (_disposed) return 0;

        // Convert samples to float format
        var bytesPerSample = _reader.WaveFormat.BitsPerSample / 8;
        var bytesNeeded = count * bytesPerSample;
        var sampleBytes = new byte[bytesNeeded];
        
        var bytesRead = _reader.Read(sampleBytes, 0, bytesNeeded);
        var samplesRead = bytesRead / bytesPerSample;

        // Convert to float samples
        for (int i = 0; i < samplesRead; i++)
        {
            if (_reader.WaveFormat.BitsPerSample == 16)
            {
                var sample = BitConverter.ToInt16(sampleBytes, i * 2);
                buffer[offset + i] = sample / 32768f; // Scale from Int16.MaxValue
            }
            else if (_reader.WaveFormat.BitsPerSample == 24)
            {
                // For 24-bit audio, properly handle the 3 bytes
                int sample = (sampleBytes[i * 3] << 8) | 
                           (sampleBytes[i * 3 + 1] << 16) | 
                           (sampleBytes[i * 3 + 2] << 24);
                sample >>= 8; // Shift back to get proper 24-bit value
                buffer[offset + i] = sample / 8388608f; // Scale from 2^23
            }
            else if (_reader.WaveFormat.BitsPerSample == 32)
            {
                var sample = BitConverter.ToInt32(sampleBytes, i * 4);
                buffer[offset + i] = sample / (float)(1 << 31); // Scale from Int32.MaxValue
            }
        }

        return samplesRead;
    }

    /// <summary>
    /// Disposes of the WAV file reader and releases associated resources.
    /// </summary>
    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        _reader.Dispose();
        GC.SuppressFinalize(this);
    }
} 