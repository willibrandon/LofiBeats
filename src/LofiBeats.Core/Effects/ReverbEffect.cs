using NAudio.Wave;
using Microsoft.Extensions.Logging;

namespace LofiBeats.Core.Effects;

public class ReverbEffect : IAudioEffect
{
    private ISampleProvider _source;
    private readonly ILogger<ReverbEffect> _logger;
    private float[] _delayBuffer;
    private int _writePos;
    private readonly int _delaySamples;
    private float _feedback;
    private float _mix;

    public string Name => "reverb";
    public WaveFormat WaveFormat => _source.WaveFormat;

    public ReverbEffect(
        ISampleProvider source,
        ILogger<ReverbEffect> logger,
        float delayMs = 250f,
        float feedback = 0.3f,
        float mix = 0.5f)
    {
        _source = source ?? throw new ArgumentNullException(nameof(source));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _feedback = feedback;
        _mix = mix;

        int sampleRate = source.WaveFormat.SampleRate;
        _delaySamples = (int)(delayMs / 1000f * sampleRate) * source.WaveFormat.Channels;
        _delayBuffer = new float[_delaySamples];

        _logger.LogInformation("ReverbEffect initialized with delay: {DelayMs}ms, feedback: {Feedback}, mix: {Mix}", 
            delayMs, feedback, mix);
    }

    public void SetSource(ISampleProvider source)
    {
        _source = source ?? throw new ArgumentNullException(nameof(source));
        _logger.LogInformation("ReverbEffect source updated");
    }

    public int Read(float[] buffer, int offset, int count)
    {
        int samplesRead = _source.Read(buffer, offset, count);
        ApplyEffect(buffer, offset, samplesRead);
        return samplesRead;
    }

    public void ApplyEffect(float[] buffer, int offset, int count)
    {
        for (int i = 0; i < count; i++)
        {
            float inputSample = buffer[offset + i];
            float delayedSample = _delayBuffer[_writePos];

            // Mixed output
            float outputSample = (inputSample * (1 - _mix)) + (delayedSample * _mix);
            buffer[offset + i] = outputSample;

            // Write to the delay buffer
            _delayBuffer[_writePos] = inputSample + delayedSample * _feedback;

            // Increment write position
            _writePos++;
            if (_writePos >= _delayBuffer.Length)
                _writePos = 0;
        }

        _logger.LogTrace("Applied reverb effect to {SampleCount} samples", count);
    }
} 