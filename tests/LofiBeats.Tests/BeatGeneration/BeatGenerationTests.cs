using LofiBeats.Core.BeatGeneration;
using LofiBeats.Core.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace LofiBeats.Tests.BeatGeneration;

[Collection("AI Generated Tests")]
public class BeatGenerationTests
{
    private readonly Mock<ILogger<BasicLofiBeatGenerator>> _loggerMock;
    private readonly BasicLofiBeatGenerator _generator;

    public BeatGenerationTests()
    {
        _loggerMock = new Mock<ILogger<BasicLofiBeatGenerator>>();
        _generator = new BasicLofiBeatGenerator(_loggerMock.Object);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void BasicLofiBeatGenerator_ReturnsPattern()
    {
        // Act
        var pattern = _generator.GeneratePattern();

        // Assert
        Assert.NotNull(pattern);
        Assert.True(pattern.BPM >= 70 && pattern.BPM <= 90);
        Assert.NotNull(pattern.DrumSequence);
        Assert.NotNull(pattern.ChordProgression);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void BasicLofiBeatGenerator_DrumSequence_HasValidLength()
    {
        // Act
        var pattern = _generator.GeneratePattern();

        // Assert
        Assert.Equal(8, pattern.DrumSequence.Length);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void BasicLofiBeatGenerator_ChordProgression_HasValidLength()
    {
        // Act
        var pattern = _generator.GeneratePattern();

        // Assert
        Assert.Equal(4, pattern.ChordProgression.Length);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void BasicLofiBeatGenerator_DrumSequence_ContainsValidElements()
    {
        // Act
        var pattern = _generator.GeneratePattern();

        // Assert
        var validDrums = new[] { "kick", "hat", "snare", "_" };
        Assert.All(pattern.DrumSequence, drum => Assert.Contains(drum, validDrums));
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void BasicLofiBeatGenerator_ChordProgression_ContainsValidChords()
    {
        // Act
        var pattern = _generator.GeneratePattern();

        // Assert
        var validBaseChords = new[]
        {
            "Fmaj7", "Am7", "Dm7", "G7",
            "Em7", "A7", "Cmaj7"
        };

        // Validate each chord
        foreach (var chord in pattern.ChordProgression)
        {
            // Split chord if it has an inversion
            var parts = chord.Split('/');
            var baseChord = parts[0];

            // Remove any extensions (9, 11, 13) to get base chord
            baseChord = baseChord.Replace("13", "7")
                               .Replace("11", "7")
                               .Replace("9", "7");

            Assert.Contains(baseChord, validBaseChords);
        }
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void ChordProgressions_HaveVariations()
    {
        // Arrange
        var patterns = new List<BeatPattern>();
        var uniqueChords = new HashSet<string>();

        // Act
        // Generate multiple patterns to ensure we get variations
        for (int i = 0; i < 10; i++)
        {
            var pattern = _generator.GeneratePattern();
            patterns.Add(pattern);
            foreach (var chord in pattern.ChordProgression)
            {
                uniqueChords.Add(chord);
            }
        }

        // Assert
        // We should find some variations (inversions or extensions)
        var hasInversions = uniqueChords.Any(c => c.Contains('/'));
        var hasExtensions = uniqueChords.Any(c => c.Contains('9') || c.Contains("11") || c.Contains("13"));
        
        Assert.True(hasInversions || hasExtensions, 
            "Should find chord variations (inversions or extensions) across multiple generations");
    }
} 