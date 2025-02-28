using NAudio.Wave;
using Microsoft.Extensions.Logging;
using LofiBeats.Core.PluginApi;

namespace LofiBeats.Core.Effects;

public class LowPassFilterEffect : IAudioEffect
{
    private ISampleProvider _source;
    private readonly ILogger<LowPassFilterEffect> _logger;
    private readonly float _cutoffFrequency;
    private readonly float _resonance;
    private float _prevSample;
    private readonly float _alpha;

    public string Author => "LofiBeats Team";

    public string Description => "Reduces high frequencies for that warm, mellow sound";

    public string Name => "lowpass";

    public string Version => "1.0.0";

    public WaveFormat WaveFormat => _source.WaveFormat;

    public LowPassFilterEffect(
        ISampleProvider source,
        ILogger<LowPassFilterEffect> logger,
        float cutoffFrequency = 2000f,
        float resonance = 1.0f)
    {
        _source = source ?? throw new ArgumentNullException(nameof(source));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _cutoffFrequency = cutoffFrequency;
        _resonance = resonance;

        // Calculate filter coefficient
        float dt = 1f / WaveFormat.SampleRate;
        float rc = 1f / (2 * MathF.PI * cutoffFrequency);
        _alpha = dt / (rc + dt);

        _logger.LogInformation("LowPassFilterEffect initialized with cutoff frequency {Frequency}Hz", cutoffFrequency);
    }

    public void SetSource(ISampleProvider source)
    {
        _source = source ?? throw new ArgumentNullException(nameof(source));
        _logger.LogInformation("LowPassFilterEffect source updated");
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
            float current = buffer[offset + i];
            float filtered = _prevSample + _alpha * (current - _prevSample);
            buffer[offset + i] = filtered;
            _prevSample = filtered;
        }

        _logger.LogTrace("Applied low-pass filter to {SampleCount} samples", count);
    }
} 