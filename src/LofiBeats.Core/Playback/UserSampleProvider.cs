using NAudio.Wave;

namespace LofiBeats.Core.Playback;

/// <summary>
/// Provides audio samples from a file source.
/// </summary>
public class UserSampleProvider : ISampleProvider, IDisposable
{
    private readonly AudioFileReader _reader;
    private bool _disposed;

    public WaveFormat WaveFormat => _reader.WaveFormat;

    public UserSampleProvider(string filePath)
    {
        _reader = new AudioFileReader(filePath);
    }

    public int Read(float[] buffer, int offset, int count)
    {
        if (_disposed) return 0;
        return _reader.Read(buffer, offset, count);
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        _reader.Dispose();
        GC.SuppressFinalize(this);
    }
} 