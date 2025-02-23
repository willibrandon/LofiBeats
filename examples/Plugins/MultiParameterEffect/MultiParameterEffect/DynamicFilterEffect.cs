using LofiBeats.Core.PluginApi;
using NAudio.Wave;

namespace LofiBeats.Plugins.MultiParameterEffect;

/// <summary>
/// A dynamic filter effect that combines multiple audio processing parameters.
/// This demonstrates a more complex audio effect plugin with multiple adjustable parameters.
/// </summary>
public class DynamicFilterEffect : IAudioEffect
{
    private ISampleProvider? _source;
    private readonly float[] _filterBuffer;
    private float _cutoffFrequency;
    private float _resonance;
    private float _modulationRate;
    private float _modulationDepth;
    private float _phase;
    private readonly float _sampleRate;

    // Filter state variables
    private float _lastInput;
    private float _lastOutput;

    public string Author => "LofiBeats Team";

    public string Description => "A classic delay effect that creates echoes of the input audio";

    public string Name => "dynamicfilter";

    public string Version => "1.0.0";

    public WaveFormat WaveFormat => _source?.WaveFormat ?? 
        WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);

    public DynamicFilterEffect()
    {
        _sampleRate = 44100f;
        _filterBuffer = new float[4096];
        
        // Default parameter values
        _cutoffFrequency = 1000f;    // Initial cutoff frequency in Hz
        _resonance = 0.7f;           // Resonance factor (0.0 to 1.0)
        _modulationRate = 0.5f;      // LFO rate in Hz
        _modulationDepth = 0.3f;     // Modulation amount (0.0 to 1.0)
        _phase = 0f;
    }

    public void SetSource(ISampleProvider source)
    {
        ArgumentNullException.ThrowIfNull(source);
        _source = source;
        Array.Clear(_filterBuffer, 0, _filterBuffer.Length);
        _lastInput = 0f;
        _lastOutput = 0f;
    }

    public int Read(float[] buffer, int offset, int count)
    {
        if (_source == null) return 0;

        // Read from source
        int samplesRead = _source.Read(buffer, offset, count);

        // Apply filter effect
        ApplyEffect(buffer, offset, samplesRead);

        return samplesRead;
    }

    public void ApplyEffect(float[] buffer, int offset, int count)
    {
        ArgumentNullException.ThrowIfNull(buffer);
        ArgumentOutOfRangeException.ThrowIfNegative(offset);
        ArgumentOutOfRangeException.ThrowIfNegative(count);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(offset + count, buffer.Length);

        for (int i = 0; i < count; i++)
        {
            int bufferIndex = offset + i;
            float input = buffer[bufferIndex];

            // Calculate modulated cutoff frequency
            float lfoValue = MathF.Sin(_phase);
            float modulatedCutoff = _cutoffFrequency * (1f + lfoValue * _modulationDepth);
            modulatedCutoff = Math.Clamp(modulatedCutoff, 20f, 20000f);
            
            // Update LFO phase
            _phase += 2f * MathF.PI * _modulationRate / _sampleRate;
            if (_phase >= 2f * MathF.PI) _phase -= 2f * MathF.PI;

            // Calculate filter coefficients
            float omega = 2f * MathF.PI * modulatedCutoff / _sampleRate;
            float alpha = MathF.Sin(omega) / (2f * _resonance);
            
            float a0 = 1f + alpha;
            float a1 = -2f * MathF.Cos(omega);
            float a2 = 1f - alpha;
            float b0 = (1f - MathF.Cos(omega)) / 2f;
            float b1 = 1f - MathF.Cos(omega);
            float b2 = (1f - MathF.Cos(omega)) / 2f;

            // Apply filter with output clamping
            float output = (b0 * input + b1 * _lastInput + b2 * _lastInput
                          - a1 * _lastOutput - a2 * _lastOutput) / a0;

            // Clamp output to prevent infinity/NaN
            output = Math.Clamp(output, -1f, 1f);

            // Update state variables
            _lastInput = input;
            _lastOutput = output;

            // Write filtered sample back to buffer
            buffer[bufferIndex] = output;
        }
    }

    // Parameter adjustment methods
    public void SetCutoffFrequency(float frequency)
    {
        _cutoffFrequency = Math.Clamp(frequency, 20f, 20000f);
    }

    public void SetResonance(float resonance)
    {
        _resonance = Math.Clamp(resonance, 0.1f, 1.0f);
    }

    public void SetModulationRate(float rate)
    {
        _modulationRate = Math.Clamp(rate, 0.1f, 10f);
    }

    public void SetModulationDepth(float depth)
    {
        _modulationDepth = Math.Clamp(depth, 0f, 1f);
    }
}
