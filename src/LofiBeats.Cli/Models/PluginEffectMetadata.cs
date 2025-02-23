using System.Text.Json.Serialization;

namespace LofiBeats.Cli.Models;

/// <summary>
/// Represents metadata about a plugin effect returned from the service.
/// </summary>
public record PluginEffectMetadata(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("description")] string Description,
    [property: JsonPropertyName("version")] string Version,
    [property: JsonPropertyName("author")] string Author
); 