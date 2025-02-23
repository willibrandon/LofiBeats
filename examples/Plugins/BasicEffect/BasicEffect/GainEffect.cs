using LofiBeats.Core.PluginApi;
using NAudio.Wave;

namespace LofiBeats.Plugins.BasicEffect;

/// <summary>
/// A simple gain effect that adjusts the volume of the audio.
/// This demonstrates the most basic form of an audio effect plugin.
/// </summary>
public class GainEffect : IAudioEffect
{
    private ISampleProvider? _source;
    private readonly float _gainFactor = 1.5f; // 50% volume increase

    public string Author => "LofiBeats Team";

    public string Description => "A classic delay effect that creates echoes of the input audio";

    public string Name => "gain";

    public string Version => "1.0.0";

    public WaveFormat WaveFormat => _source?.WaveFormat ?? 
        WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);

    public void SetSource(ISampleProvider source)
    {
        ArgumentNullException.ThrowIfNull(source);
        _source = source;
    }

    public int Read(float[] buffer, int offset, int count)
    {
        if (_source == null) return 0;

        // Read from source
        int samplesRead = _source.Read(buffer, offset, count);

        // Apply gain effect
        ApplyEffect(buffer, offset, samplesRead);

        return samplesRead;
    }

    public void ApplyEffect(float[] buffer, int offset, int count)
    {
        ArgumentNullException.ThrowIfNull(buffer);
        ArgumentOutOfRangeException.ThrowIfNegative(offset);
        ArgumentOutOfRangeException.ThrowIfNegative(count);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(offset + count, buffer.Length);

        // Simple gain adjustment with clipping prevention
        for (int i = 0; i < count; i++)
        {
            int bufferIndex = offset + i;
            float sample = buffer[bufferIndex] * _gainFactor;
            
            // Prevent clipping
            buffer[bufferIndex] = Math.Clamp(sample, -1.0f, 1.0f);
        }
    }
}
