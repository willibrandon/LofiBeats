using LofiBeats.Core.Effects;

namespace LofiBeats.Core.Playback;

public interface IAudioPlaybackService
{
    void StartPlayback();
    void StopPlayback();
    void AddEffect(IAudioEffect effect);
    void RemoveEffect(string effectName);
} 