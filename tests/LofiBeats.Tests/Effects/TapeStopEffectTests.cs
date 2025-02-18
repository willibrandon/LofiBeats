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
        const float duration = 0.1f; // Very short duration for testing
        var effect = new TapeStopEffect(_sourceMock.Object, _loggerMock.Object, duration);
        var buffer = new float[1024];
        var sourceBuffer = new float[4096]; // Larger source buffer
        
        // Fill source with a simple sine wave to better test the effect
        double frequency = 440.0; // A4 note
        for (int i = 0; i < sourceBuffer.Length; i++)
        {
            double time = i / (double)_waveFormat.SampleRate;
            sourceBuffer[i] = (float)Math.Sin(2 * Math.PI * frequency * time);
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
        // Calculate the total samples needed for the effect duration
        int samplesPerSecond = _waveFormat.SampleRate * _waveFormat.Channels;
        int totalSamplesNeeded = (int)(duration * samplesPerSecond);
        int samplesProcessed = 0;
        bool foundModifiedSample = false;
        int readAttempts = 0;
        const int maxReadAttempts = 20; // Allow more read attempts

        var diagnostics = new System.Text.StringBuilder();
        diagnostics.AppendLine($"Sample rate: {_waveFormat.SampleRate}Hz");
        diagnostics.AppendLine($"Channels: {_waveFormat.Channels}");
        diagnostics.AppendLine($"Duration: {duration}s");
        diagnostics.AppendLine($"Total samples needed: {totalSamplesNeeded}");
        diagnostics.AppendLine($"Buffer size: {buffer.Length}");

        while (samplesProcessed < totalSamplesNeeded && readAttempts < maxReadAttempts && !effect.IsFinished)
        {
            readAttempts++;
            int samplesRead = effect.Read(buffer, 0, buffer.Length);
            
            if (samplesRead == 0)
            {
                diagnostics.AppendLine($"Read attempt {readAttempts}: Got 0 samples, effect finished: {effect.IsFinished}");
                break;
            }

            samplesProcessed += samplesRead;
            diagnostics.AppendLine($"Read attempt {readAttempts}: Got {samplesRead} samples, total: {samplesProcessed}");

            // Check for modifications in this buffer
            for (int i = 0; i < samplesRead && !foundModifiedSample; i++)
            {
                float originalSample = sourceBuffer[Math.Max(0, sourcePosition - samplesRead + i)];
                float processedSample = buffer[i];
                float difference = Math.Abs(processedSample - originalSample);

                if (difference > 0.01f)
                {
                    foundModifiedSample = true;
                    diagnostics.AppendLine($"Found modified sample at position {i}: original={originalSample:F3}, processed={processedSample:F3}, diff={difference:F3}");
                }
            }
        }

        // Add final state to diagnostics
        diagnostics.AppendLine($"Final state - Processed: {samplesProcessed}/{totalSamplesNeeded} samples");
        diagnostics.AppendLine($"Read attempts: {readAttempts}");
        diagnostics.AppendLine($"Found modified sample: {foundModifiedSample}");
        diagnostics.AppendLine($"Effect finished: {effect.IsFinished}");

        // First verify that the effect modifies samples
        Assert.True(foundModifiedSample, 
            "The tape stop effect should modify at least one sample value significantly.\n" + 
            diagnostics.ToString());

        // Then verify that it processes the expected number of samples or finishes
        Assert.True(samplesProcessed >= totalSamplesNeeded || effect.IsFinished,
            "The effect should process enough samples or finish.\n" + 
            diagnostics.ToString());
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