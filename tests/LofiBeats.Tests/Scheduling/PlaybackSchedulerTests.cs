using LofiBeats.Core.Scheduling;
using Microsoft.Extensions.Logging;
using Moq;

namespace LofiBeats.Tests.Scheduling;

public class PlaybackSchedulerTests
{
    private readonly Mock<ILogger<PlaybackScheduler>> _loggerMock;
    private readonly PlaybackScheduler _scheduler;

    public PlaybackSchedulerTests()
    {
        _loggerMock = new Mock<ILogger<PlaybackScheduler>>();
        _scheduler = new PlaybackScheduler(_loggerMock.Object);
    }

    [Fact]
    public async Task ScheduleAction_ExecutesAfterDelay()
    {
        // Arrange
        var executed = false;
        var delay = 100; // 100ms delay

        // Act
        var id = _scheduler.ScheduleAction(delay, () => executed = true);
        
        // Assert - Should not be executed immediately
        Assert.False(executed);
        Assert.Equal(1, _scheduler.ScheduledActionCount);

        // Wait for execution
        await Task.Delay(delay + 50); // Add buffer time
        
        // Assert - Should be executed after delay
        Assert.True(executed);
        Assert.Equal(0, _scheduler.ScheduledActionCount); // Timer should be cleaned up
    }

    [Fact]
    public void CancelAction_CancelsScheduledAction()
    {
        // Arrange
        var executed = false;
        var id = _scheduler.ScheduleAction(1000, () => executed = true);

        // Act
        var cancelled = _scheduler.CancelAction(id);

        // Assert
        Assert.True(cancelled);
        Assert.False(executed); // Verify the action was not executed
        Assert.Equal(0, _scheduler.ScheduledActionCount);
    }

    [Fact]
    public void ScheduleAction_ThrowsOnNegativeDelay()
    {
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => 
            _scheduler.ScheduleAction(-1, () => { }));
    }

    [Fact]
    public void ScheduleAction_ThrowsOnNullCallback()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            _scheduler.ScheduleAction(100, null!));
    }

    [Fact]
    public void Dispose_CancelsAllScheduledActions()
    {
        // Arrange
        var executed1 = false;
        var executed2 = false;
        _scheduler.ScheduleAction(1000, () => executed1 = true);
        _scheduler.ScheduleAction(1000, () => executed2 = true);
        Assert.Equal(2, _scheduler.ScheduledActionCount);

        // Act
        _scheduler.Dispose();

        // Assert
        Assert.Equal(0, _scheduler.ScheduledActionCount);
        Assert.False(executed1);
        Assert.False(executed2);
    }
} 