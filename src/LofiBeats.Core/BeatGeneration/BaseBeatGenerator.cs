using LofiBeats.Core.Models;
using Microsoft.Extensions.Logging;

namespace LofiBeats.Core.BeatGeneration;

public abstract class BaseBeatGenerator : IBeatGenerator
{
    protected readonly Random _rnd;
    protected readonly ILogger _logger;
    protected readonly string[][] _chordProgressions;
    protected readonly string[][] _drumPatterns;
    protected int _bpm;

    public abstract string Style { get; }
    public abstract (int MinBpm, int MaxBpm) BpmRange { get; }

    protected BaseBeatGenerator(ILogger logger)
    {
        _logger = logger;
        _rnd = new Random((int)(DateTime.Now.Ticks & 0xFFFFFFFF));
        _chordProgressions = DefineChordProgressions();
        _drumPatterns = DefineDrumPatterns();
        _bpm = GetDefaultBPM();
        _logger.LogInformation("{Style} beat generator initialized", Style);
    }

    protected virtual int GetDefaultBPM()
    {
        var (min, max) = BpmRange;
        return _rnd.Next(min, max + 1);
    }

    public void SetBPM(int value)
    {
        var (min, max) = BpmRange;
        if (value < min || value > max)
        {
            throw new ArgumentOutOfRangeException(nameof(value), 
                $"BPM must be between {min} and {max} for {Style} style");
        }
        _bpm = value;
    }

    public BeatPattern GeneratePattern()
    {
        return GeneratePattern(null);
    }

    public BeatPattern GeneratePattern(int? bpm)
    {
        var (minBpm, maxBpm) = BpmRange;
        int actualBpm = bpm ?? _rnd.Next(minBpm, maxBpm + 1);

        // Clamp BPM to valid range if provided
        if (bpm.HasValue)
        {
            actualBpm = Math.Clamp(actualBpm, minBpm, maxBpm);
            _logger.LogInformation("Using custom BPM {Bpm} (clamped to range {Min}-{Max})", actualBpm, minBpm, maxBpm);
        }

        // Select random progression and pattern
        var chords = _chordProgressions[_rnd.Next(_chordProgressions.Length)].ToArray();
        var basePattern = _drumPatterns[_rnd.Next(_drumPatterns.Length)].ToArray();

        // Apply style-specific modifications
        ModifyPattern(basePattern);
        ModifyChordProgression(chords);

        var pattern = new BeatPattern
        {
            BPM = actualBpm,
            DrumSequence = basePattern,
            ChordProgression = chords,
            Key = "C" // Default key is C
        };

        // Transpose chords if needed
        if (!string.IsNullOrEmpty(pattern.Key) && pattern.Key != "C")
        {
            pattern.ChordProgression = ChordTransposer.TransposeChords(pattern.ChordProgression, "C", pattern.Key);
            _logger.LogInformation("Transposed chord progression to key {Key}", pattern.Key);
        }

        _logger.LogInformation("Generated new {Style} beat pattern: {Pattern}", Style, pattern);
        return pattern;
    }

    protected abstract string[][] DefineChordProgressions();
    protected abstract string[][] DefineDrumPatterns();
    protected abstract void ModifyPattern(string[] pattern);

    protected virtual float GetVariationProbability() => 0.3f;
    protected virtual float GetStepModificationProbability() => 0.2f;

    // New methods for chord progression variation
    protected virtual void ModifyChordProgression(string[] chords)
    {
        for (int i = 0; i < chords.Length; i++)
        {
            // 20% chance to do an inversion
            if (_rnd.NextDouble() < 0.2)
            {
                chords[i] = AddInversion(chords[i]);
            }

            // 10% chance to add an extension
            if (_rnd.NextDouble() < 0.1)
            {
                chords[i] = AddExtension(chords[i]);
            }
        }
    }

    protected virtual string AddInversion(string chord)
    {
        // Parse the chord to get root note and quality
        var (root, quality) = ParseChord(chord);
        
        // Get possible bass notes based on chord quality
        var bassNotes = GetPossibleBassNotes(root, quality);
        if (bassNotes.Length == 0) return chord;

        // Add a random bass note
        return $"{chord}/{bassNotes[_rnd.Next(bassNotes.Length)]}";
    }

    protected virtual string AddExtension(string chord)
    {
        // Don't add extensions to chords that already have them
        if (chord.Contains('1')) return chord;

        string[] possibleExtensions = ["9", "11", "13"];
        var extension = possibleExtensions[_rnd.Next(possibleExtensions.Length)];

        // Handle different chord types appropriately
        if (chord.Contains("maj7"))
            return chord.Replace("maj7", $"maj{extension}");
        else if (chord.Contains("m7"))
            return chord.Replace("m7", $"m{extension}");
        else if (chord.Contains('7'))
            return chord.Replace("7", extension);
        
        return chord;
    }

    protected static (string Root, string Quality) ParseChord(string chord)
    {
        // Basic chord parsing
        if (string.IsNullOrEmpty(chord)) return ("", "");

        // Get root note (first character, or two characters if sharp/flat)
        string root = chord[0].ToString();
        int qualityStart = 1;

        if (chord.Length > 1 && (chord[1] == '#' || chord[1] == 'b'))
        {
            root += chord[1];
            qualityStart = 2;
        }

        string quality = chord[qualityStart..];
        return (root, quality);
    }

    protected static string[] GetPossibleBassNotes(string root, string quality)
    {
        // Handle special case for invalid chords
        if (root == "X") return ["X"];

        // Define possible bass notes based on chord quality
        if (quality.StartsWith("maj"))
        {
            // Major chord - use major triad notes
            var third = GetMajorThird(root);
            var fifth = GetFifth(root);
            return [third, fifth];
        }
        else if (quality.StartsWith('m'))
        {
            // Minor chord - use minor triad notes
            var third = GetMinorThird(root);
            var fifth = GetFifth(root);
            return [third, fifth];
        }
        else
        {
            // Dominant chord - use major triad notes by default
            var third = GetMajorThird(root);
            var fifth = GetFifth(root);
            return [third, fifth];
        }
    }

    protected static string GetMinorThird(string root)
    {
        // Simplified - just handle basic cases
        return root switch
        {
            "C" => "Eb",
            "D" => "F",
            "E" => "G",
            "F" => "Ab",
            "G" => "Bb",
            "A" => "C",
            "B" => "D",
            _ => root
        };
    }

    protected static string GetMajorThird(string root)
    {
        // Simplified - just handle basic cases
        return root switch
        {
            "C" => "E",
            "D" => "F#",
            "E" => "G#",
            "F" => "A",
            "G" => "B",
            "A" => "C#",
            "B" => "D#",
            _ => root
        };
    }

    protected static string GetFifth(string root)
    {
        // Simplified - just handle basic cases
        return root switch
        {
            "C" => "G",
            "D" => "A",
            "E" => "B",
            "F" => "C",
            "G" => "D",
            "A" => "E",
            "B" => "F#",
            _ => root
        };
    }
} 