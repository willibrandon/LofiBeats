using LofiBeats.Core.BeatGeneration;
using Xunit;

namespace LofiBeats.Tests.BeatGeneration;

public class KeyHelperTests
{
    [Theory]
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
} 