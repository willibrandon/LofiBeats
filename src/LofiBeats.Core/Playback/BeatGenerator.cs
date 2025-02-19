using LofiBeats.Core.BeatGeneration;
using LofiBeats.Core.Models;
using Microsoft.Extensions.Logging;

namespace LofiBeats.Core.Playback;

public class BeatGenerator : IBeatGenerator
{
    private readonly string _style;
    private readonly ILogger<BeatGenerator> _logger;
    private int _bpm;

    public string Style => _style;

    public (int MinBpm, int MaxBpm) BpmRange => _style switch
    {
        "jazzy" => (75, 95),
        "chillhop" => (65, 85),
        "hiphop" => (80, 100),
        _ => (70, 90) // basic
    };

    public BeatGenerator(string style, ILogger<BeatGenerator> logger)
    {
        _style = style;
        _logger = logger;
        _bpm = GetDefaultBPM(style);
    }

    private static int GetDefaultBPM(string style)
    {
        return style switch
        {
            "jazzy" => 85,
            "chillhop" => 90,
            "hiphop" => 95,
            _ => 80 // basic
        };
    }

    public void SetBPM(int value)
    {
        if (value < 60 || value > 140)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "BPM must be between 60 and 140");
        }
        _bpm = value;
    }

    public BeatPattern GeneratePattern()
    {
        return GeneratePattern(null);
    }

    public BeatPattern GeneratePattern(int? bpm)
    {
        // If BPM is provided, temporarily set it for this pattern
        var originalBpm = _bpm;
        if (bpm.HasValue)
        {
            SetBPM(bpm.Value);
        }

        try
        {
            // Use the stored BPM value
            var pattern = new BeatPattern
            {
                BPM = _bpm,
                DrumSequence = GenerateDrumSequence(),
                ChordProgression = GenerateChordProgression(),
                UserSampleSteps = new Dictionary<int, string>() // Initialize empty dictionary
            };

            _logger.LogInformation("Generated {Style} pattern at {BPM} BPM", _style, _bpm);
            return pattern;
        }
        finally
        {
            // Restore original BPM if we changed it
            if (bpm.HasValue)
            {
                _bpm = originalBpm;
            }
        }
    }

    private string[] GenerateDrumSequence()
    {
        // Simple 16-step pattern based on style
        return _style switch
        {
            "jazzy" => ["kick", "hat", "snare", "hat", "kick", "hat", "snare", "hat",
                       "kick", "hat", "snare", "hat", "kick", "hat", "snare", "hat"],
            "chillhop" => ["kick", "hat", "snare", "hat", "kick", "hat", "snare", "hat",
                          "kick", "hat", "snare", "hat", "kick", "hat", "snare", "hat"],
            "hiphop" => ["kick", "hat", "snare", "hat", "kick", "hat", "snare", "hat",
                        "kick", "hat", "snare", "hat", "kick", "hat", "snare", "hat"],
            _ => ["kick", "hat", "snare", "hat", "kick", "hat", "snare", "hat",
                  "kick", "hat", "snare", "hat", "kick", "hat", "snare", "hat"]
        };
    }

    private string[] GenerateChordProgression()
    {
        // Simple 4-chord progression based on style
        return _style switch
        {
            "jazzy" => ["Cm7", "Fm7", "Bb7", "Eb7"],
            "chillhop" => ["Fmaj7", "Em7", "Dm7", "Cmaj7"],
            "hiphop" => ["Am7", "Dm7", "G7", "Cmaj7"],
            _ => ["Cmaj7", "Am7", "Fmaj7", "G7"]
        };
    }
} 