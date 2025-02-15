using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;

namespace LofiBeats.Core.Telemetry;

/// <summary>
/// Provides convenient methods for tracking various telemetry metrics and events.
/// </summary>
public class TelemetryTracker
{
    private readonly ITelemetryService _telemetry;
    private readonly ILogger<TelemetryTracker> _logger;
    private readonly Stopwatch _sessionStopwatch;
    private readonly Dictionary<string, Stopwatch> _activeTimers;
    private DateTimeOffset _lastUserInteraction;
    private int _userInteractionCount;

    public TelemetryTracker(ITelemetryService telemetry, ILogger<TelemetryTracker> logger)
    {
        _telemetry = telemetry;
        _logger = logger;
        _sessionStopwatch = Stopwatch.StartNew();
        _activeTimers = new Dictionary<string, Stopwatch>();
        _lastUserInteraction = DateTimeOffset.UtcNow;
        
        // Track initial application data
        TrackApplicationStart();
    }

    #region Application Lifecycle

    public void TrackApplicationStart()
    {
        var properties = new Dictionary<string, string>
        {
            { TelemetryConstants.Properties.OsVersion, RuntimeInformation.OSDescription },
            { TelemetryConstants.Properties.RuntimeVersion, RuntimeInformation.FrameworkDescription },
            { TelemetryConstants.Properties.ProcessorArchitecture, RuntimeInformation.ProcessArchitecture.ToString() },
            { TelemetryConstants.Properties.UserLocale, Thread.CurrentThread.CurrentCulture.Name },
            { TelemetryConstants.Properties.UserTimeZone, TimeZoneInfo.Local.Id }
        };

        _telemetry.TrackEvent(TelemetryConstants.Events.ApplicationStarted, properties);
    }

    public async Task TrackApplicationStop()
    {
        // Track final session duration
        var sessionDuration = _sessionStopwatch.Elapsed;
        _telemetry.TrackMetric(TelemetryConstants.Metrics.SessionDuration, sessionDuration.TotalSeconds);

        // Calculate and track user interaction frequency
        var interactionsPerHour = (_userInteractionCount / sessionDuration.TotalHours);
        _telemetry.TrackMetric(TelemetryConstants.Metrics.UserInteractionFrequency, interactionsPerHour);

        // Track stop event
        _telemetry.TrackEvent(TelemetryConstants.Events.ApplicationStopped);

        // Ensure all telemetry is flushed
        await _telemetry.FlushAsync();
    }

    #endregion

    #region Audio Metrics

    public void TrackAudioQuality(float signalToNoise, float peakAmplitude, float rmsLevel, int clippingCount)
    {
        _telemetry.TrackMetric(TelemetryConstants.Metrics.SignalToNoiseRatio, signalToNoise);
        _telemetry.TrackMetric(TelemetryConstants.Metrics.PeakAmplitude, peakAmplitude);
        _telemetry.TrackMetric(TelemetryConstants.Metrics.RmsLevel, rmsLevel);
        _telemetry.TrackMetric(TelemetryConstants.Metrics.AudioClippingCount, clippingCount);
    }

    public void TrackAudioDeviceChange(string deviceName, string deviceType, string capabilities)
    {
        var properties = new Dictionary<string, string>
        {
            { TelemetryConstants.Properties.DeviceName, deviceName },
            { TelemetryConstants.Properties.DeviceType, deviceType },
            { TelemetryConstants.Properties.DeviceCapabilities, capabilities }
        };

        _telemetry.TrackEvent(TelemetryConstants.Events.AudioDeviceChanged, properties);
    }

    public void TrackAudioLatency(TimeSpan latency, double bufferUsage, double processingLoad)
    {
        _telemetry.TrackMetric(TelemetryConstants.Metrics.AudioLatency, latency.TotalMilliseconds);
        _telemetry.TrackMetric(TelemetryConstants.Metrics.AudioBufferUsage, bufferUsage);
        _telemetry.TrackMetric(TelemetryConstants.Metrics.AudioProcessingLoad, processingLoad);

        if (latency.TotalMilliseconds > 100) // Arbitrary threshold for demonstration
        {
            _telemetry.TrackEvent(TelemetryConstants.Events.AudioLatencySpike, new Dictionary<string, string>
            {
                { "LatencyMs", latency.TotalMilliseconds.ToString("F2") }
            });
        }
    }

    #endregion

    #region Beat Generation

    public void TrackBeatGeneration(string style, int tempo, string key, double complexity,
        TimeSpan generationTime, double syncAccuracy)
    {
        var properties = new Dictionary<string, string>
        {
            { TelemetryConstants.Properties.BeatStyle, style },
            { TelemetryConstants.Properties.BeatTempo, tempo.ToString() },
            { TelemetryConstants.Properties.BeatKey, key },
            { TelemetryConstants.Properties.BeatComplexity, complexity.ToString("F2") }
        };

        _telemetry.TrackEvent(TelemetryConstants.Events.BeatGenerated, properties);
        _telemetry.TrackMetric(TelemetryConstants.Metrics.BeatGenerationTime, generationTime.TotalMilliseconds);
        _telemetry.TrackMetric(TelemetryConstants.Metrics.BeatComplexityScore, complexity);
        _telemetry.TrackMetric(TelemetryConstants.Metrics.BeatSyncAccuracy, syncAccuracy);
    }

    #endregion

    #region Effects

    public void TrackEffectChange(string effectName, string action, IDictionary<string, string>? parameters = null)
    {
        var properties = new Dictionary<string, string>
        {
            { TelemetryConstants.Properties.EffectName, effectName }
        };

        if (parameters != null)
        {
            properties[TelemetryConstants.Properties.EffectParameters] = 
                string.Join(";", parameters.Select(p => $"{p.Key}={p.Value}"));
        }

        string eventName = action switch
        {
            "add" => TelemetryConstants.Events.EffectAdded,
            "remove" => TelemetryConstants.Events.EffectRemoved,
            "modify" => TelemetryConstants.Events.EffectParameterChanged,
            _ => throw new ArgumentException("Invalid effect action", nameof(action))
        };

        _telemetry.TrackEvent(eventName, properties);
    }

    public void StartEffectUsageTimer(string effectName)
    {
        var key = $"effect_{effectName}";
        if (!_activeTimers.ContainsKey(key))
        {
            _activeTimers[key] = Stopwatch.StartNew();
        }
    }

    public void StopEffectUsageTimer(string effectName)
    {
        var key = $"effect_{effectName}";
        if (_activeTimers.TryGetValue(key, out var timer))
        {
            timer.Stop();
            _telemetry.TrackMetric(TelemetryConstants.Metrics.EffectUsageTime, timer.Elapsed.TotalSeconds,
                new Dictionary<string, string> { { TelemetryConstants.Properties.EffectName, effectName } });
            _activeTimers.Remove(key);
        }
    }

    #endregion

    #region System Resources

    public void TrackSystemResources()
    {
        var process = Process.GetCurrentProcess();
        
        _telemetry.TrackMetric(TelemetryConstants.Metrics.MemoryUsage, 
            process.WorkingSet64 / 1024.0 / 1024.0); // Convert to MB

        _telemetry.TrackMetric(TelemetryConstants.Metrics.CpuUsage,
            process.TotalProcessorTime.TotalSeconds / Environment.ProcessorCount);

        // Track thread pool usage
        ThreadPool.GetAvailableThreads(out int workerThreads, out int completionPortThreads);
        ThreadPool.GetMaxThreads(out int maxWorkerThreads, out int maxCompletionPortThreads);

        var threadPoolUsage = 1.0 - ((double)workerThreads / maxWorkerThreads);
        _telemetry.TrackMetric(TelemetryConstants.Metrics.ThreadPoolUsage, threadPoolUsage * 100);
    }

    #endregion

    #region User Interaction

    public void TrackUserInteraction(string interactionType)
    {
        _userInteractionCount++;
        var timeSinceLastInteraction = DateTimeOffset.UtcNow - _lastUserInteraction;
        _lastUserInteraction = DateTimeOffset.UtcNow;

        _telemetry.TrackEvent($"UserInteraction.{interactionType}", new Dictionary<string, string>
        {
            { "TimeSinceLastInteraction", timeSinceLastInteraction.TotalSeconds.ToString("F1") }
        });
    }

    public void TrackUserPreference(string preferenceName, string value)
    {
        _telemetry.TrackEvent(TelemetryConstants.Events.UserPreferenceChanged, new Dictionary<string, string>
        {
            { "PreferenceName", preferenceName },
            { "NewValue", value }
        });
    }

    #endregion

    #region Error Tracking

    public void TrackError(Exception ex, string context)
    {
        var properties = new Dictionary<string, string>
        {
            { "Context", context },
            { "SessionUptime", _sessionStopwatch.Elapsed.ToString() }
        };

        _telemetry.TrackException(ex, properties);

        if (ex is OutOfMemoryException or StackOverflowException)
        {
            // Track critical errors separately
            _telemetry.TrackEvent(TelemetryConstants.Events.ApplicationCrashed, properties);
        }
    }

    #endregion
} 