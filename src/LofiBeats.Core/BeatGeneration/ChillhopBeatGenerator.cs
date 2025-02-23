using Microsoft.Extensions.Logging;

namespace LofiBeats.Core.BeatGeneration;

public class ChillhopBeatGenerator(ILogger<ChillhopBeatGenerator> logger) : BaseBeatGenerator(logger)
{
    public override string Style => "chillhop";

    public override (int MinBpm, int MaxBpm) BpmRange => (65, 85);

    protected override string[][] DefineChordProgressions() =>
    [
        ["Cmaj9", "Am7", "Fmaj9", "Em7"],   // Smooth progression
        ["Dm11", "G9", "Em7", "Am9"],       // Neo-soul style
        ["Fmaj7", "Em7", "Dm7", "Cmaj9"],   // Descending progression
        ["Am9", "Gmaj9", "Fmaj7", "Em11"],  // Ambient progression
        ["Dm9", "Cmaj7", "Bm7b5", "E7b9"]   // Complex chillhop changes
    ];

    protected override string[][] DefineDrumPatterns() =>
    [
        ["kick", "_", "snare", "hat", "kick", "hat", "snare", "_"],               // Laid back
        ["kick", "hat", "snare", "_", "kick", "_", "snare", "hat"],               // Spacious
        ["kick", "hat", "_", "snare", "kick", "hat", "snare", "hat"],             // Neo-soul
        ["_", "kick", "snare", "hat", "kick", "_", "snare", "hat"],               // Off-beat
        ["kick", "hat", "snare", "kick", "_", "hat", "snare", "_"]                // Minimal
    ];

    protected override float GetVariationProbability() => 0.2f;
    protected override float GetStepModificationProbability() => 0.15f;

    protected override void ModifyPattern(string[] pattern)
    {
        if (_rnd.NextDouble() < GetVariationProbability())
        {
            for (int i = 0; i < pattern.Length; i++)
            {
                if (_rnd.NextDouble() < GetStepModificationProbability())
                {
                    pattern[i] = _rnd.Next(6) switch
                    {
                        0 => "kick",
                        1 => "hat",
                        2 => "snare",
                        _ => "_"      // More rests for laid-back feel
                    };
                }
            }
        }
    }
} 