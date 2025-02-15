using LofiBeats.Core.Models;

namespace LofiBeats.Core.BeatGeneration;

public interface IBeatGenerator
{
    BeatPattern GeneratePattern(string style = "basic");
    string[] GetAvailableStyles();
} 