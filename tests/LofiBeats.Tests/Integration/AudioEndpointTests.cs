using System.Net;
using LofiBeats.Tests.Infrastructure;
using NAudio.Wave;
using Xunit.Abstractions;

namespace LofiBeats.Tests.Integration;

[Collection("Audio Tests")]
public class AudioEndpointTests(ITestOutputHelper output) : LofiBeatsTestBase(output)
{
    [SkippableFact]
    public async Task Play_WithBasicStyle_StartsPlayback()
    {
        Skip.If(!HasAudioCapabilities(), "No audio device available");

        // Act
        var response = await Client.PostAsync("/api/lofi/play?style=basic", null);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(PlaybackState.Playing, AudioService.GetPlaybackState());
        Assert.Equal("basic", AudioService.CurrentStyle);
    }

    [SkippableFact]
    public async Task Play_WithCrossfade_TransitionsSmootly()
    {
        Skip.If(!HasAudioCapabilities(), "No audio device available");

        // Arrange - Start with basic style
        await Client.PostAsync("/api/lofi/play?style=basic", null);
        Assert.Equal("basic", AudioService.CurrentStyle);

        // Act - Transition to jazz with crossfade
        var response = await Client.PostAsync("/api/lofi/play?style=jazz&transition=crossfade&xfadeDuration=2.0", null);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(PlaybackState.Playing, AudioService.GetPlaybackState());
        Assert.Equal("jazz", AudioService.CurrentStyle);
    }

    [SkippableFact]
    public async Task StopWithEffect_AppliesTapeStop()
    {
        Skip.If(!HasAudioCapabilities(), "No audio device available");

        // Arrange
        await Client.PostAsync("/api/lofi/play?style=basic", null);
        Assert.Equal(PlaybackState.Playing, AudioService.GetPlaybackState());

        // Act
        var response = await Client.PostAsync("/api/lofi/stop?tapestop=true", null);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        // Note: The actual stopping happens after the effect completes
        Assert.Equal(PlaybackState.Playing, AudioService.GetPlaybackState());
        
        // Wait for effect to complete with timeout
        var timeout = TimeSpan.FromSeconds(5);
        var pollInterval = TimeSpan.FromMilliseconds(100);
        var startTime = DateTime.UtcNow;

        while (DateTime.UtcNow - startTime < timeout)
        {
            if (AudioService.GetPlaybackState() == PlaybackState.Stopped)
            {
                break;
            }
            await Task.Delay(pollInterval);
        }

        Assert.Equal(PlaybackState.Stopped, AudioService.GetPlaybackState());
    }

    [SkippableFact]
    public async Task Effect_AddAndRemove_ModifiesPlayback()
    {
        Skip.If(!HasAudioCapabilities(), "No audio device available");

        // Arrange
        await Client.PostAsync("/api/lofi/play?style=basic", null);

        // Act - Add effect
        var addResponse = await Client.PostAsync("/api/lofi/effect?name=lowpass&enable=true", null);
        Assert.Equal(HttpStatusCode.OK, addResponse.StatusCode);

        // Let effect process for a moment
        await Task.Delay(100);

        // Act - Remove effect
        var removeResponse = await Client.PostAsync("/api/lofi/effect?name=lowpass&enable=false", null);
        Assert.Equal(HttpStatusCode.OK, removeResponse.StatusCode);
    }

    [SkippableFact]
    public async Task ScheduleStop_WithDelay_StopsAfterDelay()
    {
        Skip.If(!HasAudioCapabilities(), "No audio device available");

        // Arrange
        await Client.PostAsync("/api/lofi/play?style=basic", null);
        Assert.Equal(PlaybackState.Playing, AudioService.GetPlaybackState());

        // Act
        var response = await Client.PostAsync("/api/lofi/schedule-stop?delay=1s&tapeStop=false", null);

        // Assert initial state
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(PlaybackState.Playing, AudioService.GetPlaybackState());

        // Wait for scheduled stop
        await Task.Delay(1100);
        Assert.Equal(PlaybackState.Stopped, AudioService.GetPlaybackState());
    }

    [SkippableFact]
    public async Task SchedulePlay_WithDelay_StartsAfterDelay()
    {
        Skip.If(!HasAudioCapabilities(), "No audio device available");

        // Act
        var response = await Client.PostAsync("/api/lofi/schedule-play?style=jazz&delay=1s", null);

        // Assert initial state
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(PlaybackState.Stopped, AudioService.GetPlaybackState());

        // Wait for scheduled play with timeout
        var timeout = TimeSpan.FromSeconds(5);
        var pollInterval = TimeSpan.FromMilliseconds(100);
        var startTime = DateTime.UtcNow;

        var playbackStarted = false;
        var styleChanged = false;

        while (DateTime.UtcNow - startTime < timeout)
        {
            if (AudioService.GetPlaybackState() == PlaybackState.Playing)
            {
                playbackStarted = true;
            }
            if (AudioService.CurrentStyle == "jazz")
            {
                styleChanged = true;
            }
            if (playbackStarted && styleChanged)
            {
                break;
            }
            await Task.Delay(pollInterval);
        }

        Assert.Equal(PlaybackState.Playing, AudioService.GetPlaybackState());
        Assert.Equal("jazz", AudioService.CurrentStyle);
    }
} 