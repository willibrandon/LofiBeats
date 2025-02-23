using NAudio.Wave;

namespace LofiBeats.Core.Playback;

public class AutoDisposingSampleProvider(ISampleProvider provider) : ISampleProvider
{
    private readonly ISampleProvider _provider = provider;
    private bool _isDisposed;

    public WaveFormat WaveFormat { get; } = provider.WaveFormat;

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