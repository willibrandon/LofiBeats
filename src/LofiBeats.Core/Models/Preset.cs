using System.Text.Json.Serialization;

namespace LofiBeats.Core.Models;

/// <summary>
/// Represents a preset configuration for LofiBeats, including style, volume, and active effects.
/// </summary>
/// <param name="Name">The unique name of the preset</param>
/// <param name="Style">The beat generation style (e.g., "basic", "jazzy")</param>
/// <param name="Volume">The master volume level (0.0 to 1.0)</param>
/// <param name="Effects">List of active effect names</param>
public record Preset
{
    private const float MinVolume = 0.0f;
    private const float MaxVolume = 1.0f;

    /// <summary>
    /// Gets the unique name of this preset.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// Gets the beat generation style for this preset.
    /// </summary>
    [JsonPropertyName("style")]
    public required string Style { get; init; } = "basic";

    /// <summary>
    /// Gets the master volume level (range: 0.0 to 1.0).
    /// </summary>
    [JsonPropertyName("volume")]
    public float Volume { get; init; } = 1.0f;

    /// <summary>
    /// Gets the list of active effect names.
    /// </summary>
    [JsonPropertyName("effects")]
    public IReadOnlyList<string> Effects { get; init; } = [];

    /// <summary>
    /// Validates the preset configuration.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when preset values are invalid.</exception>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new ArgumentException("Preset name cannot be empty.", nameof(Name));

        if (string.IsNullOrWhiteSpace(Style))
            throw new ArgumentException("Style cannot be empty.", nameof(Style));

        if (Volume < MinVolume || Volume > MaxVolume)
            throw new ArgumentException($"Volume must be between {MinVolume} and {MaxVolume}.", nameof(Volume));
    }
} 