using NAudio.Wave;

namespace LofiBeats.Core.Playback;

/// <summary>
/// Represents a platform-agnostic audio output device.
/// </summary>
public interface IAudioOutput : IDisposable
{
    /// <summary>
    /// Gets the current playback state.
    /// </summary>
    PlaybackState PlaybackState { get; }

    /// <summary>
    /// Initializes the audio output with the specified wave provider.
    /// </summary>
    void Init(IWaveProvider waveProvider);

    /// <summary>
    /// Starts or resumes audio playback.
    /// </summary>
    void Play();

    /// <summary>
    /// Pauses audio playback.
    /// </summary>
    void Pause();

    /// <summary>
    /// Stops audio playback.
    /// </summary>
    void Stop();

    /// <summary>
    /// Sets the volume level (0.0 to 1.0).
    /// </summary>
    void SetVolume(float volume);
} 