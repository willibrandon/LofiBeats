using LofiBeats.Core.Models;
using Microsoft.Extensions.Logging;

namespace LofiBeats.Core.BeatGeneration;

public abstract class BaseBeatGenerator : IBeatGenerator
{
    protected readonly Random _rnd;
    protected readonly ILogger _logger;
    protected readonly string[][] _chordProgressions;
    protected readonly string[][] _drumPatterns;

    public abstract string Style { get; }
    public abstract (int MinTempo, int MaxTempo) TempoRange { get; }

    protected BaseBeatGenerator(ILogger logger)
    {
        _logger = logger;
        _rnd = new Random((int)(DateTime.Now.Ticks & 0xFFFFFFFF));
        _chordProgressions = DefineChordProgressions();
        _drumPatterns = DefineDrumPatterns();
        _logger.LogInformation("{Style} beat generator initialized", Style);
    }

    public BeatPattern GeneratePattern()
    {
        var (minTempo, maxTempo) = TempoRange;
        int tempo = _rnd.Next(minTempo, maxTempo + 1);

        // Select random progression and pattern
        var chords = _chordProgressions[_rnd.Next(_chordProgressions.Length)];
        var basePattern = _drumPatterns[_rnd.Next(_drumPatterns.Length)].ToArray();

        // Apply style-specific modifications
        ModifyPattern(basePattern);

        var pattern = new BeatPattern
        {
            Tempo = tempo,
            DrumSequence = basePattern,
            ChordProgression = chords
        };

        _logger.LogInformation("Generated new {Style} beat pattern: {Pattern}", Style, pattern);
        return pattern;
    }

    protected abstract string[][] DefineChordProgressions();
    protected abstract string[][] DefineDrumPatterns();
    protected abstract void ModifyPattern(string[] pattern);

    protected virtual float GetVariationProbability() => 0.3f;
    protected virtual float GetStepModificationProbability() => 0.2f;
} 