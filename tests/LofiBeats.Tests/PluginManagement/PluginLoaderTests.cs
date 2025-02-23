using LofiBeats.Core.Effects;
using LofiBeats.Core.PluginManagement;
using Microsoft.Extensions.Logging;
using Moq;
using NAudio.Wave;

namespace LofiBeats.Tests.PluginManagement;

[Collection("Plugin Tests")]
public class PluginLoaderTests : IDisposable
{
    private readonly string _testPluginDir;
    private readonly Mock<ILogger<PluginLoader>> _loggerMock;
    private readonly PluginLoader _loader;

    public PluginLoaderTests()
    {
        _testPluginDir = PluginPathHelper.GetPluginDirectory();
        _loggerMock = new Mock<ILogger<PluginLoader>>();
        _loader = new PluginLoader(_loggerMock.Object);

        // Ensure clean test environment
        if (Directory.Exists(_testPluginDir))
        {
            Directory.Delete(_testPluginDir, true);
        }
        Directory.CreateDirectory(_testPluginDir);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void LoadEffectTypes_EmptyDirectory_ReturnsEmptyCollection()
    {
        // Act
        var types = _loader.LoadEffectTypes();

        // Assert
        Assert.Empty(types);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void LoadEffectTypes_NonPluginDll_ReturnsEmptyCollection()
    {
        // Arrange
        var dummyDllPath = Path.Combine(_testPluginDir, "dummy.dll");
        File.WriteAllBytes(dummyDllPath, []);

        // Act
        var types = _loader.LoadEffectTypes();

        // Assert
        Assert.Empty(types);
    }

    public void Dispose()
    {
        // Cleanup test directory
        if (Directory.Exists(_testPluginDir))
        {
            Directory.Delete(_testPluginDir, true);
        }

        GC.SuppressFinalize(this);
    }
} 