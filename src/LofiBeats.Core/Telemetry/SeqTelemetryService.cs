using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;

namespace LofiBeats.Core.Telemetry;

/// <summary>
/// A telemetry service implementation that sends telemetry data to Seq.
/// </summary>
public class SeqTelemetryService : ITelemetryService, IDisposable
{
    private readonly ILogger<SeqTelemetryService> _logger;
    private readonly TelemetryConfiguration _config;
    private readonly string _sessionId;
    private readonly Logger _seqLogger;
    private bool _disposed;

    public SeqTelemetryService(
        ILogger<SeqTelemetryService> logger,
        TelemetryConfiguration config)
    {
        _logger = logger;
        _config = config;
        _sessionId = Guid.NewGuid().ToString();

        var loggerConfig = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .Enrich.WithProperty("SessionId", _sessionId)
            .WriteTo.Seq(
                _config.SeqServerUrl,
                apiKey: _config.SeqApiKey,
                controlLevelSwitch: new LoggingLevelSwitch(LogEventLevel.Debug));

        _seqLogger = loggerConfig.CreateLogger();
        
        _logger.LogInformation("Seq telemetry service initialized with server URL: {SeqServerUrl}", 
            _config.SeqServerUrl);
    }

    public void TrackEvent(string eventName, IDictionary<string, string>? properties = null, DateTimeOffset? timestamp = null)
    {
        if (_disposed || !_config.IsEnabled || _config.ShouldFilterEvent(eventName)) return;

        if (!TelemetryConfiguration.IsValidTimestamp(timestamp ?? DateTimeOffset.UtcNow))
        {
            _logger.LogWarning("Rejected event with future timestamp: {EventName}", eventName);
            return;
        }

        using (LogContext.PushProperty("Timestamp", timestamp))
        using (LogContext.PushProperty("EventType", "Event"))
        {
            if (properties?.Count > 0)
            {
                var propertiesObj = properties.ToDictionary(
                    kvp => kvp.Key,
                    kvp => (object)kvp.Value);
                
                _seqLogger.Information("{EventName} {@Properties}", eventName, propertiesObj);
            }
            else
            {
                _seqLogger.Information("{EventName}", eventName);
            }
        }
    }

    public void TrackMetric(string metricName, double value, IDictionary<string, string>? properties = null)
    {
        if (_disposed || !_config.IsEnabled || _config.ShouldFilterEvent(metricName)) return;

        using (LogContext.PushProperty("EventType", "Metric"))
        {
            var props = new Dictionary<string, object>
            {
                ["MetricValue"] = value
            };

            if (properties != null)
            {
                foreach (var prop in properties)
                {
                    props[prop.Key] = prop.Value;
                }
            }

            _seqLogger.Information("{MetricName} {MetricValue} {@Properties}", 
                metricName, value, props);
        }
    }

    public void TrackException(Exception ex, IDictionary<string, string>? properties = null)
    {
        if (_disposed || !_config.IsEnabled) return;

        using (LogContext.PushProperty("EventType", "Exception"))
        {
            if (properties?.Count > 0)
            {
                _seqLogger.Error(ex, "{@Properties}", properties);
            }
            else
            {
                _seqLogger.Error(ex, ex.Message);
            }
        }
    }

    public void TrackPerformance(string operation, TimeSpan duration, IDictionary<string, string>? properties = null)
    {
        if (_disposed || !_config.IsEnabled) return;

        var props = new Dictionary<string, object>
        {
            ["DurationMs"] = duration.TotalMilliseconds
        };

        if (properties != null)
        {
            foreach (var prop in properties)
            {
                props[prop.Key] = prop.Value;
            }
        }

        using (LogContext.PushProperty("EventType", "Performance"))
        {
            _seqLogger.Information("Performance.{OperationName} {@Properties}", 
                operation, props);
        }
    }

    public Task FlushAsync()
    {
        if (_disposed || !_config.IsEnabled) return Task.CompletedTask;

        try
        {
            _seqLogger.Dispose();
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error flushing Seq telemetry");
            throw;
        }
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        
        try
        {
            _seqLogger.Dispose();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error disposing Seq logger");
        }

        GC.SuppressFinalize(this);
    }
} 