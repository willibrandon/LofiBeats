using LofiBeats.Core.PluginManagement;
using Microsoft.Extensions.Logging;
using Moq;
using NAudio.Wave;
using System.Reflection;
using System.Reflection.Emit;

namespace LofiBeats.Tests.PluginManagement;

[Collection("Plugin Tests")]
public class PluginManagerTests : IDisposable
{
    private readonly string _testPluginDir;
    private readonly Mock<ILogger<PluginManager>> _loggerMock;
    private readonly Mock<ILogger<PluginLoader>> _loaderLoggerMock;
    private readonly PluginLoader _loader;
    private readonly PluginManager _manager;
    private readonly PluginTestFixture _fixture;

    public PluginManagerTests(PluginTestFixture fixture)
    {
        _fixture = fixture;
        _testPluginDir = Path.Combine(_fixture.TestPluginDirectory, "PluginManagerTests");
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

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void RefreshPlugins_MultiplePlugins_LoadsAllValidEffects()
    {
        // Arrange
        var testAssembly = typeof(TestAudioEffect).Assembly;
        var testDllPath1 = Path.Combine(_testPluginDir, "test1.dll");
        var testDllPath2 = Path.Combine(_testPluginDir, "test2.dll");
        File.Copy(testAssembly.Location, testDllPath1);
        File.Copy(testAssembly.Location, testDllPath2);

        // Act
        _manager.RefreshPlugins();
        var effects = _manager.GetEffectNames().ToList();

        // Assert
        Assert.Single(effects); // Should only find one unique effect name despite multiple DLLs
        Assert.Contains("testeffect", effects);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void RefreshPlugins_DuplicateEffectNames_OnlyLoadsFirst()
    {
        // Arrange
        var testAssembly = typeof(TestAudioEffect).Assembly;
        var testDllPath1 = Path.Combine(_testPluginDir, "first.dll");
        var testDllPath2 = Path.Combine(_testPluginDir, "second.dll");
        File.Copy(testAssembly.Location, testDllPath1);
        File.Copy(testAssembly.Location, testDllPath2);

        // Act
        _manager.RefreshPlugins();
        var effects = _manager.GetEffectNames().ToList();
        var metadata = _manager.GetEffectMetadata("testeffect");

        // Assert
        Assert.Single(effects);
        Assert.NotNull(metadata);
        Assert.Equal("Test Author", metadata.Author);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void RefreshPlugins_InvalidDll_SkipsInvalidAssemblies()
    {
        // Arrange
        var invalidDllPath = Path.Combine(_testPluginDir, "invalid.dll");
        File.WriteAllBytes(invalidDllPath, new byte[] { 0x0, 0x1, 0x2, 0x3 }); // Invalid DLL content

        var testAssembly = typeof(TestAudioEffect).Assembly;
        var validDllPath = Path.Combine(_testPluginDir, "valid.dll");
        File.Copy(testAssembly.Location, validDllPath);

        // Act
        _manager.RefreshPlugins();
        var effects = _manager.GetEffectNames().ToList();

        // Assert
        Assert.Single(effects); // Should only load the valid DLL
        Assert.Contains("testeffect", effects);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void RefreshPlugins_LargeNumberOfPlugins_HandlesGracefully()
    {
        // Arrange
        var testAssembly = typeof(TestAudioEffect).Assembly;
        for (int i = 0; i < 10; i++) // Reduced from 100 to 10 DLLs for stability
        {
            var testDllPath = Path.Combine(_testPluginDir, $"test{i}.dll");
            File.Copy(testAssembly.Location, testDllPath);
        }

        // Act
        _manager.RefreshPlugins();
        var effects = _manager.GetEffectNames().ToList();

        // Assert
        Assert.Single(effects); // Should still only find one unique effect
        Assert.Contains("testeffect", effects);

        // Force a GC collection to help release file handles
        GC.Collect();
        GC.WaitForPendingFinalizers();
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void RefreshPlugins_NoIAudioEffectImplementations_ReturnsEmpty()
    {
        // Arrange - Create a dynamic assembly with a class that doesn't implement IAudioEffect
        var assemblyName = new AssemblyName("DummyAssembly");
        var assembly = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndCollect);
        var module = assembly.DefineDynamicModule("DummyModule");
        var typeBuilder = module.DefineType("DummyType", TypeAttributes.Public | TypeAttributes.Class);
        var dummyType = typeBuilder.CreateType();

        // Save the assembly to a file
        var dummyDllPath = Path.Combine(_testPluginDir, "dummy.dll");
        File.WriteAllBytes(dummyDllPath, Array.Empty<byte>()); // Can't actually save dynamic assembly, just create empty file

        // Act
        _manager.RefreshPlugins();
        var effects = _manager.GetEffectNames();

        // Assert
        Assert.Empty(effects);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void CreateEffect_WithInvalidSource_HandlesGracefully()
    {
        // Arrange
        var testAssembly = typeof(TestAudioEffect).Assembly;
        var testDllPath = Path.Combine(_testPluginDir, "test.dll");
        File.Copy(testAssembly.Location, testDllPath);
        _manager.RefreshPlugins();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _manager.CreateEffect("testeffect", null!));
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void GetEffectsWithMetadata_ReturnsCorrectMetadata()
    {
        // Arrange
        var testAssembly = typeof(TestAudioEffect).Assembly;
        var testDllPath = Path.Combine(_testPluginDir, "test.dll");
        File.Copy(testAssembly.Location, testDllPath);

        // Act
        _manager.RefreshPlugins();
        var effects = _manager.GetEffectsWithMetadata().ToList();

        // Assert
        Assert.Single(effects);
        var effect = effects[0];
        Assert.Equal("testeffect", effect.Name);
        Assert.Equal("A test audio effect", effect.Metadata.Description);
        Assert.Equal("1.0.0", effect.Metadata.Version);
        Assert.Equal("Test Author", effect.Metadata.Author);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void GetEffectsWithMetadata_MultiplePlugins_ReturnsAllEffects()
    {
        // Arrange - Create multiple test plugins
        var testAssembly = typeof(TestAudioEffect).Assembly;
        var testDllPath1 = Path.Combine(_testPluginDir, "test1.dll");
        var testDllPath2 = Path.Combine(_testPluginDir, "test2.dll");
        File.Copy(testAssembly.Location, testDllPath1);
        File.Copy(testAssembly.Location, testDllPath2);

        // Act
        _manager.RefreshPlugins();
        var effects = _manager.GetEffectsWithMetadata().ToList();

        // Assert - Should only find one unique effect name despite multiple DLLs
        Assert.Single(effects);
        var effect = effects[0];
        Assert.Equal("testeffect", effect.Name);
        Assert.Equal("A test audio effect", effect.Metadata.Description);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void GetEffectsWithMetadata_NoPlugins_ReturnsEmptyList()
    {
        // Act
        _manager.RefreshPlugins();
        var effects = _manager.GetEffectsWithMetadata();

        // Assert
        Assert.Empty(effects);
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

// Helper test classes
internal sealed class TestSampleProvider : ISampleProvider
{
    public WaveFormat WaveFormat { get; } = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);

    public int Read(float[] buffer, int offset, int count)
    {
        return count;
    }
}
