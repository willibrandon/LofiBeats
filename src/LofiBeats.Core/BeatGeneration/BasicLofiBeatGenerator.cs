using LofiBeats.Core.Models;
using Microsoft.Extensions.Logging;

namespace LofiBeats.Core.BeatGeneration;

public class BasicLofiBeatGenerator : IBeatGenerator
{
    private static readonly Random _rnd = new();
    private readonly ILogger<BasicLofiBeatGenerator> _logger;

    public BasicLofiBeatGenerator(ILogger<BasicLofiBeatGenerator> logger)
    {
        _logger = logger;
    }

    public BeatPattern GeneratePattern()
    {
        int tempo = _rnd.Next(70, 91);
        var drums = new[] { "kick", "hat", "snare", "hat" };
        var chords = new[] { "Fmaj7", "Am7", "Dm7", "G7" };

        var pattern = new BeatPattern
        {
            Tempo = tempo,
            DrumSequence = drums,
            ChordProgression = chords
        };

        _logger.LogInformation("Generated new beat pattern: {Pattern}", pattern);
        return pattern;
    }
} 