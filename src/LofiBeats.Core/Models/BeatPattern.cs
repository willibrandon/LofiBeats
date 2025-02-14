namespace LofiBeats.Core.Models;

public class BeatPattern
{
    public int Tempo { get; set; }
    public string[] DrumSequence { get; set; } = Array.Empty<string>();
    public string[] ChordProgression { get; set; } = Array.Empty<string>();

    public override string ToString()
    {
        return $"Tempo: {Tempo} BPM, Drums: [{string.Join(", ", DrumSequence)}], Chords: [{string.Join(", ", ChordProgression)}]";
    }
} 