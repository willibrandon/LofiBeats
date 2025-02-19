using LofiBeats.Core.WebSocket;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LofiBeats.Service.WebSocket;

/// <summary>
/// Represents a command received from a WebSocket client.
/// </summary>
public sealed class WebSocketCommand
{
    [JsonPropertyName("type")]
    public WebSocketMessageType Type { get; set; }

    [JsonPropertyName("action")]
    public string Action { get; set; } = string.Empty;

    [JsonPropertyName("payload")]
    public JsonElement Payload { get; set; }
} 