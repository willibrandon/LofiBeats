using LofiBeats.Core.Effects;
using NAudio.Wave;

namespace LofiBeats.Core.Playback;

public interface IAudioPlaybackService
{
    ISampleProvider? CurrentSource { get; }
    
    /// <summary>
    /// Gets or sets the current beat generation style.
    /// </summary>
    string CurrentStyle { get; set; }
    
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
} 