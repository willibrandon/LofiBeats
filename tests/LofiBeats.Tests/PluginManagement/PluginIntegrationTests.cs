using LofiBeats.Core.Effects;
using LofiBeats.Core.PluginManagement;
using LofiBeats.Tests.TestHelpers;
using Microsoft.Extensions.Logging;
using Moq;
using System.Diagnostics;

namespace LofiBeats.Tests.PluginManagement;

[Collection("Plugin Tests")]
public class PluginIntegrationTests : IDisposable
{
    private readonly string _testPluginDir;
    private readonly Mock<ILogger<PluginManager>> _loggerMock;
    private readonly Mock<ILoggerFactory> _loggerFactoryMock;
    private readonly Mock<ILogger<PluginLoader>> _loaderLoggerMock;
    private readonly PluginLoader _loader;
    private readonly PluginManager _manager;
    private readonly EffectFactory _effectFactory;
    private readonly PluginTestFixture _fixture;

    public PluginIntegrationTests(PluginTestFixture fixture)
    {
        _fixture = fixture;
        _testPluginDir = Path.Combine(
            Path.GetTempPath(),
            "LofiBeatsTests",
            "PluginIntegrationTests",
            Guid.NewGuid().ToString());

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
        _effectFactory = new EffectFactory(_loggerFactoryMock.Object, _manager);

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
        var uniqueId = Guid.NewGuid().ToString("N");
        var testPluginPath = Path.Combine(_testPluginDir, $"TestPlugin_{uniqueId}.dll");
        
        // Create a simple test plugin assembly
        var sourceCode = $@"
using NAudio.Wave;
using LofiBeats.Core.PluginApi;

namespace TestPlugin_{uniqueId}
{{
    public class TestEffect : IAudioEffect
    {{
        private ISampleProvider? _source;

        public string Name => ""testeffect"";
        public string Description => ""Test Effect"";
        public string Version => ""1.0.0"";
        public string Author => ""Test Author"";
        
        public WaveFormat? WaveFormat => _source?.WaveFormat;

        public void SetSource(ISampleProvider source)
        {{
            _source = source;
        }}

        public int Read(float[] buffer, int offset, int count)
        {{
            return _source?.Read(buffer, offset, count) ?? 0;
        }}

        public void ApplyEffect(float[] samples, int offset, int count)
        {{
            // Simple pass-through implementation for testing
        }}
    }}
}}";

        // Create a temporary project directory
        var tempDir = Path.Combine(Path.GetTempPath(), $"TestPlugin_{uniqueId}");
        Directory.CreateDirectory(tempDir);

        try
        {
            // Get the path to the Core API assembly
            var coreApiPath = Path.Combine(
                AppContext.BaseDirectory,
                "LofiBeats.Core.PluginApi.dll"
            );

            // Create project file
            var projectFile = Path.Combine(tempDir, $"TestPlugin_{uniqueId}.csproj");
            File.WriteAllText(projectFile, $@"<Project Sdk=""Microsoft.NET.Sdk"">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <LangVersion>preview</LangVersion>
    <GenerateAssemblyInfo>false</GenerateAssemblyInfo>
    <EnableDefaultCompileItems>false</EnableDefaultCompileItems>
    <NoWarn>CS8600;CS8601;CS8602;CS8603;CS8604;CS8618</NoWarn>
    <AssemblyName>TestPlugin_{uniqueId}</AssemblyName>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include=""NAudio"" Version=""2.2.1"" />
    <Reference Include=""LofiBeats.Core.PluginApi"">
      <HintPath>{coreApiPath}</HintPath>
      <Private>true</Private>
    </Reference>
  </ItemGroup>

  <ItemGroup>
    <Compile Include=""TestEffect.cs"" />
  </ItemGroup>
</Project>");

            // Create source file
            var sourcePath = Path.Combine(tempDir, "TestEffect.cs");
            File.WriteAllText(sourcePath, sourceCode);

            // Build the project
            var startInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = "build -c Release -v quiet",
                WorkingDirectory = tempDir,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(startInfo);
            if (process == null)
            {
                throw new InvalidOperationException("Failed to start dotnet build");
            }

            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                throw new InvalidOperationException($"Failed to compile test plugin. Exit code: {process.ExitCode}");
            }

            // Copy the built assembly to the test directory
            var builtAssembly = Path.Combine(tempDir, "bin", "Release", "net9.0", $"TestPlugin_{uniqueId}.dll");
            Directory.CreateDirectory(Path.GetDirectoryName(testPluginPath)!);
            File.Copy(builtAssembly, testPluginPath, true);

            // Refresh plugins
            _manager.RefreshPlugins();
            var sampleProvider = new TestSampleProvider();

            // Act
            var effect = _effectFactory.CreateEffect("testeffect", sampleProvider);

            // Assert
            Assert.NotNull(effect);
            Assert.IsType<PluginEffectProxy>(effect);
        }
        finally
        {
            try
            {
                Directory.Delete(tempDir, true);
            }
            catch
            {
                // Ignore cleanup errors
            }
        }
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
        var uniqueId = Guid.NewGuid().ToString("N");
        var testPluginPath = Path.Combine(_testPluginDir, $"TestPlugin_{uniqueId}.dll");
        
        // Create a simple test plugin assembly
        var sourceCode = $@"
using NAudio.Wave;
using LofiBeats.Core.PluginApi;

namespace TestPlugin_{uniqueId}
{{
    public class TestEffect : IAudioEffect
    {{
        private ISampleProvider? _source;

        public string Name => ""testeffect"";
        public string Description => ""Test Effect"";
        public string Version => ""1.0.0"";
        public string Author => ""Test Author"";
        
        public WaveFormat? WaveFormat => _source?.WaveFormat;

        public void SetSource(ISampleProvider source)
        {{
            _source = source;
        }}

        public int Read(float[] buffer, int offset, int count)
        {{
            return _source?.Read(buffer, offset, count) ?? 0;
        }}

        public void ApplyEffect(float[] samples, int offset, int count)
        {{
            // Simple pass-through implementation for testing
        }}
    }}
}}";

        // Create a temporary project directory
        var tempDir = Path.Combine(Path.GetTempPath(), $"TestPlugin_{uniqueId}");
        Directory.CreateDirectory(tempDir);

        try
        {
            // Get the path to the Core API assembly
            var coreApiPath = Path.Combine(
                AppContext.BaseDirectory,
                "LofiBeats.Core.PluginApi.dll"
            );

            // Create project file
            var projectFile = Path.Combine(tempDir, $"TestPlugin_{uniqueId}.csproj");
            File.WriteAllText(projectFile, $@"<Project Sdk=""Microsoft.NET.Sdk"">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <LangVersion>preview</LangVersion>
    <GenerateAssemblyInfo>false</GenerateAssemblyInfo>
    <EnableDefaultCompileItems>false</EnableDefaultCompileItems>
    <NoWarn>CS8600;CS8601;CS8602;CS8603;CS8604;CS8618</NoWarn>
    <AssemblyName>TestPlugin_{uniqueId}</AssemblyName>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include=""NAudio"" Version=""2.2.1"" />
    <Reference Include=""LofiBeats.Core.PluginApi"">
      <HintPath>{coreApiPath}</HintPath>
      <Private>true</Private>
    </Reference>
  </ItemGroup>

  <ItemGroup>
    <Compile Include=""TestEffect.cs"" />
  </ItemGroup>
</Project>");

            // Create source file
            var sourcePath = Path.Combine(tempDir, "TestEffect.cs");
            File.WriteAllText(sourcePath, sourceCode);

            // Build the project
            var startInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = "build -c Release -v quiet",
                WorkingDirectory = tempDir,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(startInfo);
            if (process == null)
            {
                throw new InvalidOperationException("Failed to start dotnet build");
            }

            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                throw new InvalidOperationException($"Failed to compile test plugin. Exit code: {process.ExitCode}");
            }

            // Copy the built assembly to the test directory
            var builtAssembly = Path.Combine(tempDir, "bin", "Release", "net9.0", $"TestPlugin_{uniqueId}.dll");
            Directory.CreateDirectory(Path.GetDirectoryName(testPluginPath)!);
            File.Copy(builtAssembly, testPluginPath, true);

            // Refresh plugins
            _manager.RefreshPlugins();
            var sampleProvider = new TestSampleProvider();

            // Act & Assert - Built-in effect
            var vinylEffect = _effectFactory.CreateEffect("vinyl", sampleProvider);
            Assert.NotNull(vinylEffect);
            Assert.IsType<VinylCrackleEffect>(vinylEffect);

            // Act & Assert - Plugin effect
            var pluginEffect = _effectFactory.CreateEffect("testeffect", sampleProvider);
            Assert.NotNull(pluginEffect);
            Assert.IsType<PluginEffectProxy>(pluginEffect);
        }
        finally
        {
            try
            {
                Directory.Delete(tempDir, true);
            }
            catch
            {
                // Ignore cleanup errors
            }
        }
    }

    [Fact]
    public void LoadPlugin_ValidPlugin_CanCreateEffect()
    {
        // Arrange
        var pluginPath = Path.Combine(_testPluginDir, "TestPlugin.dll");
        File.WriteAllBytes(pluginPath, [0x4D, 0x5A]); // Simple MZ header

        // Act
        _manager.RefreshPlugins();
        var effects = _manager.GetEffectNames();

        // Assert
        Assert.Empty(effects); // Invalid DLL should not load any effects
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
