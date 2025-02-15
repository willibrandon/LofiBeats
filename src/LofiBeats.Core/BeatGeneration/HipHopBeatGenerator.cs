using Microsoft.Extensions.Logging;

namespace LofiBeats.Core.BeatGeneration;

public class HipHopBeatGenerator : BaseBeatGenerator
{
    public override string Style => "lofihiphop";
    public override (int MinTempo, int MaxTempo) TempoRange => (80, 100);

    public HipHopBeatGenerator(ILogger<HipHopBeatGenerator> logger) : base(logger)
    {
    }

    protected override string[][] DefineChordProgressions() =>
    [
        ["Cm9", "Fm7", "Ab6", "G7"],        // Minor hip hop progression
        ["Ebmaj7", "Dm7b5", "Cm9", "Bb13"], // Jazz-influenced hip hop
        ["Gm7", "C9", "Fm7", "Bb7"],        // Minor groove
        ["Am7", "F9", "Dm7", "E7b9"],       // Urban progression
        ["Bbmaj9", "Gm11", "Eb6/9", "Cm7"]  // Modern lofi hip hop
    ];

    protected override string[][] DefineDrumPatterns() =>
    [
        ["kick", "hat", "_", "snare", "kick", "hat", "snare", "hat"],            // Classic boom bap
        ["kick", "_", "snare", "kick", "hat", "kick", "snare", "_"],             // Trap influenced
        ["kick", "hat", "kick", "snare", "_", "hat", "snare", "hat"],            // Modern bounce
        ["kick", "hat", "snare", "_", "kick", "hat", "_", "snare"],              // Lo-fi groove
        ["_", "kick", "snare", "hat", "kick", "kick", "snare", "hat"]            // Syncopated hip hop
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
                    // Hip hop style often has double kicks and more varied patterns
                    pattern[i] = _rnd.Next(7) switch
                    {
                        0 => "kick",
                        1 => "hat",
                        2 => "snare",
                        3 => "kick",  // Extra kick for hip hop feel
                        4 => "hat",   // Extra hat for rhythm
                        5 => "_",     // Rest for groove
                        _ => pattern[i] // Sometimes keep existing
                    };
                }
            }
        }
    }
} 