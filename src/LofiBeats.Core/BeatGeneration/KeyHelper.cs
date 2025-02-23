namespace LofiBeats.Core.BeatGeneration;

/// <summary>
/// Helper class for validating and normalizing musical keys.
/// Provides functionality to handle both sharp and flat notations,
/// normalizing to a consistent sharp-based representation.
/// </summary>
public static class KeyHelper
{
    /// <summary>
    /// The canonical set of valid musical keys, using sharp notation.
    /// </summary>
    private static readonly string[] ValidKeys =
    [
        "C", "C#", "D", "D#", "E", "F",
        "F#", "G", "G#", "A", "A#", "B"
    ];

    /// <summary>
    /// Maps flat notation to their sharp equivalents for normalization.
    /// </summary>
    private static readonly Dictionary<string, string> EnharmonicMap = new()
    {
        { "Db", "C#" },
        { "Eb", "D#" },
        { "Gb", "F#" },
        { "Ab", "G#" },
        { "Bb", "A#" },
        { "Cb", "B" }
    };

    /// <summary>
    /// Gets all valid keys in their canonical sharp form.
    /// </summary>
    /// <returns>Array of valid musical keys.</returns>
    public static string[] GetValidKeys() => [.. ValidKeys];

    /// <summary>
    /// Validates a musical key and normalizes it to sharp notation.
    /// </summary>
    /// <param name="key">The key to validate (e.g., "C", "F#", "Bb").</param>
    /// <param name="normalized">The normalized key in sharp notation if valid; empty string if invalid.</param>
    /// <returns>True if the key is valid; false otherwise.</returns>
    public static bool IsValidKey(string key, out string normalized)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            normalized = string.Empty;
            return false;
        }

        // Convert to correct letter-casing: e.g., "f#" => "F#"
        key = NormalizeKeyCasing(key);

        // If in the main set, return true
        if (ValidKeys.Contains(key))
        {
            normalized = key;
            return true;
        }

        // If in the flat map, convert and accept
        if (EnharmonicMap.TryGetValue(key, out var sharpEquivalent) && ValidKeys.Contains(sharpEquivalent))
        {
            normalized = sharpEquivalent;
            return true;
        }

        normalized = string.Empty;
        return false;
    }

    /// <summary>
    /// Normalizes the casing of a key string.
    /// </summary>
    /// <param name="key">The key to normalize (e.g., "f#", "bb", "DB").</param>
    /// <returns>The key with normalized casing (e.g., "F#", "Bb", "Db").</returns>
    private static string NormalizeKeyCasing(string key)
    {
        key = key.Trim();
        
        // Handle single letter keys (e.g., "c" => "C")
        if (key.Length == 1) 
            return key.ToUpper();
        
        // Handle two-character keys (e.g., "f#" => "F#", "bb" => "Bb", "DB" => "Db")
        if (key.Length == 2)
        {
            char second = char.ToLower(key[1]);
            if (second == '#' || second == 'b')
                return char.ToUpper(key[0]) + second.ToString();
        }
        
        return key;
    }

    /// <summary>
    /// Gets a formatted string of all valid keys for display purposes.
    /// </summary>
    /// <returns>A string listing all valid keys, including both sharp and flat notations.</returns>
    public static string GetValidKeyList()
    {
        var flatKeys = EnharmonicMap.Keys.OrderBy(k => k);
        var sharpKeys = ValidKeys.Where(k => k.Contains('#')).OrderBy(k => k);
        var naturalKeys = ValidKeys.Where(k => k.Length == 1).OrderBy(k => k);

        return $"Natural keys: {string.Join(", ", naturalKeys)}\n" +
               $"Sharp keys: {string.Join(", ", sharpKeys)}\n" +
               $"Flat keys: {string.Join(", ", flatKeys)}";
    }
}
