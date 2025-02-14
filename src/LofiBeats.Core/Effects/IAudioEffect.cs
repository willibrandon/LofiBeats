using NAudio.Wave;

namespace LofiBeats.Core.Effects;

public interface IAudioEffect : ISampleProvider
{
    string Name { get; }
    void ApplyEffect(float[] buffer, int offset, int count);
} 