using LofiBeats.Core.Configuration;

namespace LofiBeats.Tests.Configuration;

[Collection("AI Generated Tests")]
public class AudioSettingsTests
{
    [Fact]
    [Trait("Category", "AI_Generated")]
    public void AudioSettings_HasCorrectDefaultValues()
    {
        // Arrange & Act
        var settings = new AudioSettings();

        // Assert
        Assert.Equal(80, settings.DefaultBPM);
        Assert.Equal(44100, settings.SampleRate);
        Assert.Equal(2, settings.Channels);
        Assert.NotNull(settings.Effects);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void AudioSettings_CanModifyProperties()
    {
        // Arrange
        var settings = new AudioSettings
        {
            DefaultBPM = 90,
            SampleRate = 48000,
            Channels = 1
        };

        // Assert
        Assert.Equal(90, settings.DefaultBPM);
        Assert.Equal(48000, settings.SampleRate);
        Assert.Equal(1, settings.Channels);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void EffectSettings_HasCorrectDefaultValues()
    {
        // Arrange & Act
        var settings = new EffectSettings();

        // Assert
        Assert.NotNull(settings.VinylCrackle);
        Assert.NotNull(settings.LowPass);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void VinylCrackleSettings_HasCorrectDefaultValues()
    {
        // Arrange & Act
        var settings = new VinylCrackleSettings();

        // Assert
        Assert.Equal(0.0005, settings.Frequency);
        Assert.Equal(0.2, settings.Amplitude);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void VinylCrackleSettings_CanModifyProperties()
    {
        // Arrange
        var settings = new VinylCrackleSettings
        {
            Frequency = 0.001,
            Amplitude = 0.3
        };

        // Assert
        Assert.Equal(0.001, settings.Frequency);
        Assert.Equal(0.3, settings.Amplitude);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void LowPassSettings_HasCorrectDefaultValues()
    {
        // Arrange & Act
        var settings = new LowPassSettings();

        // Assert
        Assert.Equal(2000f, settings.CutoffFrequency);
        Assert.Equal(1.0f, settings.Resonance);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void LowPassSettings_CanModifyProperties()
    {
        // Arrange
        var settings = new LowPassSettings
        {
            CutoffFrequency = 1500f,
            Resonance = 0.8f
        };

        // Assert
        Assert.Equal(1500f, settings.CutoffFrequency);
        Assert.Equal(0.8f, settings.Resonance);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void AudioSettings_EffectsChainIsComplete()
    {
        // Arrange
        var settings = new AudioSettings();

        // Assert - Verify the entire effects chain is properly initialized
        Assert.NotNull(settings.Effects);
        Assert.NotNull(settings.Effects.VinylCrackle);
        Assert.NotNull(settings.Effects.LowPass);
        
        // Verify default values are properly set through the chain
        Assert.Equal(0.0005, settings.Effects.VinylCrackle.Frequency);
        Assert.Equal(0.2, settings.Effects.VinylCrackle.Amplitude);
        Assert.Equal(2000f, settings.Effects.LowPass.CutoffFrequency);
        Assert.Equal(1.0f, settings.Effects.LowPass.Resonance);
    }
} 