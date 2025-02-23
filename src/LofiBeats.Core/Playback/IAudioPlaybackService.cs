using LofiBeats.Core.Effects;
using LofiBeats.Core.Models;
using LofiBeats.Core.PluginApi;
using NAudio.Wave;

namespace LofiBeats.Core.Playback;

/// <summary>
/// Interface for the audio playback service.
/// </summary>
public interface IAudioPlaybackService
{
    /// <summary>
    /// Gets the current audio source.
    /// </summary>
    ISampleProvider? CurrentSource { get; }
    
    /// <summary>
    /// Gets or sets the current beat generation style.
    /// </summary>
    string CurrentStyle { get; set; }
    
    /// <summary>
    /// Gets the current volume level (range: 0.0 to 1.0).
    /// </summary>
    float CurrentVolume { get; }
    
    /// <summary>
    /// Sets the audio source for playback.
    /// </summary>
    /// <param name="source">The sample provider to use as the source.</param>
    void SetSource(ISampleProvider source);
    
    /// <summary>
    /// Starts playback of the current source.
    /// </summary>
    void StartPlayback();
    
    /// <summary>
    /// Stops the current playback.
    /// </summary>
    void StopPlayback();
    
    /// <summary>
    /// Stops playback with a specified effect.
    /// </summary>
    /// <param name="effect">The effect to apply during stop.</param>
    void StopWithEffect(IAudioEffect effect);
    
    /// <summary>
    /// Pauses the current playback.
    /// </summary>
    void PausePlayback();
    
    /// <summary>
    /// Resumes playback from a paused state.
    /// </summary>
    void ResumePlayback();
    
    /// <summary>
    /// Gets the current playback state.
    /// </summary>
    /// <returns>The current playback state.</returns>
    PlaybackState GetPlaybackState();
    
    /// <summary>
    /// Adds an audio effect to the playback chain.
    /// </summary>
    /// <param name="effect">The effect to add.</param>
    void AddEffect(IAudioEffect effect);
    
    /// <summary>
    /// Removes an audio effect from the playback chain.
    /// </summary>
    /// <param name="effectName">The name of the effect to remove.</param>
    void RemoveEffect(string effectName);
    
    /// <summary>
    /// Sets the playback volume.
    /// </summary>
    /// <param name="volume">The volume level (0.0 to 1.0).</param>
    void SetVolume(float volume);
    
    /// <summary>
    /// Gets a preset object representing the current playback state.
    /// </summary>
    /// <returns>A preset containing the current style, volume, and effects.</returns>
    Preset GetCurrentPreset();
    
    /// <summary>
    /// Applies a preset to the current playback state.
    /// </summary>
    /// <param name="preset">The preset to apply.</param>
    /// <param name="effectFactory">Factory for creating effects from names.</param>
    /// <exception cref="ArgumentNullException">Thrown when preset or effectFactory is null.</exception>
    void ApplyPreset(Preset preset, IEffectFactory effectFactory);

    /// <summary>
    /// Crossfades from the current pattern to a new pattern over the specified duration.
    /// </summary>
    /// <param name="newPattern">The new beat pattern to fade to.</param>
    /// <param name="crossfadeDuration">Duration of the crossfade in seconds.</param>
    void CrossfadeToPattern(BeatPattern newPattern, float crossfadeDuration);
} 