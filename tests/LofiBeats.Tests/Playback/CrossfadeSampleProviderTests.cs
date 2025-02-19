using Moq;
using NAudio.Wave;
using System;
using System.Linq;
using Xunit;
using LofiBeats.Core.Playback;

namespace LofiBeats.Tests.Playback;

[Collection("AI Generated Tests")]
public class CrossfadeSampleProviderTests : IDisposable
{
    private readonly Mock<ISampleProvider> _oldProvider;
    private readonly Mock<ISampleProvider> _newProvider;
    private readonly CrossfadeManager _xfadeManager;
    private readonly WaveFormat _waveFormat;
    private bool _disposed;

    public CrossfadeSampleProviderTests()
    {
        // Setup common test objects
        _waveFormat = new WaveFormat(44100, 16, 2); // Stereo 44.1kHz
        _oldProvider = new Mock<ISampleProvider>();
        _newProvider = new Mock<ISampleProvider>();
        _xfadeManager = new CrossfadeManager(1.0f); // 1 second crossfade

        // Setup wave format for both providers
        _oldProvider.Setup(p => p.WaveFormat).Returns(_waveFormat);
        _newProvider.Setup(p => p.WaveFormat).Returns(_waveFormat);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void Constructor_WithValidInputs_Succeeds()
    {
        // Act
        var provider = new CrossfadeSampleProvider(
            _oldProvider.Object,
            _newProvider.Object,
            _xfadeManager);

        // Assert
        Assert.NotNull(provider);
        Assert.Equal(_waveFormat, provider.WaveFormat);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void Constructor_WithNullProvider_ThrowsArgumentNullException()
    {
        // Assert
        Assert.Throws<ArgumentNullException>(() => new CrossfadeSampleProvider(
            null!,
            _newProvider.Object,
            _xfadeManager));

        Assert.Throws<ArgumentNullException>(() => new CrossfadeSampleProvider(
            _oldProvider.Object,
            null!,
            _xfadeManager));

        Assert.Throws<ArgumentNullException>(() => new CrossfadeSampleProvider(
            _oldProvider.Object,
            _newProvider.Object,
            null!));
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void Constructor_WithMismatchedFormats_ThrowsArgumentException()
    {
        // Arrange
        var differentFormat = new WaveFormat(48000, 16, 1); // Different sample rate and channels
        _newProvider.Setup(p => p.WaveFormat).Returns(differentFormat);

        // Assert
        Assert.Throws<ArgumentException>(() => new CrossfadeSampleProvider(
            _oldProvider.Object,
            _newProvider.Object,
            _xfadeManager));
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void Read_DuringCrossfade_MixesAudioCorrectly()
    {
        // Arrange
        const int sampleCount = 1000;
        var oldSamples = Enumerable.Repeat(0.5f, sampleCount).ToArray();
        var newSamples = Enumerable.Repeat(0.7f, sampleCount).ToArray();
        var outputBuffer = new float[sampleCount];

        _oldProvider.Setup(p => p.Read(It.IsAny<float[]>(), It.IsAny<int>(), It.IsAny<int>()))
            .Callback<float[], int, int>((buffer, offset, count) => 
            {
                Array.Copy(oldSamples, 0, buffer, offset, count);
            })
            .Returns(sampleCount);

        _newProvider.Setup(p => p.Read(It.IsAny<float[]>(), It.IsAny<int>(), It.IsAny<int>()))
            .Callback<float[], int, int>((buffer, offset, count) => 
            {
                Array.Copy(newSamples, 0, buffer, offset, count);
            })
            .Returns(sampleCount);

        var provider = new CrossfadeSampleProvider(
            _oldProvider.Object,
            _newProvider.Object,
            _xfadeManager);

        // Act
        _xfadeManager.BeginCrossfade();
        int samplesRead = provider.Read(outputBuffer, 0, sampleCount);

        // Assert
        Assert.Equal(sampleCount, samplesRead);
        
        // Verify that output samples are a mix of old and new
        // The exact values will depend on the crossfade progress
        for (int i = 0; i < samplesRead; i++)
        {
            Assert.True(outputBuffer[i] >= 0f);
            Assert.True(outputBuffer[i] <= 1.0f); // Assuming normalized audio
            
            // Should be between old and new sample values
            Assert.True(outputBuffer[i] >= Math.Min(oldSamples[i], newSamples[i]));
            Assert.True(outputBuffer[i] <= Math.Max(oldSamples[i], newSamples[i]));
        }
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void Read_WhenDisposed_ReturnsZero()
    {
        // Arrange
        var provider = new CrossfadeSampleProvider(
            _oldProvider.Object,
            _newProvider.Object,
            _xfadeManager);
        var buffer = new float[1000];

        // Act
        provider.Dispose();
        int samplesRead = provider.Read(buffer, 0, buffer.Length);

        // Assert
        Assert.Equal(0, samplesRead);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void Dispose_DisposesUnderlyingProviders()
    {
        // Arrange
        var disposableOld = new Mock<ISampleProvider>();
        disposableOld.As<IDisposable>().Setup(d => d.Dispose());
        disposableOld.Setup(p => p.WaveFormat).Returns(_waveFormat);

        var disposableNew = new Mock<ISampleProvider>();
        disposableNew.As<IDisposable>().Setup(d => d.Dispose());
        disposableNew.Setup(p => p.WaveFormat).Returns(_waveFormat);

        var provider = new CrossfadeSampleProvider(
            disposableOld.Object,
            disposableNew.Object,
            _xfadeManager);

        // Act
        provider.Dispose();

        // Assert
        disposableOld.As<IDisposable>().Verify(d => d.Dispose(), Times.Once);
        disposableNew.As<IDisposable>().Verify(d => d.Dispose(), Times.Once);
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _disposed = true;
            // Clean up any resources if needed
        }
    }
} 