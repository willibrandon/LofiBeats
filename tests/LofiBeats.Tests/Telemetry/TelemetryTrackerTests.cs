using System.Runtime.InteropServices;
using LofiBeats.Core.Telemetry;
using Microsoft.Extensions.Logging;
using Moq;

namespace LofiBeats.Tests.Telemetry;

[Collection("AI Generated Tests")]
public class TelemetryTrackerTests
{
    private readonly Mock<ITelemetryService> _telemetryMock;
    private readonly Mock<ILogger<TelemetryTracker>> _loggerMock;
    private readonly TelemetryTracker _tracker;

    public TelemetryTrackerTests()
    {
        _telemetryMock = new Mock<ITelemetryService>();
        _loggerMock = new Mock<ILogger<TelemetryTracker>>();
        _tracker = new TelemetryTracker(_telemetryMock.Object, _loggerMock.Object);
    }

    #region Application Lifecycle Tests

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void TrackApplicationStart_SendsCorrectSystemInfo()
    {
        // Arrange & Act - Constructor calls TrackApplicationStart automatically

        // Assert
        _telemetryMock.Verify(t => t.TrackEvent(
            It.Is<string>(s => s == TelemetryConstants.Events.ApplicationStarted),
            It.Is<IDictionary<string, string>>(d =>
                d[TelemetryConstants.Properties.OsVersion] == RuntimeInformation.OSDescription &&
                d[TelemetryConstants.Properties.RuntimeVersion] == RuntimeInformation.FrameworkDescription &&
                d[TelemetryConstants.Properties.ProcessorArchitecture] == RuntimeInformation.ProcessArchitecture.ToString() &&
                d[TelemetryConstants.Properties.UserLocale] == Thread.CurrentThread.CurrentCulture.Name &&
                d[TelemetryConstants.Properties.UserTimeZone] == TimeZoneInfo.Local.Id
            ),
            It.IsAny<DateTimeOffset?>()), Times.Once);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task TrackApplicationStop_TracksSessionMetrics()
    {
        // Arrange
        await Task.Delay(100); // Ensure some session duration

        // Act
        await _tracker.TrackApplicationStop();

        // Assert
        _telemetryMock.Verify(t => t.TrackMetric(
            It.Is<string>(s => s == TelemetryConstants.Metrics.SessionDuration),
            It.Is<double>(d => d > 0),
            null), Times.Once);
        
        _telemetryMock.Verify(t => t.TrackEvent(
            It.Is<string>(s => s == TelemetryConstants.Events.ApplicationStopped),
            null,
            It.IsAny<DateTimeOffset?>()), Times.Once);
        
        _telemetryMock.Verify(t => t.FlushAsync(), Times.Once);
    }

    #endregion

    #region Audio Metrics Tests

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void TrackAudioQuality_SendsAllMetrics()
    {
        // Arrange
        const float snr = 60.0f;
        const float peak = 0.8f;
        const float rms = 0.5f;
        const int clipping = 2;

        // Act
        _tracker.TrackAudioQuality(snr, peak, rms, clipping);

        // Assert
        _telemetryMock.Verify(t => t.TrackMetric(
            It.Is<string>(s => s == TelemetryConstants.Metrics.SignalToNoiseRatio), 
            It.Is<double>(d => d == snr),
            null), Times.Once);
        _telemetryMock.Verify(t => t.TrackMetric(
            It.Is<string>(s => s == TelemetryConstants.Metrics.PeakAmplitude), 
            It.Is<double>(d => d == peak),
            null), Times.Once);
        _telemetryMock.Verify(t => t.TrackMetric(
            It.Is<string>(s => s == TelemetryConstants.Metrics.RmsLevel), 
            It.Is<double>(d => d == rms),
            null), Times.Once);
        _telemetryMock.Verify(t => t.TrackMetric(
            It.Is<string>(s => s == TelemetryConstants.Metrics.AudioClippingCount), 
            It.Is<double>(d => d == clipping),
            null), Times.Once);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void TrackAudioLatency_DetectsLatencySpikes()
    {
        // Arrange
        var highLatency = TimeSpan.FromMilliseconds(150);
        var normalLatency = TimeSpan.FromMilliseconds(50);
        const double bufferUsage = 0.75;
        const double processingLoad = 0.5;

        // Act & Assert - High Latency
        _tracker.TrackAudioLatency(highLatency, bufferUsage, processingLoad);
        _telemetryMock.Verify(t => t.TrackEvent(
            It.Is<string>(s => s == TelemetryConstants.Events.AudioLatencySpike),
            It.Is<IDictionary<string, string>>(d => 
                d["LatencyMs"] == "150.00"),
            It.IsAny<DateTimeOffset?>()), Times.Once);

        // Act & Assert - Normal Latency
        _tracker.TrackAudioLatency(normalLatency, bufferUsage, processingLoad);
        _telemetryMock.Verify(t => t.TrackEvent(
            It.Is<string>(s => s == TelemetryConstants.Events.AudioLatencySpike),
            It.IsAny<IDictionary<string, string>>(),
            It.IsAny<DateTimeOffset?>()), Times.Once); // Should not increment
    }

    #endregion

    #region Beat Generation Tests

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void TrackBeatGeneration_SendsEventAndMetrics()
    {
        // Arrange
        const string style = "lofi";
        const int tempo = 90;
        const string key = "Cm";
        const double complexity = 0.75;
        var generationTime = TimeSpan.FromMilliseconds(200);
        const double syncAccuracy = 0.95;

        // Act
        _tracker.TrackBeatGeneration(style, tempo, key, complexity, generationTime, syncAccuracy);

        // Assert
        _telemetryMock.Verify(t => t.TrackEvent(
            It.Is<string>(s => s == TelemetryConstants.Events.BeatGenerated),
            It.Is<IDictionary<string, string>>(d =>
                d[TelemetryConstants.Properties.BeatStyle] == style &&
                d[TelemetryConstants.Properties.BeatTempo] == tempo.ToString() &&
                d[TelemetryConstants.Properties.BeatKey] == key &&
                d[TelemetryConstants.Properties.BeatComplexity] == complexity.ToString("F2")
            ),
            It.IsAny<DateTimeOffset?>()), Times.Once);

        _telemetryMock.Verify(t => t.TrackMetric(
            It.Is<string>(s => s == TelemetryConstants.Metrics.BeatGenerationTime),
            It.Is<double>(d => d == 200),
            null), Times.Once);
        _telemetryMock.Verify(t => t.TrackMetric(
            It.Is<string>(s => s == TelemetryConstants.Metrics.BeatComplexityScore),
            It.Is<double>(d => d == complexity),
            null), Times.Once);
        _telemetryMock.Verify(t => t.TrackMetric(
            It.Is<string>(s => s == TelemetryConstants.Metrics.BeatSyncAccuracy),
            It.Is<double>(d => d == syncAccuracy),
            null), Times.Once);
    }

    #endregion

    #region Effects Tests

    [Theory]
    [InlineData("add", TelemetryConstants.Events.EffectAdded)]
    [InlineData("remove", TelemetryConstants.Events.EffectRemoved)]
    [InlineData("modify", TelemetryConstants.Events.EffectParameterChanged)]
    [Trait("Category", "AI_Generated")]
    public void TrackEffectChange_HandlesAllActions(string action, string expectedEvent)
    {
        // Arrange
        const string effectName = "reverb";
        var parameters = new Dictionary<string, string>
        {
            { "wetness", "0.5" },
            { "roomSize", "0.8" }
        };

        // Act
        _tracker.TrackEffectChange(effectName, action, parameters);

        // Assert
        _telemetryMock.Verify(t => t.TrackEvent(
            It.Is<string>(s => s == expectedEvent),
            It.Is<IDictionary<string, string>>(d =>
                d[TelemetryConstants.Properties.EffectName] == effectName &&
                d[TelemetryConstants.Properties.EffectParameters].Contains("wetness=0.5") &&
                d[TelemetryConstants.Properties.EffectParameters].Contains("roomSize=0.8")
            ),
            It.IsAny<DateTimeOffset?>()), Times.Once);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void TrackEffectChange_ThrowsOnInvalidAction()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => 
            _tracker.TrackEffectChange("reverb", "invalid"));
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void EffectUsageTimer_TracksCorrectDuration()
    {
        // Arrange
        const string effectName = "delay";

        // Act
        _tracker.StartEffectUsageTimer(effectName);
        Thread.Sleep(100); // Simulate effect usage
        _tracker.StopEffectUsageTimer(effectName);

        // Assert
        _telemetryMock.Verify(t => t.TrackMetric(
            It.Is<string>(s => s == TelemetryConstants.Metrics.EffectUsageTime),
            It.Is<double>(d => d >= 0.1),
            It.Is<IDictionary<string, string>>(d => 
                d[TelemetryConstants.Properties.EffectName] == effectName)), Times.Once);
    }

    #endregion

    #region System Resources Tests

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void TrackSystemResources_SendsAllMetrics()
    {
        // Act
        _tracker.TrackSystemResources();

        // Assert
        _telemetryMock.Verify(t => t.TrackMetric(
            It.Is<string>(s => s == TelemetryConstants.Metrics.MemoryUsage),
            It.Is<double>(d => d > 0),
            null), Times.Once);

        _telemetryMock.Verify(t => t.TrackMetric(
            It.Is<string>(s => s == TelemetryConstants.Metrics.CpuUsage),
            It.IsAny<double>(),
            null), Times.Once);

        _telemetryMock.Verify(t => t.TrackMetric(
            It.Is<string>(s => s == TelemetryConstants.Metrics.ThreadPoolUsage),
            It.Is<double>(d => d >= 0 && d <= 100),
            null), Times.Once);
    }

    #endregion

    #region User Interaction Tests

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void TrackUserInteraction_UpdatesInteractionCount()
    {
        // Arrange
        const string interactionType = "ButtonClick";

        // Act
        _tracker.TrackUserInteraction(interactionType);
        _tracker.TrackUserInteraction(interactionType);

        // Assert
        _telemetryMock.Verify(t => t.TrackEvent(
            It.Is<string>(s => s == $"UserInteraction.{interactionType}"),
            It.Is<IDictionary<string, string>>(d => 
                d.ContainsKey("TimeSinceLastInteraction")),
            It.IsAny<DateTimeOffset?>()), Times.Exactly(2));
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void TrackUserPreference_SendsCorrectEvent()
    {
        // Arrange
        const string prefName = "Theme";
        const string prefValue = "Dark";

        // Act
        _tracker.TrackUserPreference(prefName, prefValue);

        // Assert
        _telemetryMock.Verify(t => t.TrackEvent(
            It.Is<string>(s => s == TelemetryConstants.Events.UserPreferenceChanged),
            It.Is<IDictionary<string, string>>(d =>
                d["PreferenceName"] == prefName &&
                d["NewValue"] == prefValue),
            It.IsAny<DateTimeOffset?>()), Times.Once);
    }

    #endregion

    #region Error Tracking Tests

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void TrackError_HandlesRegularExceptions()
    {
        // Arrange
        var exception = new InvalidOperationException("Test error");
        const string context = "TestOperation";

        // Act
        _tracker.TrackError(exception, context);

        // Assert
        _telemetryMock.Verify(t => t.TrackException(
            It.Is<Exception>(e => e == exception),
            It.Is<IDictionary<string, string>>(d =>
                d["Context"] == context &&
                d.ContainsKey("SessionUptime"))), Times.Once);

        // Should not track as crash
        _telemetryMock.Verify(t => t.TrackEvent(
            It.Is<string>(s => s == TelemetryConstants.Events.ApplicationCrashed),
            null,
            It.IsAny<DateTimeOffset?>()), Times.Never);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void TrackError_HandlesCriticalExceptions()
    {
        // Arrange
        var exception = new OutOfMemoryException("Critical error");
        const string context = "CriticalOperation";

        // Act
        _tracker.TrackError(exception, context);

        // Assert
        _telemetryMock.Verify(t => t.TrackException(
            It.Is<Exception>(e => e == exception),
            It.Is<IDictionary<string, string>>(d =>
                d["Context"] == context &&
                d.ContainsKey("SessionUptime"))), Times.Once);

        // Should track as crash
        _telemetryMock.Verify(t => t.TrackEvent(
            It.Is<string>(s => s == TelemetryConstants.Events.ApplicationCrashed),
            It.Is<IDictionary<string, string>>(d =>
                d["Context"] == context),
            It.IsAny<DateTimeOffset?>()), Times.Once);
    }

    #endregion
} 