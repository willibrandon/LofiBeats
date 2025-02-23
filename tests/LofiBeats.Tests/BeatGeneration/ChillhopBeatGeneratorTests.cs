using LofiBeats.Core.BeatGeneration;
using Microsoft.Extensions.Logging;
using Moq;

namespace LofiBeats.Tests.BeatGeneration;

[Collection("AI Generated Tests")]
public class ChillhopBeatGeneratorTests
{
    private readonly Mock<ILogger<ChillhopBeatGenerator>> _loggerMock;
    private readonly TestChillhopBeatGenerator _generator;

    public ChillhopBeatGeneratorTests()
    {
        _loggerMock = new Mock<ILogger<ChillhopBeatGenerator>>();
        _generator = new TestChillhopBeatGenerator(_loggerMock.Object);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void ModifyPattern_WithControlledRandom_ModifiesPatternCorrectly()
    {
        // Arrange
        string[] pattern = ["kick", "hat", "snare", "hat"];
        _generator.SetRandomValues(
            variationCheck: 0.1f,      // Less than 0.2f to trigger variation
            modificationChecks: [0.1f], // Less than 0.15f to trigger modification
            nextValues: [0]            // Will select "kick" from the switch
        );

        // Act
        _generator.TestModifyPattern(pattern);

        // Assert
        Assert.Equal("kick", pattern[0]); // First element should be modified to "kick"
        Assert.Equal(["kick", "hat", "snare", "hat"], pattern); // Rest should be unchanged
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void ModifyPattern_WithControlledRandomForRest_ModifiesPatternToRest()
    {
        // Arrange
        string[] pattern = ["kick", "hat", "snare", "hat"];
        _generator.SetRandomValues(
            variationCheck: 0.1f,      // Less than 0.2f to trigger variation
            modificationChecks: [0.1f], // Less than 0.15f to trigger modification
            nextValues: [3]            // Will select "_" (rest) from the switch
        );

        // Act
        _generator.TestModifyPattern(pattern);

        // Assert
        Assert.Equal("_", pattern[0]); // First element should be modified to rest
        Assert.Equal(["_", "hat", "snare", "hat"], pattern); // Rest should be unchanged
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void ModifyPattern_WithHighVariationProbability_SkipsModification()
    {
        // Arrange
        string[] pattern = ["kick", "hat", "snare", "hat"];
        string[] expected = ["kick", "hat", "snare", "hat"];
        _generator.SetRandomValues(
            variationCheck: 0.3f,      // Higher than 0.2f to skip variation
            modificationChecks: [],     // Not used since variation is skipped
            nextValues: []             // Not used since variation is skipped
        );

        // Act
        _generator.TestModifyPattern(pattern);

        // Assert
        Assert.Equal(expected, pattern); // Pattern should remain unchanged
    }

    /// <summary>
    /// Test implementation of ChillhopBeatGenerator that allows controlling random values
    /// </summary>
    private sealed class TestChillhopBeatGenerator : ChillhopBeatGenerator
    {
        private readonly Queue<double> _randomDoubles = new();
        private readonly Queue<int> _randomInts = new();

        public TestChillhopBeatGenerator(ILogger<ChillhopBeatGenerator> logger) : base(logger)
        {
        }

        public void SetRandomValues(float variationCheck, float[] modificationChecks, int[] nextValues)
        {
            _randomDoubles.Clear();
            _randomInts.Clear();

            // Add variation check
            _randomDoubles.Enqueue(variationCheck);

            // Add modification checks
            foreach (var check in modificationChecks)
            {
                _randomDoubles.Enqueue(check);
            }

            // Add next values for switch cases
            foreach (var value in nextValues)
            {
                _randomInts.Enqueue(value);
            }
        }

        public void TestModifyPattern(string[] pattern)
        {
            // Replace the random number generator with our controlled version
            var field = typeof(BaseBeatGenerator).GetField("_rnd", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field?.SetValue(this, new ControlledRandom(_randomDoubles, _randomInts));

            // Call the protected method
            ModifyPattern(pattern);
        }

        private sealed class ControlledRandom : Random
        {
            private readonly Queue<double> _doubles;
            private readonly Queue<int> _ints;

            public ControlledRandom(Queue<double> doubles, Queue<int> ints)
            {
                _doubles = doubles;
                _ints = ints;
            }

            public override double NextDouble()
            {
                return _doubles.TryDequeue(out var value) ? value : 1.0;
            }

            public override int Next(int maxValue)
            {
                return _ints.TryDequeue(out var value) ? value : 0;
            }
        }
    }
} 