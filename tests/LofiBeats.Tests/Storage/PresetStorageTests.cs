using LofiBeats.Core.Models;
using LofiBeats.Core.Storage;
using System.Text.Json;

namespace LofiBeats.Tests.Storage;

[Collection("AI Generated Tests")]
public class PresetStorageTests : IDisposable
{
    private readonly string _testDirectory;

    public PresetStorageTests()
    {
        // Create a unique test directory
        _testDirectory = Path.Combine(Path.GetTempPath(), $"LofiBeatsTests_{Guid.NewGuid()}");
        Directory.CreateDirectory(_testDirectory);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void SaveAndLoadPreset_Success()
    {
        // Arrange
        var filePath = Path.Combine(_testDirectory, "test_preset.json");
        var preset = new Preset
        {
            Name = "Test Preset",
            Style = "jazzy",
            Volume = 0.8f,
            Effects = ["vinyl", "reverb"]
        };

        // Act
        PresetStorage.SavePreset(filePath, preset);
        var loadedPreset = PresetStorage.LoadPreset(filePath);

        // Assert
        Assert.NotNull(loadedPreset);
        Assert.Equal(preset.Name, loadedPreset.Name);
        Assert.Equal(preset.Style, loadedPreset.Style);
        Assert.Equal(preset.Volume, loadedPreset.Volume);
        Assert.Equal(preset.Effects, loadedPreset.Effects);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void LoadPreset_NonExistentFile_ReturnsNull()
    {
        // Arrange
        var filePath = Path.Combine(_testDirectory, "nonexistent.json");

        // Act
        var result = PresetStorage.LoadPreset(filePath);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void SavePreset_CreatesDirectory()
    {
        // Arrange
        var subDir = Path.Combine(_testDirectory, "subdir");
        var filePath = Path.Combine(subDir, "test_preset.json");
        var preset = new Preset
        {
            Name = "Test Preset",
            Style = "basic",
            Volume = 0.5f,
            Effects = []
        };

        // Act
        PresetStorage.SavePreset(filePath, preset);

        // Assert
        Assert.True(Directory.Exists(subDir));
        Assert.True(File.Exists(filePath));
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void SavePreset_WithNullArguments_ThrowsException()
    {
        // Arrange
        var filePath = Path.Combine(_testDirectory, "test.json");
        var preset = new Preset
        {
            Name = "Test",
            Style = "basic",
            Volume = 1.0f,
            Effects = []
        };

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => PresetStorage.SavePreset(null!, preset));
        Assert.Throws<ArgumentNullException>(() => PresetStorage.SavePreset(filePath, null!));
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void LoadPreset_WithInvalidJson_ThrowsException()
    {
        // Arrange
        var filePath = Path.Combine(_testDirectory, "invalid.json");
        File.WriteAllText(filePath, "{ invalid json }");

        // Act & Assert
        Assert.Throws<JsonException>(() => PresetStorage.LoadPreset(filePath));
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void PresetExists_ReturnsCorrectValue()
    {
        // Arrange
        var filePath = Path.Combine(_testDirectory, "exists_test.json");
        var preset = new Preset
        {
            Name = "Test",
            Style = "basic",
            Volume = 1.0f,
            Effects = []
        };

        // Act & Assert - Before saving
        Assert.False(PresetStorage.PresetExists(filePath));

        // Save the preset
        PresetStorage.SavePreset(filePath, preset);

        // Act & Assert - After saving
        Assert.True(PresetStorage.PresetExists(filePath));
    }

    public void Dispose()
    {
        // Clean up test directory
        if (Directory.Exists(_testDirectory))
        {
            try
            {
                Directory.Delete(_testDirectory, true);
            }
            catch
            {
                // Ignore cleanup errors
            }
        }
    }
} 