namespace LofiBeats.Core.Telemetry.Models;

/// <summary>
/// Represents a telemetry metric measurement.
/// </summary>
public class TelemetryMetric
{
    /// <summary>
    /// Gets or sets the name of the metric.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the value of the metric.
    /// </summary>
    public double Value { get; set; }

    private DateTimeOffset _timestamp;

    /// <summary>
    /// Gets or sets the timestamp when the metric was recorded.
    /// </summary>
    public DateTimeOffset Timestamp
    {
        get => _timestamp;
        set => _timestamp = value.ToUniversalTime();
    }

    /// <summary>
    /// Gets or sets the properties associated with the metric.
    /// </summary>
    public IDictionary<string, string>? Properties { get; set; }

    /// <summary>
    /// Gets or sets the session ID associated with this metric.
    /// </summary>
    public string? SessionId { get; set; }

    public TelemetryMetric()
    {
        Timestamp = DateTimeOffset.UtcNow;
    }
} 