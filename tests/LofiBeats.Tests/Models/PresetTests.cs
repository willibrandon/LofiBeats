using LofiBeats.Core.Models;

namespace LofiBeats.Tests.Models;

[Collection("AI Generated Tests")]
public class PresetTests
{
    [Theory]
    [Trait("Category", "AI_Generated")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_WithInvalidName_ThrowsArgumentException(string? invalidName)
    {
        // Arrange
        var preset = new Preset
        {
            Name = invalidName ?? "",
            Style = "basic",
            Volume = 0.5f,
            Effects = []
        };

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => preset.Validate());
        Assert.Equal("Preset name cannot be empty. (Parameter 'Name')", exception.Message);
    }

    [Theory]
    [Trait("Category", "AI_Generated")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_WithInvalidStyle_ThrowsArgumentException(string? invalidStyle)
    {
        // Arrange
        var preset = new Preset
        {
            Name = "Test Preset",
            Style = invalidStyle ?? "",
            Volume = 0.5f,
            Effects = []
        };

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => preset.Validate());
        Assert.Equal("Style cannot be empty. (Parameter 'Style')", exception.Message);
    }

    [Theory]
    [Trait("Category", "AI_Generated")]
    [InlineData(-0.1f)]
    [InlineData(-1.0f)]
    [InlineData(1.1f)]
    [InlineData(2.0f)]
    public void Validate_WithInvalidVolume_ThrowsArgumentException(float invalidVolume)
    {
        // Arrange
        var preset = new Preset
        {
            Name = "Test Preset",
            Style = "basic",
            Volume = invalidVolume,
            Effects = []
        };

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => preset.Validate());
        Assert.Equal("Volume must be between 0 and 1. (Parameter 'Volume')", exception.Message);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void Validate_WithValidPreset_DoesNotThrow()
    {
        // Arrange
        var preset = new Preset
        {
            Name = "Test Preset",
            Style = "basic",
            Volume = 0.5f,
            Effects = ["reverb", "delay"]
        };

        // Act & Assert
        var exception = Record.Exception(() => preset.Validate());
        Assert.Null(exception);
    }
} 