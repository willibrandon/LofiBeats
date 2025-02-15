using System.Diagnostics.CodeAnalysis;

namespace LofiBeats.Core.Telemetry;

/// <summary>
/// Defines the core telemetry operations for the LofiBeats application.
/// </summary>
public interface ITelemetryService
{
    /// <summary>
    /// Tracks a named event with optional properties.
    /// </summary>
    /// <param name="eventName">Name of the event to track</param>
    /// <param name="properties">Optional properties to associate with the event</param>
    /// <param name="timestamp">Optional timestamp for the event. If not provided, current time is used.</param>
    void TrackEvent(string eventName, IDictionary<string, string>? properties = null, DateTimeOffset? timestamp = null);

    /// <summary>
    /// Tracks a metric value.
    /// </summary>
    /// <param name="metricName">Name of the metric</param>
    /// <param name="value">Value to record</param>
    /// <param name="properties">Optional properties to associate with the metric</param>
    void TrackMetric(string metricName, double value, IDictionary<string, string>? properties = null);

    /// <summary>
    /// Tracks an exception that occurred in the application.
    /// </summary>
    /// <param name="ex">The exception that occurred</param>
    /// <param name="properties">Optional properties to associate with the exception</param>
    void TrackException(Exception ex, IDictionary<string, string>? properties = null);

    /// <summary>
    /// Tracks the performance duration of an operation.
    /// </summary>
    /// <param name="operation">Name of the operation being measured</param>
    /// <param name="duration">Duration of the operation</param>
    /// <param name="properties">Optional properties to associate with the operation</param>
    void TrackPerformance(string operation, TimeSpan duration, IDictionary<string, string>? properties = null);

    /// <summary>
    /// Flushes any buffered telemetry data.
    /// </summary>
    Task FlushAsync();
} 