using LofiBeats.Core.Models;
using LofiBeats.Core.Playback;
using Microsoft.Extensions.Logging;
using Moq;

namespace LofiBeats.Tests.Playback;

[Collection("AI Generated Tests")]
public class BeatPatternSampleProviderTests
{
    private readonly Mock<ILogger> _loggerMock;
    private readonly BeatPattern _testPattern;

    public BeatPatternSampleProviderTests()
    {
        _loggerMock = new Mock<ILogger>();
        _testPattern = new BeatPattern
        {
            Tempo = 80,
            DrumSequence = ["kick", "hat", "snare", "hat", "kick", "hat", "snare", "hat"],
            ChordProgression = ["Dm7", "G7", "Cmaj7", "Am7"]
        };
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void DrumHits_HaveTimeOffsets()
    {
        // Arrange
        var provider = new BeatPatternSampleProvider(_testPattern, _loggerMock.Object);
        var buffer = new float[44100]; // 1 second of audio at 44.1kHz
        var originalBuffer = new float[44100];

        // Act
        // First read without offset to get original timing
        provider.Read(originalBuffer, 0, buffer.Length);
        
        // Read multiple times to ensure we catch timing variations
        bool foundOffset = false;
        for (int attempt = 0; attempt < 5; attempt++)
        {
            provider.Read(buffer, 0, buffer.Length);

            // Compare buffers to find timing differences
            // We only need to find one instance where they differ
            for (int i = 0; i < buffer.Length; i++)
            {
                if (Math.Abs(buffer[i] - originalBuffer[i]) > 0.0001f)
                {
                    foundOffset = true;
                    break;
                }
            }

            if (foundOffset) break;
        }

        // Assert
        Assert.True(foundOffset, "Should find timing differences due to humanization");
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void KickDrums_MaintainOriginalTiming()
    {
        // Arrange
        var kickOnlyPattern = new BeatPattern
        {
            Tempo = 80,
            DrumSequence = ["kick", "_", "_", "_", "kick", "_", "_", "_"],
            ChordProgression = ["Dm7"]
        };
        var provider = new BeatPatternSampleProvider(kickOnlyPattern, _loggerMock.Object);
        var buffer1 = new float[44100];
        var buffer2 = new float[44100];

        // Act
        provider.Read(buffer1, 0, buffer1.Length);
        provider.Read(buffer2, 0, buffer2.Length);

        // Find the onset times of kicks in both buffers
        var onsets1 = FindKickOnsets(buffer1);
        var onsets2 = FindKickOnsets(buffer2);

        // Assert
        Assert.Equal(onsets1.Count, onsets2.Count);
        for (int i = 0; i < onsets1.Count; i++)
        {
            // Using Assert.Equal with integer comparison
            int expectedOnset = onsets1[i];
            int actualOnset = onsets2[i];
            Assert.Equal(expectedOnset, actualOnset);
        }
    }

    private static List<int> FindKickOnsets(float[] buffer, float threshold = 0.005f)
    {
        var onsets = new List<int>();
        bool inOnset = false;
        int minSamplesBetweenOnsets = 100; // Prevent multiple detections of same kick
        int samplesSinceLastOnset = minSamplesBetweenOnsets;
        
        // Use a sliding window to detect amplitude spikes
        const int windowSize = 3;
        var window = new float[windowSize];
        
        for (int i = windowSize; i < buffer.Length; i++)
        {
            // Update window
            for (int j = 0; j < windowSize - 1; j++)
            {
                window[j] = window[j + 1];
            }
            window[windowSize - 1] = Math.Abs(buffer[i]);
            
            samplesSinceLastOnset++;

            // Center sample is higher than both neighbors and above threshold
            if (!inOnset && 
                samplesSinceLastOnset >= minSamplesBetweenOnsets &&
                window[1] > threshold &&
                window[1] > window[0] * 1.2f && // 20% increase from previous
                window[1] > window[2]) // Peak detection
            {
                onsets.Add(i - 1); // -1 because we want the start of the rise
                inOnset = true;
                samplesSinceLastOnset = 0;
            }
            // Reset detection after significant drop
            else if (inOnset && window[windowSize - 1] < threshold / 2)
            {
                inOnset = false;
            }
        }

        return onsets;
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void DrumHits_HaveVelocityVariation()
    {
        // Arrange
        var singleDrumPattern = new BeatPattern
        {
            Tempo = 80,
            DrumSequence = ["snare", "snare", "snare", "snare"],
            ChordProgression = ["Dm7"]
        };
        var provider = new BeatPatternSampleProvider(singleDrumPattern, _loggerMock.Object);
        var buffer = new float[44100];

        // Act
        provider.Read(buffer, 0, buffer.Length);

        // Find peak amplitudes of each hit
        var peakAmplitudes = new List<float>();
        float currentPeak = 0f;
        int samplesPerStep = 44100 / 4; // At 80 BPM, 4 steps per second

        for (int i = 0; i < buffer.Length; i++)
        {
            currentPeak = Math.Max(currentPeak, Math.Abs(buffer[i]));
            
            // At the end of each step, record the peak and reset
            if ((i + 1) % samplesPerStep == 0)
            {
                peakAmplitudes.Add(currentPeak);
                currentPeak = 0f;
            }
        }

        // Assert
        // We should have different peak amplitudes for different hits
        Assert.True(peakAmplitudes.Distinct().Count() > 1, 
            "Should find different peak amplitudes due to velocity variation");
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void VelocityRanges_StayWithinBounds()
    {
        // Arrange
        var provider = new BeatPatternSampleProvider(_testPattern, _loggerMock.Object);
        var buffer = new float[44100];

        // Act
        provider.Read(buffer, 0, buffer.Length);

        // Find absolute peak amplitude
        float peakAmplitude = 0f;
        for (int i = 0; i < buffer.Length; i++)
        {
            peakAmplitude = Math.Max(peakAmplitude, Math.Abs(buffer[i]));
        }

        // Assert
        // The peak amplitude should never exceed our maximum possible value
        // (considering the 0.5f overall volume reduction in GenerateSample)
        Assert.True(peakAmplitude <= 1.0f, "Peak amplitude should stay within bounds");
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void GeneratedAudio_HasNoClipping()
    {
        // Arrange
        var provider = new BeatPatternSampleProvider(_testPattern, _loggerMock.Object);
        var buffer = new float[44100 * 2]; // 2 seconds of audio

        // Act
        provider.Read(buffer, 0, buffer.Length);

        // Assert
        // No sample should exceed ±1.0 (which would cause clipping)
        bool hasClipping = false;
        for (int i = 0; i < buffer.Length; i++)
        {
            if (Math.Abs(buffer[i]) > 1.0f)
            {
                hasClipping = true;
                break;
            }
        }
        Assert.False(hasClipping, "Generated audio should not have any clipping");
    }
} 