using LofiBeats.Core.BeatGeneration;
using Microsoft.Extensions.Logging;
using Xunit;
using Moq;

namespace LofiBeats.Tests.BeatGeneration;

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
    public void BasicLofiBeatGenerator_ReturnsPattern()
    {
        // Act
        var pattern = _generator.GeneratePattern();

        // Assert
        Assert.NotNull(pattern);
        Assert.True(pattern.Tempo >= 70 && pattern.Tempo <= 90);
        Assert.NotNull(pattern.DrumSequence);
        Assert.NotNull(pattern.ChordProgression);
    }

    [Fact]
    public void BasicLofiBeatGenerator_DrumSequence_HasValidLength()
    {
        // Act
        var pattern = _generator.GeneratePattern();

        // Assert
        Assert.Equal(8, pattern.DrumSequence.Length);
    }

    [Fact]
    public void BasicLofiBeatGenerator_ChordProgression_HasValidLength()
    {
        // Act
        var pattern = _generator.GeneratePattern();

        // Assert
        Assert.Equal(4, pattern.ChordProgression.Length);
    }

    [Fact]
    public void BasicLofiBeatGenerator_DrumSequence_ContainsValidElements()
    {
        // Act
        var pattern = _generator.GeneratePattern();

        // Assert
        var validDrums = new[] { "kick", "hat", "snare", "_" };
        Assert.All(pattern.DrumSequence, drum => Assert.Contains(drum, validDrums));
    }

    [Fact]
    public void BasicLofiBeatGenerator_ChordProgression_ContainsValidChords()
    {
        // Act
        var pattern = _generator.GeneratePattern();

        // Assert
        var validChords = new[]
        {
            "Fmaj7", "Am7", "Dm7", "G7",
            "Em7", "A7",
            "Cmaj7"
        };
        Assert.All(pattern.ChordProgression, chord => Assert.Contains(chord, validChords));
    }
} 