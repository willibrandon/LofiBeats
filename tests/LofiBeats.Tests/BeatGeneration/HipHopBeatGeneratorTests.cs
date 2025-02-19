using LofiBeats.Core.BeatGeneration;
using Microsoft.Extensions.Logging;
using Moq;

namespace LofiBeats.Tests.BeatGeneration;

[Collection("AI Generated Tests")]
public class HipHopBeatGeneratorTests
{
    private readonly Mock<ILogger<HipHopBeatGenerator>> _loggerMock;
    private readonly TestableHipHopBeatGenerator _generator;

    public HipHopBeatGeneratorTests()
    {
        _loggerMock = new Mock<ILogger<HipHopBeatGenerator>>();
        _generator = new TestableHipHopBeatGenerator(_loggerMock.Object);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void ModifyPattern_PreservesPatternLength()
    {
        // Arrange
        var pattern = new[] { "kick", "hat", "snare", "hat", "kick", "hat", "snare", "hat" };
        var originalLength = pattern.Length;

        // Act
        _generator.TestModifyPattern(pattern);

        // Assert
        Assert.Equal(originalLength, pattern.Length);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void ModifyPattern_OnlyContainsValidElements()
    {
        // Arrange
        var pattern = new[] { "kick", "hat", "snare", "hat", "kick", "hat", "snare", "hat" };
        var validElements = new HashSet<string> { "kick", "hat", "snare", "_" };

        // Act
        _generator.TestModifyPattern(pattern);

        // Assert
        Assert.All(pattern, element => Assert.Contains(element, validElements));
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void ModifyPattern_MaintainsKickEmphasis()
    {
        // Arrange
        var pattern = new[] { "kick", "hat", "snare", "hat", "kick", "hat", "snare", "hat" };
        var iterations = 100;
        var kickCount = 0;

        // Act
        for (int i = 0; i < iterations; i++)
        {
            var testPattern = pattern.ToArray(); // Create a new copy for each iteration
            _generator.TestModifyPattern(testPattern);
            kickCount += testPattern.Count(x => x == "kick");
        }

        // Assert
        // Due to the extra weight on kicks in the modification logic,
        // we expect the average number of kicks to be relatively high
        var averageKicksPerPattern = (double)kickCount / iterations;
        Assert.True(averageKicksPerPattern >= 2.0, 
            $"Expected average kicks per pattern to be >= 2.0, but got {averageKicksPerPattern:F2}");
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void ModifyPattern_CanCreateKickDoubling()
    {
        // Arrange
        var pattern = new[] { "kick", "hat", "snare", "hat", "kick", "hat", "snare", "hat" };
        var foundDoubleKick = false;
        var iterations = 100;

        // Act
        for (int i = 0; i < iterations && !foundDoubleKick; i++)
        {
            var testPattern = pattern.ToArray();
            _generator.TestModifyPattern(testPattern);
            
            // Check for consecutive kicks
            for (int j = 0; j < testPattern.Length - 1; j++)
            {
                if (testPattern[j] == "kick" && testPattern[j + 1] == "kick")
                {
                    foundDoubleKick = true;
                    break;
                }
            }
        }

        // Assert
        Assert.True(foundDoubleKick, "Expected to find at least one instance of consecutive kicks");
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void ModifyPattern_PreservesOriginalPatternWhenNoVariation()
    {
        // Arrange
        var pattern = new[] { "kick", "hat", "snare", "hat", "kick", "hat", "snare", "hat" };
        var originalPattern = pattern.ToArray();
        
        // Set variation probability to 0
        _generator.SetVariationProbability(0);

        // Act
        _generator.TestModifyPattern(pattern);

        // Assert
        Assert.Equal(originalPattern, pattern);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void ModifyPattern_ModifiesPatternWithHighProbability()
    {
        // Arrange
        var pattern = new[] { "kick", "hat", "snare", "hat", "kick", "hat", "snare", "hat" };
        var originalPattern = pattern.ToArray();
        
        // Set high variation probability
        _generator.SetVariationProbability(1.0f);
        _generator.SetStepModificationProbability(1.0f);

        // Act
        _generator.TestModifyPattern(pattern);

        // Assert
        Assert.NotEqual(originalPattern, pattern);
    }

    /// <summary>
    /// Test helper class to expose protected members for testing
    /// </summary>
    private sealed class TestableHipHopBeatGenerator : HipHopBeatGenerator
    {
        private float _variationProbability = 0.35f;
        private float _stepModificationProbability = 0.3f;

        public TestableHipHopBeatGenerator(ILogger<HipHopBeatGenerator> logger) : base(logger)
        {
        }

        public void TestModifyPattern(string[] pattern)
        {
            ModifyPattern(pattern);
        }

        public void SetVariationProbability(float probability)
        {
            _variationProbability = probability;
        }

        public void SetStepModificationProbability(float probability)
        {
            _stepModificationProbability = probability;
        }

        protected override float GetVariationProbability() => _variationProbability;
        protected override float GetStepModificationProbability() => _stepModificationProbability;
    }
} 