using NAudio.Wave;

namespace LofiBeats.Core.PluginApi;

/// <summary>
/// Represents an audio effect that can process audio samples in real-time.
/// </summary>
public interface IAudioEffect : ISampleProvider
{
    /// <summary>
    /// Gets the name of the effect.
    /// </summary>
    string Name { get; }
    
    /// <summary>
    /// Sets or changes the source that this effect will process.
    /// </summary>
    /// <param name="source">The incoming sample provider to process.</param>
    void SetSource(ISampleProvider source);
    
    /// <summary>
    /// Process audio samples in the buffer.
    /// </summary>
    /// <param name="buffer">The buffer containing audio samples to process.</param>
    /// <param name="offset">The offset in the buffer to start processing from.</param>
    /// <param name="count">The number of samples to process.</param>
    void ApplyEffect(float[] buffer, int offset, int count);
} 