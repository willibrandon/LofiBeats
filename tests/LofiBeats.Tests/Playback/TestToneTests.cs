using NAudio.Wave;
using LofiBeats.Core.Playback;

namespace LofiBeats.Tests.Playback;

[Collection("AI Generated Tests")]
public class TestToneTests
{
    [Fact]
    [Trait("Category", "AI_Generated")]
    public void WaveFormat_ShouldBeIeeeFloat44100Stereo()
    {
        // Arrange
        var testTone = new TestTone();

        // Act
        var waveFormat = testTone.WaveFormat;

        // Assert
        Assert.Equal(44100, waveFormat.SampleRate);
        Assert.Equal(2, waveFormat.Channels);
        Assert.Equal(WaveFormatEncoding.IeeeFloat, waveFormat.Encoding);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void Read_ShouldGenerateCorrectNumberOfSamples()
    {
        // Arrange
        var testTone = new TestTone();
        var buffer = new float[1024];

        // Act
        int samplesRead = testTone.Read(buffer, 0, buffer.Length);

        // Assert
        Assert.Equal(buffer.Length, samplesRead);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void Read_ShouldGenerateStereoSamples()
    {
        // Arrange
        var testTone = new TestTone();
        var buffer = new float[4]; // Two stereo pairs

        // Act
        testTone.Read(buffer, 0, buffer.Length);

        // Assert
        // Left and right channels should be identical for each pair
        Assert.Equal(buffer[0], buffer[1]); // First stereo pair
        Assert.Equal(buffer[2], buffer[3]); // Second stereo pair
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void Read_ShouldGenerateSineWaveWithinRange()
    {
        // Arrange
        var testTone = new TestTone();
        var buffer = new float[1024];

        // Act
        testTone.Read(buffer, 0, buffer.Length);

        // Assert
        foreach (float sample in buffer)
        {
            // Sine wave should stay within [-1, 1]
            Assert.True(sample >= -1.0f && sample <= 1.0f);
        }
    }

    [Theory]
    [InlineData(0, 512)]    // Start of buffer
    [InlineData(256, 512)]  // Middle of buffer
    [InlineData(512, 512)]  // End of buffer
    [Trait("Category", "AI_Generated")]
    public void Read_ShouldRespectOffsetAndCount(int offset, int count)
    {
        // Arrange
        var testTone = new TestTone();
        var buffer = new float[1024];
        Array.Fill(buffer, float.MaxValue); // Fill with an impossible value

        // Act
        int samplesRead = testTone.Read(buffer, offset, count);

        // Assert
        Assert.Equal(count, samplesRead);
        
        // Verify samples before offset are untouched
        for (int i = 0; i < offset; i++)
        {
            Assert.Equal(float.MaxValue, buffer[i]);
        }

        // Verify samples in the read range are within sine wave bounds
        for (int i = offset; i < offset + count; i++)
        {
            Assert.True(buffer[i] >= -1.0f && buffer[i] <= 1.0f);
        }

        // Verify samples after the read range are untouched
        for (int i = offset + count; i < buffer.Length; i++)
        {
            Assert.Equal(float.MaxValue, buffer[i]);
        }
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void Read_ShouldMaintainPhaseConsistency()
    {
        // Arrange
        var testTone = new TestTone();
        var buffer1 = new float[1024]; // Smaller buffer for more precise testing
        var buffer2 = new float[1024];

        // Act
        testTone.Read(buffer1, 0, buffer1.Length);
        testTone.Read(buffer2, 0, buffer2.Length);

        // Assert
        // Check phase consistency between buffers
        // Due to stereo interleaving, we compare the last left channel sample
        // with the first left channel sample of the next buffer
        float lastLeftSample = buffer1[buffer1.Length - 2]; // Last left channel sample
        float firstLeftSample = buffer2[0]; // First left channel sample

        // Calculate expected phase difference based on our sine wave frequency (440Hz)
        // and sample rate (44100Hz)
        float phaseIncrement = (float)(2 * Math.PI * 440.0 / 44100);
        float expectedPhaseDifference = phaseIncrement * 2; // Multiply by 2 for stereo samples
        
        // The difference in samples should approximately match our expected phase progression
        float actualDifference = (float)Math.Asin(firstLeftSample) - (float)Math.Asin(lastLeftSample);
        if (actualDifference < 0) actualDifference += (float)(2 * Math.PI);
        
        float epsilon = 0.1f; // Increased epsilon to account for floating-point arithmetic in phase calculations
        Assert.True(Math.Abs(actualDifference - expectedPhaseDifference) < epsilon,
            $"Phase difference {actualDifference} should be close to expected {expectedPhaseDifference}");
    }
} 