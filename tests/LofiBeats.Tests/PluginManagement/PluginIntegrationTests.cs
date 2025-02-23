using LofiBeats.Core.Effects;
using LofiBeats.Core.PluginManagement;
using Microsoft.Extensions.Logging;
using Moq;

namespace LofiBeats.Tests.PluginManagement;

[Collection("Plugin Tests")]
public class PluginIntegrationTests : IDisposable
{
    private readonly string _testPluginDir;
    private readonly Mock<ILoggerFactory> _loggerFactoryMock;
    private readonly Mock<ILogger<EffectFactory>> _effectFactoryLoggerMock;
    private readonly Mock<ILogger<PluginManager>> _pluginManagerLoggerMock;
    private readonly Mock<ILogger<PluginLoader>> _pluginLoaderLoggerMock;
    private readonly PluginLoader _loader;
    private readonly PluginManager _pluginManager;
    private readonly EffectFactory _effectFactory;
    private readonly PluginTestFixture _fixture;

    public PluginIntegrationTests(PluginTestFixture fixture)
    {
        _fixture = fixture;
        _testPluginDir = Path.Combine(_fixture.TestPluginDirectory, "PluginIntegrationTests");
        _loggerFactoryMock = new Mock<ILoggerFactory>();
        _effectFactoryLoggerMock = new Mock<ILogger<EffectFactory>>();
        _pluginManagerLoggerMock = new Mock<ILogger<PluginManager>>();
        _pluginLoaderLoggerMock = new Mock<ILogger<PluginLoader>>();

        // Setup logger factory
        _loggerFactoryMock
            .Setup(x => x.CreateLogger(It.IsAny<string>()))
            .Returns(_effectFactoryLoggerMock.Object);

        // Create plugin system components
        _loader = new PluginLoader(_pluginLoaderLoggerMock.Object, _testPluginDir);
        _pluginManager = new PluginManager(_pluginManagerLoggerMock.Object, _loader);
        _effectFactory = new EffectFactory(_loggerFactoryMock.Object, _pluginManager);

        // Ensure clean test environment
        if (Directory.Exists(_testPluginDir))
        {
            Directory.Delete(_testPluginDir, true);
        }
        Directory.CreateDirectory(_testPluginDir);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void CreateEffect_PluginEffect_WorksWithEffectFactory()
    {
        // Arrange
        var testAssembly = typeof(TestAudioEffect).Assembly;
        var testDllPath = Path.Combine(_testPluginDir, "test.dll");
        File.Copy(testAssembly.Location, testDllPath);
        _pluginManager.RefreshPlugins();

        var sampleProvider = new TestSampleProvider();

        // Act
        var effect = _effectFactory.CreateEffect("testeffect", sampleProvider);

        // Assert
        Assert.NotNull(effect);
        Assert.IsType<TestAudioEffect>(effect);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void CreateEffect_UnknownEffect_ThrowsArgumentException()
    {
        // Arrange
        var sampleProvider = new TestSampleProvider();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _effectFactory.CreateEffect("unknowneffect", sampleProvider));
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void CreateEffect_BuiltInAndPlugin_BothWork()
    {
        // Arrange
        var testAssembly = typeof(TestAudioEffect).Assembly;
        var testDllPath = Path.Combine(_testPluginDir, "test.dll");
        File.Copy(testAssembly.Location, testDllPath);
        _pluginManager.RefreshPlugins();

        var sampleProvider = new TestSampleProvider();

        // Act & Assert - Built-in effect
        var vinylEffect = _effectFactory.CreateEffect("vinyl", sampleProvider);
        Assert.NotNull(vinylEffect);
        Assert.IsType<VinylCrackleEffect>(vinylEffect);

        // Act & Assert - Plugin effect
        var pluginEffect = _effectFactory.CreateEffect("testeffect", sampleProvider);
        Assert.NotNull(pluginEffect);
        Assert.IsType<TestAudioEffect>(pluginEffect);
    }

    public void Dispose()
    {
        try
        {
            if (Directory.Exists(_testPluginDir))
            {
                Directory.Delete(_testPluginDir, true);
            }
        }
        catch
        {
            // Ignore cleanup errors in individual tests
        }
    }
}
