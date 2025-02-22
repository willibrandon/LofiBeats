using LofiBeats.Core.BeatGeneration;

namespace LofiBeats.Tests.BeatGeneration;

[Collection("AI Generated Tests")]
public class KeyHelperTests
{
    [Theory]
    [Trait("Category", "AI_Generated")]
    [InlineData("C", "C", true)]
    [InlineData("f#", "F#", true)]
    [InlineData("Bb", "A#", true)]
    [InlineData("db", "C#", true)]
    [InlineData("XYZ", "", false)]
    [InlineData("", "", false)]
    [InlineData(null, "", false)]
    [InlineData("H", "", false)]
    public void IsValidKey_ValidatesAndNormalizesKeys(string? input, string expectedNormalized, bool expectedValid)
    {
        // Act
        bool isValid = KeyHelper.IsValidKey(input!, out string normalized);

        // Assert
        Assert.Equal(expectedValid, isValid);
        Assert.Equal(expectedNormalized, normalized);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void GetValidKeys_ReturnsAllTwelveKeys()
    {
        // Act
        var keys = KeyHelper.GetValidKeys();

        // Assert
        Assert.Equal(12, keys.Length);
        Assert.Contains("C", keys);
        Assert.Contains("F#", keys);
        Assert.Contains("B", keys);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void GetValidKeyList_ReturnsFormattedString()
    {
        // Act
        var list = KeyHelper.GetValidKeyList();

        // Assert
        Assert.Contains("Natural keys:", list);
        Assert.Contains("Sharp keys:", list);
        Assert.Contains("Flat keys:", list);
        Assert.Contains("C", list);
        Assert.Contains("F#", list);
        Assert.Contains("Bb", list);
    }

    [Theory]
    [Trait("Category", "AI_Generated")]
    [InlineData("Eb", "D#")]
    [InlineData("Bb", "A#")]
    [InlineData("Db", "C#")]
    [InlineData("Gb", "F#")]
    [InlineData("Ab", "G#")]
    [InlineData("eb", "D#")]  // Test case sensitivity
    [InlineData("bb", "A#")]  // Test case sensitivity
    [InlineData("DB", "C#")]  // Test case sensitivity
    public void IsValidKey_HandlesAllEnharmonicEquivalents(string input, string expectedNormalized)
    {
        // Act
        bool isValid = KeyHelper.IsValidKey(input, out string normalized);

        // Assert
        Assert.True(isValid);
        Assert.Equal(expectedNormalized, normalized);
    }
}
