using System.Text.Json;
using System.Text.Json.Serialization;

namespace LofiBeats.PluginHost.Models;

/// <summary>
/// Represents a message exchanged between the main process and plugin host
/// </summary>
public record PluginMessage
{
    [JsonPropertyName("action")]
    public required string Action { get; init; }

    [JsonPropertyName("payload")]
    public JsonElement? Payload { get; init; }
} 