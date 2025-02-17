namespace LofiBeats.Core.Models;

public class BeatPattern
{
    public int BPM { get; set; }
    public string[] DrumSequence { get; set; } = Array.Empty<string>();
    public string[] ChordProgression { get; set; } = Array.Empty<string>();

    public override string ToString()
    {
        return $"Tempo: {BPM} BPM, Drums: [{string.Join(", ", DrumSequence)}], Chords: [{string.Join(", ", ChordProgression)}]";
    }
} 