using LofiBeats.Core.Effects;
using LofiBeats.Core.PluginManagement;
using Microsoft.Extensions.Logging;
using Moq;
using NAudio.Wave;
using Xunit;

namespace LofiBeats.Tests.Effects;

[Collection("AI Generated Tests")]
public class EffectsTests : IDisposable
{
    private readonly Mock<ILogger<VinylCrackleEffect>> _vinylLoggerMock;
    private readonly Mock<ILogger<LowPassFilterEffect>> _lowpassLoggerMock;
    private readonly Mock<ISampleProvider> _sampleProviderMock;
    private readonly WaveFormat _waveFormat;

    public EffectsTests()
    {
        _vinylLoggerMock = new Mock<ILogger<VinylCrackleEffect>>();
        _lowpassLoggerMock = new Mock<ILogger<LowPassFilterEffect>>();
        _sampleProviderMock = new Mock<ISampleProvider>();
        _waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);
        _sampleProviderMock.Setup(x => x.WaveFormat).Returns(_waveFormat);
        
        // Setup mock to return some sample data when Read is called
        _sampleProviderMock.Setup(x => x.Read(It.IsAny<float[]>(), It.IsAny<int>(), It.IsAny<int>()))
            .Callback<float[], int, int>((buffer, offset, count) => 
            {
                // Fill buffer with some test data
                for (int i = 0; i < count; i++)
                {
                    buffer[offset + i] = 1.0f;
                }
            })
            .Returns<float[], int, int>((buffer, offset, count) => count); // Return actual count
    }

    public void Dispose()
    {
        // No resources to dispose
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void VinylCrackleEffect_ModifiesSampleData()
    {
        // Arrange
        var effect = new VinylCrackleEffect(_sampleProviderMock.Object, _vinylLoggerMock.Object);
        var buffer = new float[1000];
        var originalBuffer = new float[1000];

        // Force some crackle by reading multiple times
        for (int attempt = 0; attempt < 10; attempt++)
        {
            Array.Fill(buffer, 1.0f);
            Array.Fill(originalBuffer, 1.0f);
            
            // Act
            effect.Read(buffer, 0, buffer.Length);

            // Check if any samples were modified
            bool anyDifferent = false;
            for (int i = 0; i < buffer.Length; i++)
            {
                if (Math.Abs(buffer[i] - originalBuffer[i]) > 0.0001f)
                {
                    anyDifferent = true;
                    break;
                }
            }

            if (anyDifferent)
            {
                return; // Test passes if we find any differences
            }
        }

        // If we get here, no differences were found after multiple attempts
        Assert.Fail("Vinyl crackle effect should modify at least some samples after multiple attempts");
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void VinylCrackleEffect_PreservesWaveFormat()
    {
        // Arrange
        var effect = new VinylCrackleEffect(_sampleProviderMock.Object, _vinylLoggerMock.Object);

        // Act & Assert
        Assert.Equal(_sampleProviderMock.Object.WaveFormat, effect.WaveFormat);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void LowPassFilter_ModifiesSampleData()
    {
        // Arrange
        var effect = new LowPassFilterEffect(_sampleProviderMock.Object, _lowpassLoggerMock.Object);
        var buffer = new float[1000];
        Array.Fill(buffer, 1.0f);
        var originalBuffer = new float[1000];
        Array.Fill(originalBuffer, 1.0f);

        // Act
        effect.Read(buffer, 0, buffer.Length);

        // Assert
        // Low-pass filter should smooth the signal, resulting in different values
        bool anyDifferent = false;
        for (int i = 0; i < buffer.Length; i++)
        {
            if (Math.Abs(buffer[i] - originalBuffer[i]) > 0.0001f)
            {
                anyDifferent = true;
                break;
            }
        }
        Assert.True(anyDifferent, "Low-pass filter should modify the sample values");
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void LowPassFilter_PreservesWaveFormat()
    {
        // Arrange
        var effect = new LowPassFilterEffect(_sampleProviderMock.Object, _lowpassLoggerMock.Object);

        // Act & Assert
        Assert.Equal(_sampleProviderMock.Object.WaveFormat, effect.WaveFormat);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void EffectFactory_CreatesVinylEffect()
    {
        // Arrange
        var loggerFactory = new Mock<ILoggerFactory>();
        loggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>()))
            .Returns(_vinylLoggerMock.Object);
        var pluginLoaderMock = new Mock<PluginLoader>(Mock.Of<ILogger<PluginLoader>>());
        var pluginManagerMock = new Mock<PluginManager>(Mock.Of<ILogger<PluginManager>>(), pluginLoaderMock.Object);
        var factory = new EffectFactory(loggerFactory.Object, pluginManagerMock.Object);

        // Act
        var effect = factory.CreateEffect("vinyl", _sampleProviderMock.Object);

        // Assert
        Assert.NotNull(effect);
        Assert.Equal("vinyl", effect.Name);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void EffectFactory_CreatesLowPassEffect()
    {
        // Arrange
        var loggerFactory = new Mock<ILoggerFactory>();
        loggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>()))
            .Returns(_lowpassLoggerMock.Object);
        var pluginLoaderMock = new Mock<PluginLoader>(Mock.Of<ILogger<PluginLoader>>());
        var pluginManagerMock = new Mock<PluginManager>(Mock.Of<ILogger<PluginManager>>(), pluginLoaderMock.Object);
        var factory = new EffectFactory(loggerFactory.Object, pluginManagerMock.Object);

        // Act
        var effect = factory.CreateEffect("lowpass", _sampleProviderMock.Object);

        // Assert
        Assert.NotNull(effect);
        Assert.Equal("lowpass", effect.Name);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void EffectFactory_CreatesPluginEffect()
    {
        // Arrange
        var loggerFactory = new Mock<ILoggerFactory>();
        loggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>()))
            .Returns(_lowpassLoggerMock.Object);
        var pluginLoaderMock = new Mock<PluginLoader>(Mock.Of<ILogger<PluginLoader>>());
        var pluginManagerMock = new Mock<PluginManager>(Mock.Of<ILogger<PluginManager>>(), pluginLoaderMock.Object);
        var testEffect = new TestAudioEffect();
        pluginManagerMock.Setup(x => x.CreateEffect("testeffect", _sampleProviderMock.Object))
            .Returns(testEffect);
        var factory = new EffectFactory(loggerFactory.Object, pluginManagerMock.Object);

        // Act
        var effect = factory.CreateEffect("testeffect", _sampleProviderMock.Object);

        // Assert
        Assert.NotNull(effect);
        Assert.Same(testEffect, effect);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void EffectFactory_UnknownEffect_ThrowsArgumentException()
    {
        // Arrange
        var loggerFactory = new Mock<ILoggerFactory>();
        loggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>()))
            .Returns(_lowpassLoggerMock.Object);
        var pluginLoaderMock = new Mock<PluginLoader>(Mock.Of<ILogger<PluginLoader>>());
        var pluginManagerMock = new Mock<PluginManager>(Mock.Of<ILogger<PluginManager>>(), pluginLoaderMock.Object);
        pluginManagerMock.Setup(x => x.CreateEffect("unknown", _sampleProviderMock.Object))
            .Returns((IAudioEffect?)null);
        var factory = new EffectFactory(loggerFactory.Object, pluginManagerMock.Object);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => factory.CreateEffect("unknown", _sampleProviderMock.Object));
    }
}

[PluginEffectName("testeffect", Description = "A test audio effect", Version = "1.0.0", Author = "Test Author")]
public sealed class TestAudioEffect : IAudioEffect
{
    private WaveFormat _waveFormat;
    private ISampleProvider? _source;

    public string Name => "Test Effect";
    public WaveFormat WaveFormat => _waveFormat;

    public TestAudioEffect()
    {
        _waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);
    }

    public void SetSource(ISampleProvider source)
    {
        _source = source;
        _waveFormat = source.WaveFormat;
    }

    public void ApplyEffect(float[] buffer, int offset, int count)
    {
        // No-op for test
    }

    public int Read(float[] buffer, int offset, int count)
    {
        return _source?.Read(buffer, offset, count) ?? count;
    }
} 