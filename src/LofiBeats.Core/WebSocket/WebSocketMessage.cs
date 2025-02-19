using System.Text.Json.Serialization;

namespace LofiBeats.Core.WebSocket;

/// <summary>
/// Represents the type of WebSocket message being sent or received.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum WebSocketMessageType
{
    Command,
    Event,
    Error
}

/// <summary>
/// Base record for all WebSocket messages, providing a common structure for commands, events, and errors.
/// </summary>
/// <param name="Type">The type of message (command, event, or error)</param>
/// <param name="Action">The specific action or event name</param>
/// <param name="Payload">The data payload for this message</param>
public sealed record WebSocketMessage(
    [property: JsonPropertyName("type")] WebSocketMessageType Type,
    [property: JsonPropertyName("action")] string Action,
    [property: JsonPropertyName("payload")] object? Payload = null
);

/// <summary>
/// Represents the payload for a play command.
/// </summary>
/// <param name="Style">The beat style to play (e.g., "basic", "chillhop")</param>
/// <param name="Bpm">Optional BPM override</param>
/// <param name="Transition">The transition type ("immediate" or "crossfade")</param>
/// <param name="XfadeDuration">Duration of crossfade in seconds, if applicable</param>
public sealed record PlayCommandPayload(
    [property: JsonPropertyName("style")] string Style = "basic",
    [property: JsonPropertyName("bpm")] int? Bpm = null,
    [property: JsonPropertyName("transition")] string Transition = "immediate",
    [property: JsonPropertyName("xfadeDuration")] double XfadeDuration = 2.0
);

/// <summary>
/// Represents the payload for a stop command.
/// </summary>
/// <param name="TapeStop">Whether to use the tape stop effect</param>
public sealed record StopCommandPayload(
    [property: JsonPropertyName("tapestop")] bool TapeStop = false
);

/// <summary>
/// Represents the payload for a volume change event.
/// </summary>
/// <param name="NewVolume">The new volume level (0.0 to 1.0)</param>
public sealed record VolumeChangedPayload(
    [property: JsonPropertyName("newVolume")] float NewVolume
);

/// <summary>
/// Represents the payload for a playback started event.
/// </summary>
/// <param name="Style">The beat style that started playing</param>
/// <param name="Bpm">The BPM of the playing beat</param>
public sealed record PlaybackStartedPayload(
    [property: JsonPropertyName("style")] string Style,
    [property: JsonPropertyName("bpm")] int Bpm
);

/// <summary>
/// Represents the payload for an error message.
/// </summary>
/// <param name="Message">The error message</param>
public sealed record ErrorPayload(
    [property: JsonPropertyName("message")] string Message
);

/// <summary>
/// Constants for WebSocket message actions.
/// </summary>
public static class WebSocketActions
{
    public static class Commands
    {
        public const string Play = "play";
        public const string Stop = "stop";
        public const string Volume = "volume";
        public const string AddEffect = "add-effect";
        public const string RemoveEffect = "remove-effect";
        public const string SyncState = "sync-state";
    }

    public static class Events
    {
        public const string VolumeChanged = "volume-changed";
        public const string PlaybackStarted = "playback-started";
        public const string PlaybackStopped = "playback-stopped";
        public const string BeatGenerated = "beat-generated";
        public const string EffectAdded = "effect-added";
        public const string EffectRemoved = "effect-removed";
    }

    public static class Errors
    {
        public const string UnknownCommand = "unknown-command";
        public const string InvalidPayload = "invalid-payload";
        public const string AuthenticationFailed = "auth-failed";
    }
} 