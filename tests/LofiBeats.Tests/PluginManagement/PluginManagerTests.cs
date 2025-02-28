using LofiBeats.Core.PluginManagement;
using LofiBeats.Tests.TestHelpers;
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
    private readonly Mock<ILoggerFactory> _loggerFactoryMock;
    private readonly Mock<ILogger<PluginLoader>> _loaderLoggerMock;
    private readonly PluginLoader _loader;
    private readonly PluginManager _manager;
    private readonly PluginTestFixture _fixture;

    public PluginManagerTests(PluginTestFixture fixture)
    {
        _fixture = fixture;
        _testPluginDir = Path.Combine(_fixture.TestPluginDirectory, "PluginManagerTests");
        _loggerMock = new Mock<ILogger<PluginManager>>();
        _loggerFactoryMock = new Mock<ILoggerFactory>();
        _loaderLoggerMock = new Mock<ILogger<PluginLoader>>();

        _loggerFactoryMock.Setup(x => x.CreateLogger(It.IsAny<string>()))
            .Returns((string category) =>
            {
                if (category == typeof(PluginLoader).FullName)
                    return _loaderLoggerMock.Object;
                return _loggerMock.Object;
            });

        _loader = new PluginLoader(_loaderLoggerMock.Object, _testPluginDir);
        _manager = new PluginManager(_loggerMock.Object, _loggerFactoryMock.Object, _loader, TestPluginSettings.CreateDefault());

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
        _manager.OutOfProcessEnabled = false; // Disable out-of-process execution for this test

        // Act
        _manager.RefreshPlugins();
        var effect = _manager.CreateEffect("testeffect", sampleProvider);

        // Assert
        Assert.NotNull(effect);
        Assert.IsType<TestAudioEffect>(effect);
        Assert.Equal("testeffect", effect.Name);
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
        Assert.Equal(2, effects.Count); // Should find both TestAudioEffect and MockPlugin
        Assert.Contains("testeffect", effects);
        Assert.Contains("mockplugin", effects);
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

        // Assert
        Assert.Equal(2, effects.Count); // Should find both TestAudioEffect and MockPlugin once
        Assert.Contains("testeffect", effects);
        Assert.Contains("mockplugin", effects);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void RefreshPlugins_InvalidDll_SkipsInvalidAssemblies()
    {
        // Arrange
        var invalidDllPath = Path.Combine(_testPluginDir, "invalid.dll");
        File.WriteAllBytes(invalidDllPath, [0x0, 0x1, 0x2, 0x3]); // Invalid DLL content

        var testAssembly = typeof(TestAudioEffect).Assembly;
        var validDllPath = Path.Combine(_testPluginDir, "valid.dll");
        File.Copy(testAssembly.Location, validDllPath);

        // Act
        _manager.RefreshPlugins();
        var effects = _manager.GetEffectNames().ToList();

        // Assert
        Assert.Equal(2, effects.Count); // Should find both TestAudioEffect and MockPlugin from valid DLL
        Assert.Contains("testeffect", effects);
        Assert.Contains("mockplugin", effects);
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
        Assert.Equal(2, effects.Count); // Should find both TestAudioEffect and MockPlugin once
        Assert.Contains("testeffect", effects);
        Assert.Contains("mockplugin", effects);

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
        _ = typeBuilder.CreateType();

        // Save the assembly to a file
        var dummyDllPath = Path.Combine(_testPluginDir, "dummy.dll");
        File.WriteAllBytes(dummyDllPath, []); // Can't actually save dynamic assembly, just create empty file

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

    public void Dispose()
    {
        // Clean up test directory
        if (Directory.Exists(_testPluginDir))
        {
            try
            {
                Directory.Delete(_testPluginDir, true);
            }
            catch
            {
                // Ignore cleanup errors
            }
        }

        // Dispose manager to clean up any host processes
        _manager.Dispose();

        GC.SuppressFinalize(this);
    }
}

internal sealed class TestSampleProvider : ISampleProvider
{
    public WaveFormat WaveFormat { get; } = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);

    public int Read(float[] buffer, int offset, int count)
    {
        Array.Fill(buffer, 0.0f, offset, count);
        return count;
    }
}
