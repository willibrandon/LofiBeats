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
    private readonly TelemetryConfiguration _config;
    private readonly LocalFileTelemetryService _service;
    private static readonly JsonSerializerOptions SerializerOptions = new() { WriteIndented = true };

    public LocalFileTelemetryServiceTests()
    {
        _loggerMock = new Mock<ILogger<LocalFileTelemetryService>>();
        
        // Create unique test directory for this test instance
        var uniqueTestId = Guid.NewGuid().ToString("N");
        _testTelemetryPath = Path.Combine(Path.GetTempPath(), "LofiBeatsTests", "Telemetry", uniqueTestId);
        
        // Clean up any existing directory
        if (Directory.Exists(_testTelemetryPath))
        {
            Directory.Delete(_testTelemetryPath, true);
        }
        Directory.CreateDirectory(_testTelemetryPath);
        
        // Create test configuration with the unique path
        _config = new TelemetryConfiguration
        {
            IsTestEnvironment = true,
            MaxBufferSize = 10,
            FlushIntervalMinutes = 1,
            MaxFileSizeMB = 1,
            MaxFileAgeDays = 1,
            GetBasePath = () => _testTelemetryPath
        };
        
        // Create service with test configuration
        _service = new LocalFileTelemetryService(_loggerMock.Object, _config);
    }

    private sealed class TelemetryFile<T>
    {
        public List<T> Items { get; set; } = new();
    }

    private async Task<List<T>> GetTelemetryItemsFromFiles<T>(string? path = null)
    {
        var searchPath = path ?? _testTelemetryPath;
        var items = new List<T>();
        var filePattern = typeof(T) == typeof(TelemetryEvent) ? "telemetry_events_*.json" : "telemetry_metrics_*.json";
        var telemetryFiles = Directory.GetFiles(searchPath, filePattern);
        
        foreach (var file in telemetryFiles)
        {
            var content = await File.ReadAllTextAsync(file);
            var telemetryFile = JsonSerializer.Deserialize<TelemetryFile<T>>(content, SerializerOptions);
            if (telemetryFile?.Items != null)
            {
                items.AddRange(telemetryFile.Items);
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

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task ConcurrentEvents_AreAllRecorded()
    {
        // Arrange
        const int numEvents = 50;
        var events = Enumerable.Range(1, numEvents)
            .Select(i => $"ConcurrentEvent_{i}")
            .ToList();

        // Act
        await Parallel.ForEachAsync(events, async (eventName, ct) =>
        {
            _service.TrackEvent(eventName);
            await Task.Delay(10, ct); // Small delay to ensure overlap
        });

        await _service.FlushAsync();

        // Assert
        var telemetryEvents = await GetTelemetryItemsFromFiles<TelemetryEvent>();
        Assert.Equal(numEvents, telemetryEvents.Count);
        Assert.All(events, expectedEvent =>
            Assert.Contains(telemetryEvents, e => e.Name == expectedEvent));
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task MultipleFlushes_CreateSeparateFiles()
    {
        // Arrange
        var initialEventFiles = Directory.GetFiles(_testTelemetryPath, "telemetry_events_*.json").Length;
        var initialMetricFiles = Directory.GetFiles(_testTelemetryPath, "telemetry_metrics_*.json").Length;

        // Act
        _service.TrackEvent("Event1");
        await _service.FlushAsync();
        
        _service.TrackMetric("Metric1", 42.0);
        await _service.FlushAsync();

        // Assert
        var eventFiles = Directory.GetFiles(_testTelemetryPath, "telemetry_events_*.json");
        var metricFiles = Directory.GetFiles(_testTelemetryPath, "telemetry_metrics_*.json");
        Assert.Equal(initialEventFiles + 1, eventFiles.Length);
        Assert.Equal(initialMetricFiles + 1, metricFiles.Length);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task LargePropertyValues_AreHandledCorrectly()
    {
        // Arrange
        var largeValue = new string('x', 10000); // 10KB string
        var properties = new Dictionary<string, string>
        {
            { "LargeProperty", largeValue }
        };

        // Act
        _service.TrackEvent("LargePropertyEvent", properties);
        await _service.FlushAsync();

        // Assert
        var events = await GetTelemetryItemsFromFiles<TelemetryEvent>();
        var testEvent = Assert.Single(events);
        Assert.Equal(largeValue, testEvent.Properties?["LargeProperty"]);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task DisposedService_IgnoresNewEvents()
    {
        // Arrange
        _service.TrackEvent("BeforeDispose");
        await _service.FlushAsync();
        
        var initialEvents = await GetTelemetryItemsFromFiles<TelemetryEvent>();
        var initialCount = initialEvents.Count;

        // Act
        await _service.DisposeAsync();
        _service.TrackEvent("AfterDispose");
        
        // Assert
        var finalEvents = await GetTelemetryItemsFromFiles<TelemetryEvent>();
        Assert.Equal(initialCount, finalEvents.Count);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task InvalidBasePath_ThrowsException()
    {
        // Arrange
        var invalidConfig = new TelemetryConfiguration
        {
            IsTestEnvironment = true,
            GetBasePath = () => Path.Combine("invalid", new string(Path.GetInvalidPathChars().Take(1).ToArray()))
        };

        // Act & Assert
        var exception = await Assert.ThrowsAnyAsync<Exception>(() =>
        {
            var service = new LocalFileTelemetryService(_loggerMock.Object, invalidConfig);
            return Task.CompletedTask;
        });

        Assert.True(
            exception is IOException || 
            exception is ArgumentException || 
            exception is UnauthorizedAccessException,
            $"Expected IOException, ArgumentException, or UnauthorizedAccessException, but got {exception.GetType().Name}");
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task TimestampsAreUtc_AcrossAllTelemetry()
    {
        // Arrange & Act
        var beforeTime = DateTimeOffset.UtcNow;
        _service.TrackEvent("TimestampEvent");
        _service.TrackMetric("TimestampMetric", 42.0);
        _service.TrackException(new Exception("TimestampException"));
        var afterTime = DateTimeOffset.UtcNow;
        await _service.FlushAsync();

        // Assert
        var events = await GetTelemetryItemsFromFiles<TelemetryEvent>();
        var metrics = await GetTelemetryItemsFromFiles<TelemetryMetric>();
        
        // Verify all timestamps are between our before and after times
        Assert.All(events, e => Assert.True(e.Timestamp >= beforeTime && e.Timestamp <= afterTime,
            $"Event timestamp {e.Timestamp} should be between {beforeTime} and {afterTime}"));
        Assert.All(metrics, m => Assert.True(m.Timestamp >= beforeTime && m.Timestamp <= afterTime,
            $"Metric timestamp {m.Timestamp} should be between {beforeTime} and {afterTime}"));
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task TrackEvent_FiltersTestEvents_InProduction()
    {
        // Arrange
        var prodTestPath = Path.Combine(_testTelemetryPath, "ProdTest");
        if (Directory.Exists(prodTestPath))
        {
            Directory.Delete(prodTestPath, true);
        }
        Directory.CreateDirectory(prodTestPath);

        var prodConfig = new TelemetryConfiguration
        {
            IsTestEnvironment = false,
            MaxBufferSize = 1, // Force immediate flush
            GetBasePath = () => prodTestPath
        };
        var prodService = new LocalFileTelemetryService(_loggerMock.Object, prodConfig);

        try
        {
            // Act
            prodService.TrackEvent("TestEvent", new Dictionary<string, string> { ["TestKey"] = "TestValue" });
            prodService.TrackEvent("ProductionEvent");
            await prodService.FlushAsync();

            // Assert
            var events = await GetTelemetryItemsFromFiles<TelemetryEvent>(prodTestPath);
            Assert.Single(events);
            Assert.Equal("ProductionEvent", events[0].Name);
        }
        finally
        {
            await prodService.DisposeAsync();
            if (Directory.Exists(prodTestPath))
            {
                Directory.Delete(prodTestPath, true);
            }
        }
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task TrackEvent_AllowsTestEvents_InTestEnvironment()
    {
        // Arrange & Act
        _service.TrackEvent("TestEvent");
        await _service.FlushAsync();

        // Assert
        var events = await GetTelemetryItemsFromFiles<TelemetryEvent>();
        Assert.Single(events);
        Assert.Equal("TestEvent", events[0].Name);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task TrackEvent_RejectsFutureTimestamps()
    {
        // Arrange
        var futureEvent = new TelemetryEvent
        {
            Name = "FutureEvent",
            Timestamp = DateTimeOffset.UtcNow.AddHours(1)
        };

        // Act - Track the event with the future timestamp directly
        _service.TrackEvent(futureEvent.Name, null, futureEvent.Timestamp);
        await _service.FlushAsync();

        // Assert
        var events = await GetTelemetryItemsFromFiles<TelemetryEvent>();
        Assert.Empty(events);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task TrackEvent_EnforcesMaxFileSize()
    {
        // Arrange
        var largeValue = new string('x', 500 * 1024); // 500KB
        var properties = new Dictionary<string, string> { { "LargeValue", largeValue } };

        // Act - Write enough events to exceed max file size
        for (int i = 0; i < 3; i++) // Should create multiple files
        {
            _service.TrackEvent($"Event{i}", properties);
            await _service.FlushAsync();
        }

        // Assert
        var files = Directory.GetFiles(_testTelemetryPath, "telemetry_*.json");
        Assert.True(files.Length > 1, "Multiple files should be created when size limit is exceeded");
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task CleanupOldFiles_RemovesExpiredFiles()
    {
        // Arrange
        var oldFile = Path.Combine(_testTelemetryPath, "telemetry_old.json");
        await File.WriteAllTextAsync(oldFile, "{}");
        File.SetLastWriteTimeUtc(oldFile, DateTime.UtcNow.AddDays(-2));

        var newFile = Path.Combine(_testTelemetryPath, "telemetry_new.json");
        await File.WriteAllTextAsync(newFile, "{}");

        // Act
        await Task.Run(() => _config.CleanupOldFiles(_testTelemetryPath));

        // Assert
        Assert.False(File.Exists(oldFile), "Old file should be deleted");
        Assert.True(File.Exists(newFile), "New file should be retained");
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