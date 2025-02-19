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
    private bool _isInitialized;

    public WindowsAudioOutput(ILogger<WindowsAudioOutput> logger)
    {
        _logger = logger;
        _waveOut = new WaveOutEvent();
    }

    public PlaybackState PlaybackState => _waveOut?.PlaybackState ?? PlaybackState.Stopped;

    public void Init(IWaveProvider waveProvider)
    {
        if (_isDisposed) return;
        try
        {
            _waveOut?.Init(waveProvider);
            _isInitialized = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize audio output");
            _isInitialized = false;
        }
    }

    public void Play()
    {
        if (_isDisposed || !_isInitialized) return;
        try
        {
            _waveOut?.Play();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to start playback");
        }
    }

    public void Pause()
    {
        if (_isDisposed || !_isInitialized) return;
        try
        {
            _waveOut?.Pause();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to pause playback");
        }
    }

    public void Stop()
    {
        if (_isDisposed || !_isInitialized) return;
        try
        {
            _waveOut?.Stop();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to stop playback");
        }
    }

    public void SetVolume(float volume)
    {
        if (_isDisposed || !_isInitialized) return;
        try
        {
            if (_waveOut != null)
            {
                _waveOut.Volume = Math.Clamp(volume, 0.0f, 1.0f);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to set volume to {Volume}", volume);
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (disposing)
            {
                try
                {
                    _waveOut?.Dispose();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error disposing WaveOutEvent");
                }
                _waveOut = null;
            }
            _isDisposed = true;
            _isInitialized = false;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
} 