using LofiBeats.Core.Telemetry;
using Moq;

namespace LofiBeats.Tests.Telemetry;

[Collection("AI Generated Tests")]
public class CompositeTelemetryServiceTests
{
    private readonly Mock<ITelemetryService> _service1Mock;
    private readonly Mock<ITelemetryService> _service2Mock;
    private readonly CompositeTelemetryService _compositeService;

    public CompositeTelemetryServiceTests()
    {
        _service1Mock = new Mock<ITelemetryService>();
        _service2Mock = new Mock<ITelemetryService>();
        _compositeService = new([_service1Mock.Object, _service2Mock.Object]);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void Constructor_ThrowsArgumentNullException_WhenServicesIsNull()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new CompositeTelemetryService(null!));
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void TrackEvent_DelegatesToAllServices()
    {
        // Arrange
        var eventName = "TestEvent";
        var properties = new Dictionary<string, string> { ["key"] = "value" };
        var timestamp = DateTimeOffset.UtcNow;

        // Act
        _compositeService.TrackEvent(eventName, properties, timestamp);

        // Assert
        _service1Mock.Verify(s => s.TrackEvent(eventName, properties, timestamp), Times.Once);
        _service2Mock.Verify(s => s.TrackEvent(eventName, properties, timestamp), Times.Once);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void TrackMetric_DelegatesToAllServices()
    {
        // Arrange
        var metricName = "TestMetric";
        var value = 42.0;
        var properties = new Dictionary<string, string> { ["key"] = "value" };

        // Act
        _compositeService.TrackMetric(metricName, value, properties);

        // Assert
        _service1Mock.Verify(s => s.TrackMetric(metricName, value, properties), Times.Once);
        _service2Mock.Verify(s => s.TrackMetric(metricName, value, properties), Times.Once);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void TrackException_DelegatesToAllServices()
    {
        // Arrange
        var exception = new Exception("Test exception");
        var properties = new Dictionary<string, string> { ["key"] = "value" };

        // Act
        _compositeService.TrackException(exception, properties);

        // Assert
        _service1Mock.Verify(s => s.TrackException(exception, properties), Times.Once);
        _service2Mock.Verify(s => s.TrackException(exception, properties), Times.Once);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void TrackPerformance_DelegatesToAllServices()
    {
        // Arrange
        var operation = "TestOperation";
        var duration = TimeSpan.FromSeconds(1);
        var properties = new Dictionary<string, string> { ["key"] = "value" };

        // Act
        _compositeService.TrackPerformance(operation, duration, properties);

        // Assert
        _service1Mock.Verify(s => s.TrackPerformance(operation, duration, properties), Times.Once);
        _service2Mock.Verify(s => s.TrackPerformance(operation, duration, properties), Times.Once);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task FlushAsync_DelegatesToAllServices()
    {
        // Act
        await _compositeService.FlushAsync();

        // Assert
        _service1Mock.Verify(s => s.FlushAsync(), Times.Once);
        _service2Mock.Verify(s => s.FlushAsync(), Times.Once);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task DisposeAsync_DisposesAsyncDisposableServices()
    {
        // Arrange
        var asyncDisposableMock = new Mock<ITelemetryService>();
        asyncDisposableMock.As<IAsyncDisposable>();
        
        var service = new CompositeTelemetryService([asyncDisposableMock.Object]);

        // Act
        await service.DisposeAsync();

        // Assert
        asyncDisposableMock.As<IAsyncDisposable>().Verify(d => d.DisposeAsync(), Times.Once);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task DisposeAsync_DisposesDisposableServices()
    {
        // Arrange
        var disposableMock = new Mock<ITelemetryService>();
        disposableMock.As<IDisposable>();
        
        var service = new CompositeTelemetryService([disposableMock.Object]);

        // Act
        await service.DisposeAsync();

        // Assert
        disposableMock.As<IDisposable>().Verify(d => d.Dispose(), Times.Once);
    }
} 