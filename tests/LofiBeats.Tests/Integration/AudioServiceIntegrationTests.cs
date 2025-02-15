using LofiBeats.Core.Effects;
using LofiBeats.Core.Playback;
using LofiBeats.Service;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NAudio.Wave;
using System.Net;
using System.Net.Http.Json;

namespace LofiBeats.Tests.Integration;

[Collection("AI Generated Tests")]
public class AudioServiceIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public AudioServiceIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Replace the real AudioPlaybackService with our test version
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IAudioPlaybackService));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }
                services.AddSingleton<IAudioPlaybackService, TestAudioPlaybackService>();
            });
        });
        _client = _factory.CreateClient();
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task PlaybackLifecycle_Success()
    {
        // Start playback
        var playResponse = await _client.PostAsync("/api/lofi/play", null);
        Assert.Equal(HttpStatusCode.OK, playResponse.StatusCode);
        var playResult = await playResponse.Content.ReadFromJsonAsync<PlaybackResponse>();
        Assert.NotNull(playResult?.Pattern);

        // Pause playback
        var pauseResponse = await _client.PostAsync("/api/lofi/pause", null);
        Assert.Equal(HttpStatusCode.OK, pauseResponse.StatusCode);
        var pauseResult = await pauseResponse.Content.ReadFromJsonAsync<ApiResponse>();
        Assert.Equal("Playback paused", pauseResult?.Message);

        // Try to pause again (should fail)
        var pauseAgainResponse = await _client.PostAsync("/api/lofi/pause", null);
        Assert.Equal(HttpStatusCode.BadRequest, pauseAgainResponse.StatusCode);

        // Resume playback
        var resumeResponse = await _client.PostAsync("/api/lofi/resume", null);
        Assert.Equal(HttpStatusCode.OK, resumeResponse.StatusCode);
        var resumeResult = await resumeResponse.Content.ReadFromJsonAsync<ApiResponse>();
        Assert.Equal("Playback resumed", resumeResult?.Message);

        // Try to resume again (should fail)
        var resumeAgainResponse = await _client.PostAsync("/api/lofi/resume", null);
        Assert.Equal(HttpStatusCode.BadRequest, resumeAgainResponse.StatusCode);

        // Stop playback
        var stopResponse = await _client.PostAsync("/api/lofi/stop", null);
        Assert.Equal(HttpStatusCode.OK, stopResponse.StatusCode);
        var stopResult = await stopResponse.Content.ReadFromJsonAsync<ApiResponse>();
        Assert.Equal("Playback stopped", stopResult?.Message);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task PauseWithoutPlaying_ReturnsBadRequest()
    {
        var response = await _client.PostAsync("/api/lofi/pause", null);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
        Assert.Equal("No active playback to pause", result?.Error);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task ResumeWithoutPausing_ReturnsBadRequest()
    {
        // First start playback to get into a playing state
        await _client.PostAsync("/api/lofi/play", null);
        
        var response = await _client.PostAsync("/api/lofi/resume", null);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
        Assert.Equal("Playback is not paused", result?.Error);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task ResumeAfterPlayAndPause_ShouldWork()
    {
        var testService = _factory.Services.GetRequiredService<IAudioPlaybackService>();
        
        // Initial state should be Stopped
        Assert.Equal(PlaybackState.Stopped, testService.GetPlaybackState());
        Assert.Null(testService.CurrentSource);

        // 1. Start playback
        var playResponse = await _client.PostAsync("/api/lofi/play", null);
        Assert.Equal(HttpStatusCode.OK, playResponse.StatusCode);
        Assert.Equal(PlaybackState.Playing, testService.GetPlaybackState());
        Assert.NotNull(testService.CurrentSource);

        // 2. Pause playback
        var pauseResponse = await _client.PostAsync("/api/lofi/pause", null);
        Assert.Equal(HttpStatusCode.OK, pauseResponse.StatusCode);
        Assert.Equal(PlaybackState.Paused, testService.GetPlaybackState());
        Assert.NotNull(testService.CurrentSource);

        // 3. Resume playback
        var resumeResponse = await _client.PostAsync("/api/lofi/resume", null);
        Assert.Equal(HttpStatusCode.OK, resumeResponse.StatusCode);
        var result = await resumeResponse.Content.ReadFromJsonAsync<ApiResponse>();
        Assert.Equal("Playback resumed", result?.Message);
        Assert.Equal(PlaybackState.Playing, testService.GetPlaybackState());
        Assert.NotNull(testService.CurrentSource);
    }

    private record ApiResponse
    {
        public string? Message { get; init; }
        public string? Error { get; init; }
    }

    private sealed record PlaybackResponse : ApiResponse
    {
        public object? Pattern { get; init; }
    }
}

// Test implementation of IAudioPlaybackService that doesn't actually play audio
public class TestAudioPlaybackService : IAudioPlaybackService
{
    private PlaybackState _state = PlaybackState.Stopped;
    private ISampleProvider? _currentSource;
    private SerialEffectChain? _effectChain;
    private float _volume = 1.0f;
    private readonly Mock<ILoggerFactory> _loggerFactoryMock;

    public TestAudioPlaybackService()
    {
        _loggerFactoryMock = new Mock<ILoggerFactory>();
        _loggerFactoryMock.Setup(x => x.CreateLogger(It.IsAny<string>()))
            .Returns(new Mock<ILogger>().Object);
    }

    public ISampleProvider? CurrentSource => _currentSource;

    public void AddEffect(IAudioEffect effect)
    {
        if (_effectChain == null && _currentSource != null)
        {
            _effectChain = new SerialEffectChain(
                _currentSource, 
                new Mock<ILogger<SerialEffectChain>>().Object,
                _loggerFactoryMock.Object);
        }
        if (_effectChain != null)
        {
            _effectChain.AddEffect(effect);
        }
    }

    public void RemoveEffect(string effectName)
    {
        _effectChain?.RemoveEffect(effectName);
    }

    public void SetSource(ISampleProvider source)
    {
        _currentSource = source;
        if (_effectChain != null)
        {
            _effectChain = new SerialEffectChain(
                _currentSource, 
                new Mock<ILogger<SerialEffectChain>>().Object,
                _loggerFactoryMock.Object);
        }
    }

    public void SetVolume(float volume) => _volume = volume;

    public void StartPlayback()
    {
        _state = PlaybackState.Playing;
    }

    public void StopPlayback()
    {
        _state = PlaybackState.Stopped;
        _currentSource = null;
        _effectChain = null;
    }

    public void StopWithEffect(IAudioEffect effect)
    {
        // In test implementation, just stop immediately
        StopPlayback();
    }

    public void PausePlayback()
    {
        if (_state == PlaybackState.Playing)
            _state = PlaybackState.Paused;
    }

    public void ResumePlayback()
    {
        if (_state == PlaybackState.Paused)
            _state = PlaybackState.Playing;
    }

    public PlaybackState GetPlaybackState() => _state;
} 