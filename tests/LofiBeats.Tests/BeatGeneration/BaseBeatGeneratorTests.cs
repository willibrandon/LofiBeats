using LofiBeats.Core.BeatGeneration;
using Microsoft.Extensions.Logging;
using Moq;
using System.Runtime.CompilerServices;

namespace LofiBeats.Tests.BeatGeneration;

[Collection("AI Generated Tests")]
public class BaseBeatGeneratorTests
{
    private readonly Mock<ILogger<TestBeatGenerator>> _loggerMock;
    private readonly TestBeatGenerator _generator;

    public BaseBeatGeneratorTests()
    {
        _loggerMock = new Mock<ILogger<TestBeatGenerator>>();
        _generator = new TestBeatGenerator(_loggerMock.Object);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void SetBPM_WithValidValue_SetsBPM()
    {
        // Arrange
        var validBpm = (_generator.BpmRange.MinBpm + _generator.BpmRange.MaxBpm) / 2;

        // Act
        _generator.SetBPM(validBpm);

        // Assert
        Assert.Equal(validBpm, _generator.CurrentBPM);
    }

    [Theory]
    [Trait("Category", "AI_Generated")]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1000)]
    public void SetBPM_WithInvalidValue_ThrowsException(int invalidBpm)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => _generator.SetBPM(invalidBpm));
        Assert.Contains("BPM must be between", exception.Message);
    }

    [Theory]
    [Trait("Category", "AI_Generated")]
    [InlineData("C", "Eb")]
    [InlineData("D", "F")]
    [InlineData("E", "G")]
    [InlineData("F", "Ab")]
    [InlineData("G", "Bb")]
    [InlineData("A", "C")]
    [InlineData("B", "D")]
    [InlineData("X", "X")] // Test default case
    public void GetMinorThird_ReturnsCorrectNote(string root, string expected)
    {
        // Act
        var result = TestBeatGenerator.TestGetMinorThird(root);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [Trait("Category", "AI_Generated")]
    [InlineData("C", "E")]
    [InlineData("D", "F#")]
    [InlineData("E", "G#")]
    [InlineData("F", "A")]
    [InlineData("G", "B")]
    [InlineData("A", "C#")]
    [InlineData("B", "D#")]
    [InlineData("X", "X")] // Test default case
    public void GetMajorThird_ReturnsCorrectNote(string root, string expected)
    {
        // Act
        var result = TestBeatGenerator.TestGetMajorThird(root);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [Trait("Category", "AI_Generated")]
    [InlineData("C", "G")]
    [InlineData("D", "A")]
    [InlineData("E", "B")]
    [InlineData("F", "C")]
    [InlineData("G", "D")]
    [InlineData("A", "E")]
    [InlineData("B", "F#")]
    [InlineData("X", "X")] // Test default case
    public void GetFifth_ReturnsCorrectNote(string root, string expected)
    {
        // Act
        var result = TestBeatGenerator.TestGetFifth(root);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [Trait("Category", "AI_Generated")]
    [InlineData("Cm7", "C", "m7")]
    [InlineData("F#maj7", "F#", "maj7")]
    [InlineData("Bb7", "Bb", "7")]
    [InlineData("", "", "")] // Test empty string
    public void ParseChord_ReturnsCorrectParts(string chord, string expectedRoot, string expectedQuality)
    {
        // Act
        var (Root, Quality) = TestBeatGenerator.TestParseChord(chord);

        // Assert
        Assert.Equal(expectedRoot, Root);
        Assert.Equal(expectedQuality, Quality);
    }

    [Theory]
    [Trait("Category", "AI_Generated")]
    [InlineData("X", new[] { "X" })]             // Invalid chord
    [InlineData("Cmaj7", new[] { "E", "G" })]    // Major chord
    [InlineData("Cm7", new[] { "Eb", "G" })]     // Minor chord
    public void GetPossibleBassNotes_ReturnsCorrectNotes(string chord, string[] expectedNotes)
    {
        // Act
        var result = TestBeatGenerator.TestGetPossibleBassNotes(chord);

        // Assert
        Assert.Equal(expectedNotes, result);
    }

    [Theory]
    [Trait("Category", "AI_Generated")]
    [InlineData("Cm7", true)]      // Should add inversion
    [InlineData("Cmaj7", true)]    // Should add inversion
    [InlineData("C7", true)]       // Should add inversion
    public void AddInversion_ModifiesChordAppropriately(string chord, bool shouldModify)
    {
        // Arrange
        _generator.SetRandomSeed(42); // For reproducible tests

        // Act
        var result = _generator.TestAddInversion(chord);

        // Assert
        if (shouldModify)
        {
            Assert.Contains('/', result);
            Assert.StartsWith(chord, result);
        }
        else
        {
            Assert.Equal(chord, result);
        }
    }

    [Theory]
    [Trait("Category", "AI_Generated")]
    [InlineData("Cmaj7", true)]    // Should add extension
    [InlineData("Cm7", true)]      // Should add extension
    [InlineData("C7", true)]       // Should add extension
    [InlineData("Cmaj9", false)]   // Already has extension
    [InlineData("Cm11", false)]    // Already has extension
    public void AddExtension_ModifiesChordAppropriately(string chord, bool shouldModify)
    {
        // Arrange
        _generator.SetRandomSeed(42); // For reproducible tests

        // Act
        var result = _generator.TestAddExtension(chord);

        // Assert
        if (shouldModify)
        {
            Assert.True(result.Contains('9') || result.Contains("11") || result.Contains("13"), 
                "Expected chord to have an extension (9, 11, or 13)");
            Assert.StartsWith("C", result);
        }
        else
        {
            Assert.Equal(chord, result);
        }
    }

    /// <summary>
    /// Test implementation of BaseBeatGenerator
    /// </summary>
    public sealed class TestBeatGenerator(ILogger logger) : BaseBeatGenerator(logger)
    {
        public override string Style => "test";
        public override (int MinBpm, int MaxBpm) BpmRange => (60, 120);
        public int CurrentBPM => _bpm;

        public void SetRandomSeed(int seed)
        {
            // For testing, we need to reset the random number generator
            typeof(BaseBeatGenerator)
                .GetField("_rnd", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(this, new Random(seed));
        }

        protected override string[][] DefineChordProgressions() => 
        [
            ["Cm7", "Fm7", "Bb7", "Eb7"]
        ];

        protected override string[][] DefineDrumPatterns() =>
        [
            ["kick", "hat", "snare", "hat"]
        ];

        protected override void ModifyPattern(string[] pattern)
        {
            // No modifications for test implementation
        }

        // Expose protected methods for testing
        public string TestAddInversion(string chord) => AddInversion(chord);
        public string TestAddExtension(string chord) => AddExtension(chord);
        public static string TestGetMinorThird(string root) => GetMinorThird(root);
        public static string TestGetMajorThird(string root) => GetMajorThird(root);
        public static string TestGetFifth(string root) => GetFifth(root);
        public static (string Root, string Quality) TestParseChord(string chord) => ParseChord(chord);
        public static string[] TestGetPossibleBassNotes(string chord)
        {
            var (root, quality) = ParseChord(chord);
            return GetPossibleBassNotes(root, quality);
        }
    }
} 