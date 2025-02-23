using LofiBeats.Core.Effects;
using LofiBeats.Core.PluginManagement;
using Microsoft.Extensions.Logging;
using Moq;
using NAudio.Wave;

namespace LofiBeats.Tests.PluginManagement;

[Collection("Plugin Tests")]
public class PluginManagerTests : IDisposable
{
    private readonly string _testPluginDir;
    private readonly Mock<ILogger<PluginManager>> _loggerMock;
    private readonly Mock<ILogger<PluginLoader>> _loaderLoggerMock;
    private readonly PluginLoader _loader;
    private readonly PluginManager _manager;

    public PluginManagerTests()
    {
        _testPluginDir = PluginPathHelper.GetPluginDirectory();
        _loggerMock = new Mock<ILogger<PluginManager>>();
        _loaderLoggerMock = new Mock<ILogger<PluginLoader>>();
        _loader = new PluginLoader(_loaderLoggerMock.Object);
        _manager = new PluginManager(_loggerMock.Object, _loader);

        // Ensure clean test environment
        if (Directory.Exists(_testPluginDir))
        {
            Directory.Delete(_testPluginDir, true);
        }
        Directory.CreateDirectory(_testPluginDir);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void RefreshPlugins_EmptyDirectory_NoEffectsRegistered()
    {
        // Act
        _manager.RefreshPlugins();

        // Assert
        Assert.Empty(_manager.GetEffectNames());
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void CreateEffect_UnknownEffect_ReturnsNull()
    {
        // Arrange
        var sampleProvider = new TestSampleProvider();

        // Act
        var effect = _manager.CreateEffect("nonexistent", sampleProvider);

        // Assert
        Assert.Null(effect);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void CreateEffect_WithTestPlugin_ReturnsInstance()
    {
        // Arrange
        var sampleProvider = new TestSampleProvider();
        var testAssembly = typeof(TestAudioEffect).Assembly;
        var testDllPath = Path.Combine(_testPluginDir, "test.dll");
        File.Copy(testAssembly.Location, testDllPath);

        // Act
        _manager.RefreshPlugins();
        var effect = _manager.CreateEffect("testeffect", sampleProvider);

        // Assert
        Assert.NotNull(effect);
        Assert.IsType<TestAudioEffect>(effect);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void GetEffectMetadata_ReturnsCorrectMetadata()
    {
        // Arrange
        var testAssembly = typeof(TestAudioEffect).Assembly;
        var testDllPath = Path.Combine(_testPluginDir, "test.dll");
        File.Copy(testAssembly.Location, testDllPath);

        // Act
        _manager.RefreshPlugins();
        var metadata = _manager.GetEffectMetadata("testeffect");

        // Assert
        Assert.NotNull(metadata);
        Assert.Equal("testeffect", metadata.Name);
        Assert.Equal("A test audio effect", metadata.Description);
        Assert.Equal("1.0.0", metadata.Version);
        Assert.Equal("Test Author", metadata.Author);
    }

    public void Dispose()
    {
        // Cleanup test directory
        if (Directory.Exists(_testPluginDir))
        {
            Directory.Delete(_testPluginDir, true);
        }
    }
}

// Helper test classes
internal sealed class TestSampleProvider : ISampleProvider
{
    public WaveFormat WaveFormat { get; } = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);

    public int Read(float[] buffer, int offset, int count)
    {
        return count;
    }
} 