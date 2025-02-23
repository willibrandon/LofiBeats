using NAudio.Wave;
using Microsoft.Extensions.Logging;
using LofiBeats.Core.PluginApi;

namespace LofiBeats.Core.Effects;

public class TapeFlutterAndHissEffect : IAudioEffect
{
    private ISampleProvider _source;
    private readonly ILogger<TapeFlutterAndHissEffect> _logger;

    // Flutter fields
    private float _flutterPhase;
    private readonly float _flutterSpeed;   // cycles/sec for pitch drift
    private readonly float _flutterDepth;   // ± pitch variation

    // Hiss fields
    private readonly float _hissLevel;      // amplitude of hiss
    private readonly Random _rand;

    public string Author => "LofiBeats Team";

    public string Description => "Adds wow/flutter pitch drift and tape hiss for vintage vibes";

    public string Name => "tapeflutter";

    public string Version => "1.0.0";

    public WaveFormat WaveFormat => _source.WaveFormat;

    public TapeFlutterAndHissEffect(ISampleProvider source,
        ILogger<TapeFlutterAndHissEffect> logger,
        float flutterSpeed = 0.3f,
        float flutterDepth = 0.01f,
        float hissLevel = 0.01f)
    {
        _source = source ?? throw new ArgumentNullException(nameof(source));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _flutterSpeed = flutterSpeed;
        _flutterDepth = flutterDepth;
        _hissLevel = hissLevel;
        _rand = new Random();

        _logger.LogInformation("TapeFlutterAndHissEffect initialized (speed: {flutterSpeed}, depth: {flutterDepth}, hiss: {hissLevel})",
            flutterSpeed, flutterDepth, hissLevel);
    }

    public void SetSource(ISampleProvider source)
    {
        _source = source ?? throw new ArgumentNullException(nameof(source));
        _logger.LogInformation("TapeFlutterAndHissEffect source updated");
    }

    public int Read(float[] buffer, int offset, int count)
    {
        // Read from the underlying source first
        int samplesRead = _source.Read(buffer, offset, count);

        // Apply effect
        ApplyEffect(buffer, offset, samplesRead);

        return samplesRead;
    }

    public void ApplyEffect(float[] buffer, int offset, int count)
    {
        if (count == 0) return;

        // Flutter increment per sample
        float flutterIncrement = (float)(2 * Math.PI * _flutterSpeed / WaveFormat.SampleRate);

        for (int i = 0; i < count; i++)
        {
            // Add hiss
            buffer[offset + i] += (float)(_rand.NextDouble() * 2 - 1.0) * _hissLevel;

            // Calculate flutter factor
            _flutterPhase += flutterIncrement;
            if (_flutterPhase > 2 * Math.PI)
            {
                _flutterPhase -= (float)(2 * Math.PI);
            }

            float flutterFactor = 1.0f + (float)Math.Sin(_flutterPhase) * _flutterDepth;

            // Apply pitch shift in a simplistic manner: scale sample amplitude 
            // For a truly accurate pitch shift, you'd do advanced processing. 
            // We'll do a simpler "wobble" approach:
            buffer[offset + i] *= flutterFactor;
        }
    }
} 