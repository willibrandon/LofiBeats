namespace LofiBeats.Core.Telemetry.Models;

/// <summary>
/// Represents a telemetry event with its associated data.
/// </summary>
public class TelemetryEvent
{
    /// <summary>
    /// Gets or sets the name of the event.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the event occurred.
    /// </summary>
    public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;

    /// <summary>
    /// Gets or sets the properties associated with the event.
    /// </summary>
    public IDictionary<string, string>? Properties { get; set; }

    /// <summary>
    /// Gets or sets the session ID associated with this event.
    /// </summary>
    public string? SessionId { get; set; }
} 