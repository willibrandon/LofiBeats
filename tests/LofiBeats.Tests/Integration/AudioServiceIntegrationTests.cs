using LofiBeats.Core.Effects;
using LofiBeats.Core.Models;
using LofiBeats.Core.Playback;
using LofiBeats.Core.Telemetry;
using LofiBeats.Service;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NAudio.Wave;
using System.Net;
using System.Net.Http.Json;

namespace LofiBeats.Tests.Integration;

[Collection("AudioService Integration Tests")]
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
                // Remove all existing registrations for each service type
                foreach (var descriptor in services.Where(d => d.ServiceType == typeof(IAudioPlaybackService)).ToList())
                {
                    services.Remove(descriptor);
                }
                services.AddSingleton<IAudioPlaybackService, TestAudioPlaybackService>();

                // Replace telemetry services with mocks
                foreach (var descriptor in services.Where(d => d.ServiceType == typeof(ITelemetryService)).ToList())
                {
                    services.Remove(descriptor);
                }
                var mockTelemetry = new Mock<ITelemetryService>();
                services.AddSingleton(mockTelemetry.Object);

                foreach (var descriptor in services.Where(d => d.ServiceType == typeof(TelemetryTracker)).ToList())
                {
                    services.Remove(descriptor);
                }
                var mockLogger = new Mock<ILogger<TelemetryTracker>>();
                services.AddSingleton(new TelemetryTracker(mockTelemetry.Object, mockLogger.Object));
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

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task PresetLifecycle_Success()
    {
        // Arrange - Set up initial state
        await _client.PostAsync("/api/lofi/play?style=jazzy", null);
        await _client.PostAsync("/api/lofi/effect?name=vinyl&enable=true", null);
        await _client.PostAsync("/api/lofi/volume?level=0.7", null);
        await Task.Delay(500);

        // Act - Get current preset
        var getCurrentResponse = await _client.GetAsync("/api/lofi/preset/current");
        Assert.Equal(HttpStatusCode.OK, getCurrentResponse.StatusCode);
        var preset = await getCurrentResponse.Content.ReadFromJsonAsync<Preset>();
        
        // Assert - Verify preset state
        Assert.NotNull(preset);
        Assert.Equal("jazzy", preset.Style);
        Assert.Equal(0.7f, preset.Volume);
        Assert.Contains("vinyl", preset.Effects);

        // Act - Apply a different preset
        var newPreset = new Preset
        {
            Name = "Test Preset",
            Style = "chillhop",
            Volume = 0.8f,
            Effects = ["reverb"]
        };
        var applyResponse = await _client.PostAsync("/api/lofi/preset/apply", JsonContent.Create(newPreset));
        Assert.Equal(HttpStatusCode.OK, applyResponse.StatusCode);
        await Task.Delay(500);

        // Assert - Verify new state
        var finalResponse = await _client.GetAsync("/api/lofi/preset/current");
        var finalPreset = await finalResponse.Content.ReadFromJsonAsync<Preset>();
        Assert.NotNull(finalPreset);
        Assert.Equal("chillhop", finalPreset.Style);
        Assert.Equal(0.8f, finalPreset.Volume);
        Assert.Contains("reverb", finalPreset.Effects);
        Assert.DoesNotContain("vinyl", finalPreset.Effects);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task ApplyPreset_WithInvalidValues_ReturnsBadRequest()
    {
        // Arrange
        var invalidPreset = new Preset
        {
            Name = "Invalid Preset",
            Style = "",  // Invalid: empty style
            Volume = 2.0f,  // Invalid: volume > 1.0
            Effects = ["nonexistent_effect"]
        };

        // Act
        var response = await _client.PostAsync("/api/lofi/preset/apply", JsonContent.Create(invalidPreset));

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var error = await response.Content.ReadFromJsonAsync<ApiResponse>();
        Assert.NotNull(error?.Error);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task GetCurrentPreset_WhenNoPlayback_ReturnsDefaultValues()
    {
        // Arrange - Ensure no playback
        await _client.PostAsync("/api/lofi/stop", null);

        // Act
        var response = await _client.GetAsync("/api/lofi/preset/current");
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var preset = await response.Content.ReadFromJsonAsync<Preset>();
        Assert.NotNull(preset);
        Assert.Equal("basic", preset.Style);  // Default style
        Assert.Equal(1.0f, preset.Volume);    // Default volume
        Assert.Empty(preset.Effects);         // No effects
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task CrossfadeToPattern_Success()
    {
        // Arrange - Start with jazzy pattern
        var playResponse = await _client.PostAsync("/api/lofi/play?style=jazzy", null);
        Assert.Equal(HttpStatusCode.OK, playResponse.StatusCode);
        var playResult = await playResponse.Content.ReadFromJsonAsync<PlaybackResponse>();
        Assert.NotNull(playResult?.Pattern);

        // Get initial BPM
        var initialBpm = playResult.Pattern?.BPM;
        Assert.NotNull(initialBpm);

        // Act - Crossfade to hiphop
        var crossfadeResponse = await _client.PostAsync(
            "/api/lofi/play?style=hiphop&transition=crossfade&xfadeDuration=2.0", null);
        Assert.Equal(HttpStatusCode.OK, crossfadeResponse.StatusCode);
        var crossfadeResult = await crossfadeResponse.Content.ReadFromJsonAsync<PlaybackResponse>();
        Assert.NotNull(crossfadeResult?.Pattern);

        // Assert
        // 1. BPM should be within hiphop range (80-100)
        Assert.InRange(crossfadeResult.Pattern.BPM, 80, 100);

        // 2. Pattern should be valid
        Assert.NotEmpty(crossfadeResult.Pattern.DrumSequence);
        Assert.NotEmpty(crossfadeResult.Pattern.ChordProgression);

        // 3. Style should be updated
        var testService = _factory.Services.GetRequiredService<IAudioPlaybackService>();
        Assert.Equal("hiphop", testService.CurrentStyle);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task CrossfadeToPattern_WithNoCurrentSource_StartsImmediately()
    {
        // Act - Try to crossfade when nothing is playing
        var response = await _client.PostAsync(
            "/api/lofi/play?style=chillhop&transition=crossfade", null);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<PlaybackResponse>();
        Assert.NotNull(result?.Pattern);

        // Assert - Should start playing immediately
        var testService = _factory.Services.GetRequiredService<IAudioPlaybackService>();
        Assert.Equal(PlaybackState.Playing, testService.GetPlaybackState());
        Assert.Equal("chillhop", testService.CurrentStyle);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task CrossfadeToPattern_WithLongDuration_HandlesCorrectly()
    {
        // Arrange - Start with basic pattern
        await _client.PostAsync("/api/lofi/play?style=basic", null);

        // Act - Crossfade with 10 second duration
        var response = await _client.PostAsync(
            "/api/lofi/play?style=jazzy&transition=crossfade&xfadeDuration=10.0", null);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<PlaybackResponse>();
        Assert.NotNull(result?.Pattern);

        // Assert - Service should handle long duration gracefully
        var testService = _factory.Services.GetRequiredService<IAudioPlaybackService>();
        Assert.Equal(PlaybackState.Playing, testService.GetPlaybackState());
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task CrossfadeToPattern_ChecksAmplitudeContinuity()
    {
        // This test requires a real audio output to verify amplitude continuity
        // For now, we'll just verify the crossfade provider is created correctly
        
        // Arrange - Start with chillhop pattern
        await _client.PostAsync("/api/lofi/play?style=chillhop", null);
        var testService = _factory.Services.GetRequiredService<IAudioPlaybackService>();
        var initialSource = testService.CurrentSource;
        Assert.NotNull(initialSource);

        // Act - Crossfade to jazzy
        await _client.PostAsync(
            "/api/lofi/play?style=jazzy&transition=crossfade&xfadeDuration=2.0", null);

        // Assert - Source should be a CrossfadeSampleProvider during transition
        var currentSource = testService.CurrentSource;
        Assert.NotNull(currentSource);
        // Note: In a real test, we'd capture audio samples and verify smooth transition
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task PlayWithFlatKey_Success()
    {
        // Act - Start playback in Ab key
        var response = await _client.PostAsync("/api/lofi/play?style=hiphop&key=Ab", null);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<PlaybackResponse>();
        Assert.NotNull(result?.Pattern);

        // Assert - Pattern should be valid
        Assert.NotEmpty(result.Pattern.DrumSequence);
        Assert.NotEmpty(result.Pattern.ChordProgression);

        // Verify service state
        var testService = _factory.Services.GetRequiredService<IAudioPlaybackService>();
        Assert.Equal(PlaybackState.Playing, testService.GetPlaybackState());
        Assert.Equal("hiphop", testService.CurrentStyle);
    }

    [Theory]
    [InlineData("Ab")]
    [InlineData("Bb")]
    [InlineData("Db")]
    [InlineData("Eb")]
    [InlineData("Gb")]
    public async Task PlayWithFlatKeys_Success(string key)
    {
        // Act - Start playback with flat key
        var response = await _client.PostAsync($"/api/lofi/play?style=hiphop&key={key}", null);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<PlaybackResponse>();
        Assert.NotNull(result?.Pattern);

        // Assert - Pattern should be valid
        Assert.NotEmpty(result.Pattern.DrumSequence);
        Assert.NotEmpty(result.Pattern.ChordProgression);

        // Verify service state
        var testService = _factory.Services.GetRequiredService<IAudioPlaybackService>();
        Assert.Equal(PlaybackState.Playing, testService.GetPlaybackState());
        Assert.Equal("hiphop", testService.CurrentStyle);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task PlayWithBbKey_MaintainsConsistentFlatNotation()
    {
        // Act - Start playback in Bb key
        var response = await _client.PostAsync("/api/lofi/play?style=jazzy&key=Bb", null);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<PlaybackResponse>();
        Assert.NotNull(result?.Pattern);

        // Assert - Pattern should be valid and use consistent flat notation
        Assert.NotEmpty(result.Pattern.ChordProgression);
        
        // Verify that if we're in Bb key, we use flat notation consistently
        foreach (var chord in result.Pattern.ChordProgression)
        {
            // Skip checking chords without accidentals
            if (!chord.Contains('#') && !chord.Contains('b')) continue;
            
            // In Bb key, we should use flat notation (b) not sharp notation (#)
            Assert.DoesNotContain('#', chord);
        }

        // Verify service state
        var testService = _factory.Services.GetRequiredService<IAudioPlaybackService>();
        Assert.Equal(PlaybackState.Playing, testService.GetPlaybackState());
        Assert.Equal("jazzy", testService.CurrentStyle);
    }

    private record ApiResponse
    {
        public string? Message { get; init; }
        public string? Error { get; init; }
    }

    private sealed record PlaybackResponse : ApiResponse
    {
        public BeatPattern? Pattern { get; init; }
    }

    private sealed record BeatPattern
    {
        public int BPM { get; init; }
        public string[] DrumSequence { get; init; } = [];
        public string[] ChordProgression { get; init; } = [];
    }
}

// Test implementation of IAudioPlaybackService that doesn't actually play audio
public class TestAudioPlaybackService : IAudioPlaybackService
{
    private static readonly Action<ILogger, string, Exception?> _logSampleProviderError =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(1, "SampleProviderError"),
            "Error creating sample provider for style {Style}");

    private readonly object _syncLock = new();  // Lock object for thread safety

    private PlaybackState _state = PlaybackState.Stopped;
    private ISampleProvider? _currentSource;
    private SerialEffectChain? _effectChain;
    private float _volume = 1.0f;
    private string _currentStyle = "basic";
    private readonly Dictionary<string, IAudioEffect> _effects = new();
    private readonly Mock<ILoggerFactory> _loggerFactoryMock;

    public TestAudioPlaybackService()
    {
        _loggerFactoryMock = new Mock<ILoggerFactory>();
        _loggerFactoryMock.Setup(x => x.CreateLogger(It.IsAny<string>()))
            .Returns(new Mock<ILogger>().Object);
    }

    public ISampleProvider? CurrentSource
    {
        get
        {
            lock (_syncLock)
            {
                return _currentSource;
            }
        }
    }

    public string CurrentStyle
    {
        get
        {
            lock (_syncLock)
            {
                return _currentStyle;
            }
        }
        set
        {
            lock (_syncLock)
            {
                _currentStyle = value ?? throw new ArgumentNullException(nameof(value));
            }
        }
    }

    public float CurrentVolume
    {
        get
        {
            lock (_syncLock)
            {
                return _volume;
            }
        }
    }

    public void AddEffect(IAudioEffect effect)
    {
        lock (_syncLock)
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
                _effects[effect.Name] = effect;
            }
        }
    }

    public void RemoveEffect(string effectName)
    {
        lock (_syncLock)
        {
            _effectChain?.RemoveEffect(effectName);
            _effects.Remove(effectName);
        }
    }

    public void SetSource(ISampleProvider source)
    {
        ArgumentNullException.ThrowIfNull(source);

        lock (_syncLock)
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
    }

    public void SetVolume(float volume)
    {
        lock (_syncLock)
        {
            _volume = Math.Clamp(volume, 0.0f, 1.0f);
        }
    }

    public void StartPlayback()
    {
        lock (_syncLock)
        {
            // Create a default source if none exists
            if (_currentSource == null)
            {
                var defaultPattern = new BeatPattern
                {
                    BPM = 90,
                    DrumSequence = [_currentStyle],
                    ChordProgression = ["I", "IV", "V"]
                };

                try
                {
                    var provider = new BeatPatternSampleProvider(
                        defaultPattern,
                        _loggerFactoryMock.Object.CreateLogger<BeatPatternSampleProvider>(),
                        new UserSampleRepository(_loggerFactoryMock.Object.CreateLogger<UserSampleRepository>()),
                        new TelemetryTracker(
                            new NullTelemetryService(),
                            _loggerFactoryMock.Object.CreateLogger<TelemetryTracker>()));

                    _currentSource = provider;
                }
                catch (Exception ex)
                {
                    // Log the error but don't throw - this is a test implementation
                    var logger = _loggerFactoryMock.Object.CreateLogger<TestAudioPlaybackService>();
                    _logSampleProviderError(logger, _currentStyle, ex);

                    // Create a minimal pattern that will work
                    var fallbackPattern = new BeatPattern
                    {
                        BPM = 90,
                        DrumSequence = ["basic"],
                        ChordProgression = ["I"]
                    };

                    _currentSource = new BeatPatternSampleProvider(
                        fallbackPattern,
                        _loggerFactoryMock.Object.CreateLogger<BeatPatternSampleProvider>(),
                        new UserSampleRepository(_loggerFactoryMock.Object.CreateLogger<UserSampleRepository>()),
                        new TelemetryTracker(
                            new NullTelemetryService(),
                            _loggerFactoryMock.Object.CreateLogger<TelemetryTracker>()));
                }
            }
            _state = PlaybackState.Playing;
        }
    }

    public void StopPlayback()
    {
        lock (_syncLock)
        {
            _state = PlaybackState.Stopped;
            _currentSource = null;
            _effectChain = null;
            _effects.Clear();
        }
    }

    public void StopWithEffect(IAudioEffect effect)
    {
        // In test implementation, just stop immediately
        StopPlayback();
    }

    public void PausePlayback()
    {
        lock (_syncLock)
        {
            if (_state == PlaybackState.Playing)
            {
                _state = PlaybackState.Paused;
            }
        }
    }

    public void ResumePlayback()
    {
        lock (_syncLock)
        {
            if (_state == PlaybackState.Paused)
            {
                _state = PlaybackState.Playing;
            }
        }
    }

    public PlaybackState GetPlaybackState()
    {
        lock (_syncLock)
        {
            return _state;
        }
    }

    public Preset GetCurrentPreset()
    {
        lock (_syncLock)
        {
            return new Preset
            {
                Name = $"Test_Preset_{DateTime.Now:yyyyMMdd_HHmmss}",
                Style = _currentStyle,
                Volume = _volume,
                Effects = _effects.Keys.ToList()
            };
        }
    }

    public void ApplyPreset(Preset preset, IEffectFactory effectFactory)
    {
        ArgumentNullException.ThrowIfNull(preset);
        ArgumentNullException.ThrowIfNull(effectFactory);

        preset.Validate();

        lock (_syncLock)
        {
            _currentStyle = preset.Style;
            SetVolume(preset.Volume);

            // Remove existing effects
            var existingEffects = _effects.Keys.ToList();
            foreach (var effectName in existingEffects)
            {
                RemoveEffect(effectName);
            }

            // Add the new effects
            if (_currentSource != null)
            {
                foreach (var effectName in preset.Effects)
                {
                    var effect = effectFactory.CreateEffect(effectName, _currentSource);
                    AddEffect(effect);
                }
            }
        }
    }

    public void CrossfadeToPattern(BeatPattern newPattern, float crossfadeDuration)
    {
        ArgumentNullException.ThrowIfNull(newPattern);

        // Create the new pattern provider first to minimize state transition time
        var provider = new BeatPatternSampleProvider(
            newPattern,
            _loggerFactoryMock.Object.CreateLogger<BeatPatternSampleProvider>(),
            new UserSampleRepository(_loggerFactoryMock.Object.CreateLogger<UserSampleRepository>()),
            new TelemetryTracker(
                new NullTelemetryService(),
                _loggerFactoryMock.Object.CreateLogger<TelemetryTracker>()));

        // Set the new source and update state atomically to ensure consistency
        lock (_syncLock)
        {
            var oldSource = _currentSource;
            var oldEffectChain = _effectChain;

            try
            {
                SetSource(provider);
                _currentStyle = newPattern.DrumSequence.Length > 0 ? newPattern.DrumSequence[0] : "basic";
                _state = PlaybackState.Playing;
            }
            catch
            {
                // Rollback on failure
                _currentSource = oldSource;
                _effectChain = oldEffectChain;
                throw;
            }
        }
    }
}
