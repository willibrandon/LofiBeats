namespace LofiBeats.Core.Telemetry;

/// <summary>
/// A telemetry service that combines multiple telemetry services into one.
/// This allows sending telemetry to multiple destinations simultaneously.
/// </summary>
public class CompositeTelemetryService : ITelemetryService, IAsyncDisposable
{
    private readonly IEnumerable<ITelemetryService> _services;

    public CompositeTelemetryService(IEnumerable<ITelemetryService> services)
    {
        _services = services ?? throw new ArgumentNullException(nameof(services));
    }

    public void TrackEvent(string eventName, IDictionary<string, string>? properties = null, DateTimeOffset? timestamp = null)
    {
        foreach (var service in _services)
        {
            service.TrackEvent(eventName, properties, timestamp);
        }
    }

    public void TrackMetric(string metricName, double value, IDictionary<string, string>? properties = null)
    {
        foreach (var service in _services)
        {
            service.TrackMetric(metricName, value, properties);
        }
    }

    public void TrackException(Exception ex, IDictionary<string, string>? properties = null)
    {
        foreach (var service in _services)
        {
            service.TrackException(ex, properties);
        }
    }

    public void TrackPerformance(string operation, TimeSpan duration, IDictionary<string, string>? properties = null)
    {
        foreach (var service in _services)
        {
            service.TrackPerformance(operation, duration, properties);
        }
    }

    public async Task FlushAsync()
    {
        foreach (var service in _services)
        {
            await service.FlushAsync();
        }
    }

    public async ValueTask DisposeAsync()
    {
        foreach (var service in _services)
        {
            if (service is IAsyncDisposable asyncDisposable)
            {
                await asyncDisposable.DisposeAsync();
            }
            else if (service is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        GC.SuppressFinalize(this);
    }
} 