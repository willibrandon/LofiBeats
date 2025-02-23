using LofiBeats.Core.PluginManagement;
using Microsoft.Extensions.Logging;
using Moq;

namespace LofiBeats.Tests.PluginManagement;

[Collection("Plugin Tests")]
public class PluginSecurityTests : IDisposable
{
    private readonly string _testPluginDir;
    private readonly Mock<ILogger<PluginManager>> _loggerMock;
    private readonly Mock<ILogger<PluginLoader>> _loaderLoggerMock;
    private readonly PluginLoader _loader;
    private readonly PluginManager _manager;
    private readonly PluginTestFixture _fixture;

    public PluginSecurityTests(PluginTestFixture fixture)
    {
        _fixture = fixture;
        _testPluginDir = Path.Combine(_fixture.TestPluginDirectory, "PluginSecurityTests");
        _loggerMock = new Mock<ILogger<PluginManager>>();
        _loaderLoggerMock = new Mock<ILogger<PluginLoader>>();
        _loader = new PluginLoader(_loaderLoggerMock.Object, _testPluginDir);
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
    public void LoadPlugin_WithFileSystemAccess_LoadsSuccessfully()
    {
        // Arrange
        var testAssembly = typeof(TestAudioEffect).Assembly;
        var testDllPath = Path.Combine(_testPluginDir, "test.dll");
        File.Copy(testAssembly.Location, testDllPath);

        // Act
        _manager.RefreshPlugins();
        var effects = _manager.GetEffectNames().ToList();

        // Assert
        Assert.Contains("testeffect", effects);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void LoadPlugin_WithMalformedAssembly_HandlesGracefully()
    {
        // Arrange
        var malformedDllPath = Path.Combine(_testPluginDir, "malformed.dll");
        
        // Create a malformed DLL (not a valid PE file)
        File.WriteAllText(malformedDllPath, "This is not a valid DLL file");

        // Act
        _manager.RefreshPlugins();
        var effects = _manager.GetEffectNames();

        // Assert
        Assert.Empty(effects);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void LoadPlugin_WithCorruptedMetadata_HandlesGracefully()
    {
        // Arrange
        var corruptDllPath = Path.Combine(_testPluginDir, "corrupt.dll");
        
        // Create a file that looks like a DLL but has corrupted metadata
        using (var fs = File.Create(corruptDllPath))
        {
            // Write a minimal PE header
            var header = new byte[] 
            {
                0x4D, 0x5A, // MZ header
                0x90, 0x00, // More header bytes
                0x03, 0x00, // Corrupted section
                0x00, 0x00,
                0x04, 0x00,
                0x00, 0x00
            };
            fs.Write(header);
        }

        // Act
        _manager.RefreshPlugins();
        var effects = _manager.GetEffectNames();

        // Assert
        Assert.Empty(effects);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void LoadPlugin_WithLargeFile_HandlesGracefully()
    {
        // Arrange
        var largeDllPath = Path.Combine(_testPluginDir, "large.dll");
        
        // Create a large but invalid DLL file (100 MB)
        using (var fs = File.Create(largeDllPath))
        {
            fs.SetLength(100 * 1024 * 1024); // 100 MB
        }

        // Act
        _manager.RefreshPlugins();
        var effects = _manager.GetEffectNames();

        // Assert
        Assert.Empty(effects);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void CreateEffect_WithRecursiveInstantiation_HandlesGracefully()
    {
        // Arrange
        var testAssembly = typeof(TestAudioEffect).Assembly;
        var testDllPath = Path.Combine(_testPluginDir, "test.dll");
        File.Copy(testAssembly.Location, testDllPath);
        _manager.RefreshPlugins();

        var sampleProvider = new TestSampleProvider();
        var firstEffect = _manager.CreateEffect("testeffect", sampleProvider);

        // Act & Assert
        // Attempt to create a chain of effects (which could lead to stack overflow)
        Assert.NotNull(firstEffect);
        var secondEffect = _manager.CreateEffect("testeffect", firstEffect);
        Assert.NotNull(secondEffect);
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
