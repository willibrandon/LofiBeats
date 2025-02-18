namespace LofiBeats.Core.Scheduling;

/// <summary>
/// Provides methods for parsing delay strings into TimeSpan values.
/// </summary>
public static class DelayParser
{
    /// <summary>
    /// Parses a delay string into a TimeSpan.
    /// </summary>
    /// <param name="input">The delay string (e.g., "10m", "30s", "5h")</param>
    /// <returns>A TimeSpan if the input is valid, null otherwise</returns>
    public static TimeSpan? ParseDelay(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return null;

        input = input.Trim().ToLowerInvariant();
        
        // Ensure the input is at least 2 characters (1 digit + 1 unit)
        if (input.Length < 2)
            return null;

        char unit = input[^1]; // Get the last character
        string numberPart = input[..^1]; // Get everything except the last character

        // Verify that the unit is the only non-digit character
        if (!numberPart.All(char.IsDigit))
            return null;

        return unit switch
        {
            'm' => int.TryParse(numberPart, out var minutes) ? TimeSpan.FromMinutes(minutes) : null,
            's' => int.TryParse(numberPart, out var seconds) ? TimeSpan.FromSeconds(seconds) : null,
            'h' => int.TryParse(numberPart, out var hours) ? TimeSpan.FromHours(hours) : null,
            _ => null
        };
    }
} 