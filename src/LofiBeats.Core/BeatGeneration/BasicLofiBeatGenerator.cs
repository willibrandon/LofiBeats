using LofiBeats.Core.Models;
using Microsoft.Extensions.Logging;

namespace LofiBeats.Core.BeatGeneration;

public class BasicLofiBeatGenerator : IBeatGenerator
{
    private readonly Random _rnd;
    private readonly ILogger<BasicLofiBeatGenerator> _logger;

    // Common lofi chord progressions
    private static readonly string[][] _chordProgressions = new[]
    {
        new[] { "Fmaj7", "Am7", "Dm7", "G7" },      // ii-V-I progression
        new[] { "Am7", "Dm7", "Em7", "Am7" },       // Minor progression
        new[] { "Cmaj7", "Am7", "Fmaj7", "G7" },    // I-vi-IV-V progression
        new[] { "Dm7", "G7", "Cmaj7", "Am7" },      // ii-V-I-vi progression
        new[] { "Em7", "A7", "Dm7", "G7" }          // iii-VI-ii-V progression
    };

    // Basic drum patterns (8 steps)
    private static readonly string[][] _drumPatterns = new[]
    {
        new[] { "kick", "hat", "snare", "hat", "kick", "hat", "snare", "hat" },           // Basic pattern
        new[] { "kick", "hat", "snare", "kick", "kick", "hat", "snare", "hat" },          // Double kick
        new[] { "kick", "hat", "snare", "hat", "kick", "kick", "snare", "hat" },          // Syncopated
        new[] { "kick", "hat", "_", "hat", "kick", "hat", "snare", "hat" },               // With rest
        new[] { "kick", "hat", "snare", "hat", "kick", "hat", "snare", "kick" }           // Kick end
    };

    public BasicLofiBeatGenerator(ILogger<BasicLofiBeatGenerator> logger)
    {
        _logger = logger;
        _rnd = new Random((int)(DateTime.Now.Ticks & 0xFFFFFFFF)); // Seed with current time
        _logger.LogInformation("Beat generator initialized with seed: {Seed}", DateTime.Now.Ticks);
    }

    public BeatPattern GeneratePattern()
    {
        // Random tempo between 70-90 BPM (typical lofi range)
        int tempo = _rnd.Next(70, 91);

        // Randomly select a chord progression
        var chords = _chordProgressions[_rnd.Next(_chordProgressions.Length)];

        // Randomly select and possibly modify a drum pattern
        var basePattern = _drumPatterns[_rnd.Next(_drumPatterns.Length)].ToArray();
        ModifyPattern(basePattern);

        var pattern = new BeatPattern
        {
            Tempo = tempo,
            DrumSequence = basePattern,
            ChordProgression = chords
        };

        _logger.LogInformation("Generated new beat pattern: {Pattern}", pattern);
        return pattern;
    }

    private void ModifyPattern(string[] pattern)
    {
        // 30% chance to add some variation
        if (_rnd.NextDouble() < 0.3)
        {
            // Randomly modify some steps
            for (int i = 0; i < pattern.Length; i++)
            {
                if (_rnd.NextDouble() < 0.2) // 20% chance per step
                {
                    switch (_rnd.Next(3))
                    {
                        case 0:
                            pattern[i] = "kick"; // Add extra kick
                            break;
                        case 1:
                            pattern[i] = "hat"; // Add extra hat
                            break;
                        case 2:
                            pattern[i] = "_"; // Add rest
                            break;
                    }
                }
            }
        }
    }
} 