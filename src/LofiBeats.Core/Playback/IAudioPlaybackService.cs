using LofiBeats.Core.Effects;
using NAudio.Wave;

namespace LofiBeats.Core.Playback;

public interface IAudioPlaybackService
{
    ISampleProvider? CurrentSource { get; }
    void SetSource(ISampleProvider source);
    void StartPlayback();
    void StopPlayback();
    void PausePlayback();
    void ResumePlayback();
    PlaybackState GetPlaybackState();
    void AddEffect(IAudioEffect effect);
    void RemoveEffect(string effectName);
    void SetVolume(float volume);
} 