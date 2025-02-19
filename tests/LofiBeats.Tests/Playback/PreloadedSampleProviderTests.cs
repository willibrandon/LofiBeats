using NAudio.Wave;
using LofiBeats.Core.Playback;

namespace LofiBeats.Tests.Playback;

[Collection("AI Generated Tests")]
public class PreloadedSampleProviderTests
{
    [Theory]
    [Trait("Category", "AI_Generated")]
    [InlineData(8)]
    [InlineData(16)]
    [InlineData(32)]
    public void Constructor_InitializesCorrectly(int bitsPerSample)
    {
        // Arrange
        var waveFormat = new WaveFormat(44100, bitsPerSample, 1);
        var audioData = new byte[1024]; // Power of 2 buffer size

        // Act
        var provider = new PreloadedSampleProvider(audioData, waveFormat);

        // Assert
        Assert.Equal(waveFormat, provider.WaveFormat);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void Read_With16BitSamples_ConvertsCorrectly()
    {
        // Arrange
        var waveFormat = new WaveFormat(44100, 16, 1);
        var audioData = new byte[4]; // 2 samples (2 bytes each)
        
        // Set first sample to max positive (32767)
        audioData[0] = 0xFF;
        audioData[1] = 0x7F;
        // Set second sample to max negative (-32768)
        audioData[2] = 0x00;
        audioData[3] = 0x80;

        var provider = new PreloadedSampleProvider(audioData, waveFormat);
        var buffer = new float[2];

        // Act
        int samplesRead = provider.Read(buffer, 0, 2);

        // Assert
        Assert.Equal(2, samplesRead);
        Assert.Equal(32767f/32768f, buffer[0], 4); // Almost 1.0
        Assert.Equal(-1.0f, buffer[1], 4);         // Exactly -1.0
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void Read_With32BitSamples_ConvertsCorrectly()
    {
        // Arrange
        var waveFormat = new WaveFormat(44100, 32, 1);
        var audioData = new byte[8]; // 2 samples (4 bytes each)
        
        // Set first sample to max positive (2147483647)
        audioData[0] = 0xFF;
        audioData[1] = 0xFF;
        audioData[2] = 0xFF;
        audioData[3] = 0x7F;
        // Set second sample to max negative (-2147483648)
        audioData[4] = 0x00;
        audioData[5] = 0x00;
        audioData[6] = 0x00;
        audioData[7] = 0x80;

        var provider = new PreloadedSampleProvider(audioData, waveFormat);
        var buffer = new float[2];

        // Act
        int samplesRead = provider.Read(buffer, 0, 2);

        // Assert
        Assert.Equal(2, samplesRead);
        Assert.Equal(1.0f, buffer[0], 4);  // Almost 1.0
        Assert.Equal(-1.0f, buffer[1], 4);  // Exactly -1.0
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void Read_With8BitSamples_ConvertsCorrectly()
    {
        // Arrange
        var waveFormat = new WaveFormat(44100, 8, 1);
        var audioData = new byte[2]; // 2 samples (1 byte each)
        
        // Set first sample to max positive (255)
        audioData[0] = 0xFF;
        // Set second sample to max negative (0)
        audioData[1] = 0x00;

        var provider = new PreloadedSampleProvider(audioData, waveFormat);
        var buffer = new float[2];

        // Act
        int samplesRead = provider.Read(buffer, 0, 2);

        // Assert
        Assert.Equal(2, samplesRead);
        Assert.Equal(0.9921875f, buffer[0], 4);  // (255-128)/128 ≈ 0.9921875
        Assert.Equal(-1.0f, buffer[1], 4);       // (0-128)/128 = -1.0
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void Read_AtEndOfData_ReturnsCorrectSampleCount()
    {
        // Arrange
        var waveFormat = new WaveFormat(44100, 16, 1);
        var audioData = new byte[4]; // 2 samples
        var provider = new PreloadedSampleProvider(audioData, waveFormat);
        var buffer = new float[4]; // Request more samples than available

        // Act
        int samplesRead = provider.Read(buffer, 0, 4);

        // Assert
        Assert.Equal(2, samplesRead); // Should only read available samples
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void Reset_ResetsPositionCorrectly()
    {
        // Arrange
        var waveFormat = new WaveFormat(44100, 16, 1);
        var audioData = new byte[4]; // 2 samples
        var provider = new PreloadedSampleProvider(audioData, waveFormat);
        var buffer = new float[2];

        // Act
        provider.Read(buffer, 0, 2); // Read all samples
        int firstReadCount = provider.Read(buffer, 0, 2); // Try to read again
        provider.Reset(); // Reset position
        int secondReadCount = provider.Read(buffer, 0, 2); // Read after reset

        // Assert
        Assert.Equal(0, firstReadCount); // No samples available before reset
        Assert.Equal(2, secondReadCount); // All samples available after reset
    }
} 