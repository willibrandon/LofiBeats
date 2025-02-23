using LofiBeats.Core.Scheduling;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net.Http.Json;

namespace LofiBeats.Tests.Integration;

public class ScheduledPlaybackTests(ServiceTestFixture fixture) : IClassFixture<ServiceTestFixture>
{
    private readonly ServiceTestFixture _fixture = fixture;
    private readonly HttpClient _client = fixture.CreateClient();

    [Fact]
    public async Task ScheduleStop_WithValidDelay_ReturnsActionId()
    {
        // Arrange
        var delay = "5s";
        var tapeStop = true;

        // Act
        var response = await _client.PostAsync(
            $"/api/lofi/schedule-stop?tapeStop={tapeStop}&delay={delay}",
            null);

        // Assert
        Assert.True(response.IsSuccessStatusCode);
        var result = await response.Content.ReadFromJsonAsync<ScheduleResponse>();
        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.ActionId);
        Assert.Contains("Scheduled stop in", result.Message);
    }

    [Fact]
    public async Task ScheduleStop_WithInvalidDelay_ReturnsBadRequest()
    {
        // Arrange
        var delay = "invalid";
        var tapeStop = true;

        // Act
        var response = await _client.PostAsync(
            $"/api/lofi/schedule-stop?tapeStop={tapeStop}&delay={delay}",
            null);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.NotNull(error);
        Assert.Contains("Invalid delay format", error.Error);
    }

    [Fact]
    public async Task SchedulePlay_WithValidDelay_ReturnsActionId()
    {
        // Arrange
        var delay = "5s";
        var style = "chillhop";

        // Act
        var response = await _client.PostAsync(
            $"/api/lofi/schedule-play?style={style}&delay={delay}",
            null);

        // Assert
        Assert.True(response.IsSuccessStatusCode);
        var result = await response.Content.ReadFromJsonAsync<ScheduleResponse>();
        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.ActionId);
        Assert.Contains("Scheduled chillhop beat to play in", result.Message);
    }

    [Fact]
    public async Task SchedulePlay_WithInvalidDelay_ReturnsBadRequest()
    {
        // Arrange
        var delay = "invalid";
        var style = "chillhop";

        // Act
        var response = await _client.PostAsync(
            $"/api/lofi/schedule-play?style={style}&delay={delay}",
            null);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.NotNull(error);
        Assert.Contains("Invalid delay format", error.Error);
    }

    [Fact]
    public async Task ScheduleMultipleStops_OnlyFirstOneStopsPlayback()
    {
        // Arrange
        var scheduler = new PlaybackScheduler(Mock.Of<ILogger<PlaybackScheduler>>());
        var stopCount = 0;
        var firstStopExecuted = new TaskCompletionSource();
        var secondStopAttempted = new TaskCompletionSource();

        // Schedule two stops with callbacks to track execution
        var id1 = scheduler.ScheduleStopAction(200, () => 
        {
            Interlocked.Increment(ref stopCount);
            firstStopExecuted.SetResult();
        }, "First stop");

        var id2 = scheduler.ScheduleStopAction(300, () => 
        {
            secondStopAttempted.SetResult();
            Interlocked.Increment(ref stopCount);
        }, "Second stop");

        // Act
        // Wait for the first stop to execute and a reasonable time for the second to attempt
        await Task.WhenAny(
            firstStopExecuted.Task.WaitAsync(TimeSpan.FromSeconds(2)),
            secondStopAttempted.Task.WaitAsync(TimeSpan.FromSeconds(2))
        );
        await Task.Delay(100); // Small additional delay to ensure cleanup

        // Assert
        Assert.Equal(1, stopCount); // Only the first stop should execute
        Assert.Equal(0, scheduler.ScheduledActionCount); // All actions should be cleaned up
    }

    private sealed record ScheduleResponse(string Message, Guid ActionId);
    private sealed record ErrorResponse(string Error);
} 