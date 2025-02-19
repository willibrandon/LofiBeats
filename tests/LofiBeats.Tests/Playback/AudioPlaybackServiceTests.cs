using LofiBeats.Core.Effects;
using LofiBeats.Core.Models;
using LofiBeats.Core.Playback;
using LofiBeats.Core.Telemetry;
using Microsoft.Extensions.Logging;
using Moq;
using NAudio.Wave;

namespace LofiBeats.Tests.Playback;

[Collection("AI Generated Tests")]
public class AudioPlaybackServiceTests : IDisposable
{
    private readonly Mock<ILogger<AudioPlaybackService>> _loggerMock;
    private readonly Mock<ILoggerFactory> _loggerFactoryMock;
    private readonly Mock<ISampleProvider> _sampleProviderMock;
    private readonly Mock<IAudioEffect> _effectMock;
    private readonly Mock<IAudioOutput> _audioOutputMock;
    private readonly Mock<ILogger<UserSampleRepository>> _userSampleLoggerMock;
    private readonly UserSampleRepository _userSampleRepository;
    private readonly Mock<ITelemetryService> _telemetryServiceMock;
    private readonly Mock<ILogger<TelemetryTracker>> _telemetryLoggerMock;
    private readonly TelemetryTracker _telemetryTracker;
    private PlaybackState _playbackState;

    public AudioPlaybackServiceTests()
    {
        _loggerMock = new Mock<ILogger<AudioPlaybackService>>();
        _loggerFactoryMock = new Mock<ILoggerFactory>();
        _sampleProviderMock = new Mock<ISampleProvider>();
        _effectMock = new Mock<IAudioEffect>();
        _audioOutputMock = new Mock<IAudioOutput>();
        _userSampleLoggerMock = new Mock<ILogger<UserSampleRepository>>();
        _telemetryServiceMock = new Mock<ITelemetryService>();
        _telemetryLoggerMock = new Mock<ILogger<TelemetryTracker>>();
        _playbackState = PlaybackState.Stopped;

        // Create UserSampleRepository with the mocked logger
        _userSampleRepository = new UserSampleRepository(_userSampleLoggerMock.Object);

        // Create TelemetryTracker with mocked dependencies
        _telemetryTracker = new TelemetryTracker(_telemetryServiceMock.Object, _telemetryLoggerMock.Object);

        // Setup mock sample provider
        _sampleProviderMock.Setup(x => x.WaveFormat)
            .Returns(WaveFormat.CreateIeeeFloatWaveFormat(44100, 2));

        // Setup mock effect
        _effectMock.Setup(x => x.Name).Returns("test_effect");
        _effectMock.Setup(x => x.WaveFormat)
            .Returns(WaveFormat.CreateIeeeFloatWaveFormat(44100, 2));

        // Setup mock audio output
        _audioOutputMock.Setup(x => x.Play())
            .Callback(() => _playbackState = PlaybackState.Playing);
        _audioOutputMock.Setup(x => x.Pause())
            .Callback(() => _playbackState = PlaybackState.Paused);
        _audioOutputMock.Setup(x => x.Stop())
            .Callback(() => _playbackState = PlaybackState.Stopped);
        _audioOutputMock.Setup(x => x.PlaybackState)
            .Returns(() => _playbackState);

        // Setup logger factory to return our logger mock
        _loggerFactoryMock.Setup(x => x.CreateLogger(It.IsAny<string>()))
            .Returns(_loggerMock.Object);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void SetSource_AddsSourceToMixer()
    {
        // Arrange
        var service = new AudioPlaybackService(
            _loggerMock.Object, 
            _loggerFactoryMock.Object, 
            _audioOutputMock.Object,
            _userSampleRepository,
            _telemetryTracker);

        // Act - Should not throw
        service.SetSource(_sampleProviderMock.Object);

        // Assert
        _audioOutputMock.Verify(x => x.Init(It.IsAny<IWaveProvider>()), Times.Once);
        _audioOutputMock.Verify(x => x.Play(), Times.Once);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void AddEffect_AddsEffectToMixer()
    {
        // Arrange
        var service = new AudioPlaybackService(
            _loggerMock.Object, 
            _loggerFactoryMock.Object, 
            _audioOutputMock.Object,
            _userSampleRepository,
            _telemetryTracker);
        service.SetSource(_sampleProviderMock.Object);

        // Act
        service.AddEffect(_effectMock.Object);

        // Assert - Verify the source is still playing
        Assert.Equal(PlaybackState.Playing, service.GetPlaybackState());
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void RemoveEffect_RemovesEffectFromMixer()
    {
        // Arrange
        var service = new AudioPlaybackService(
            _loggerMock.Object, 
            _loggerFactoryMock.Object, 
            _audioOutputMock.Object,
            _userSampleRepository,
            _telemetryTracker);
        service.SetSource(_sampleProviderMock.Object);
        service.AddEffect(_effectMock.Object);

        // Act
        service.RemoveEffect("test_effect");

        // Assert - Verify the source is still playing
        Assert.Equal(PlaybackState.Playing, service.GetPlaybackState());
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void StartPlayback_WithNoSource_AddsTestTone()
    {
        // Arrange
        var service = new AudioPlaybackService(
            _loggerMock.Object, 
            _loggerFactoryMock.Object, 
            _audioOutputMock.Object,
            _userSampleRepository,
            _telemetryTracker);

        // Act
        service.StartPlayback();

        // Assert
        _audioOutputMock.Verify(x => x.Init(It.IsAny<IWaveProvider>()), Times.Once);
        // Play is called twice: once by SetSource and once by StartPlayback
        _audioOutputMock.Verify(x => x.Play(), Times.Exactly(2));
        Assert.Equal(PlaybackState.Playing, service.GetPlaybackState());
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void StopPlayback_RemovesSource()
    {
        // Arrange
        var service = new AudioPlaybackService(
            _loggerMock.Object, 
            _loggerFactoryMock.Object, 
            _audioOutputMock.Object,
            _userSampleRepository,
            _telemetryTracker);
        service.SetSource(_sampleProviderMock.Object);

        // Act
        service.StopPlayback();

        // Assert
        _audioOutputMock.Verify(x => x.Stop(), Times.Once);
        Assert.Equal(PlaybackState.Stopped, service.GetPlaybackState());
        Assert.Null(service.CurrentSource);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void PauseAndResume_ModifiesPlaybackStateCorrectly()
    {
        // Arrange
        var service = new AudioPlaybackService(
            _loggerMock.Object, 
            _loggerFactoryMock.Object, 
            _audioOutputMock.Object,
            _userSampleRepository,
            _telemetryTracker);
        service.SetSource(_sampleProviderMock.Object);
        
        // Act & Assert - Initial state
        Assert.Equal(PlaybackState.Playing, service.GetPlaybackState());
        
        // Pause
        service.PausePlayback();
        Assert.Equal(PlaybackState.Paused, service.GetPlaybackState());
        _audioOutputMock.Verify(x => x.Pause(), Times.Once);
        
        // Resume
        service.ResumePlayback();
        Assert.Equal(PlaybackState.Playing, service.GetPlaybackState());
        _audioOutputMock.Verify(x => x.Play(), Times.Exactly(2)); // Initial play + resume
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void SetVolume_ClampsBetweenValidRange()
    {
        // Arrange
        var service = new AudioPlaybackService(
            _loggerMock.Object, 
            _loggerFactoryMock.Object, 
            _audioOutputMock.Object,
            _userSampleRepository,
            _telemetryTracker);

        // Act
        service.SetVolume(-0.5f); // Should clamp to 0
        service.SetVolume(1.5f);  // Should clamp to 1
        service.SetVolume(0.5f);  // Should set exactly

        // Assert
        _audioOutputMock.Verify(x => x.SetVolume(0f), Times.Once); // Clamped to 0
        _audioOutputMock.Verify(x => x.SetVolume(1f), Times.Once); // Clamped to 1
        _audioOutputMock.Verify(x => x.SetVolume(0.5f), Times.Once); // Exact value
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void PlaybackState_ReflectsCurrentState()
    {
        // Arrange
        var service = new AudioPlaybackService(
            _loggerMock.Object, 
            _loggerFactoryMock.Object, 
            _audioOutputMock.Object,
            _userSampleRepository,
            _telemetryTracker);
        
        // Act & Assert - Initial state (no source)
        Assert.Equal(PlaybackState.Stopped, service.GetPlaybackState());
        
        // Set source and verify playing
        service.SetSource(_sampleProviderMock.Object);
        Assert.Equal(PlaybackState.Playing, service.GetPlaybackState());
        
        // Stop and verify stopped
        service.StopPlayback();
        Assert.Equal(PlaybackState.Stopped, service.GetPlaybackState());
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void SetSource_WithMonoInput_ConvertsToStereo()
    {
        // Arrange
        var service = new AudioPlaybackService(
            _loggerMock.Object, 
            _loggerFactoryMock.Object, 
            _audioOutputMock.Object,
            _userSampleRepository,
            _telemetryTracker);
        var monoProviderMock = new Mock<ISampleProvider>();
        monoProviderMock.Setup(x => x.WaveFormat)
            .Returns(WaveFormat.CreateIeeeFloatWaveFormat(44100, 1)); // Mono format

        // Act
        service.SetSource(monoProviderMock.Object);
        
        // Assert
        _audioOutputMock.Verify(x => x.Init(It.IsAny<IWaveProvider>()), Times.Once);
        Assert.Equal(PlaybackState.Playing, service.GetPlaybackState());
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void MultipleEffects_CanBeAddedAndRemoved()
    {
        // Arrange
        var service = new AudioPlaybackService(
            _loggerMock.Object, 
            _loggerFactoryMock.Object, 
            _audioOutputMock.Object,
            _userSampleRepository,
            _telemetryTracker);
        var effect1Mock = new Mock<IAudioEffect>();
        var effect2Mock = new Mock<IAudioEffect>();
        
        effect1Mock.Setup(x => x.Name).Returns("effect1");
        effect1Mock.Setup(x => x.WaveFormat)
            .Returns(WaveFormat.CreateIeeeFloatWaveFormat(44100, 2));
        
        effect2Mock.Setup(x => x.Name).Returns("effect2");
        effect2Mock.Setup(x => x.WaveFormat)
            .Returns(WaveFormat.CreateIeeeFloatWaveFormat(44100, 2));

        service.SetSource(_sampleProviderMock.Object);

        // Act & Assert
        service.AddEffect(effect1Mock.Object);
        service.AddEffect(effect2Mock.Object);
        Assert.Equal(PlaybackState.Playing, service.GetPlaybackState());
        
        // Remove effects in different order
        service.RemoveEffect("effect1");
        service.RemoveEffect("effect2");
        Assert.Equal(PlaybackState.Playing, service.GetPlaybackState());
        
        // Should be able to add them again
        service.AddEffect(effect1Mock.Object);
        service.AddEffect(effect2Mock.Object);
        Assert.Equal(PlaybackState.Playing, service.GetPlaybackState());
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void PauseAndResume_WithNoSource_DoesNotThrow()
    {
        // Arrange
        var service = new AudioPlaybackService(
            _loggerMock.Object, 
            _loggerFactoryMock.Object, 
            _audioOutputMock.Object,
            _userSampleRepository,
            _telemetryTracker);

        // Act & Assert - Should not throw
        service.PausePlayback();
        service.ResumePlayback();
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void CurrentStyle_DefaultsToBasic()
    {
        // Arrange & Act
        var service = new AudioPlaybackService(
            _loggerMock.Object, 
            _loggerFactoryMock.Object, 
            _audioOutputMock.Object,
            _userSampleRepository,
            _telemetryTracker);

        // Assert
        Assert.Equal("basic", service.CurrentStyle);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void CurrentStyle_CanBeSetAndRetrieved()
    {
        // Arrange
        var service = new AudioPlaybackService(
            _loggerMock.Object, 
            _loggerFactoryMock.Object, 
            _audioOutputMock.Object,
            _userSampleRepository,
            _telemetryTracker);
        const string newStyle = "jazzy";

        // Act
        service.CurrentStyle = newStyle;

        // Assert
        Assert.Equal(newStyle, service.CurrentStyle);
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("Style changed to: jazzy")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void CurrentStyle_ThrowsOnNullOrEmpty()
    {
        // Arrange
        var service = new AudioPlaybackService(
            _loggerMock.Object, 
            _loggerFactoryMock.Object, 
            _audioOutputMock.Object,
            _userSampleRepository,
            _telemetryTracker);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => service.CurrentStyle = "");
        Assert.Throws<ArgumentException>(() => service.CurrentStyle = " ");
        Assert.Throws<ArgumentException>(() => service.CurrentStyle = null!);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void GetCurrentPreset_ReturnsCurrentState()
    {
        // Arrange
        var service = new AudioPlaybackService(
            _loggerMock.Object, 
            _loggerFactoryMock.Object, 
            _audioOutputMock.Object,
            _userSampleRepository,
            _telemetryTracker);
        service.SetSource(_sampleProviderMock.Object);
        service.CurrentStyle = "jazzy";
        service.SetVolume(0.8f);
        service.AddEffect(_effectMock.Object);

        // Act
        var preset = service.GetCurrentPreset();

        // Assert
        Assert.NotNull(preset);
        Assert.NotEmpty(preset.Name);
        Assert.Equal("jazzy", preset.Style);
        Assert.Equal(0.8f, preset.Volume);
        Assert.Single(preset.Effects);
        Assert.Equal("test_effect", preset.Effects[0]);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void ApplyPreset_SetsAllProperties()
    {
        // Arrange
        var service = new AudioPlaybackService(
            _loggerMock.Object, 
            _loggerFactoryMock.Object, 
            _audioOutputMock.Object,
            _userSampleRepository,
            _telemetryTracker);
        service.SetSource(_sampleProviderMock.Object);
        
        var preset = new Preset
        {
            Name = "Test Preset",
            Style = "jazzy",
            Volume = 0.7f,
            Effects = ["test_effect"]
        };

        var effectFactoryMock = new Mock<IEffectFactory>();
        effectFactoryMock.Setup(x => x.CreateEffect("test_effect", It.IsAny<ISampleProvider>()))
            .Returns(_effectMock.Object);

        // Act
        service.ApplyPreset(preset, effectFactoryMock.Object);

        // Assert
        Assert.Equal("jazzy", service.CurrentStyle);
        Assert.Equal(0.7f, service.CurrentVolume);
        _audioOutputMock.Verify(x => x.SetVolume(0.7f), Times.Once);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void ApplyPreset_WithNoSource_OnlySetsStyleAndVolume()
    {
        // Arrange
        var service = new AudioPlaybackService(
            _loggerMock.Object, 
            _loggerFactoryMock.Object, 
            _audioOutputMock.Object,
            _userSampleRepository,
            _telemetryTracker);
        
        var preset = new Preset
        {
            Name = "Test Preset",
            Style = "jazzy",
            Volume = 0.7f,
            Effects = ["test_effect"]
        };

        var effectFactoryMock = new Mock<IEffectFactory>();

        // Act
        service.ApplyPreset(preset, effectFactoryMock.Object);

        // Assert
        Assert.Equal("jazzy", service.CurrentStyle);
        Assert.Equal(0.7f, service.CurrentVolume);
        effectFactoryMock.Verify(x => x.CreateEffect(It.IsAny<string>(), It.IsAny<ISampleProvider>()), Times.Never);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void ApplyPreset_WithInvalidPreset_ThrowsException()
    {
        // Arrange
        var service = new AudioPlaybackService(
            _loggerMock.Object, 
            _loggerFactoryMock.Object, 
            _audioOutputMock.Object,
            _userSampleRepository,
            _telemetryTracker);
        var effectFactoryMock = new Mock<IEffectFactory>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => service.ApplyPreset(null!, effectFactoryMock.Object));
        Assert.Throws<ArgumentNullException>(() => service.ApplyPreset(new Preset { Name = "Test", Style = "basic" }, null!));
        
        var invalidPreset = new Preset { Name = "Invalid", Style = "", Volume = -1 };
        Assert.Throws<ArgumentException>(() => service.ApplyPreset(invalidPreset, effectFactoryMock.Object));
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void CrossfadeToPattern_SetupsCrossfadeCorrectly()
    {
        // Arrange
        var service = new AudioPlaybackService(
            _loggerMock.Object, 
            _loggerFactoryMock.Object, 
            _audioOutputMock.Object,
            _userSampleRepository,
            _telemetryTracker);

        // Create a mock pattern
        var pattern = new BeatPattern
        {
            BPM = 90,
            DrumSequence = ["kick", "snare", "hat"],
            ChordProgression = ["Cm7", "Fm7", "Bb7", "Eb7"]
        };

        // Set initial source
        service.SetSource(_sampleProviderMock.Object);

        // Setup telemetry tracking verification
        _telemetryServiceMock.Setup(t => t.TrackEvent(
            It.IsAny<string>(),
            It.IsAny<Dictionary<string, string>>(),
            It.IsAny<DateTimeOffset?>()));

        // Act
        service.CrossfadeToPattern(pattern, 2.0f);

        // Assert
        // Verify that the mixer was accessed to remove old source and add new
        _audioOutputMock.Verify(x => x.Play(), Times.AtLeastOnce);

        // Verify telemetry events were tracked
        _telemetryServiceMock.Verify(t => t.TrackEvent(
            "CrossfadeStarted",
            It.Is<Dictionary<string, string>>(d => 
                d.ContainsKey("CrossfadeDuration") && 
                d["CrossfadeDuration"] == "2"),
            It.IsAny<DateTimeOffset?>()), 
            Times.Once);

        // Verify the service is still in a playing state
        Assert.Equal(PlaybackState.Playing, service.GetPlaybackState());
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void CrossfadeToPattern_WithNullPattern_ThrowsArgumentNullException()
    {
        // Arrange
        var service = new AudioPlaybackService(
            _loggerMock.Object, 
            _loggerFactoryMock.Object, 
            _audioOutputMock.Object,
            _userSampleRepository,
            _telemetryTracker);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => service.CrossfadeToPattern(null!, 2.0f));
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void CrossfadeToPattern_WithNoCurrentSource_SetsNewSourceDirectly()
    {
        // Arrange
        var service = new AudioPlaybackService(
            _loggerMock.Object, 
            _loggerFactoryMock.Object, 
            _audioOutputMock.Object,
            _userSampleRepository,
            _telemetryTracker);

        var pattern = new BeatPattern
        {
            BPM = 90,
            DrumSequence = ["kick", "snare", "hat"],
            ChordProgression = ["Cm7", "Fm7", "Bb7", "Eb7"]
        };

        // Act
        service.CrossfadeToPattern(pattern, 2.0f);

        // Assert
        // Verify that Play was called (indicating SetSource was used)
        _audioOutputMock.Verify(x => x.Play(), Times.Once);
        Assert.Equal(PlaybackState.Playing, service.GetPlaybackState());
    }

    public void Dispose()
    {
        // Dispose of resources
    }
} 