using LofiBeats.Core.Effects;
using LofiBeats.Core.Playback;
using Microsoft.Extensions.Logging;
using Moq;
using NAudio.Wave;

namespace LofiBeats.Tests.Playback;

[Collection("AI Generated Tests")]
public class AudioPlaybackServiceTests
{
    private readonly Mock<ILogger<AudioPlaybackService>> _loggerMock;
    private readonly Mock<ILoggerFactory> _loggerFactoryMock;
    private readonly Mock<ISampleProvider> _sampleProviderMock;
    private readonly Mock<IAudioEffect> _effectMock;
    private readonly Mock<IAudioOutput> _audioOutputMock;
    private PlaybackState _playbackState;

    public AudioPlaybackServiceTests()
    {
        _loggerMock = new Mock<ILogger<AudioPlaybackService>>();
        _loggerFactoryMock = new Mock<ILoggerFactory>();
        _sampleProviderMock = new Mock<ISampleProvider>();
        _effectMock = new Mock<IAudioEffect>();
        _audioOutputMock = new Mock<IAudioOutput>();
        _playbackState = PlaybackState.Stopped;

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
        var service = new AudioPlaybackService(_loggerMock.Object, _loggerFactoryMock.Object, _audioOutputMock.Object);

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
        var service = new AudioPlaybackService(_loggerMock.Object, _loggerFactoryMock.Object, _audioOutputMock.Object);
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
        var service = new AudioPlaybackService(_loggerMock.Object, _loggerFactoryMock.Object, _audioOutputMock.Object);
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
        var service = new AudioPlaybackService(_loggerMock.Object, _loggerFactoryMock.Object, _audioOutputMock.Object);

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
        var service = new AudioPlaybackService(_loggerMock.Object, _loggerFactoryMock.Object, _audioOutputMock.Object);
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
        var service = new AudioPlaybackService(_loggerMock.Object, _loggerFactoryMock.Object, _audioOutputMock.Object);
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
        var service = new AudioPlaybackService(_loggerMock.Object, _loggerFactoryMock.Object, _audioOutputMock.Object);

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
        var service = new AudioPlaybackService(_loggerMock.Object, _loggerFactoryMock.Object, _audioOutputMock.Object);
        
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
        var service = new AudioPlaybackService(_loggerMock.Object, _loggerFactoryMock.Object, _audioOutputMock.Object);
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
        var service = new AudioPlaybackService(_loggerMock.Object, _loggerFactoryMock.Object, _audioOutputMock.Object);
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
        var service = new AudioPlaybackService(_loggerMock.Object, _loggerFactoryMock.Object, _audioOutputMock.Object);

        // Act & Assert - Should not throw
        service.PausePlayback();
        service.ResumePlayback();
    }
} 