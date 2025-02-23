using LofiBeats.Core.Effects;
using LofiBeats.Core.PluginApi;
using LofiBeats.Core.PluginManagement;
using Microsoft.Extensions.Logging;
using Moq;
using NAudio.Wave;
using System.Reflection;

namespace LofiBeats.Tests.PluginManagement;

// Mock plugin for testing
public class MockPlugin : IAudioEffect
{
    public string Name => "MockPlugin";
    public string Description => "A mock plugin for testing";
    public string Version => "1.0.0";
    public string Author => "Test Author";
    public WaveFormat WaveFormat => throw new NotImplementedException();
    public void SetSource(ISampleProvider source) { }
    public int Read(float[] buffer, int offset, int count) => 0;
    public void ApplyEffect(float[] buffer, int offset, int count) { }
}

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

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void LoadEffectTypes_PluginsInSubdirectories_LoadsAllPlugins()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<PluginLoader>>();
        mockLogger.Setup(x => x.IsEnabled(It.IsAny<LogLevel>())).Returns(true);

        var testAssembly = Assembly.GetExecutingAssembly();
        var testAssemblyPath = testAssembly.Location;

        var pluginDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(pluginDir);

        // Create subdirectories and copy test assembly
        var plugin1Path = Path.Combine(pluginDir, "Plugin1");
        var plugin2Path = Path.Combine(pluginDir, "Plugin2");
        var nestedPluginPath = Path.Combine(plugin2Path, "NestedPlugin");

        Directory.CreateDirectory(plugin1Path);
        Directory.CreateDirectory(plugin2Path);
        Directory.CreateDirectory(nestedPluginPath);

        var plugin1DllPath = Path.Combine(plugin1Path, "plugin1.dll");
        var plugin2DllPath = Path.Combine(plugin2Path, "plugin2.dll");
        var plugin3DllPath = Path.Combine(nestedPluginPath, "plugin3.dll");

        File.Copy(testAssemblyPath, plugin1DllPath);
        File.Copy(testAssemblyPath, plugin2DllPath);
        File.Copy(testAssemblyPath, plugin3DllPath);

        var loader = new PluginLoader(mockLogger.Object, pluginDir);

        // Act
        var types = loader.LoadEffectTypes().ToList();

        try
        {
            // Assert
            // We expect to find MockPlugin and TestAudioEffect types from each directory
            Assert.Equal(6, types.Count); // 2 types * 3 directories

            // Verify we found the correct types
            var mockPluginCount = types.Count(t => t.Name == nameof(MockPlugin));
            var testEffectCount = types.Count(t => t.Name == "TestAudioEffect");
            Assert.Equal(3, mockPluginCount); // One from each directory
            Assert.Equal(3, testEffectCount); // One from each directory

            // Verify logging for each DLL
            mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true), // Match any object
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)
                ),
                Times.Exactly(3) // Once for each DLL that contained plugin types
            );
        }
        finally
        {
            // Cleanup
            Directory.Delete(pluginDir, true);
        }
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