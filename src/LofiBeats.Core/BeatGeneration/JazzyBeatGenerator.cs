using Microsoft.Extensions.Logging;

namespace LofiBeats.Core.BeatGeneration;

public class JazzyBeatGenerator : BaseBeatGenerator
{
    public override string Style => "jazzy";
    public override (int MinBpm, int MaxBpm) BpmRange => (75, 95);

    public JazzyBeatGenerator(ILogger<JazzyBeatGenerator> logger) : base(logger)
    {
    }

    protected override string[][] DefineChordProgressions() =>
    [
        ["Dm9", "G13", "Cmaj9", "Am11"],    // ii-V-I with extensions
        ["Fmaj9", "Em11", "Dm9", "G13"],    // Complex jazz progression
        ["Am9", "D13", "Gmaj9", "Cm7b5"],   // Minor ii-V-I with alterations
        ["Bm11", "E9", "Amaj13", "F#m9"],   // Extended harmony
        ["Gm9", "C13", "Fmaj7#11", "Em11"]  // Altered jazz changes
    ];

    protected override string[][] DefineDrumPatterns() =>
    [
        ["hat", "kick", "hat", "snare", "hat", "kick", "hat", "snare"],           // Swing feel
        ["kick", "hat", "_", "snare", "hat", "kick", "snare", "hat"],             // Syncopated jazz
        ["hat", "kick", "hat", "snare", "kick", "hat", "kick", "snare"],          // Complex jazz
        ["kick", "hat", "snare", "hat", "kick", "snare", "hat", "kick"],          // Broken pattern
        ["hat", "kick", "snare", "kick", "hat", "snare", "kick", "hat"]           // Jazz fusion
    ];

    protected override float GetVariationProbability() => 0.4f;
    protected override float GetStepModificationProbability() => 0.25f;

    protected override void ModifyPattern(string[] pattern)
    {
        if (_rnd.NextDouble() < GetVariationProbability())
        {
            for (int i = 0; i < pattern.Length; i++)
            {
                if (_rnd.NextDouble() < GetStepModificationProbability())
                {
                    pattern[i] = _rnd.Next(5) switch
                    {
                        0 => "kick",
                        1 => "hat",
                        2 => "snare",
                        3 => "kick",  // More emphasis on kick
                        _ => "_"      // More rests for jazz feel
                    };
                }
            }
        }
    }
} 