using NAudio.Wave;

namespace LofiBeats.Core.Playback;

public class AutoDisposingSampleProvider : ISampleProvider
{
    private readonly ISampleProvider _provider;
    private bool _isDisposed;

    public AutoDisposingSampleProvider(ISampleProvider provider)
    {
        _provider = provider;
        WaveFormat = provider.WaveFormat;
    }

    public WaveFormat WaveFormat { get; }

    public int Read(float[] buffer, int offset, int count)
    {
        if (_isDisposed)
            return 0;

        int read = _provider.Read(buffer, offset, count);
        if (read == 0)
        {
            if (_provider is IDisposable disposable)
            {
                disposable.Dispose();
            }
            _isDisposed = true;
        }
        return read;
    }
} 