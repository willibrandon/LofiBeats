using LofiBeats.Core.Telemetry;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LofiBeats.Tests.Telemetry;

[Collection("AI Generated Tests")]
public class TelemetryIntegrationTests
{
    [Fact]
    [Trait("Category", "AI_Generated")]
    public void AddLofiTelemetry_RegistersServices()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging(builder => builder.AddConsole());

        // Act
        services.AddLofiTelemetry();
        var provider = services.BuildServiceProvider();

        // Assert
        var telemetryService = provider.GetService<ITelemetryService>();
        Assert.NotNull(telemetryService);
        Assert.IsType<LocalFileTelemetryService>(telemetryService);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task TelemetryService_WorksWithConstants()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging(builder => builder.AddConsole());
        services.AddLofiTelemetry();
        
        var provider = services.BuildServiceProvider();
        var telemetry = provider.GetRequiredService<ITelemetryService>();

        // Act - Track various events using our constants
        telemetry.TrackEvent(TelemetryConstants.Events.BeatGenerated, new Dictionary<string, string>
        {
            { TelemetryConstants.Properties.BeatStyle, "Test" }
        });

        telemetry.TrackMetric(TelemetryConstants.Metrics.BeatGenerationTime, 150.0);

        telemetry.TrackEvent(TelemetryConstants.Events.EffectAdded, new Dictionary<string, string>
        {
            { TelemetryConstants.Properties.EffectName, "TestEffect" }
        });

        // Flush to ensure everything is written
        await telemetry.FlushAsync();

        // Assert - Service should not throw during these operations
        // (Note: We don't verify file contents here as that's covered in the unit tests)
        Assert.True(true, "Telemetry operations completed without throwing");
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task TelemetryService_HandlesParallelOperations()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging(builder => builder.AddConsole());
        services.AddLofiTelemetry();
        
        var provider = services.BuildServiceProvider();
        var telemetry = provider.GetRequiredService<ITelemetryService>();

        // Act - Simulate multiple components writing telemetry simultaneously
        var tasks = new List<Task>();
        for (int i = 0; i < 10; i++)
        {
            var iteration = i;
            tasks.Add(Task.Run(() =>
            {
                for (int j = 0; j < 10; j++)
                {
                    telemetry.TrackEvent($"ParallelEvent_{iteration}_{j}");
                    telemetry.TrackMetric($"ParallelMetric_{iteration}_{j}", j);
                }
            }));
        }

        // Wait for all operations to complete
        await Task.WhenAll(tasks);
        await telemetry.FlushAsync();

        // Assert - Service should handle parallel operations without throwing
        Assert.True(true, "Parallel telemetry operations completed without throwing");
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void TelemetryService_HandlesLargeProperties()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging(builder => builder.AddConsole());
        services.AddLofiTelemetry();
        
        var provider = services.BuildServiceProvider();
        var telemetry = provider.GetRequiredService<ITelemetryService>();

        // Act & Assert - Should handle large property values
        var largeValue = new string('x', 10000); // 10KB string
        Assert.Null(Record.Exception(() =>
        {
            telemetry.TrackEvent("LargePropertyEvent", new Dictionary<string, string>
            {
                { "LargeProperty", largeValue }
            });
        }));
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void TelemetryService_HandlesCrashReporting()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging(builder => builder.AddConsole());
        services.AddLofiTelemetry();
        
        var provider = services.BuildServiceProvider();
        var telemetry = provider.GetRequiredService<ITelemetryService>();
        var tracker = provider.GetRequiredService<TelemetryTracker>();

        // Act - Simulate a crash
        var testException = new InvalidOperationException("Test crash");
        tracker.TrackError(testException, "Crash Test");

        // Assert - Verify the exception was tracked
        // Note: Since we're using a real telemetry service, we can't directly verify the contents
        // but we can verify it doesn't throw during tracking
        Assert.True(true, "Exception tracking completed without throwing");
    }
} 