using LofiBeats.Core.Models;
using Microsoft.Extensions.Logging;

namespace LofiBeats.Core.BeatGeneration;

public class BasicLofiBeatGenerator : IBeatGenerator
{
    private readonly Random _rnd;
    private readonly ILogger<BasicLofiBeatGenerator> _logger;

    // Common lofi chord progressions by style
    private static readonly Dictionary<string, string[][]> _styleChordProgressions = new()
    {
        ["basic"] =
        [
            ["Fmaj7", "Am7", "Dm7", "G7"],      // ii-V-I progression
            ["Am7", "Dm7", "Em7", "Am7"],       // Minor progression
            ["Cmaj7", "Am7", "Fmaj7", "G7"],    // I-vi-IV-V progression
            ["Dm7", "G7", "Cmaj7", "Am7"],      // ii-V-I-vi progression
            ["Em7", "A7", "Dm7", "G7"]          // iii-VI-ii-V progression
        ],
        ["jazzy"] =
        [
            ["Dm9", "G13", "Cmaj9", "Am11"],    // ii-V-I with extensions
            ["Fmaj9", "Em11", "Dm9", "G13"],    // Complex jazz progression
            ["Am9", "D13", "Gmaj9", "Cm7b5"],   // Minor ii-V-I with alterations
            ["Bm11", "E9", "Amaj13", "F#m9"],   // Extended harmony
            ["Gm9", "C13", "Fmaj7#11", "Em11"]  // Altered jazz changes
        ],
        ["chillhop"] =
        [
            ["Cmaj9", "Am7", "Fmaj9", "Em7"],   // Smooth progression
            ["Dm11", "G9", "Em7", "Am9"],       // Neo-soul style
            ["Fmaj7", "Em7", "Dm7", "Cmaj9"],   // Descending progression
            ["Am9", "Gmaj9", "Fmaj7", "Em11"],  // Ambient progression
            ["Dm9", "Cmaj7", "Bm7b5", "E7b9"]   // Complex chillhop changes
        ]
    };

    // Drum patterns by style
    private static readonly Dictionary<string, string[][]> _styleDrumPatterns = new()
    {
        ["basic"] =
        [
            ["kick", "hat", "snare", "hat", "kick", "hat", "snare", "hat"],           // Basic pattern
            ["kick", "hat", "snare", "kick", "kick", "hat", "snare", "hat"],          // Double kick
            ["kick", "hat", "snare", "hat", "kick", "kick", "snare", "hat"],          // Syncopated
            ["kick", "hat", "_", "hat", "kick", "hat", "snare", "hat"],               // With rest
            ["kick", "hat", "snare", "hat", "kick", "hat", "snare", "kick"]           // Kick end
        ],
        ["jazzy"] =
        [
            ["hat", "kick", "hat", "snare", "hat", "kick", "hat", "snare"],           // Swing feel
            ["kick", "hat", "_", "snare", "hat", "kick", "snare", "hat"],             // Syncopated jazz
            ["hat", "kick", "hat", "snare", "kick", "hat", "kick", "snare"],          // Complex jazz
            ["kick", "hat", "snare", "hat", "kick", "snare", "hat", "kick"],          // Broken pattern
            ["hat", "kick", "snare", "kick", "hat", "snare", "kick", "hat"]           // Jazz fusion
        ],
        ["chillhop"] =
        [
            ["kick", "_", "snare", "hat", "kick", "hat", "snare", "_"],               // Laid back
            ["kick", "hat", "snare", "_", "kick", "_", "snare", "hat"],               // Spacious
            ["kick", "hat", "_", "snare", "kick", "hat", "snare", "hat"],             // Neo-soul
            ["_", "kick", "snare", "hat", "kick", "_", "snare", "hat"],               // Off-beat
            ["kick", "hat", "snare", "kick", "_", "hat", "snare", "_"]                // Minimal
        ]
    };

    private static readonly Dictionary<string, (int Min, int Max)> _styleTempoRanges = new()
    {
        ["basic"] = (70, 90),
        ["jazzy"] = (75, 95),
        ["chillhop"] = (65, 85)
    };

    public BasicLofiBeatGenerator(ILogger<BasicLofiBeatGenerator> logger)
    {
        _logger = logger;
        _rnd = new Random((int)(DateTime.Now.Ticks & 0xFFFFFFFF)); // Seed with current time
        _logger.LogInformation("Beat generator initialized with seed: {Seed}", DateTime.Now.Ticks);
    }

    public string[] GetAvailableStyles() => _styleChordProgressions.Keys.ToArray();

    public BeatPattern GeneratePattern(string style = "basic")
    {
        // Normalize and validate style
        style = style.ToLower();
        if (!_styleChordProgressions.ContainsKey(style))
        {
            _logger.LogWarning("Unknown style '{Style}', falling back to 'basic'", style);
            style = "basic";
        }

        // Get style-specific tempo range
        var (minTempo, maxTempo) = _styleTempoRanges[style];
        int tempo = _rnd.Next(minTempo, maxTempo + 1);

        // Get style-specific progressions and patterns
        var chordProgressions = _styleChordProgressions[style];
        var drumPatterns = _styleDrumPatterns[style];

        // Select random progression and pattern
        var chords = chordProgressions[_rnd.Next(chordProgressions.Length)];
        var basePattern = drumPatterns[_rnd.Next(drumPatterns.Length)].ToArray();

        // Apply style-specific modifications
        ModifyPattern(basePattern, style);

        var pattern = new BeatPattern
        {
            Tempo = tempo,
            DrumSequence = basePattern,
            ChordProgression = chords
        };

        _logger.LogInformation("Generated new {Style} beat pattern: {Pattern}", style, pattern);
        return pattern;
    }

    private void ModifyPattern(string[] pattern, string style)
    {
        // Style-specific variation probabilities
        var variationChance = style switch
        {
            "jazzy" => 0.4f,    // More variations for jazz
            "chillhop" => 0.2f, // Fewer variations for chillhop
            _ => 0.3f           // Default variation rate
        };

        // Style-specific step modification probabilities
        var stepModChance = style switch
        {
            "jazzy" => 0.25f,   // More frequent modifications
            "chillhop" => 0.15f,// Less frequent modifications
            _ => 0.2f           // Default modification rate
        };

        if (_rnd.NextDouble() < variationChance)
        {
            for (int i = 0; i < pattern.Length; i++)
            {
                if (_rnd.NextDouble() < stepModChance)
                {
                    pattern[i] = style switch
                    {
                        "jazzy" => _rnd.Next(4) switch
                        {
                            0 => "kick",
                            1 => "hat",
                            2 => "snare",
                            _ => "_" // More rests in jazz
                        },
                        "chillhop" => _rnd.Next(5) switch
                        {
                            0 => "kick",
                            1 => "hat",
                            2 => "snare",
                            _ => "_" // Even more rests in chillhop
                        },
                        _ => _rnd.Next(3) switch
                        {
                            0 => "kick",
                            1 => "hat",
                            2 => "snare",
                            _ => "_" // Default to rest for basic style too
                        }
                    };
                }
            }
        }
    }
} 