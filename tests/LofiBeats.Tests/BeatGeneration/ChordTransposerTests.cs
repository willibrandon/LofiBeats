using Xunit;
using LofiBeats.Core.BeatGeneration;

namespace LofiBeats.Tests.BeatGeneration;

[Collection("AI Generated Tests")]
public class ChordTransposerTests
{
    [Theory]
    [Trait("Category", "AI_Generated")]
    [InlineData(new[] { "Cmaj7", "Am7", "Fmaj7", "G7" }, "C", "F", new[] { "Fmaj7", "Dm7", "Bbmaj7", "C7" })]
    [InlineData(new[] { "Dm7", "G7", "Cmaj7", "Am7" }, "C", "D", new[] { "Em7", "A7", "Dmaj7", "Bm7" })]
    [InlineData(new[] { "Em7", "A7", "Dm7", "G7" }, "C", "F#", new[] { "A#m7", "D#7", "G#m7", "C#7" })]
    public void TransposeChords_TransposesCorrectly(string[] input, string fromKey, string toKey, string[] expected)
    {
        // Act
        var result = ChordTransposer.TransposeChords(input, fromKey, toKey);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [Trait("Category", "AI_Generated")]
    [InlineData(new[] { "Cmaj7", "Am7", "Fmaj7", "G7" }, "C", "C")]
    [InlineData(new[] { "Dm7", "G7", "Cmaj7", "Am7" }, "D", "D")]
    public void TransposeChords_SameKey_ReturnsOriginal(string[] input, string fromKey, string toKey)
    {
        // Act
        var result = ChordTransposer.TransposeChords(input, fromKey, toKey);

        // Assert
        Assert.Equal(input, result);
    }

    [Theory]
    [Trait("Category", "AI_Generated")]
    [InlineData(new[] { "Bbmaj7", "Gm7", "Ebmaj7", "F7" }, "Bb", "C", new[] { "Cmaj7", "Am7", "Fmaj7", "G7" })]
    [InlineData(new[] { "Ebm7", "Ab7", "Dbmaj7", "Bbm7" }, "Db", "D", new[] { "Em7", "A7", "Dmaj7", "Bm7" })]
    public void TransposeChords_HandlesFlatsCorrectly(string[] input, string fromKey, string toKey, string[] expected)
    {
        // Act
        var result = ChordTransposer.TransposeChords(input, fromKey, toKey);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [Trait("Category", "AI_Generated")]
    [InlineData(new[] { "Cmaj7/G", "Am7/E", "Fmaj7/C", "G7/D" }, "C", "F", new[] { "Fmaj7/C", "Dm7/A", "Bbmaj7/F", "C7/G" })]
    [InlineData(new[] { "Dm9", "G13", "Cmaj9", "Am11" }, "C", "D", new[] { "Em9", "A13", "Dmaj9", "Bm11" })]
    public void TransposeChords_PreservesExtensionsAndInversions(string[] input, string fromKey, string toKey, string[] expected)
    {
        // Act
        var result = ChordTransposer.TransposeChords(input, fromKey, toKey);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void TransposeChords_HandlesEmptyInput()
    {
        // Arrange
        string[] emptyArray = [];
        string[]? nullArray = null;

        // Act & Assert
        Assert.Empty(ChordTransposer.TransposeChords(emptyArray, "C", "F"));
        Assert.Empty(ChordTransposer.TransposeChords(nullArray ?? [], "C", "F"));
    }

    [Theory]
    [Trait("Category", "AI_Generated")]
    [InlineData("H", "C")]
    [InlineData("C", "H")]
    public void TransposeChords_ThrowsOnInvalidKey(string fromKey, string toKey)
    {
        // Arrange
        var chords = new[] { "Cmaj7", "Am7" };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => ChordTransposer.TransposeChords(chords, fromKey, toKey));
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void TransposeChords_ThrowsOnInvalidChord()
    {
        // Arrange
        var invalidChords1 = new[] { "Hmaj7", "Xm7" };
        var invalidChords2 = new[] { "invalid" };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => ChordTransposer.TransposeChords(invalidChords1));
        Assert.Throws<ArgumentException>(() => ChordTransposer.TransposeChords(invalidChords2));
    }
} 