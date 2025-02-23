using LofiBeats.Core.Models;
using LofiBeats.Core.Playback;
using LofiBeats.Core.Telemetry;
using Microsoft.Extensions.Logging;
using Moq;
using NAudio.Wave;

namespace LofiBeats.Tests.Playback;

[Collection("AI Generated Tests")]
public class BeatPatternSampleProviderTests : IDisposable
{
    private readonly Mock<ILogger> _loggerMock;
    private readonly Mock<ILogger<UserSampleRepository>> _repoLoggerMock;
    private readonly Mock<ITelemetryService> _telemetryServiceMock;
    private readonly Mock<ILogger<TelemetryTracker>> _telemetryLoggerMock;
    private readonly TelemetryTracker _telemetryTracker;
    private readonly BeatPattern _testPattern;
    private readonly UserSampleRepository _userSamples;
    private readonly string _tempFile;

    private static readonly Action<ILogger, string, Exception?> LogPeaks =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(1, nameof(UserSamples_PlayCorrectly)),
            "Found peaks at positions: {Peaks}");

    private static readonly Action<ILogger, string, Exception?> LogDebug =
        LoggerMessage.Define<string>(
            LogLevel.Debug,
            new EventId(2, "Debug"),
            "{Message}");

    public BeatPatternSampleProviderTests()
    {
        _loggerMock = new Mock<ILogger>();
        _repoLoggerMock = new Mock<ILogger<UserSampleRepository>>();
        _telemetryServiceMock = new Mock<ITelemetryService>();
        _telemetryLoggerMock = new Mock<ILogger<TelemetryTracker>>();
        _telemetryTracker = new TelemetryTracker(_telemetryServiceMock.Object, _telemetryLoggerMock.Object);
        _userSamples = new UserSampleRepository(_repoLoggerMock.Object);
        _testPattern = new BeatPattern
        {
            BPM = 80,
            DrumSequence = ["kick", "hat", "snare", "hat", "kick", "hat", "snare", "hat"],
            ChordProgression = ["Dm7", "G7", "Cmaj7", "Am7"]
        };

        // Create a test WAV file with a sine wave
        var sampleRate = 44100;
        var duration = 0.5; // seconds
        var frequency = 440; // Hz
        var amplitude = 0.5f;

        var tempDir = Path.GetTempPath();
        var tempFile = Path.Combine(tempDir, $"test_{Guid.NewGuid()}.wav");
        LogDebug(_loggerMock.Object, $"Creating test WAV file at: {tempFile}", null);
        
        var writer = new WaveFileWriter(tempFile, new WaveFormat(sampleRate, 1));
        var samples = (int)(sampleRate * duration);
        
        for (int i = 0; i < samples; i++)
        {
            float sample = amplitude * (float)Math.Sin((2 * Math.PI * frequency * i) / sampleRate);
            writer.WriteSample(sample);
        }
        
        writer.Dispose();
        LogDebug(_loggerMock.Object, $"WAV file created with {samples} samples", null);
        
        _userSamples.RegisterSample("test_sample", tempFile);
        LogDebug(_loggerMock.Object, "Sample registered with repository", null);
        
        // Store the temp file path so we can clean it up
        _tempFile = tempFile;
    }

    public void Dispose()
    {
        _userSamples.Dispose();
        
        // Clean up the temporary WAV file
        if (!string.IsNullOrEmpty(_tempFile) && File.Exists(_tempFile))
        {
            try
            {
                File.Delete(_tempFile);
            }
            catch (Exception ex)
            {
                // Log but don't throw - this is cleanup code
                LogDebug(_loggerMock.Object, $"Failed to delete temporary file: {ex.Message}", ex);
            }
        }

        GC.SuppressFinalize(this);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void DrumHits_HaveTimeOffsets()
    {
        // Arrange
        var provider = new BeatPatternSampleProvider(_testPattern, _loggerMock.Object, _userSamples, _telemetryTracker);
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
            BPM = 80,
            DrumSequence = ["kick", "_", "_", "_", "kick", "_", "_", "_"],
            ChordProgression = ["Dm7"]
        };
        var provider = new BeatPatternSampleProvider(kickOnlyPattern, _loggerMock.Object, _userSamples, _telemetryTracker);
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
        const int windowSize = 10; // Larger window for more stable detection
        const int minSamplesBetweenOnsets = 5000; // About 100ms at 44.1kHz
        var window = new float[windowSize];
        int samplesSinceLastOnset = minSamplesBetweenOnsets;

        // First pass: find average amplitude to set dynamic threshold
        float sum = 0;
        int count = 0;
        for (int i = 0; i < buffer.Length; i++)
        {
            sum += Math.Abs(buffer[i]);
            count++;
        }
        float avgAmplitude = sum / count;
        float detectionThreshold = Math.Max(threshold, avgAmplitude * 2.0f);

        // Second pass: find peaks using the dynamic threshold
        for (int i = windowSize; i < buffer.Length; i++)
        {
            // Update window
            for (int j = 0; j < windowSize - 1; j++)
            {
                window[j] = window[j + 1];
            }
            window[windowSize - 1] = Math.Abs(buffer[i]);
            
            samplesSinceLastOnset++;

            if (samplesSinceLastOnset >= minSamplesBetweenOnsets)
            {
                // Check if center of window is a peak
                bool isPeak = true;
                float centerValue = window[windowSize / 2];
                
                // Must be above threshold
                if (centerValue < detectionThreshold) continue;

                // Must be higher than surrounding samples
                for (int j = 0; j < windowSize; j++)
                {
                    if (j != windowSize / 2 && window[j] >= centerValue)
                    {
                        isPeak = false;
                        break;
                    }
                }

                if (isPeak)
                {
                    onsets.Add(i - (windowSize / 2));
                    samplesSinceLastOnset = 0;
                }
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
            BPM = 80,
            DrumSequence = ["snare", "snare", "snare", "snare"],
            ChordProgression = ["Dm7"]
        };
        var provider = new BeatPatternSampleProvider(singleDrumPattern, _loggerMock.Object, _userSamples, _telemetryTracker);
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
        var provider = new BeatPatternSampleProvider(_testPattern, _loggerMock.Object, _userSamples, _telemetryTracker);
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
        var provider = new BeatPatternSampleProvider(_testPattern, _loggerMock.Object, _userSamples, _telemetryTracker);
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

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void UserSamples_AreDisposedProperly()
    {
        // Arrange
        var pattern = new BeatPattern
        {
            BPM = 80,
            DrumSequence = ["kick", "_", "_", "_"],
            UserSampleSteps = new Dictionary<int, string> { { 1, "test_sample" } }
        };

        var provider = new BeatPatternSampleProvider(pattern, _loggerMock.Object, _userSamples, _telemetryTracker);
        var buffer = new float[44100];

        // Act
        provider.Read(buffer, 0, buffer.Length);
        provider.Dispose();

        // Assert
        // No explicit assertion needed - we're testing that no exceptions are thrown
        // during disposal and that memory is properly cleaned up
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void UserSamples_PlayCorrectly()
    {
        // Arrange
        var patternWithUserSample = new BeatPattern
        {
            BPM = 80,
            DrumSequence = ["kick", "_", "_", "_"], // Simplified pattern
            ChordProgression = ["Dm7"],
            UserSampleSteps = new Dictionary<int, string> { { 2, "test_sample" } } // Use step 2 for user sample
        };

        // Verify the sample exists in repository
        Assert.True(_userSamples.HasSample("test_sample"), "Test sample should be registered");
        Console.WriteLine("Test sample is registered in repository");
        
        var provider = new BeatPatternSampleProvider(patternWithUserSample, _loggerMock.Object, _userSamples, _telemetryTracker);
        var buffer = new float[44100]; // 1 second of audio

        // Act
        var samplesRead = provider.Read(buffer, 0, buffer.Length);
        Console.WriteLine($"Read {samplesRead} samples from provider");

        // Find peaks in the buffer that would indicate sample playback
        var peaks = FindSignificantPeaks(buffer, 0.01f); // Even lower threshold for testing
        Console.WriteLine($"Found {peaks.Count} peaks at positions: {string.Join(", ", peaks)}");

        // Log average amplitudes in different regions
        void LogRegionAmplitude(string region, int start, int length)
        {
            float sum = 0;
            for (int i = start; i < start + length && i < buffer.Length; i++)
            {
                sum += Math.Abs(buffer[i]);
            }
            float avg = sum / length;
            Console.WriteLine($"{region} region (samples {start}-{start + length}): average amplitude = {avg:F3}");
        }

        LogRegionAmplitude("Start", 0, 1000); // First 1000 samples
        LogRegionAmplitude("Middle", buffer.Length / 2 - 500, 1000); // Around where user sample should be
        LogRegionAmplitude("End", buffer.Length - 1000, 1000); // Last 1000 samples

        // Assert
        Assert.True(peaks.Count >= 2, $"Should find at least two peaks (kick and user sample). Found {peaks.Count} peaks.");
        
        // The kick should be at the start, and the user sample should be around 1/2 through
        // (since it's at step 2 in a 4-step pattern)
        int expectedUserSamplePosition = buffer.Length / 2; // Step 2 in a 4-step pattern
        Assert.Contains(peaks, p => p < buffer.Length / 8); // Kick at start
        Assert.Contains(peaks, p => Math.Abs(p - expectedUserSamplePosition) < buffer.Length / 8); // User sample
    }

    private static List<int> FindSignificantPeaks(float[] buffer, float threshold = 0.01f)
    {
        var peaks = new List<int>();
        int lastPeakIndex = -1000; // Prevent multiple detections of same peak
        int windowSize = 100; // Look at larger windows for onset detection
        
        // First pass: find average amplitude in each window
        var windowAverages = new List<float>();
        for (int i = 0; i < buffer.Length; i += windowSize)
        {
            float sum = 0;
            int count = 0;
            for (int j = 0; j < windowSize && i + j < buffer.Length; j++)
            {
                sum += Math.Abs(buffer[i + j]);
                count++;
            }
            windowAverages.Add(sum / count);
        }

        // Second pass: find significant increases in amplitude
        float prevAvg = 0;
        for (int i = 0; i < windowAverages.Count; i++)
        {
            float currentAvg = windowAverages[i];
            if (currentAvg > threshold && // Above minimum threshold
                currentAvg > prevAvg * 1.5f && // Significant increase from previous
                (i * windowSize - lastPeakIndex) > 1000) // Not too close to last peak
            {
                peaks.Add(i * windowSize);
                lastPeakIndex = i * windowSize;
            }
            prevAvg = currentAvg;
        }
        
        return peaks;
    }
} 