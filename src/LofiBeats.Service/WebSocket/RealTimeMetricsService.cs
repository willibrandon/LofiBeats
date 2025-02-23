using LofiBeats.Core.Playback;
using LofiBeats.Core.Telemetry;
using LofiBeats.Core.WebSocket;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace LofiBeats.Service.WebSocket;

/// <summary>
/// Background service that broadcasts real-time metrics over WebSocket.
/// </summary>
public sealed class RealTimeMetricsService(
    ILogger<RealTimeMetricsService> logger,
    IWebSocketBroadcaster broadcaster,
    IAudioPlaybackService playback,
    TelemetryTracker telemetry,
    IOptions<WebSocketConfiguration> config) : BackgroundService
{
    private static readonly Action<ILogger, Exception?> _logStarting =
        LoggerMessage.Define(LogLevel.Information,
            new EventId(1, "Starting"),
            "Starting real-time metrics service");

    private static readonly Action<ILogger, Exception?> _logStopping =
        LoggerMessage.Define(LogLevel.Information,
            new EventId(2, "Stopping"),
            "Stopping real-time metrics service");

    private static readonly Action<ILogger, Exception> _logServiceError =
        LoggerMessage.Define(LogLevel.Error,
            new EventId(3, "ServiceError"),
            "Error in real-time metrics service");

    private static readonly Action<ILogger, Exception> _logBroadcastError =
        LoggerMessage.Define(LogLevel.Error,
            new EventId(4, "BroadcastError"),
            "Error broadcasting metrics");

    private readonly ILogger<RealTimeMetricsService> _logger = logger;
    private readonly IWebSocketBroadcaster _broadcaster = broadcaster;
    private readonly IAudioPlaybackService _playback = playback;
    private readonly TelemetryTracker _telemetry = telemetry;
    private readonly WebSocketConfiguration _config = config.Value;
    private readonly PeriodicTimer _timer = new(TimeSpan.FromMilliseconds(50));
    private readonly Process _currentProcess = Process.GetCurrentProcess();
    
    // Cache previous values to detect changes
    private NAudio.Wave.PlaybackState _lastPlaybackState;
    private float _lastVolume;
    private string? _lastStyle;
    private double _lastMemoryMB;
    private double _lastCpuUsage;
    private double _lastThreadPoolUsage;
    private DateTimeOffset _lastBroadcast = DateTimeOffset.MinValue;
    private const double ChangeThreshold = 0.5; // 0.5% change threshold for numeric values
    private static readonly TimeSpan BroadcastInterval = TimeSpan.FromMilliseconds(200); // 5Hz instead of 20Hz

    private bool _disposed;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logStarting(_logger, null);

        try
        {
            while (await _timer.WaitForNextTickAsync(stoppingToken))
            {
                if (stoppingToken.IsCancellationRequested)
                    break;

                await BroadcastMetricsAsync(stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            // Normal shutdown
        }
        catch (Exception ex)
        {
            _logServiceError(_logger, ex);
        }
    }

    private static bool HasSignificantChange(double current, double previous)
    {
        if (Math.Abs(previous) < 0.0001) // or some small epsilon
        {
            // If the previous value was near zero, any non-trivial change might be considered significant.
            return Math.Abs(current - previous) > 0.1;
        }

        return Math.Abs((current - previous) / previous * 100) > ChangeThreshold;
    }

    private async Task BroadcastMetricsAsync(CancellationToken cancellationToken)
    {
        try
        {
            // Get current metrics
            var playbackState = _playback.GetPlaybackState();
            var volume = _playback.CurrentVolume;
            var style = _playback.CurrentStyle;
            var memoryMB = Math.Round(_currentProcess.WorkingSet64 / 1024.0 / 1024.0, 2);
            var cpuTime = Math.Round(_currentProcess.TotalProcessorTime.TotalSeconds / Environment.ProcessorCount * 100, 1);
            
            ThreadPool.GetAvailableThreads(out int workerThreads, out int completionPortThreads);
            ThreadPool.GetMaxThreads(out int maxWorkerThreads, out int maxCompletionPortThreads);
            var threadPoolUsage = Math.Round((1.0 - ((double)workerThreads / maxWorkerThreads)) * 100, 1);

            // Check if enough time has passed since last broadcast
            var timeSinceLastBroadcast = DateTimeOffset.UtcNow - _lastBroadcast;
            if (timeSinceLastBroadcast < BroadcastInterval)
            {
                return;
            }

            // Check if any significant changes occurred
            bool shouldBroadcast = playbackState != _lastPlaybackState ||
                                 Math.Abs(volume - _lastVolume) > 0.01f ||
                                 style != _lastStyle ||
                                 HasSignificantChange(memoryMB, _lastMemoryMB) ||
                                 HasSignificantChange(cpuTime, _lastCpuUsage) ||
                                 HasSignificantChange(threadPoolUsage, _lastThreadPoolUsage);

            if (!shouldBroadcast)
            {
                return;
            }

            // Update cached values
            _lastPlaybackState = playbackState;
            _lastVolume = volume;
            _lastStyle = style;
            _lastMemoryMB = memoryMB;
            _lastCpuUsage = cpuTime;
            _lastThreadPoolUsage = threadPoolUsage;
            _lastBroadcast = DateTimeOffset.UtcNow;

            // Create metrics payload
            var metrics = new
            {
                timestamp = _lastBroadcast,
                playback = new
                {
                    state = playbackState.ToString(),
                    volume,
                    style
                },
                system = new
                {
                    memoryMB,
                    cpuUsage = cpuTime,
                    threadPoolUsage
                }
            };

            // Track metrics in telemetry (at full sample rate)
            _telemetry.TrackMetric(TelemetryConstants.Metrics.MemoryUsage, memoryMB);
            _telemetry.TrackMetric(TelemetryConstants.Metrics.CpuUsage, cpuTime);
            _telemetry.TrackMetric(TelemetryConstants.Metrics.ThreadPoolUsage, threadPoolUsage);

            // Broadcast only when there are significant changes
            await _broadcaster.BroadcastEventAsync(
                WebSocketActions.Events.MetricsUpdated,
                metrics,
                cancellationToken);
        }
        catch (Exception ex)
        {
            _logBroadcastError(_logger, ex);
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        // Log once
        _logStopping(_logger, null);

        // Safely dispose the PeriodicTimer here
        if (!_disposed)
        {
            _disposed = true;
            _timer?.Dispose();
        }

        await base.StopAsync(cancellationToken);
    }
} 