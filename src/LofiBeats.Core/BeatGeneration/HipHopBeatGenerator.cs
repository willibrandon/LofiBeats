using Microsoft.Extensions.Logging;

namespace LofiBeats.Core.BeatGeneration;

public class HipHopBeatGenerator(ILogger<HipHopBeatGenerator> logger) : BaseBeatGenerator(logger)
{
    public override string Style => "hiphop";

    public override (int MinBpm, int MaxBpm) BpmRange => (80, 100);

    protected override string[][] DefineChordProgressions() =>
    [
        ["Dm7", "G7", "Cmaj7", "Am7"],      // Classic jazz-hop progression
        ["Em7", "A7", "Dm7", "Bm7b5"],      // Minor jazz with half-diminished
        ["Fmaj7", "Em7", "Am7", "Dm7"],     // Descending soul progression
        ["Gm7", "C7", "Fmaj7", "Em7b5"],    // Minor to major movement
        ["Am7", "D7", "Gmaj7", "Em7"],      // Secondary dominant progression
        ["Bm7b5", "E7b9", "Am7", "D7"]      // Jazz turnaround with alterations
    ];

    protected override string[][] DefineDrumPatterns() =>
    [
        ["kick", "hat", "snare", "_", "kick", "hat", "snare", "hat"],             // Classic boom bap
        ["kick", "hat", "snare", "kick", "_", "hat", "snare", "hat"],             // Syncopated kick
        ["kick", "_", "snare", "hat", "kick", "kick", "snare", "_"],              // Double kick
        ["kick", "hat", "snare", "_", "kick", "_", "snare", "kick"],              // Kick heavy
        ["_", "kick", "snare", "hat", "kick", "hat", "snare", "_"],               // Off-beat start
        ["kick", "hat", "_", "snare", "kick", "hat", "snare", "kick"]             // Complex pattern
    ];

    protected override float GetVariationProbability() => 0.35f;
    protected override float GetStepModificationProbability() => 0.3f;

    protected override void ModifyPattern(string[] pattern)
    {
        if (_rnd.NextDouble() < GetVariationProbability())
        {
            for (int i = 0; i < pattern.Length; i++)
            {
                if (_rnd.NextDouble() < GetStepModificationProbability())
                {
                    // Hip hop patterns often emphasize kicks and have more complex variations
                    pattern[i] = _rnd.Next(6) switch
                    {
                        0 => "kick",
                        1 => "hat",
                        2 => "snare",
                        3 => "kick",  // Extra weight on kicks for boom bap feel
                        4 => "_",     // Rests for groove
                        _ => pattern[i] // Sometimes keep existing element
                    };
                }
            }

            // Add occasional kick doubling for classic boom bap feel
            if (_rnd.NextDouble() < 0.3)
            {
                int pos = _rnd.Next(pattern.Length - 1);
                if (pattern[pos] == "kick")
                {
                    pattern[pos + 1] = "kick";
                }
            }
        }
    }
} 