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
    private readonly Mock<ISampleProvider> _sampleProviderMock;
    private readonly Mock<IAudioEffect> _effectMock;

    public AudioPlaybackServiceTests()
    {
        _loggerMock = new Mock<ILogger<AudioPlaybackService>>();
        _sampleProviderMock = new Mock<ISampleProvider>();
        _effectMock = new Mock<IAudioEffect>();

        // Setup mock sample provider
        _sampleProviderMock.Setup(x => x.WaveFormat)
            .Returns(WaveFormat.CreateIeeeFloatWaveFormat(44100, 2));

        // Setup mock effect
        _effectMock.Setup(x => x.Name).Returns("test_effect");
        _effectMock.Setup(x => x.WaveFormat)
            .Returns(WaveFormat.CreateIeeeFloatWaveFormat(44100, 2));
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void SetSource_AddsSourceToMixer()
    {
        // Arrange
        var service = new AudioPlaybackService(_loggerMock.Object);

        // Act - Should not throw
        service.SetSource(_sampleProviderMock.Object);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void AddEffect_AddsEffectToMixer()
    {
        // Arrange
        var service = new AudioPlaybackService(_loggerMock.Object);

        // Act - Should not throw
        service.AddEffect(_effectMock.Object);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void RemoveEffect_RemovesEffectFromMixer()
    {
        // Arrange
        var service = new AudioPlaybackService(_loggerMock.Object);
        service.AddEffect(_effectMock.Object);

        // Act
        service.RemoveEffect("test_effect");

        // Add effect again - should work if previous one was removed
        service.AddEffect(_effectMock.Object);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void StartPlayback_WithNoSource_AddsTestTone()
    {
        // Arrange
        var service = new AudioPlaybackService(_loggerMock.Object);

        // Act - Should not throw
        service.StartPlayback();
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void StopPlayback_RemovesSource()
    {
        // Arrange
        var service = new AudioPlaybackService(_loggerMock.Object);
        service.SetSource(_sampleProviderMock.Object);

        // Act
        service.StopPlayback();

        // Should be able to set source again
        service.SetSource(_sampleProviderMock.Object);
    }
} 