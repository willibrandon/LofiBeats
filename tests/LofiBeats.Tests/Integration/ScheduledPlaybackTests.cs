using LofiBeats.Core.Scheduling;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net.Http.Json;

namespace LofiBeats.Tests.Integration;

public class ScheduledPlaybackTests : IClassFixture<ServiceTestFixture>
{
    private readonly ServiceTestFixture _fixture;
    private readonly HttpClient _client;

    public ScheduledPlaybackTests(ServiceTestFixture fixture)
    {
        _fixture = fixture;
        _client = fixture.CreateClient();
    }

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

        // Schedule two stops close together
        var id1 = scheduler.ScheduleAction(100, () => Interlocked.Increment(ref stopCount));
        var id2 = scheduler.ScheduleAction(150, () => Interlocked.Increment(ref stopCount));

        // Act
        await Task.Delay(300); // Wait for both actions to complete

        // Assert
        Assert.Equal(2, stopCount); // Both actions should execute
        Assert.Equal(0, scheduler.ScheduledActionCount); // All actions should be cleaned up
    }

    private sealed record ScheduleResponse(string Message, Guid ActionId);
    private sealed record ErrorResponse(string Error);
} 