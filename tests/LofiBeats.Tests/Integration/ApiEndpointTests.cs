using LofiBeats.Core.Models;
using LofiBeats.Tests.Infrastructure;
using NAudio.Wave;
using System.Net;
using System.Net.Http.Json;
using Xunit.Abstractions;

namespace LofiBeats.Tests.Integration;

[Collection("API Tests")]
public class ApiEndpointTests : LofiBeatsTestBase
{
    public ApiEndpointTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task HealthCheck_ReturnsHealthy()
    {
        // Act
        var response = await Client.GetAsync("/healthz");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Theory]
    [InlineData("basic")]
    [InlineData("jazz")]
    [InlineData("lofi")]
    public async Task Generate_WithValidStyle_ReturnsPattern(string style)
    {
        // Act
        var response = await Client.PostAsync($"/api/lofi/generate?style={style}", null);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadFromJsonAsync<GenerateResponse>();
        Assert.NotNull(content?.Pattern);
    }

    [Fact]
    public async Task Volume_WithInvalidLevel_ReturnsBadRequest()
    {
        // Act
        var response = await Client.PostAsync("/api/lofi/volume?level=2.0", null);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory]
    [InlineData(0.0f)]
    [InlineData(0.5f)]
    [InlineData(1.0f)]
    public async Task Volume_WithValidLevel_SetsVolume(float level)
    {
        // Act
        var response = await Client.PostAsync($"/api/lofi/volume?level={level}", null);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(level, AudioService.CurrentVolume);
    }

    [Fact]
    public async Task Stop_WhenPlaying_StopsPlayback()
    {
        // Arrange
        await Client.PostAsync("/api/lofi/play?style=basic", null);
        Assert.Equal(PlaybackState.Playing, AudioService.GetPlaybackState());

        // Act
        var response = await Client.PostAsync("/api/lofi/stop", null);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(PlaybackState.Stopped, AudioService.GetPlaybackState());
    }

    [Fact]
    public async Task Pause_WhenPlaying_PausesPlayback()
    {
        // Arrange
        await Client.PostAsync("/api/lofi/play?style=basic", null);
        Assert.Equal(PlaybackState.Playing, AudioService.GetPlaybackState());

        // Act
        var response = await Client.PostAsync("/api/lofi/pause", null);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(PlaybackState.Paused, AudioService.GetPlaybackState());
    }

    [Fact]
    public async Task Resume_WhenPaused_ResumesPlayback()
    {
        // Arrange
        await Client.PostAsync("/api/lofi/play?style=basic", null);
        await Client.PostAsync("/api/lofi/pause", null);
        Assert.Equal(PlaybackState.Paused, AudioService.GetPlaybackState());

        // Act
        var response = await Client.PostAsync("/api/lofi/resume", null);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(PlaybackState.Playing, AudioService.GetPlaybackState());
    }

    [Fact]
    public async Task Preset_GetAndApply_WorksCorrectly()
    {
        // Arrange - Set up initial state
        await Client.PostAsync("/api/lofi/play?style=jazz", null);
        await Client.PostAsync("/api/lofi/volume?level=0.8", null);

        // Act - Get current preset
        var getResponse = await Client.GetAsync("/api/lofi/preset/current");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        
        var preset = await getResponse.Content.ReadFromJsonAsync<Preset>();
        Assert.NotNull(preset);
        Assert.Equal("jazz", preset.Style);
        Assert.Equal(0.8f, preset.Volume);

        // Act - Apply a new preset
        var newPreset = new Preset
        {
            Name = "Test Preset",
            Style = "lofi",
            Volume = 0.5f,
            Effects = []
        };

        var applyResponse = await Client.PostAsJsonAsync("/api/lofi/preset/apply", newPreset);
        Assert.Equal(HttpStatusCode.OK, applyResponse.StatusCode);

        // Assert final state
        Assert.Equal("lofi", AudioService.CurrentStyle);
        Assert.Equal(0.5f, AudioService.CurrentVolume);
    }
}

public class GenerateResponse
{
    public string Message { get; set; } = "";

    public BeatPattern? Pattern { get; set; }
} 