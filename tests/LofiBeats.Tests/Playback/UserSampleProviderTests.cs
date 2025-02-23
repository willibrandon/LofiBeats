using LofiBeats.Core.Playback;
using NAudio.Wave;

namespace LofiBeats.Tests.Playback;

[Collection("AI Generated Tests")]
public class UserSampleProviderTests : IDisposable
{
    private readonly string _testFilePath;
    private bool _disposed;

    public UserSampleProviderTests()
    {
        // Create a temporary test WAV file
        _testFilePath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.wav");
        CreateTestWavFile(_testFilePath);
    }

    private static void CreateTestWavFile(string path, int bitsPerSample = 16)
    {
        // Create a simple WAV file with a sine wave
        var sampleRate = 44100;
        var duration = 0.1f; // 100ms
        var frequency = 440f; // A4 note
        var numSamples = (int)(sampleRate * duration);

        // For 32-bit, use IEEE float format
        var format = bitsPerSample == 32 
            ? WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, 1)
            : new WaveFormat(sampleRate, bitsPerSample, 1);

        using var writer = new WaveFileWriter(path, format);
        
        for (int i = 0; i < numSamples; i++)
        {
            var t = (float)i / sampleRate;
            var sample = (float)Math.Sin(2 * Math.PI * frequency * t);

            if (bitsPerSample == 16)
            {
                var value = (short)(sample * short.MaxValue);
                writer.WriteSample(value);
            }
            else if (bitsPerSample == 24)
            {
                var value = (int)(sample * 8388607); // 2^23 - 1
                var bytes = new byte[3];
                bytes[0] = (byte)(value & 0xFF);
                bytes[1] = (byte)((value >> 8) & 0xFF);
                bytes[2] = (byte)((value >> 16) & 0xFF);
                writer.Write(bytes, 0, 3);
            }
            else if (bitsPerSample == 32)
            {
                // For 32-bit, write the float sample directly
                writer.WriteSample(sample);
            }
        }
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void Constructor_WithValidWavFile_Succeeds()
    {
        // Act
        using var provider = new UserSampleProvider(_testFilePath);

        // Assert
        Assert.NotNull(provider);
        Assert.NotNull(provider.WaveFormat);
        Assert.Equal(44100, provider.WaveFormat.SampleRate);
        Assert.Equal(16, provider.WaveFormat.BitsPerSample);
        Assert.Equal(1, provider.WaveFormat.Channels);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void Constructor_WithNonWavFile_ThrowsArgumentException()
    {
        // Arrange
        var invalidPath = Path.Combine(Path.GetTempPath(), "test.mp3");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new UserSampleProvider(invalidPath));
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void Read_ReturnsValidSamples()
    {
        // Arrange
        using var provider = new UserSampleProvider(_testFilePath);
        var buffer = new float[1000];

        // Act
        int samplesRead = provider.Read(buffer, 0, buffer.Length);

        // Assert
        Assert.True(samplesRead > 0);
        Assert.True(samplesRead <= buffer.Length);

        // Verify samples are in valid range (-1.0 to 1.0)
        for (int i = 0; i < samplesRead; i++)
        {
            Assert.True(buffer[i] >= -1.0f);
            Assert.True(buffer[i] <= 1.0f);
        }
    }

    [Theory]
    [InlineData(16)]
    [InlineData(24)]
    [InlineData(32)]
    [Trait("Category", "AI_Generated")]
    public void Read_WithDifferentBitDepths_ReturnsValidSamples(int bitsPerSample)
    {
        // Arrange
        var filePath = Path.Combine(Path.GetTempPath(), $"test_{bitsPerSample}bit_{Guid.NewGuid()}.wav");
        CreateTestWavFile(filePath, bitsPerSample);

        using var provider = new UserSampleProvider(filePath);
        var buffer = new float[1000];

        // Act
        int samplesRead = provider.Read(buffer, 0, buffer.Length);

        // Assert
        Assert.True(samplesRead > 0);
        Assert.Equal(bitsPerSample, provider.WaveFormat.BitsPerSample);

        // Verify samples are in valid range (-1.0 to 1.0)
        for (int i = 0; i < samplesRead; i++)
        {
            Assert.True(buffer[i] >= -1.0f);
            Assert.True(buffer[i] <= 1.0f);
        }

        // Cleanup
        try { File.Delete(filePath); } catch { }
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void Read_AfterDispose_ReturnsZero()
    {
        // Arrange
        var provider = new UserSampleProvider(_testFilePath);
        var buffer = new float[1000];
        provider.Dispose();

        // Act
        int samplesRead = provider.Read(buffer, 0, buffer.Length);

        // Assert
        Assert.Equal(0, samplesRead);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void Read_MultipleCalls_ReturnsExpectedSamples()
    {
        // Arrange
        using var provider = new UserSampleProvider(_testFilePath);
        var buffer = new float[100];
        var totalSamples = 0;
        var readCalls = 0;

        // Act
        while (true)
        {
            int samplesRead = provider.Read(buffer, 0, buffer.Length);
            if (samplesRead == 0) break;
            totalSamples += samplesRead;
            readCalls++;
        }

        // Assert
        Assert.True(readCalls > 0);
        Assert.True(totalSamples > 0);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void Dispose_CanBeCalledMultipleTimes()
    {
        // Arrange
        var provider = new UserSampleProvider(_testFilePath);

        // Act & Assert - Should not throw
        provider.Dispose();
        provider.Dispose();
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            try
            {
                if (File.Exists(_testFilePath))
                {
                    File.Delete(_testFilePath);
                }
            }
            catch { }
            _disposed = true;
        }

        GC.SuppressFinalize(this);
    }
} 