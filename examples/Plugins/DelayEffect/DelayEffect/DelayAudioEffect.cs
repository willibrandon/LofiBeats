using LofiBeats.Core.PluginApi;
using NAudio.Wave;

namespace LofiBeats.Plugins.DelayEffect;

/// <summary>
/// A simple delay effect that creates an echo of the input audio.
/// </summary>
public class DelayAudioEffect : IAudioEffect
{
    private ISampleProvider? _source;
    private readonly float[] _delayBuffer;
    private int _writePosition;
    private readonly int _delaySamples;
    private readonly float _feedback;
    private readonly float _wetMix;
    private readonly float _dryMix;

    public string Author => "LofiBeats Team";

    public string Description => "A classic delay effect that creates echoes of the input audio";

    public string Name => "delay";

    public string Version => "1.0.0";

    public WaveFormat WaveFormat => _source?.WaveFormat ?? WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);

    /// <summary>
    /// Initializes a new instance of the DelayAudioEffect class.
    /// </summary>
    public DelayAudioEffect()
    {
        // Default to 500ms delay at 44.1kHz
        _delaySamples = (int)(0.5f * 44100);
        _delayBuffer = new float[_delaySamples];
        _feedback = 0.5f;
        _wetMix = 0.5f;
        _dryMix = 0.7f;
    }

    public void SetSource(ISampleProvider source)
    {
        _source = source;
        // Reset delay buffer when source changes
        Array.Clear(_delayBuffer, 0, _delayBuffer.Length);
        _writePosition = 0;
    }

    public int Read(float[] buffer, int offset, int count)
    {
        if (_source == null) return 0;

        // Read from source
        int samplesRead = _source.Read(buffer, offset, count);

        // Apply delay effect
        ApplyEffect(buffer, offset, samplesRead);

        return samplesRead;
    }

    public void ApplyEffect(float[] buffer, int offset, int count)
    {
        for (int i = 0; i < count; i++)
        {
            int bufferIndex = offset + i;
            float inputSample = buffer[bufferIndex];

            // Calculate read position based on write position
            int readPosition = (_writePosition - _delaySamples);
            if (readPosition < 0) readPosition += _delayBuffer.Length;

            // Read delayed sample
            float delaySample = _delayBuffer[readPosition];

            // Write to delay buffer (input + feedback)
            _delayBuffer[_writePosition] = inputSample + (delaySample * _feedback);
            _writePosition = (_writePosition + 1) % _delayBuffer.Length;

            // Mix dry and wet signals
            buffer[bufferIndex] = (inputSample * _dryMix) + (delaySample * _wetMix);
        }
    }
} 