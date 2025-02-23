using Microsoft.Extensions.Logging;
using NAudio.Wave;

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
    private readonly Lock _lock = new();

    public WindowsAudioOutput(ILogger<WindowsAudioOutput> logger)
    {
        _logger = logger;
    }

    public PlaybackState PlaybackState 
    {
        get
        {
            lock (_lock)
            {
                return _waveOut?.PlaybackState ?? PlaybackState.Stopped;
            }
        }
    }

    public void Init(IWaveProvider waveProvider)
    {
        if (_isDisposed) return;

        lock (_lock)
        {
            try
            {
                // Cleanup any existing instance
                if (_waveOut != null)
                {
                    _waveOut.Stop();
                    _waveOut.Dispose();
                    _waveOut = null;
                }

                _waveOut = new WaveOutEvent();
                _waveOut.Init(waveProvider);
                _isInitialized = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to initialize audio output");
                _isInitialized = false;
                _waveOut?.Dispose();
                _waveOut = null;
            }
        }
    }

    public void Play()
    {
        if (_isDisposed) return;

        lock (_lock)
        {
            if (!_isInitialized || _waveOut == null) return;

            try
            {
                if (_waveOut.PlaybackState != PlaybackState.Playing)
                {
                    _waveOut.Play();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to start playback");
            }
        }
    }

    public void Pause()
    {
        if (_isDisposed) return;

        lock (_lock)
        {
            if (!_isInitialized || _waveOut == null) return;

            try
            {
                if (_waveOut.PlaybackState == PlaybackState.Playing)
                {
                    _waveOut.Pause();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to pause playback");
            }
        }
    }

    public void Stop()
    {
        if (_isDisposed) return;

        lock (_lock)
        {
            if (!_isInitialized || _waveOut == null) return;

            try
            {
                _waveOut.Stop();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to stop playback");
            }
        }
    }

    public void SetVolume(float volume)
    {
        if (_isDisposed) return;

        lock (_lock)
        {
            if (!_isInitialized || _waveOut == null) return;

            try
            {
                _waveOut.Volume = Math.Clamp(volume, 0.0f, 1.0f);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to set volume to {Volume}", volume);
            }
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (disposing)
            {
                lock (_lock)
                {
                    try
                    {
                        if (_waveOut != null)
                        {
                            _waveOut.Stop();
                            _waveOut.Dispose();
                            _waveOut = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error disposing WaveOutEvent");
                    }
                }
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