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

    [Theory]
    [Trait("Category", "AI_Generated")]
    [InlineData(new[] { "Ebmaj7", "Abm7", "Dbmaj7", "Bb7" }, "Eb", "D", new[] { "Dmaj7", "Gm7", "Cmaj7", "A7" })]
    [InlineData(new[] { "Gbmaj7", "Bbm7", "Ebmaj7", "F7" }, "Gb", "F#", new[] { "F#maj7", "Am7", "D#maj7", "E7" })]
    [InlineData(new[] { "Abm7", "Db7", "Gbmaj7", "Ebm7" }, "Gb", "G", new[] { "Am7", "D7", "Gmaj7", "Em7" })]
    public void TransposeChords_HandlesComplexEnharmonicTranspositions(string[] input, string fromKey, string toKey, string[] expected)
    {
        // Act
        var result = ChordTransposer.TransposeChords(input, fromKey, toKey);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [Trait("Category", "AI_Generated")]
    [InlineData(new[] { "Ebmaj7/Bb", "Abm7/Eb", "Dbmaj7/Ab", "Bb7/F" }, "Eb", "D", 
                new[] { "Dmaj7/A", "Gm7/D", "Cmaj7/G", "A7/E" })]
    [InlineData(new[] { "Gbmaj7/Db", "Bbm7/F", "Ebmaj7/Bb", "F7/C" }, "Gb", "F#", 
                new[] { "F#maj7/C#", "Am7/E", "D#maj7/A#", "E7/B" })]
    public void TransposeChords_HandlesEnharmonicInversions(string[] input, string fromKey, string toKey, string[] expected)
    {
        // Act
        var result = ChordTransposer.TransposeChords(input, fromKey, toKey);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [Trait("Category", "AI_Generated")]
    [InlineData(new[] { "Ebm9", "Ab13", "Dbmaj9", "Bbm11" }, "Eb", "D", 
                new[] { "Dm9", "G13", "Cmaj9", "Am11" })]
    [InlineData(new[] { "Gbmaj13", "Bbm9", "Ebmaj11", "F7b9" }, "Gb", "F#", 
                new[] { "F#maj13", "Am9", "D#maj11", "E7b9" })]
    public void TransposeChords_HandlesEnharmonicExtensions(string[] input, string fromKey, string toKey, string[] expected)
    {
        // Act
        var result = ChordTransposer.TransposeChords(input, fromKey, toKey);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [Trait("Category", "AI_Generated")]
    [InlineData("Eb", "D#")]
    [InlineData("Bb", "A#")]
    [InlineData("Cb", "B")]
    [InlineData("Db", "C#")]
    [InlineData("Gb", "F#")]
    [InlineData("Ab", "G#")]
    public void TestEnharmonicAcceptance(string input, string expectedNormalized)
    {
        // Act
        Assert.True(KeyHelper.IsValidKey(input, out var norm));
        
        // Assert
        Assert.Equal(expectedNormalized, norm);
    }

    [Theory]
    [Trait("Category", "AI_Generated")]
    [InlineData(new[] { "C", "Dm7" }, "C", "F", new[] { "F", "Gm7" })]
    [InlineData(new[] { "Cmaj7" }, "C", "D", new[] { "Dmaj7" })]
    [InlineData(new[] { "Bbm7", "Eb7" }, "Bb", "C", new[] { "Cm7", "F7" })]
    public void TestBasicTranspositions(string[] chords, string fromKey, string toKey, string[] expected)
    {
        // Act
        var transposed = ChordTransposer.TransposeChords(chords, fromKey, toKey);
        
        // Assert
        Assert.Equal(expected, transposed);
    }
} 