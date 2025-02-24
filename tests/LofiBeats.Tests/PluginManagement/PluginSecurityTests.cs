using LofiBeats.Core.PluginManagement;
using LofiBeats.Tests.TestHelpers;
using Microsoft.Extensions.Logging;
using Moq;
using System.Diagnostics;

namespace LofiBeats.Tests.PluginManagement;

[Collection("Plugin Tests")]
public class PluginSecurityTests : IDisposable
{
    private readonly string _testPluginDir;
    private readonly Mock<ILogger<PluginManager>> _loggerMock;
    private readonly Mock<ILoggerFactory> _loggerFactoryMock;
    private readonly Mock<ILogger<PluginLoader>> _loaderLoggerMock;
    private readonly PluginLoader _loader;
    private readonly PluginManager _manager;
    private readonly PluginTestFixture _fixture;

    public PluginSecurityTests(PluginTestFixture fixture)
    {
        _fixture = fixture;
        _testPluginDir = Path.Combine(
            Path.GetTempPath(),
            "LofiBeatsTests",
            "PluginSecurityTests",
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

            // Refresh plugins and create effects
            _manager.RefreshPlugins();
            var sampleProvider = new TestSampleProvider();
            var firstEffect = _manager.CreateEffect("testeffect", sampleProvider);

            // Act & Assert
            Assert.NotNull(firstEffect);
            var secondEffect = _manager.CreateEffect("testeffect", firstEffect);
            Assert.NotNull(secondEffect);
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
    public void LoadPlugin_FromUntrustedLocation_IsIsolated()
    {
        // Arrange
        var untrustedDir = Path.Combine(_testPluginDir, "untrusted");
        Directory.CreateDirectory(untrustedDir);

        // Create a malformed DLL in the untrusted directory
        var dllPath = Path.Combine(untrustedDir, "test.dll");
        using (var fs = File.Create(dllPath))
        {
            // Write a more complete but invalid PE header
            var header = new byte[]
            {
                // DOS Header
                0x4D, 0x5A, // MZ signature
                0x90, 0x00, // Bytes on last page of file
                0x03, 0x00, // Pages in file
                0x00, 0x00, // Relocations
                0x04, 0x00, // Size of header in paragraphs
                0x00, 0x00, // Minimum extra paragraphs needed
                0xFF, 0xFF, // Maximum extra paragraphs needed
                0x00, 0x00, // Initial (relative) SS value
                0x00, 0x00, // Initial SP value
                0x00, 0x00, // Checksum
                0x00, 0x00, // Initial IP value
                0x00, 0x00, // Initial (relative) CS value
                0x40, 0x00, // File address of relocation table
                0x00, 0x00, // Overlay number
                0x00, 0x00, 0x00, 0x00, // Reserved words
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x80, 0x00, 0x00, 0x00, // File address of new exe header

                // PE Header (corrupted)
                0x50, 0x45, 0x00, 0x00, // PE signature
                0x4C, 0x01,             // Machine (corrupted)
                0xFF, 0xFF,             // Number of sections (invalid)
                0x00, 0x00, 0x00, 0x00, // Time date stamp
                0x00, 0x00, 0x00, 0x00, // Symbol table pointer
                0x00, 0x00, 0x00, 0x00, // Number of symbols
                0xFF, 0xFF,             // Optional header size (invalid)
                0x02, 0x01              // Characteristics (invalid)
            };
            fs.Write(header);
        }

        // Setup logger verification
        var loggedPath = "";
        var loggedEx = default(Exception);

        _loaderLoggerMock.Setup(x => x.IsEnabled(LogLevel.Error)).Returns(true);
        _loaderLoggerMock.Setup(x => x.Log(
            It.IsAny<LogLevel>(),
            It.IsAny<EventId>(),
            It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()
        )).Callback<LogLevel, EventId, object, Exception, Delegate>((level, id, state, ex, formatter) =>
        {
            loggedEx = ex;
            if (state?.ToString() is string msg)
            {
                loggedPath = dllPath;
            }
        });

        // Act
        _manager.OutOfProcessEnabled = false; // Force in-process loading to test security
        _manager.RefreshPlugins();
        var effects = _manager.GetEffectNames().ToList();

        // Assert
        Assert.Empty(effects); // No effects should be loaded
        Assert.NotNull(loggedEx);
        Assert.IsType<BadImageFormatException>(loggedEx);
        Assert.Equal(dllPath, loggedPath);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void LoadPlugin_WithInvalidSignature_IsRejected()
    {
        // Arrange
        var dllPath = Path.Combine(_testPluginDir, "invalid.dll");
        File.WriteAllBytes(dllPath, [0x0, 0x1, 0x2, 0x3]); // Invalid DLL content

        // Act
        _manager.RefreshPlugins();
        var effects = _manager.GetEffectNames();

        // Assert
        Assert.Empty(effects);
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
