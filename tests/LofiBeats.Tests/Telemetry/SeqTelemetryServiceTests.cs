using Microsoft.Extensions.Logging;
using Moq;
using LofiBeats.Core.Telemetry;
using Xunit;

namespace LofiBeats.Tests.Telemetry;

[Collection("AI Generated Tests")]
public class SeqTelemetryServiceTests
{
    private readonly Mock<ILogger<SeqTelemetryService>> _loggerMock;
    private readonly TelemetryConfiguration _config;
    private readonly SeqTelemetryService _service;

    public SeqTelemetryServiceTests()
    {
        _loggerMock = new Mock<ILogger<SeqTelemetryService>>();
        _config = new TelemetryConfiguration
        {
            IsEnabled = true,
            SeqServerUrl = "http://localhost:5341",
            SeqApiKey = "test-api-key"
        };
        _service = new SeqTelemetryService(_loggerMock.Object, _config);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void Constructor_InitializesService()
    {
        // The constructor already ran in the test setup
        // Just verify the service was initialized properly
        Assert.NotNull(_service);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void TrackEvent_WhenTelemetryDisabled_DoesNotThrow()
    {
        // Arrange
        _config.IsEnabled = false;

        // Act & Assert (should not throw)
        _service.TrackEvent("TestEvent");
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void TrackEvent_WithProperties_DoesNotThrow()
    {
        // Arrange
        var properties = new Dictionary<string, string>
        {
            ["key1"] = "value1",
            ["key2"] = "value2"
        };

        // Act & Assert (should not throw)
        _service.TrackEvent("TestEvent", properties);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void TrackMetric_WithValidMetric_DoesNotThrow()
    {
        // Act & Assert (should not throw)
        _service.TrackMetric("TestMetric", 42.0);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void TrackException_WithException_DoesNotThrow()
    {
        // Arrange
        var exception = new Exception("Test exception");

        // Act & Assert (should not throw)
        _service.TrackException(exception);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void TrackPerformance_WithValidDuration_DoesNotThrow()
    {
        // Act & Assert (should not throw)
        _service.TrackPerformance("TestOperation", TimeSpan.FromMilliseconds(100));
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task FlushAsync_CompletesSuccessfully()
    {
        // Act & Assert (should not throw)
        await _service.FlushAsync();
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void Dispose_CanBeCalledMultipleTimes()
    {
        // Act & Assert (should not throw)
        _service.Dispose();
        _service.Dispose(); // Should handle multiple dispose calls gracefully
    }
} 