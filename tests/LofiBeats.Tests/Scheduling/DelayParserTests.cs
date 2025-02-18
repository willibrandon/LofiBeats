using LofiBeats.Core.Scheduling;

namespace LofiBeats.Tests.Scheduling;

[Collection("AI Generated Tests")]
public class DelayParserTests
{
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [Trait("Category", "AI_Generated")]
    public void ParseDelay_WithEmptyOrWhitespace_ReturnsNull(string input)
    {
        // Act
        var result = DelayParser.ParseDelay(input);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void ParseDelay_WithNull_ReturnsNull()
    {
        // Arrange
        string? nullString = null;

        // Act
        var result = DelayParser.ParseDelay(nullString);

        // Assert
        Assert.Null(result);
    }

    [Theory]
    [InlineData("30m", 30)]
    [InlineData("1m", 1)]
    [InlineData("60m", 60)]
    [InlineData(" 45m ", 45)] // Test with whitespace
    [InlineData("45M", 45)]   // Test with uppercase
    [Trait("Category", "AI_Generated")]
    public void ParseDelay_WithValidMinutes_ReturnsCorrectTimeSpan(string input, int expectedMinutes)
    {
        // Act
        var result = DelayParser.ParseDelay(input);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(TimeSpan.FromMinutes(expectedMinutes), result.Value);
    }

    [Theory]
    [InlineData("30s", 30)]
    [InlineData("1s", 1)]
    [InlineData("60s", 60)]
    [InlineData(" 45s ", 45)] // Test with whitespace
    [InlineData("45S", 45)]   // Test with uppercase
    [Trait("Category", "AI_Generated")]
    public void ParseDelay_WithValidSeconds_ReturnsCorrectTimeSpan(string input, int expectedSeconds)
    {
        // Act
        var result = DelayParser.ParseDelay(input);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(TimeSpan.FromSeconds(expectedSeconds), result.Value);
    }

    [Theory]
    [InlineData("1h", 1)]
    [InlineData("24h", 24)]
    [InlineData(" 12h ", 12)] // Test with whitespace
    [InlineData("12H", 12)]   // Test with uppercase
    [Trait("Category", "AI_Generated")]
    public void ParseDelay_WithValidHours_ReturnsCorrectTimeSpan(string input, int expectedHours)
    {
        // Act
        var result = DelayParser.ParseDelay(input);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(TimeSpan.FromHours(expectedHours), result.Value);
    }

    [Theory]
    [InlineData("30x")]      // Invalid unit
    [InlineData("m")]        // Missing number
    [InlineData("s")]        // Missing number
    [InlineData("h")]        // Missing number
    [InlineData("abc")]      // No unit
    [InlineData("30")]       // No unit
    [InlineData("30mm")]     // Double unit
    [InlineData("30.5m")]    // Decimal not supported
    [InlineData("ms")]       // Invalid format
    [Trait("Category", "AI_Generated")]
    public void ParseDelay_WithInvalidInput_ReturnsNull(string input)
    {
        // Act
        var result = DelayParser.ParseDelay(input);

        // Assert
        Assert.Null(result);
    }

    [Theory]
    [InlineData("2147483648m")] // Int32.MaxValue + 1
    [InlineData("2147483648s")] // Int32.MaxValue + 1
    [InlineData("2147483648h")] // Int32.MaxValue + 1
    [Trait("Category", "AI_Generated")]
    public void ParseDelay_WithOverflowValues_ReturnsNull(string input)
    {
        // Act
        var result = DelayParser.ParseDelay(input);

        // Assert
        Assert.Null(result);
    }
} 