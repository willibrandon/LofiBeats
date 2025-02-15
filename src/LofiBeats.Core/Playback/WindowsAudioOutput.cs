using NAudio.Wave;
using Microsoft.Extensions.Logging;

namespace LofiBeats.Core.Playback;

/// <summary>
/// Windows-specific implementation of audio output using NAudio's WaveOutEvent.
/// </summary>
public class WindowsAudioOutput : IAudioOutput
{
    private readonly ILogger<WindowsAudioOutput> _logger;
    private WaveOutEvent? _waveOut;
    private bool _isDisposed;

    public WindowsAudioOutput(ILogger<WindowsAudioOutput> logger)
    {
        _logger = logger;
        _waveOut = new WaveOutEvent();
    }

    public PlaybackState PlaybackState => _waveOut?.PlaybackState ?? PlaybackState.Stopped;

    public void Init(IWaveProvider waveProvider)
    {
        _waveOut?.Init(waveProvider);
    }

    public void Play()
    {
        _waveOut?.Play();
    }

    public void Pause()
    {
        _waveOut?.Pause();
    }

    public void Stop()
    {
        _waveOut?.Stop();
    }

    public void SetVolume(float volume)
    {
        if (_waveOut != null)
        {
            _waveOut.Volume = Math.Clamp(volume, 0.0f, 1.0f);
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (disposing)
            {
                _waveOut?.Dispose();
                _waveOut = null;
            }
            _isDisposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
} 