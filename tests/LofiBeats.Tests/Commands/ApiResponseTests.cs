using LofiBeats.Cli.Commands;

namespace LofiBeats.Tests.Commands;

[Collection("AI Generated Tests")]
public class ApiResponseTests
{
    [Fact]
    [Trait("Category", "AI_Generated")]
    public void ApiResponse_WithMessageOnly_InitializesCorrectly()
    {
        // Arrange & Act
        var response = new ApiResponse { Message = "Test message" };

        // Assert
        Assert.Equal("Test message", response.Message);
        Assert.Null(response.Error);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void ApiResponse_WithErrorOnly_InitializesCorrectly()
    {
        // Arrange & Act
        var response = new ApiResponse { Error = "Test error" };

        // Assert
        Assert.Null(response.Message);
        Assert.Equal("Test error", response.Error);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void ApiResponse_WithBothMessageAndError_InitializesCorrectly()
    {
        // Arrange & Act
        var response = new ApiResponse 
        { 
            Message = "Test message",
            Error = "Test error"
        };

        // Assert
        Assert.Equal("Test message", response.Message);
        Assert.Equal("Test error", response.Error);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void PlayResponse_WithAllProperties_InitializesCorrectly()
    {
        // Arrange & Act
        var pattern = new { Tempo = 120, Style = "jazzy" };
        var response = new PlayResponse 
        { 
            Message = "Test message",
            Error = "Test error",
            Pattern = pattern
        };

        // Assert
        Assert.Equal("Test message", response.Message);
        Assert.Equal("Test error", response.Error);
        Assert.NotNull(response.Pattern);
        var patternObj = Assert.IsType<object>(response.Pattern, exactMatch: false);
        Assert.Equal(pattern, patternObj);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void PlayResponse_WithNullPattern_InitializesCorrectly()
    {
        // Arrange & Act
        var response = new PlayResponse 
        { 
            Message = "Test message",
            Pattern = null
        };

        // Assert
        Assert.Equal("Test message", response.Message);
        Assert.Null(response.Pattern);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void ApiResponse_WithNoProperties_InitializesWithNulls()
    {
        // Arrange & Act
        var response = new ApiResponse();

        // Assert
        Assert.Null(response.Message);
        Assert.Null(response.Error);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void PlayResponse_WithNoProperties_InitializesWithNulls()
    {
        // Arrange & Act
        var response = new PlayResponse();

        // Assert
        Assert.Null(response.Message);
        Assert.Null(response.Error);
        Assert.Null(response.Pattern);
    }
} 