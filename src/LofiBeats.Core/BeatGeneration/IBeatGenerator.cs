using LofiBeats.Core.Models;

namespace LofiBeats.Core.BeatGeneration;

public interface IBeatGenerator
{
    BeatPattern GeneratePattern();
    string Style { get; }
    (int MinTempo, int MaxTempo) TempoRange { get; }
}

public interface IBeatGeneratorFactory
{
    IBeatGenerator GetGenerator(string style);
    string[] GetAvailableStyles();
} 