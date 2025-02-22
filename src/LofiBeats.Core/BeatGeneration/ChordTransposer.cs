using System;
using System.Collections.Generic;

namespace LofiBeats.Core.BeatGeneration;

/// <summary>
/// Provides functionality for transposing chord progressions between different musical keys.
/// </summary>
public static class ChordTransposer
{
    /// <summary>
    /// The canonical set of keys in sharp notation, ordered by semitone distance from C.
    /// </summary>
    private static readonly string[] SharpKeys = ["C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B"];

    /// <summary>
    /// Maps sharp keys to their flat equivalents for output consistency.
    /// </summary>
    private static readonly Dictionary<string, string> SharpToFlatMap = new()
    {
        ["C#"] = "Db",
        ["D#"] = "Eb",
        ["F#"] = "Gb",
        ["G#"] = "Ab",
        ["A#"] = "Bb"
    };

    /// <summary>
    /// Transposes a chord progression from one key to another.
    /// </summary>
    /// <param name="chords">The chord progression to transpose.</param>
    /// <param name="fromKey">The source key (defaults to "C" if not specified).</param>
    /// <param name="toKey">The target key to transpose to.</param>
    /// <returns>The transposed chord progression.</returns>
    public static string[] TransposeChords(string[] chords, string fromKey = "C", string toKey = "C")
    {
        if (chords == null || chords.Length == 0) return [];

        // Validate and normalize keys
        if (!KeyHelper.IsValidKey(fromKey, out var normalizedFromKey) ||
            !KeyHelper.IsValidKey(toKey, out var normalizedToKey))
        {
            throw new ArgumentException("Invalid key specified");
        }

        // Validate all chords first
        foreach (var chord in chords)
        {
            // Split chord into main chord and bass note if it has an inversion
            string mainChord = chord;
            string bassNote = "";
            int slashIndex = chord.IndexOf('/');
            
            if (slashIndex != -1)
            {
                mainChord = chord[..slashIndex];
                bassNote = chord[(slashIndex + 1)..];
            }

            // Validate main chord
            var (root, quality) = ParseChord(mainChord);

            // Validate bass note if present
            if (!string.IsNullOrEmpty(bassNote) && !KeyHelper.IsValidKey(bassNote, out _))
            {
                throw new ArgumentException($"Invalid bass note: {bassNote}");
            }
        }

        // Calculate semitone distance
        int semitones = GetSemitoneDistance(normalizedFromKey, normalizedToKey);
        if (semitones == 0) return chords; // No transposition needed

        var transposed = new string[chords.Length];
        for (int i = 0; i < chords.Length; i++)
        {
            // Split chord into main chord and bass note if it has an inversion
            string mainChord = chords[i];
            string bassNote = "";
            int slashIndex = chords[i].IndexOf('/');
            
            if (slashIndex != -1)
            {
                mainChord = chords[i][..slashIndex];
                bassNote = chords[i][(slashIndex + 1)..];
            }

            // Parse and transpose the main chord
            var (root, quality) = ParseChord(mainChord);
            string newRoot = ShiftRoot(root, semitones);
            
            // Convert to flat notation if needed
            if (ShouldUseFlatNotation(toKey))
            {
                newRoot = ConvertToFlatNotation(newRoot);
            }

            // Handle bass note if present
            if (!string.IsNullOrEmpty(bassNote))
            {
                if (!KeyHelper.IsValidKey(bassNote, out var normalizedBass))
                {
                    throw new ArgumentException($"Invalid bass note: {bassNote}");
                }

                string newBass = ShiftRoot(normalizedBass, semitones);
                if (ShouldUseFlatNotation(toKey))
                {
                    newBass = ConvertToFlatNotation(newBass);
                }
                transposed[i] = $"{newRoot}{quality}/{newBass}";
            }
            else
            {
                transposed[i] = newRoot + quality;
            }
        }

        return transposed;
    }

    /// <summary>
    /// Determines if flat notation should be used based on the target key.
    /// </summary>
    private static bool ShouldUseFlatNotation(string key) =>
        key is "F" or "Bb" or "Eb" or "Ab" or "Db";

    /// <summary>
    /// Converts a sharp note to its flat equivalent if available.
    /// </summary>
    private static string ConvertToFlatNotation(string note) =>
        SharpToFlatMap.TryGetValue(note, out var flat) ? flat : note;

    /// <summary>
    /// Calculates the semitone distance between two keys.
    /// </summary>
    private static int GetSemitoneDistance(string fromKey, string toKey)
    {
        var fromIndex = Array.IndexOf(SharpKeys, fromKey);
        var toIndex = Array.IndexOf(SharpKeys, toKey);
        
        // Calculate distance, ensuring positive result by adding 12 if negative
        int distance = toIndex - fromIndex;
        if (distance < 0) distance += 12;
        
        return distance;
    }

    /// <summary>
    /// Parses a chord string into its root note and quality components.
    /// </summary>
    private static (string root, string quality) ParseChord(string chord)
    {
        if (string.IsNullOrWhiteSpace(chord)) 
            throw new ArgumentException("Chord cannot be empty");

        // Extract root note (first character or two if sharp/flat)
        string root = chord[0].ToString();
        int qualityStart = 1;

        if (chord.Length > 1 && (chord[1] == '#' || chord[1] == 'b'))
        {
            root += chord[1];
            qualityStart = 2;
        }

        // Extract quality (everything after the root)
        string quality = chord[qualityStart..];

        // Normalize root to sharp notation if needed
        if (!KeyHelper.IsValidKey(root, out var normalizedRoot))
        {
            throw new ArgumentException($"Invalid chord root: {root}");
        }

        // Validate chord quality
        if (!IsValidChordQuality(quality))
        {
            throw new ArgumentException($"Invalid chord quality: {quality}");
        }

        return (normalizedRoot, quality);
    }

    /// <summary>
    /// Validates the chord quality (e.g., maj7, m7, 7, etc.).
    /// </summary>
    private static bool IsValidChordQuality(string quality)
    {
        // Basic chord qualities
        var validQualities = new[]
        {
            "", // major triad
            "m", "min", "minor",
            "maj", "major",
            "dim", "aug",
            "sus2", "sus4",
            "6", "m6",
            "7", "m7", "maj7", "dim7", "m7b5", "7b5", "7#5",
            "9", "m9", "maj9",
            "11", "m11", "maj11",
            "13", "m13", "maj13"
        };

        // Remove any bass note part
        int slashIndex = quality.IndexOf('/');
        string mainQuality = slashIndex != -1 ? quality[..slashIndex] : quality;

        return validQualities.Contains(mainQuality);
    }

    /// <summary>
    /// Shifts a root note by a specified number of semitones.
    /// </summary>
    private static string ShiftRoot(string root, int semitones)
    {
        var idx = Array.IndexOf(SharpKeys, root);
        if (idx == -1) throw new ArgumentException($"Invalid root note: {root}");

        int newIdx = (idx + semitones) % 12;
        return SharpKeys[newIdx];
    }
} 