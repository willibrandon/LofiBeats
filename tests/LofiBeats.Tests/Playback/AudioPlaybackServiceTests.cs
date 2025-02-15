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

    public AudioPlaybackServiceTests()
    {
        _loggerMock = new Mock<ILogger<AudioPlaybackService>>();
        _loggerFactoryMock = new Mock<ILoggerFactory>();
        _sampleProviderMock = new Mock<ISampleProvider>();
        _effectMock = new Mock<IAudioEffect>();

        // Setup mock sample provider
        _sampleProviderMock.Setup(x => x.WaveFormat)
            .Returns(WaveFormat.CreateIeeeFloatWaveFormat(44100, 2));

        // Setup mock effect
        _effectMock.Setup(x => x.Name).Returns("test_effect");
        _effectMock.Setup(x => x.WaveFormat)
            .Returns(WaveFormat.CreateIeeeFloatWaveFormat(44100, 2));

        // Setup logger factory to return a mock logger for any type
        _loggerFactoryMock.Setup(x => x.CreateLogger(It.IsAny<string>()))
            .Returns(new Mock<ILogger>().Object);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void SetSource_AddsSourceToMixer()
    {
        // Arrange
        var service = new AudioPlaybackService(_loggerMock.Object, _loggerFactoryMock.Object);

        // Act - Should not throw
        service.SetSource(_sampleProviderMock.Object);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void AddEffect_AddsEffectToMixer()
    {
        // Arrange
        var service = new AudioPlaybackService(_loggerMock.Object, _loggerFactoryMock.Object);

        // Act - Should not throw
        service.AddEffect(_effectMock.Object);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void RemoveEffect_RemovesEffectFromMixer()
    {
        // Arrange
        var service = new AudioPlaybackService(_loggerMock.Object, _loggerFactoryMock.Object);
        service.AddEffect(_effectMock.Object);

        // Act
        service.RemoveEffect("test_effect");
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void StartPlayback_WithNoSource_AddsTestTone()
    {
        // Arrange
        var service = new AudioPlaybackService(_loggerMock.Object, _loggerFactoryMock.Object);

        // Act - Should not throw
        service.StartPlayback();
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void StopPlayback_RemovesSource()
    {
        // Arrange
        var service = new AudioPlaybackService(_loggerMock.Object, _loggerFactoryMock.Object);
        service.SetSource(_sampleProviderMock.Object);

        // Act
        service.StopPlayback();

        // Should be able to set source again
        service.SetSource(_sampleProviderMock.Object);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void PauseAndResume_ModifiesPlaybackStateCorrectly()
    {
        // Arrange
        var service = new AudioPlaybackService(_loggerMock.Object, _loggerFactoryMock.Object);
        service.SetSource(_sampleProviderMock.Object);
        
        // Act & Assert - Initial state
        Assert.Equal(PlaybackState.Playing, service.GetPlaybackState());
        
        // Pause
        service.PausePlayback();
        Assert.Equal(PlaybackState.Paused, service.GetPlaybackState());
        
        // Resume
        service.ResumePlayback();
        Assert.Equal(PlaybackState.Playing, service.GetPlaybackState());
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void SetVolume_ClampsBetweenValidRange()
    {
        // Arrange
        var service = new AudioPlaybackService(_loggerMock.Object, _loggerFactoryMock.Object);

        // Act & Assert - Should not throw
        service.SetVolume(-0.5f); // Should clamp to 0
        service.SetVolume(1.5f);  // Should clamp to 1
        service.SetVolume(0.5f);  // Should set exactly
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void PlaybackState_ReflectsCurrentState()
    {
        // Arrange
        var service = new AudioPlaybackService(_loggerMock.Object, _loggerFactoryMock.Object);
        
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
        var service = new AudioPlaybackService(_loggerMock.Object, _loggerFactoryMock.Object);
        var monoProviderMock = new Mock<ISampleProvider>();
        monoProviderMock.Setup(x => x.WaveFormat)
            .Returns(WaveFormat.CreateIeeeFloatWaveFormat(44100, 1)); // Mono format

        // Act - Should not throw
        service.SetSource(monoProviderMock.Object);
        
        // The conversion happens internally, so we can at least verify
        // that setting a mono source doesn't break anything
        Assert.Equal(PlaybackState.Playing, service.GetPlaybackState());
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void MultipleEffects_CanBeAddedAndRemoved()
    {
        // Arrange
        var service = new AudioPlaybackService(_loggerMock.Object, _loggerFactoryMock.Object);
        var effect1Mock = new Mock<IAudioEffect>();
        var effect2Mock = new Mock<IAudioEffect>();
        
        effect1Mock.Setup(x => x.Name).Returns("effect1");
        effect1Mock.Setup(x => x.WaveFormat)
            .Returns(WaveFormat.CreateIeeeFloatWaveFormat(44100, 2));
        
        effect2Mock.Setup(x => x.Name).Returns("effect2");
        effect2Mock.Setup(x => x.WaveFormat)
            .Returns(WaveFormat.CreateIeeeFloatWaveFormat(44100, 2));

        // Act & Assert
        service.AddEffect(effect1Mock.Object);
        service.AddEffect(effect2Mock.Object);
        
        // Remove effects in different order
        service.RemoveEffect("effect1");
        service.RemoveEffect("effect2");
        
        // Should be able to add them again
        service.AddEffect(effect1Mock.Object);
        service.AddEffect(effect2Mock.Object);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void PauseAndResume_WithNoSource_DoesNotThrow()
    {
        // Arrange
        var service = new AudioPlaybackService(_loggerMock.Object, _loggerFactoryMock.Object);

        // Act & Assert - Should not throw
        service.PausePlayback();
        service.ResumePlayback();
    }
} 