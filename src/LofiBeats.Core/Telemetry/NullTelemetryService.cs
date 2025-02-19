using System;
using System.Collections.Generic;

namespace LofiBeats.Core.Telemetry;

/// <summary>
/// A no-op implementation of ITelemetryService that can be used when telemetry is not needed.
/// </summary>
public class NullTelemetryService : ITelemetryService
{
    public void TrackEvent(string eventName, IDictionary<string, string>? properties = null, DateTimeOffset? timestamp = null)
    {
        // No-op
    }

    public void TrackMetric(string metricName, double value, IDictionary<string, string>? properties = null)
    {
        // No-op
    }

    public void TrackException(Exception ex, IDictionary<string, string>? properties = null)
    {
        // No-op
    }

    public void TrackPerformance(string operation, TimeSpan duration, IDictionary<string, string>? properties = null)
    {
        // No-op
    }

    public Task FlushAsync()
    {
        return Task.CompletedTask;
    }
} 