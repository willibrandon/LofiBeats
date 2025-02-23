using LofiBeats.Core.Effects;
using LofiBeats.Core.PluginManagement;
using NAudio.Wave;
using System.Reflection;

namespace LofiBeats.Plugins.BasicEffect;

/// <summary>
/// A simple gain effect that adjusts the volume of the audio.
/// This demonstrates the most basic form of an audio effect plugin.
/// </summary>
[PluginEffectName("gain", 
    Description = "Adjusts the volume of the audio",
    Version = "1.0.0",
    Author = "LofiBeats Team")]
public class GainEffect : IAudioEffect
{
    private ISampleProvider? _source;
    private readonly float _gainFactor = 1.5f; // 50% volume increase

    public string Name => GetType()
        .GetCustomAttribute<PluginEffectNameAttribute>()
        ?.Name ?? "gain";

    public WaveFormat WaveFormat => _source?.WaveFormat ?? 
        WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);

    public void SetSource(ISampleProvider source)
    {
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
