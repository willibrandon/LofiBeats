using System.Text.Json;
using LofiBeats.Core.Telemetry;
using LofiBeats.Core.Telemetry.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace LofiBeats.Tests.Telemetry;

[Collection("AI Generated Tests")]
public class LocalFileTelemetryServiceTests : IAsyncDisposable
{
    private static readonly Action<ILogger, string, Exception?> LogCleanupError =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(1, nameof(LogCleanupError)),
            "Error cleaning up test directory: {Path}");

    private readonly Mock<ILogger<LocalFileTelemetryService>> _loggerMock;
    private readonly string _testTelemetryPath;
    private readonly LocalFileTelemetryService _service;
    private static readonly JsonSerializerOptions SerializerOptions = new() { WriteIndented = true };

    public LocalFileTelemetryServiceTests()
    {
        _loggerMock = new Mock<ILogger<LocalFileTelemetryService>>();
        
        // Create a temporary directory for test telemetry using platform-specific paths
        _testTelemetryPath = Path.Combine(
            Path.GetTempPath(),
            "LofiBeatsTests",
            "Telemetry",
            Guid.NewGuid().ToString());

        Directory.CreateDirectory(_testTelemetryPath);
        
        // Create service with test path
        _service = new LocalFileTelemetryService(_loggerMock.Object, _testTelemetryPath);
    }

    private async Task<List<T>> GetTelemetryItemsFromFiles<T>()
    {
        var items = new List<T>();
        var telemetryFiles = Directory.GetFiles(_testTelemetryPath, "telemetry_*.json");
        
        foreach (var file in telemetryFiles)
        {
            var content = await File.ReadAllTextAsync(file);
            var fileItems = JsonSerializer.Deserialize<List<T>>(content, SerializerOptions);
            if (fileItems != null)
            {
                items.AddRange(fileItems);
            }
        }
        
        return items;
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task TrackEvent_CreatesValidTelemetryFile()
    {
        // Arrange
        var eventName = "TestEvent";
        var properties = new Dictionary<string, string> { { "TestKey", "TestValue" } };

        // Act
        _service.TrackEvent(eventName, properties);
        await _service.FlushAsync();

        // Assert
        var events = await GetTelemetryItemsFromFiles<TelemetryEvent>();
        Assert.NotEmpty(events);
        var testEvent = Assert.Single(events);
        Assert.Equal(eventName, testEvent.Name);
        Assert.Equal("TestValue", testEvent.Properties?["TestKey"]);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task TrackMetric_CreatesValidTelemetryFile()
    {
        // Arrange
        var metricName = "TestMetric";
        var metricValue = 42.0;

        // Act
        _service.TrackMetric(metricName, metricValue);
        await _service.FlushAsync();

        // Assert
        var metrics = await GetTelemetryItemsFromFiles<TelemetryMetric>();
        Assert.NotEmpty(metrics);
        var testMetric = Assert.Single(metrics);
        Assert.Equal(metricName, testMetric.Name);
        Assert.Equal(metricValue, testMetric.Value);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task TrackException_IncludesExceptionDetails()
    {
        // Arrange
        var testException = new InvalidOperationException("Test exception");

        // Act
        _service.TrackException(testException);
        await _service.FlushAsync();

        // Assert
        var events = await GetTelemetryItemsFromFiles<TelemetryEvent>();
        Assert.NotEmpty(events);
        var exceptionEvent = Assert.Single(events);
        Assert.Equal("Exception", exceptionEvent.Name);
        Assert.Contains("Test exception", exceptionEvent.Properties?["Message"]);
        Assert.Equal("InvalidOperationException", exceptionEvent.Properties?["ExceptionType"]);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task TrackPerformance_RecordsCorrectDuration()
    {
        // Arrange
        var operation = "TestOperation";
        var duration = TimeSpan.FromMilliseconds(100);

        // Act
        _service.TrackPerformance(operation, duration);
        await _service.FlushAsync();

        // Assert
        var metrics = await GetTelemetryItemsFromFiles<TelemetryMetric>();
        Assert.NotEmpty(metrics);
        var performanceMetric = Assert.Single(metrics);
        Assert.Equal($"Performance.{operation}", performanceMetric.Name);
        Assert.Equal(100.0, performanceMetric.Value);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task BufferFlush_TriggersAtBufferSize()
    {
        // Arrange & Act
        var events = new List<string>();
        for (int i = 0; i < 101; i++) // Exceeds buffer size of 100
        {
            var eventName = $"Event{i}";
            events.Add(eventName);
            _service.TrackEvent(eventName);
        }

        // Wait for flush to complete
        await _service.FlushAsync();

        // Assert
        var telemetryEvents = await GetTelemetryItemsFromFiles<TelemetryEvent>();
        Assert.NotEmpty(telemetryEvents);
        Assert.Equal(101, telemetryEvents.Count);
        Assert.All(events, expectedEvent => 
            Assert.Contains(telemetryEvents, e => e.Name == expectedEvent));
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task SessionId_IsConsistentAcrossEvents()
    {
        // Arrange
        var events = new[] { "Event1", "Event2", "Event3" };

        // Act
        foreach (var eventName in events)
        {
            _service.TrackEvent(eventName);
        }
        await _service.FlushAsync();

        // Assert
        var telemetryEvents = await GetTelemetryItemsFromFiles<TelemetryEvent>();
        Assert.NotEmpty(telemetryEvents);
        
        var sessionIds = telemetryEvents.Select(e => e.SessionId).Distinct().ToList();
        Assert.Single(sessionIds);
        Assert.NotNull(sessionIds[0]);

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains(_testTelemetryPath)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.AtLeastOnce);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task Dispose_FlushesRemainingEvents()
    {
        // Arrange
        _loggerMock.Setup(x => x.IsEnabled(It.IsAny<LogLevel>())).Returns(true);
        var eventName = "FinalEvent";
        _service.TrackEvent(eventName);

        // Act - Explicitly flush and then dispose
        await _service.FlushAsync();
        await _service.DisposeAsync();

        // Assert
        var events = await GetTelemetryItemsFromFiles<TelemetryEvent>();
        Assert.NotEmpty(events);
        Assert.Contains(events, e => e.Name == eventName);

        // Verify we're using the correct path
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains(_testTelemetryPath)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.AtLeastOnce);
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            await _service.DisposeAsync();
            
            // Clean up test directory
            if (Directory.Exists(_testTelemetryPath))
            {
                Directory.Delete(_testTelemetryPath, true);
            }
        }
        catch (Exception ex)
        {
            LogCleanupError(_loggerMock.Object, _testTelemetryPath, ex);
        }
    }
} 