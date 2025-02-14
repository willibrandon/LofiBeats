using LofiBeats.Core.Effects;
using NAudio.Wave;

namespace LofiBeats.Core.Playback;

public interface IAudioPlaybackService
{
    ISampleProvider? CurrentSource { get; }
    void SetSource(ISampleProvider source);
    void StartPlayback();
    void StopPlayback();
    void AddEffect(IAudioEffect effect);
    void RemoveEffect(string effectName);
} 