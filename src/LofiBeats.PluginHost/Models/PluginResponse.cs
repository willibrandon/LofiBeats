using System.Text.Json;
using System.Text.Json.Serialization;

namespace LofiBeats.PluginHost.Models;

/// <summary>
/// Represents a response message from the plugin host to the main process
/// </summary>
public record PluginResponse
{
    [JsonPropertyName("status")]
    public required string Status { get; init; }

    [JsonPropertyName("message")]
    public string? Message { get; init; }

    [JsonPropertyName("payload")]
    public JsonElement? Payload { get; init; }
} 