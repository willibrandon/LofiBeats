using Microsoft.Extensions.Logging;
using Moq;
using NAudio.Wave;
using LofiBeats.Core.Effects;
using Xunit;
using System.Runtime.InteropServices;

namespace LofiBeats.Tests.Effects;

[Collection("AI Generated Tests")]
public class TapeStopEffectTests
{
    private readonly Mock<ILogger<TapeStopEffect>> _loggerMock;
    private readonly Mock<ISampleProvider> _sourceMock;
    private readonly WaveFormat _waveFormat;

    public TapeStopEffectTests()
    {
        _loggerMock = new Mock<ILogger<TapeStopEffect>>();
        _sourceMock = new Mock<ISampleProvider>();
        _waveFormat = new WaveFormat(44100, 2); // Standard stereo 44.1kHz
        _sourceMock.Setup(x => x.WaveFormat).Returns(_waveFormat);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void Constructor_WithValidParameters_InitializesCorrectly()
    {
        // Act
        var effect = new TapeStopEffect(_sourceMock.Object, _loggerMock.Object);

        // Assert
        Assert.Equal("tapestop", effect.Name);
        Assert.Equal(_waveFormat, effect.WaveFormat);
        Assert.False(effect.IsFinished);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void Constructor_WithNullSource_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new TapeStopEffect(null!, _loggerMock.Object));
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void Constructor_WithNullLogger_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new TapeStopEffect(_sourceMock.Object, null!));
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void Read_WhenFinished_ReturnsZero()
    {
        // Arrange
        var effect = new TapeStopEffect(_sourceMock.Object, _loggerMock.Object, 0.1f);
        var buffer = new float[1024];
        
        // Simulate effect completion
        _sourceMock.Setup(x => x.Read(It.IsAny<float[]>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns(0);

        // Act
        int samplesRead = effect.Read(buffer, 0, buffer.Length);

        // Assert
        Assert.Equal(0, samplesRead);
        Assert.True(effect.IsFinished);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void Read_WithValidBuffer_ProcessesSamples()
    {
        // Arrange
        var effect = new TapeStopEffect(_sourceMock.Object, _loggerMock.Object, 0.5f);
        var buffer = new float[1024];
        var sourceBuffer = new float[1024];
        for (int i = 0; i < sourceBuffer.Length; i++)
        {
            sourceBuffer[i] = 1.0f; // Fill with test data
        }

        _sourceMock.Setup(x => x.Read(It.IsAny<float[]>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns<float[], int, int>((buf, off, count) =>
            {
                Array.Copy(sourceBuffer, 0, buf, off, count);
                return count;
            });

        // Act & Assert
        // Read multiple times to allow the effect to progress
        bool foundModifiedSample = false;
        for (int i = 0; i < 10 && !foundModifiedSample; i++)
        {
            int samplesRead = effect.Read(buffer, 0, buffer.Length);
            Assert.Equal(buffer.Length, samplesRead);
            
            // Check if any samples have been modified
            foundModifiedSample = buffer.Any(x => Math.Abs(x - 1.0f) > 0.0001f);
        }

        // Verify that at least one sample was modified
        Assert.True(foundModifiedSample, "The tape stop effect should modify at least one sample value");
    }

    [SkippableFact]
    [Trait("Category", "AI_Generated")]
    public void Read_OnUnixPlatforms_HandlesBufferStateCorrectly()
    {
        Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || 
                   RuntimeInformation.IsOSPlatform(OSPlatform.Linux),
                   "This test is specific to macOS and Linux platforms");

        // Arrange
        var effect = new TapeStopEffect(_sourceMock.Object, _loggerMock.Object, 0.5f);
        var buffer = new float[2048]; // Larger buffer for this test
        var sourceBuffer = new float[4096]; // Source buffer twice the size
        for (int i = 0; i < sourceBuffer.Length; i++)
        {
            sourceBuffer[i] = 0.5f; // Fill with test data
        }

        int sourcePosition = 0;
        _sourceMock.Setup(x => x.Read(It.IsAny<float[]>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns<float[], int, int>((buf, off, count) =>
            {
                int remaining = sourceBuffer.Length - sourcePosition;
                int toCopy = Math.Min(count, remaining);
                if (toCopy > 0)
                {
                    Array.Copy(sourceBuffer, sourcePosition, buf, off, toCopy);
                    sourcePosition += toCopy;
                    return toCopy;
                }
                return 0;
            });

        // Act & Assert
        // Read multiple times to simulate continuous playback
        for (int i = 0; i < 5; i++)
        {
            int samplesRead = effect.Read(buffer, 0, buffer.Length);
            
            if (samplesRead == 0)
            {
                Assert.True(effect.IsFinished);
                break;
            }
            
            // Verify buffer state
            Assert.True(samplesRead <= buffer.Length);
            Assert.DoesNotContain(float.NaN, buffer.Take(samplesRead));
            Assert.DoesNotContain(float.PositiveInfinity, buffer.Take(samplesRead));
            Assert.DoesNotContain(float.NegativeInfinity, buffer.Take(samplesRead));
        }
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void SetSource_WithNewSource_UpdatesSourceCorrectly()
    {
        // Arrange
        var effect = new TapeStopEffect(_sourceMock.Object, _loggerMock.Object);
        var newSource = new Mock<ISampleProvider>();
        newSource.Setup(x => x.WaveFormat).Returns(_waveFormat);

        // Act
        effect.SetSource(newSource.Object);

        // Assert
        // Verify the effect can still process samples with the new source
        var buffer = new float[1024];
        newSource.Setup(x => x.Read(It.IsAny<float[]>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns(buffer.Length);
        
        int samplesRead = effect.Read(buffer, 0, buffer.Length);
        Assert.Equal(buffer.Length, samplesRead);
    }
} 