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
    public static TimeSpan? ParseDelay(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return null;

        input = input.Trim().ToLowerInvariant();
        if (input.EndsWith('m'))
        {
            if (int.TryParse(input.TrimEnd('m'), out var minutes))
            {
                return TimeSpan.FromMinutes(minutes);
            }
        }
        else if (input.EndsWith('s'))
        {
            if (int.TryParse(input.TrimEnd('s'), out var seconds))
            {
                return TimeSpan.FromSeconds(seconds);
            }
        }
        else if (input.EndsWith('h'))
        {
            if (int.TryParse(input.TrimEnd('h'), out var hours))
            {
                return TimeSpan.FromHours(hours);
            }
        }

        return null;
    }
} 