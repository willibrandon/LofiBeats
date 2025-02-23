using Xunit;

namespace LofiBeats.Plugins.BasicEffect.Tests;

public class GainEffectTests
{
    [Fact]
    public void Name_ReturnsCorrectEffectName()
    {
        // Arrange
        var effect = new GainEffect();

        // Act
        string name = effect.Name;

        // Assert
        Assert.Equal("gain", name);
    }

    [Fact]
    public void WaveFormat_WithoutSource_ReturnsDefaultFormat()
    {
        // Arrange
        var effect = new GainEffect();

        // Act
        var format = effect.WaveFormat;

        // Assert
        Assert.Equal(44100, format.SampleRate);
        Assert.Equal(2, format.Channels);
        Assert.Equal(32, format.BitsPerSample);
    }

    [Fact]
    public void Read_WithoutSource_ReturnsZero()
    {
        // Arrange
        var effect = new GainEffect();
        var buffer = new float[1024];

        // Act
        int samplesRead = effect.Read(buffer, 0, buffer.Length);

        // Assert
        Assert.Equal(0, samplesRead);
    }

    [Fact]
    public void ApplyEffect_AmplifiesSignalWithinBounds()
    {
        // Arrange
        var effect = new GainEffect();
        var buffer = new float[] { 0.5f, -0.5f, 0.1f, -0.1f };
        var originalValues = (float[])buffer.Clone();
        int offset = 0;
        int count = buffer.Length;

        // Act
        effect.ApplyEffect(buffer, offset, count);

        // Assert
        // Check that samples are amplified but not clipped
        for (int i = 0; i < buffer.Length; i++)
        {
            Assert.True(Math.Abs(buffer[i]) <= 1.0f);
            Assert.True(Math.Abs(buffer[i]) > Math.Abs(originalValues[i]));
        }
    }

    [Fact]
    public void ApplyEffect_PreventsSampleClipping()
    {
        // Arrange
        var effect = new GainEffect();
        var buffer = new float[] { 0.8f, -0.8f, 1.0f, -1.0f };
        int offset = 0;
        int count = buffer.Length;

        // Act
        effect.ApplyEffect(buffer, offset, count);

        // Assert
        // Check that samples are clamped to [-1.0, 1.0]
        foreach (float sample in buffer)
        {
            Assert.True(sample >= -1.0f && sample <= 1.0f);
        }
    }
}
