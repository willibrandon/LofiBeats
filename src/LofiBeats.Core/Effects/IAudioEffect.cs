using NAudio.Wave;

namespace LofiBeats.Core.Effects;

public interface IAudioEffect : ISampleProvider
{
    string Name { get; }
    
    /// <summary>
    /// Sets or changes the source that this effect will process.
    /// </summary>
    /// <param name="source">The incoming sample provider to process.</param>
    void SetSource(ISampleProvider source);
    
    /// <summary>
    /// Process audio samples in the buffer.
    /// </summary>
    void ApplyEffect(float[] buffer, int offset, int count);
} 