using System.Collections.Concurrent;
using System.Text.Json;
using LofiBeats.Core.Telemetry.Models;
using Microsoft.Extensions.Logging;

namespace LofiBeats.Core.Telemetry;

/// <summary>
/// A telemetry service implementation that stores telemetry data in local files.
/// </summary>
public class LocalFileTelemetryService : ITelemetryService, IAsyncDisposable
{
    private sealed class TelemetryFile<T>
    {
        public List<T> Items { get; set; } = new();
    }

    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = true
    };

    private readonly ILogger<LocalFileTelemetryService> _logger;
    private readonly TelemetryConfiguration _config;
    private readonly string _basePath;
    private readonly string _sessionId;
    private readonly ConcurrentQueue<object> _eventBuffer;
    private readonly Timer _flushTimer;
    private readonly object _bufferLock = new();
    private readonly SemaphoreSlim _flushLock = new(1, 1);
    private bool _disposed;
    private string? _currentEventsFile;
    private string? _currentMetricsFile;
    private long _currentEventsFileSize;
    private long _currentMetricsFileSize;

    public LocalFileTelemetryService(
        ILogger<LocalFileTelemetryService> logger,
        TelemetryConfiguration? config = null)
    {
        _logger = logger;
        _config = config ?? new TelemetryConfiguration();
        _sessionId = Guid.NewGuid().ToString();
        _eventBuffer = new ConcurrentQueue<object>();

        // Set up the telemetry directory
        _basePath = _config.GetBasePathInternal();

        // Ensure directory exists and is accessible
        try
        {
            Directory.CreateDirectory(_basePath);
            var testFile = Path.Combine(_basePath, $"test_{Guid.NewGuid()}.tmp");
            File.WriteAllText(testFile, "test");
            File.Delete(testFile);
            _logger.LogInformation("Telemetry service initialized with base path: {BasePath}", _basePath);

            // Clean up old files on startup
            _config.CleanupOldFiles(_basePath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize telemetry directory at {BasePath}", _basePath);
            throw;
        }

        // Set up a timer to flush the buffer periodically
        _flushTimer = new Timer(
            async _ => await FlushAsync(), 
            null, 
            TimeSpan.FromMinutes(_config.FlushIntervalMinutes), 
            TimeSpan.FromMinutes(_config.FlushIntervalMinutes));
    }

    public void TrackEvent(string eventName, IDictionary<string, string>? properties = null, DateTimeOffset? timestamp = null)
    {
        if (_disposed || !_config.IsEnabled || _config.ShouldFilterEvent(eventName)) return;

        var telemetryEvent = new TelemetryEvent
        {
            Name = eventName,
            Properties = properties ?? new Dictionary<string, string>(),
            SessionId = _sessionId
        };

        if (timestamp.HasValue)
        {
            telemetryEvent.Timestamp = timestamp.Value;
        }

        // Validate timestamp
        if (!TelemetryConfiguration.IsValidTimestamp(telemetryEvent.Timestamp))
        {
            _logger.LogWarning("Rejected event with future timestamp: {EventName}", eventName);
            return;
        }

        EnqueueAndMaybeFlush(telemetryEvent);
        _logger.LogDebug("Tracked event: {EventName}", eventName);
    }

    public void TrackMetric(string metricName, double value, IDictionary<string, string>? properties = null)
    {
        if (_disposed || !_config.IsEnabled || _config.ShouldFilterEvent(metricName)) return;

        var metric = new TelemetryMetric
        {
            Name = metricName,
            Value = value,
            Properties = properties ?? new Dictionary<string, string>(),
            SessionId = _sessionId
        };

        // Validate timestamp
        if (!TelemetryConfiguration.IsValidTimestamp(metric.Timestamp))
        {
            _logger.LogWarning("Rejected metric with future timestamp: {MetricName}", metricName);
            return;
        }

        EnqueueAndMaybeFlush(metric);
        _logger.LogDebug("Tracked metric: {MetricName} = {Value}", metricName, value);
    }

    public void TrackException(Exception ex, IDictionary<string, string>? properties = null)
    {
        if (_disposed || !_config.IsEnabled) return;

        var combinedProperties = new Dictionary<string, string>(properties ?? new Dictionary<string, string>())
        {
            { "ExceptionType", ex.GetType().Name },
            { "Message", ex.Message },
            { "StackTrace", ex.StackTrace ?? "No stack trace" }
        };

        TrackEvent("Exception", combinedProperties);
        _logger.LogDebug("Tracked exception: {ExceptionType}", ex.GetType().Name);
    }

    public void TrackPerformance(string operation, TimeSpan duration, IDictionary<string, string>? properties = null)
    {
        if (_disposed || !_config.IsEnabled) return;
        TrackMetric($"Performance.{operation}", duration.TotalMilliseconds, properties);
    }

    public async Task FlushAsync()
    {
        if (_disposed || !_config.IsEnabled) return;

        List<object> itemsToProcess;
        lock (_bufferLock)
        {
            itemsToProcess = _eventBuffer.ToList();
            _eventBuffer.Clear();
        }

        if (itemsToProcess.Count == 0) return;

        try
        {
            await _flushLock.WaitAsync();

            var events = itemsToProcess.OfType<TelemetryEvent>().ToList();
            var metrics = itemsToProcess.OfType<TelemetryMetric>().ToList();

            if (events.Count > 0)
            {
                // Split events into chunks based on size
                var currentChunk = new List<TelemetryEvent>();
                var currentSize = 0;
                var maxSize = _config.MaxFileSizeMB * 1024 * 1024;

                foreach (var evt in events)
                {
                    var evtJson = JsonSerializer.Serialize(evt, SerializerOptions);
                    var evtSize = System.Text.Encoding.UTF8.GetByteCount(evtJson);

                    if (currentSize + evtSize >= maxSize)
                    {
                        // Write current chunk
                        await WriteEventsToFile(currentChunk);
                        currentChunk.Clear();
                        currentSize = 0;
                    }

                    currentChunk.Add(evt);
                    currentSize += evtSize;
                }

                // Write remaining events
                if (currentChunk.Count > 0)
                {
                    await WriteEventsToFile(currentChunk);
                }
            }

            if (metrics.Count > 0)
            {
                // Split metrics into chunks based on size
                var currentChunk = new List<TelemetryMetric>();
                var currentSize = 0;
                var maxSize = _config.MaxFileSizeMB * 1024 * 1024;

                foreach (var metric in metrics)
                {
                    var metricJson = JsonSerializer.Serialize(metric, SerializerOptions);
                    var metricSize = System.Text.Encoding.UTF8.GetByteCount(metricJson);

                    if (currentSize + metricSize >= maxSize)
                    {
                        // Write current chunk
                        await WriteMetricsToFile(currentChunk);
                        currentChunk.Clear();
                        currentSize = 0;
                    }

                    currentChunk.Add(metric);
                    currentSize += metricSize;
                }

                // Write remaining metrics
                if (currentChunk.Count > 0)
                {
                    await WriteMetricsToFile(currentChunk);
                }
            }

            // Clean up old files after successful flush
            _config.CleanupOldFiles(_basePath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error flushing telemetry buffer");
            throw;
        }
        finally
        {
            _flushLock.Release();
        }
    }

    private void EnqueueAndMaybeFlush(object telemetryItem)
    {
        if (_disposed) return;

        lock (_bufferLock)
        {
            _eventBuffer.Enqueue(telemetryItem);

            // If we've exceeded the buffer size, trigger a flush
            if (_eventBuffer.Count >= _config.MaxBufferSize)
            {
                try
                {
                    FlushAsync().GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during synchronous flush");
                }
            }
        }
    }

    private async Task WriteEventsToFile(List<TelemetryEvent> events)
    {
        _currentEventsFile = Path.Combine(_basePath, 
            $"telemetry_events_{DateTime.UtcNow:yyyyMMdd_HHmmss}_{Guid.NewGuid():N}.json");
        
        var eventsFile = new TelemetryFile<TelemetryEvent> { Items = events };
        var eventsJson = JsonSerializer.Serialize(eventsFile, SerializerOptions);
        await File.WriteAllTextAsync(_currentEventsFile, eventsJson);
        _currentEventsFileSize = new FileInfo(_currentEventsFile).Length;

        _logger.LogInformation("Flushed {Count} telemetry events to {FileName}", 
            events.Count, _currentEventsFile);
    }

    private async Task WriteMetricsToFile(List<TelemetryMetric> metrics)
    {
        _currentMetricsFile = Path.Combine(_basePath, 
            $"telemetry_metrics_{DateTime.UtcNow:yyyyMMdd_HHmmss}_{Guid.NewGuid():N}.json");
        
        var metricsFile = new TelemetryFile<TelemetryMetric> { Items = metrics };
        var metricsJson = JsonSerializer.Serialize(metricsFile, SerializerOptions);
        await File.WriteAllTextAsync(_currentMetricsFile, metricsJson);
        _currentMetricsFileSize = new FileInfo(_currentMetricsFile).Length;

        _logger.LogInformation("Flushed {Count} telemetry metrics to {FileName}", 
            metrics.Count, _currentMetricsFile);
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed) return;

        _disposed = true;
        await _flushTimer.DisposeAsync();
        _flushLock.Dispose();
        
        // Ensure any remaining telemetry is flushed
        await FlushAsync();
    }

    public void Dispose()
    {
        DisposeAsync().AsTask().GetAwaiter().GetResult();
    }
} 