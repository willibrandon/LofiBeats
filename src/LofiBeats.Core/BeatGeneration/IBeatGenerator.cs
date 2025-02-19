using LofiBeats.Core.Models;

namespace LofiBeats.Core.BeatGeneration;

public interface IBeatGenerator
{
    BeatPattern GeneratePattern();
    BeatPattern GeneratePattern(int? bpm);
    void SetBPM(int value);

    string Style { get; }
    (int MinBpm, int MaxBpm) BpmRange { get; }
}

public interface IBeatGeneratorFactory
{
    IBeatGenerator GetGenerator(string style);
    string[] GetAvailableStyles();
} 