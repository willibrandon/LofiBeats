using NAudio.Wave;
using Microsoft.Extensions.Logging;
using LofiBeats.Core.PluginApi;

namespace LofiBeats.Core.Effects;

public class VinylCrackleEffect : IAudioEffect
{
    private ISampleProvider _source;
    private readonly Random _rand = new();
    private readonly ILogger<VinylCrackleEffect> _logger;

    public string Author => "LofiBeats Team";

    public string Description => "Adds vinyl record crackle and noise for that authentic feel";

    public string Name => "vinyl";

    public string Version => "1.0.0";

    public WaveFormat WaveFormat => _source.WaveFormat;

    public VinylCrackleEffect(ISampleProvider source, ILogger<VinylCrackleEffect> logger)
    {
        _source = source ?? throw new ArgumentNullException(nameof(source));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _logger.LogInformation("VinylCrackleEffect initialized");
    }

    public void SetSource(ISampleProvider source)
    {
        _source = source ?? throw new ArgumentNullException(nameof(source));
        _logger.LogInformation("VinylCrackleEffect source updated");
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
            if (_rand.NextDouble() < 0.0005) // Adjust this value to control crackle frequency
            {
                float crackle = (float)(_rand.NextDouble() * 2.0 - 1.0) * 0.2f; // Adjust amplitude
                buffer[offset + i] += crackle;
                _logger.LogTrace("Applied crackle at sample {SampleIndex} with amplitude {Amplitude}", offset + i, crackle);
            }
        }
    }
} 