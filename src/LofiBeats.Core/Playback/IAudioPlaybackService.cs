using LofiBeats.Core.Effects;
using LofiBeats.Core.Models;
using NAudio.Wave;

namespace LofiBeats.Core.Playback;

public interface IAudioPlaybackService
{
    ISampleProvider? CurrentSource { get; }
    
    /// <summary>
    /// Gets or sets the current beat generation style.
    /// </summary>
    string CurrentStyle { get; set; }
    
    /// <summary>
    /// Gets the current volume level (range: 0.0 to 1.0).
    /// </summary>
    float CurrentVolume { get; }
    
    void SetSource(ISampleProvider source);
    void StartPlayback();
    void StopPlayback();
    void StopWithEffect(IAudioEffect effect);
    void PausePlayback();
    void ResumePlayback();
    PlaybackState GetPlaybackState();
    void AddEffect(IAudioEffect effect);
    void RemoveEffect(string effectName);
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
} 