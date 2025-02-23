using Xunit;

namespace LofiBeats.Plugins.MultiParameterEffect.Tests;

public class DynamicFilterEffectTests
{
    [Fact]
    public void Name_ReturnsCorrectEffectName()
    {
        // Arrange
        var effect = new DynamicFilterEffect();

        // Act
        string name = effect.Name;

        // Assert
        Assert.Equal("dynamicfilter", name);
    }

    [Fact]
    public void WaveFormat_WithoutSource_ReturnsDefaultFormat()
    {
        // Arrange
        var effect = new DynamicFilterEffect();

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
        var effect = new DynamicFilterEffect();
        var buffer = new float[1024];

        // Act
        int samplesRead = effect.Read(buffer, 0, buffer.Length);

        // Assert
        Assert.Equal(0, samplesRead);
    }

    [Fact]
    public void SetCutoffFrequency_ClampsToBounds()
    {
        // Arrange
        var effect = new DynamicFilterEffect();
        var buffer = new float[1024];
        for (int i = 0; i < buffer.Length; i++)
        {
            buffer[i] = MathF.Sin(2f * MathF.PI * i / buffer.Length);
        }

        // Test values outside bounds
        effect.SetCutoffFrequency(-100f);
        effect.ApplyEffect(buffer, 0, buffer.Length);
        Assert.All(buffer, sample => Assert.True(sample >= -1.0f && sample <= 1.0f));

        effect.SetCutoffFrequency(25000f);
        effect.ApplyEffect(buffer, 0, buffer.Length);
        Assert.All(buffer, sample => Assert.True(sample >= -1.0f && sample <= 1.0f));

        // Test values within bounds
        effect.SetCutoffFrequency(1000f);
        effect.ApplyEffect(buffer, 0, buffer.Length);
        Assert.All(buffer, sample => Assert.True(sample >= -1.0f && sample <= 1.0f));
    }

    [Fact]
    public void SetResonance_ClampsToBounds()
    {
        // Arrange
        var effect = new DynamicFilterEffect();
        var buffer = new float[1024];
        for (int i = 0; i < buffer.Length; i++)
        {
            buffer[i] = MathF.Sin(2f * MathF.PI * i / buffer.Length);
        }

        // Test values outside bounds
        effect.SetResonance(-0.5f);
        effect.ApplyEffect(buffer, 0, buffer.Length);
        Assert.All(buffer, sample => Assert.True(sample >= -1.0f && sample <= 1.0f));

        effect.SetResonance(1.5f);
        effect.ApplyEffect(buffer, 0, buffer.Length);
        Assert.All(buffer, sample => Assert.True(sample >= -1.0f && sample <= 1.0f));

        // Test values within bounds
        effect.SetResonance(0.5f);
        effect.ApplyEffect(buffer, 0, buffer.Length);
        Assert.All(buffer, sample => Assert.True(sample >= -1.0f && sample <= 1.0f));
    }

    [Fact]
    public void SetModulationRate_ClampsToBounds()
    {
        // Arrange
        var effect = new DynamicFilterEffect();
        var buffer = new float[1024];
        for (int i = 0; i < buffer.Length; i++)
        {
            buffer[i] = MathF.Sin(2f * MathF.PI * i / buffer.Length);
        }

        // Test values outside bounds
        effect.SetModulationRate(-1f);
        effect.ApplyEffect(buffer, 0, buffer.Length);
        Assert.All(buffer, sample => Assert.True(sample >= -1.0f && sample <= 1.0f));

        effect.SetModulationRate(15f);
        effect.ApplyEffect(buffer, 0, buffer.Length);
        Assert.All(buffer, sample => Assert.True(sample >= -1.0f && sample <= 1.0f));

        // Test values within bounds
        effect.SetModulationRate(5f);
        effect.ApplyEffect(buffer, 0, buffer.Length);
        Assert.All(buffer, sample => Assert.True(sample >= -1.0f && sample <= 1.0f));
    }

    [Fact]
    public void SetModulationDepth_ClampsToBounds()
    {
        // Arrange
        var effect = new DynamicFilterEffect();
        var buffer = new float[1024];
        for (int i = 0; i < buffer.Length; i++)
        {
            buffer[i] = MathF.Sin(2f * MathF.PI * i / buffer.Length);
        }

        // Test values outside bounds
        effect.SetModulationDepth(-0.5f);
        effect.ApplyEffect(buffer, 0, buffer.Length);
        Assert.All(buffer, sample => Assert.True(sample >= -1.0f && sample <= 1.0f));

        effect.SetModulationDepth(1.5f);
        effect.ApplyEffect(buffer, 0, buffer.Length);
        Assert.All(buffer, sample => Assert.True(sample >= -1.0f && sample <= 1.0f));

        // Test values within bounds
        effect.SetModulationDepth(0.5f);
        effect.ApplyEffect(buffer, 0, buffer.Length);
        Assert.All(buffer, sample => Assert.True(sample >= -1.0f && sample <= 1.0f));
    }

    [Fact]
    public void ApplyEffect_OutputsWithinValidRange()
    {
        // Arrange
        var effect = new DynamicFilterEffect();
        var buffer = new float[1024];
        for (int i = 0; i < buffer.Length; i++)
        {
            buffer[i] = MathF.Sin(2f * MathF.PI * i / buffer.Length);
        }

        // Act
        effect.ApplyEffect(buffer, 0, buffer.Length);

        // Assert
        Assert.All(buffer, sample => Assert.True(sample >= -1.0f && sample <= 1.0f));
    }
}
