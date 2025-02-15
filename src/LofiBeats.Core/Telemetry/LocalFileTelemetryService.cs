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
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = true
    };

    private readonly ILogger<LocalFileTelemetryService> _logger;
    private readonly string _basePath;
    private readonly string _sessionId;
    private readonly ConcurrentQueue<object> _eventBuffer;
    private readonly Timer _flushTimer;
    private readonly int _bufferSize;
    private readonly SemaphoreSlim _flushLock = new(1, 1);
    private bool _disposed;

    public LocalFileTelemetryService(ILogger<LocalFileTelemetryService> logger, string? basePath = null)
    {
        _logger = logger;
        _sessionId = Guid.NewGuid().ToString();
        _eventBuffer = new ConcurrentQueue<object>();
        _bufferSize = 100; // Buffer up to 100 events before forcing a flush

        // Set up the telemetry directory
        _basePath = basePath ?? Path.Combine(
            Environment.GetEnvironmentVariable("LOCALAPPDATA") ?? 
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "LofiBeats",
            "Telemetry"
        );

        // Ensure directory exists and is accessible
        try
        {
            Directory.CreateDirectory(_basePath);
            var testFile = Path.Combine(_basePath, $"test_{Guid.NewGuid()}.tmp");
            File.WriteAllText(testFile, "test");
            File.Delete(testFile);
            _logger.LogInformation("Telemetry service initialized with base path: {BasePath}", _basePath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize telemetry directory at {BasePath}", _basePath);
            throw;
        }

        // Set up a timer to flush the buffer every minute
        _flushTimer = new Timer(async _ => await FlushAsync(), null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));
    }

    public void TrackEvent(string eventName, IDictionary<string, string>? properties = null)
    {
        if (_disposed) return;

        var telemetryEvent = new TelemetryEvent
        {
            Name = eventName,
            Properties = properties,
            SessionId = _sessionId
        };

        EnqueueAndMaybeFlush(telemetryEvent);
        _logger.LogDebug("Tracked event: {EventName}", eventName);
    }

    public void TrackMetric(string metricName, double value, IDictionary<string, string>? properties = null)
    {
        if (_disposed) return;

        var metric = new TelemetryMetric
        {
            Name = metricName,
            Value = value,
            Properties = properties,
            SessionId = _sessionId
        };

        EnqueueAndMaybeFlush(metric);
        _logger.LogDebug("Tracked metric: {MetricName} = {Value}", metricName, value);
    }

    public void TrackException(Exception ex, IDictionary<string, string>? properties = null)
    {
        if (_disposed) return;

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
        if (_disposed) return;
        TrackMetric($"Performance.{operation}", duration.TotalMilliseconds, properties);
    }

    public async Task FlushAsync()
    {
        if (_disposed) return;

        try
        {
            await _flushLock.WaitAsync();

            if (_eventBuffer.IsEmpty) return;

            var eventsToFlush = new List<object>();
            while (_eventBuffer.TryDequeue(out var item))
            {
                eventsToFlush.Add(item);
            }

            var fileName = Path.Combine(_basePath, $"telemetry_{DateTime.UtcNow:yyyyMMdd_HHmmss}_{Guid.NewGuid():N}.json");
            var json = JsonSerializer.Serialize(eventsToFlush, SerializerOptions);
            await File.WriteAllTextAsync(fileName, json);

            _logger.LogInformation("Flushed {Count} telemetry events to {FileName}", eventsToFlush.Count, fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error flushing telemetry buffer");
            throw; // Rethrow to make test failures more obvious
        }
        finally
        {
            _flushLock.Release();
        }
    }

    private void EnqueueAndMaybeFlush(object telemetryItem)
    {
        if (_disposed) return;

        _eventBuffer.Enqueue(telemetryItem);

        // If we've exceeded the buffer size, trigger a flush
        if (_eventBuffer.Count >= _bufferSize)
        {
            _ = FlushAsync();
        }
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