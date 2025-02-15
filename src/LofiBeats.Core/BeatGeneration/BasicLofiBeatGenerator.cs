using Microsoft.Extensions.Logging;

namespace LofiBeats.Core.BeatGeneration;

public class BasicLofiBeatGenerator : BaseBeatGenerator
{
    public override string Style => "basic";
    public override (int MinTempo, int MaxTempo) TempoRange => (70, 90);

    public BasicLofiBeatGenerator(ILogger<BasicLofiBeatGenerator> logger) : base(logger)
    {
    }

    protected override string[][] DefineChordProgressions() => new[]
    {
        new[] { "Fmaj7", "Am7", "Dm7", "G7" },      // ii-V-I progression
        new[] { "Am7", "Dm7", "Em7", "Am7" },       // Minor progression
        new[] { "Cmaj7", "Am7", "Fmaj7", "G7" },    // I-vi-IV-V progression
        new[] { "Dm7", "G7", "Cmaj7", "Am7" },      // ii-V-I-vi progression
        new[] { "Em7", "A7", "Dm7", "G7" }          // iii-VI-ii-V progression
    };

    protected override string[][] DefineDrumPatterns() => new[]
    {
        new[] { "kick", "hat", "snare", "hat", "kick", "hat", "snare", "hat" },           // Basic pattern
        new[] { "kick", "hat", "snare", "kick", "kick", "hat", "snare", "hat" },          // Double kick
        new[] { "kick", "hat", "snare", "hat", "kick", "kick", "snare", "hat" },          // Syncopated
        new[] { "kick", "hat", "_", "hat", "kick", "hat", "snare", "hat" },               // With rest
        new[] { "kick", "hat", "snare", "hat", "kick", "hat", "snare", "kick" }           // Kick end
    };

    protected override void ModifyPattern(string[] pattern)
    {
        if (_rnd.NextDouble() < GetVariationProbability())
        {
            for (int i = 0; i < pattern.Length; i++)
            {
                if (_rnd.NextDouble() < GetStepModificationProbability())
                {
                    pattern[i] = _rnd.Next(4) switch
                    {
                        0 => "kick",
                        1 => "hat",
                        2 => "snare",
                        _ => "_"
                    };
                }
            }
        }
    }
} 