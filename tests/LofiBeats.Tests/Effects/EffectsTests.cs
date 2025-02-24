using LofiBeats.Core.Effects;
using LofiBeats.Core.PluginManagement;
using LofiBeats.PluginHost.Models;
using LofiBeats.Tests.TestHelpers;
using Microsoft.Extensions.Logging;
using Moq;
using NAudio.Wave;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;

namespace LofiBeats.Tests.Effects;

[Collection("AI Generated Tests")]
public class EffectsTests : IDisposable
{
    private readonly Mock<ILogger<VinylCrackleEffect>> _vinylLoggerMock;
    private readonly Mock<ILogger<LowPassFilterEffect>> _lowpassLoggerMock;
    private readonly Mock<ISampleProvider> _sampleProviderMock;
    private readonly WaveFormat _waveFormat;
    private readonly Mock<ILogger<EffectFactory>> _loggerMock;
    private readonly Mock<ILoggerFactory> _loggerFactoryMock;
    private readonly PluginManager _pluginManager;
    private readonly EffectFactory _effectFactory;

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

        _loggerMock = new Mock<ILogger<EffectFactory>>();
        _loggerFactoryMock = new Mock<ILoggerFactory>();

        _loggerFactoryMock
            .Setup(x => x.CreateLogger(It.IsAny<string>()))
            .Returns(_loggerMock.Object);

        var pluginLoaderMock = new Mock<IPluginLoader>();
        _pluginManager = new PluginManager(
            Mock.Of<ILogger<PluginManager>>(),
            _loggerFactoryMock.Object,
            pluginLoaderMock.Object,
            TestPluginSettings.CreateDefault(runOutOfProcess: false));

        _effectFactory = new EffectFactory(_loggerFactoryMock.Object, _pluginManager);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void VinylCrackleEffect_ModifiesSampleData()
    {
        // Arrange
        var effect = _effectFactory.CreateEffect("vinyl", _sampleProviderMock.Object);

        // Act
        var buffer = new float[4000];
        var originalBuffer = new float[4000];
        Array.Copy(buffer, originalBuffer, buffer.Length);

        effect.Read(buffer, 0, buffer.Length);

        // Assert
        Assert.NotEqual(originalBuffer, buffer);
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
        var effect = _effectFactory.CreateEffect("LowPass", _sampleProviderMock.Object);

        // Act
        var buffer = new float[4000];
        var originalBuffer = new float[4000];
        Array.Copy(buffer, originalBuffer, buffer.Length);

        effect.Read(buffer, 0, buffer.Length);

        // Assert
        Assert.NotEqual(originalBuffer, buffer);
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
        var pluginLoaderMock = new Mock<IPluginLoader>();
        var pluginManagerMock = new PluginManager(
            Mock.Of<ILogger<PluginManager>>(),
            loggerFactory.Object,
            pluginLoaderMock.Object,
            TestPluginSettings.CreateDefault(runOutOfProcess: false));
        var factory = new EffectFactory(loggerFactory.Object, pluginManagerMock);

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
        var pluginLoaderMock = new Mock<IPluginLoader>();
        var pluginManagerMock = new PluginManager(
            Mock.Of<ILogger<PluginManager>>(),
            loggerFactory.Object,
            pluginLoaderMock.Object,
            TestPluginSettings.CreateDefault(runOutOfProcess: false));
        var factory = new EffectFactory(loggerFactory.Object, pluginManagerMock);

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
        var effectFactoryLoggerMock = new Mock<ILogger<EffectFactory>>();
        loggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>()))
            .Returns((string category) =>
            {
                if (category == typeof(LowPassFilterEffect).FullName)
                    return _lowpassLoggerMock.Object;
                if (category == typeof(VinylCrackleEffect).FullName)
                    return _vinylLoggerMock.Object;
                return effectFactoryLoggerMock.Object;
            });

        var pluginLoaderMock = new Mock<IPluginLoader>();
        var testEffect = new PluginEffectProxy(
            "testeffect",
            "Test Effect",
            "1.0.0",
            "Test Author",
            "test-effect-id",
            _sampleProviderMock.Object,
            Mock.Of<IPluginHostConnection>(),
            Mock.Of<ILogger<PluginEffectProxy>>()
        );

        // Set up the plugin loader to return our test effect type
        var testEffectType = typeof(TestAudioEffect);
        pluginLoaderMock.Setup(x => x.LoadEffectTypes())
            .Returns([testEffectType]);

        var pluginManagerMock = new PluginManager(
            Mock.Of<ILogger<PluginManager>>(),
            loggerFactory.Object,
            pluginLoaderMock.Object,
            TestPluginSettings.CreateDefault(runOutOfProcess: false));

        var factory = new EffectFactory(loggerFactory.Object, pluginManagerMock);

        // Act
        var effect = factory.CreateEffect("testeffect", _sampleProviderMock.Object);

        // Assert
        Assert.NotNull(effect);
        Assert.Equal("testeffect", effect.Name);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void CreateEffect_UnknownEffect_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            _effectFactory.CreateEffect("UnknownEffect", _sampleProviderMock.Object));
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void CreateEffect_NullSampleProvider_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            _effectFactory.CreateEffect("vinyl", null!));
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void CreateEffect_NullEffectName_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            _effectFactory.CreateEffect(null!, _sampleProviderMock.Object));
    }
} 