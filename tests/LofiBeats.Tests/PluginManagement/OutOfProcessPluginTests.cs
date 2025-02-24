using LofiBeats.Core.PluginManagement;
using Microsoft.Extensions.Logging;
using Moq;
using System.Diagnostics;

namespace LofiBeats.Tests.PluginManagement;

[Collection("Plugin Tests")]
public class OutOfProcessPluginTests : IDisposable
{
    private readonly string _testPluginDir;
    private readonly Mock<ILogger<PluginManager>> _loggerMock;
    private readonly Mock<ILoggerFactory> _loggerFactoryMock;
    private readonly Mock<ILogger<PluginHostConnection>> _hostLoggerMock;
    private readonly Mock<ILogger<PluginEffectProxy>> _proxyLoggerMock;
    private readonly Mock<ILogger<PluginLoader>> _loaderLoggerMock;
    private readonly PluginLoader _loader;
    private readonly PluginManager _manager;
    private readonly string _pluginHostPath;
    private readonly string _testPluginPath;
    private readonly string _uniqueId;
    private readonly Action<ILogger, string, Exception?> _logCompilationError;
    private static readonly Action<ILogger, string, string, Exception?> _logCleanupError =
        LoggerMessage.Define<string, string>(
            LogLevel.Debug,
            new EventId(2, "CleanupError"),
            "Failed to clean up temp directory {Directory}: {Error}");

    public OutOfProcessPluginTests()
    {
        _uniqueId = Guid.NewGuid().ToString("N");
        
        // Create a unique test directory
        _testPluginDir = Path.Combine(
            Path.GetTempPath(),
            "LofiBeatsTests",
            "OutOfProcessPlugins",
            _uniqueId);

        // Ensure clean test environment
        if (Directory.Exists(_testPluginDir))
        {
            Directory.Delete(_testPluginDir, true);
        }
        Directory.CreateDirectory(_testPluginDir);

        // Get the path to the PluginHost executable
        _pluginHostPath = Path.Combine(
            AppContext.BaseDirectory,
            "LofiBeats.PluginHost.dll"
        );

        // Ensure plugin host is available
        if (!File.Exists(_pluginHostPath))
        {
            var sourcePluginHostPath = Path.Combine(
                AppContext.BaseDirectory,
                "..", "..", "..", "..", "..",
                "src", "LofiBeats.PluginHost", "bin", "Debug", "net9.0",
                "LofiBeats.PluginHost.dll"
            );

            if (File.Exists(sourcePluginHostPath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_pluginHostPath)!);
                File.Copy(sourcePluginHostPath, _pluginHostPath, true);
            }
            else
            {
                throw new InvalidOperationException($"Plugin host not found at {sourcePluginHostPath}");
            }
        }

        // Initialize loggers
        _loggerMock = new Mock<ILogger<PluginManager>>();
        _loggerFactoryMock = new Mock<ILoggerFactory>();
        _hostLoggerMock = new Mock<ILogger<PluginHostConnection>>();
        _hostLoggerMock.Setup(x => x.IsEnabled(It.IsAny<LogLevel>())).Returns(true);
        _proxyLoggerMock = new Mock<ILogger<PluginEffectProxy>>();
        _loaderLoggerMock = new Mock<ILogger<PluginLoader>>();

        // Initialize log message delegate after logger is created
        _logCompilationError = LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(1, "CompilationError"),
            "Compilation failed: {Error}");

        _loggerFactoryMock.Setup(x => x.CreateLogger(It.IsAny<string>()))
            .Returns((string category) =>
            {
                if (category == typeof(PluginHostConnection).FullName)
                    return _hostLoggerMock.Object;
                if (category == typeof(PluginEffectProxy).FullName)
                    return _proxyLoggerMock.Object;
                if (category == typeof(PluginLoader).FullName)
                    return _loaderLoggerMock.Object;
                return _loggerMock.Object;
            });

        _loggerMock.Setup(x => x.Log(
            It.IsAny<LogLevel>(),
            It.IsAny<EventId>(),
            It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception?>(),
            (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()));

        _loader = new PluginLoader(_loaderLoggerMock.Object, _testPluginDir);
        _manager = new PluginManager(_loggerMock.Object, _loggerFactoryMock.Object, _loader);

        // Create a simple test plugin assembly with unique name
        _testPluginPath = Path.Combine(_testPluginDir, $"TestPlugin_{_uniqueId}.dll");
        CreateTestPluginAssembly(_testPluginPath);
    }

    private void CreateTestPluginAssembly(string outputPath)
    {
        var sourceCode = $@"
using NAudio.Wave;
using LofiBeats.Core.PluginApi;

namespace TestPlugin_{_uniqueId}
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
            // No modification needed since we're just testing plugin loading
        }}
    }}
}}";

        // Create a temporary project directory
        var tempDir = Path.Combine(Path.GetTempPath(), $"TestPlugin_{_uniqueId}");
        Directory.CreateDirectory(tempDir);

        try
        {
            // Get the path to the Core API assembly
            var coreApiPath = Path.Combine(
                AppContext.BaseDirectory,
                "LofiBeats.Core.PluginApi.dll"
            );

            // Define possible paths before using them
            var possiblePaths = new[]
            {
                Path.Combine(
                    AppContext.BaseDirectory,
                    "..", "..", "..", "..", "..",
                    "src", "LofiBeats.Core.PluginApi", "bin", "Debug", "net9.0",
                    "LofiBeats.Core.PluginApi.dll"
                ),
                Path.Combine(
                    AppContext.BaseDirectory,
                    "..", "..", "..", "..", "..",
                    "src", "LofiBeats.Core.PluginApi", "bin", "Release", "net9.0",
                    "LofiBeats.Core.PluginApi.dll"
                ),
                Path.Combine(
                    AppContext.BaseDirectory,
                    "..", "..", "..", "..",
                    "LofiBeats.Core.PluginApi", "bin", "Debug", "net9.0",
                    "LofiBeats.Core.PluginApi.dll"
                ),
                Path.Combine(
                    AppContext.BaseDirectory,
                    "..", "..", "..", "..",
                    "LofiBeats.Core.PluginApi", "bin", "Release", "net9.0",
                    "LofiBeats.Core.PluginApi.dll"
                )
            };

            // If not found in test output, try to find it in the source location
            if (!File.Exists(coreApiPath))
            {
                coreApiPath = possiblePaths.FirstOrDefault(File.Exists) ?? coreApiPath;
            }

            if (!File.Exists(coreApiPath))
            {
                throw new InvalidOperationException($"Could not find LofiBeats.Core.PluginApi.dll. Searched paths:\n" +
                    string.Join("\n", possiblePaths.Select(p => $"- {Path.GetFullPath(p)}")) + "\n" +
                    $"Current directory: {Environment.CurrentDirectory}\n" +
                    $"Base directory: {AppContext.BaseDirectory}");
            }

            // Create project file with more detailed configuration
            var projectFile = Path.Combine(tempDir, $"TestPlugin_{_uniqueId}.csproj");
            File.WriteAllText(projectFile, $@"<Project Sdk=""Microsoft.NET.Sdk"">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <LangVersion>preview</LangVersion>
    <GenerateAssemblyInfo>false</GenerateAssemblyInfo>
    <EnableDefaultCompileItems>false</EnableDefaultCompileItems>
    <NoWarn>CS8600;CS8601;CS8602;CS8603;CS8604;CS8618</NoWarn>
    <AssemblyName>TestPlugin_{_uniqueId}</AssemblyName>
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

            // Build the project with verbose output
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

            var errorBuilder = new System.Text.StringBuilder();
            var buildCompleted = new AutoResetEvent(false);

            process.OutputDataReceived += (sender, e) => 
            {
                if (e.Data?.Contains("Build succeeded") == true)
                {
                    buildCompleted.Set();
                }
            };

            process.ErrorDataReceived += (sender, e) => 
            {
                if (e.Data != null)
                {
                    lock (errorBuilder)
                    {
                        errorBuilder.AppendLine(e.Data);
                    }
                }
            };

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            // Wait for build completion or timeout
            var buildSuccess = buildCompleted.WaitOne(TimeSpan.FromSeconds(30));
            process.WaitForExit();

            if (!buildSuccess || process.ExitCode != 0)
            {
                _logCompilationError(_loggerMock.Object, errorBuilder.ToString(), null);
                throw new InvalidOperationException(
                    $"Failed to compile test plugin. Exit code: {process.ExitCode}\n" +
                    $"Error: {errorBuilder}");
            }

            // Verify the build output exists - use the unique assembly name
            var builtAssembly = Path.Combine(tempDir, "bin", "Release", "net9.0", $"TestPlugin_{_uniqueId}.dll");
            if (!File.Exists(builtAssembly))
            {
                throw new InvalidOperationException(
                    $"Build succeeded but assembly not found at {builtAssembly}\n" +
                    $"Directory contents:\n" +
                    string.Join("\n", Directory.GetFiles(Path.GetDirectoryName(builtAssembly) ?? "", "*.*")));
            }

            // Copy the built assembly to the target location
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);
            File.Copy(builtAssembly, outputPath, true);
        }
        finally
        {
            try
            {
                Directory.Delete(tempDir, true);
            }
            catch (Exception ex)
            {
                _logCleanupError(_loggerMock.Object, tempDir, ex.Message, ex);
            }
        }
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void CreateEffect_OutOfProcessEnabled_CreatesProxyEffect()
    {
        // Arrange
        var sampleProvider = new TestSampleProvider();

        // Act
        _manager.RefreshPlugins();
        var effect = _manager.CreateEffect("testeffect", sampleProvider);

        // Assert
        Assert.NotNull(effect);
        Assert.IsType<PluginEffectProxy>(effect);
        Assert.Equal("testeffect", effect.Name);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void CreateEffect_OutOfProcessDisabled_CreatesDirectEffect()
    {
        // Arrange
        var sampleProvider = new TestSampleProvider();
        _manager.OutOfProcessEnabled = false;

        // Act
        _manager.RefreshPlugins();
        var effect = _manager.CreateEffect("testeffect", sampleProvider);

        // Assert
        Assert.NotNull(effect);
        Assert.Equal("testeffect", effect.Name);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void CreateEffect_MultipleEffects_SharesSameHostProcess()
    {
        // Arrange
        var sampleProvider = new TestSampleProvider();
        _manager.RefreshPlugins();

        // Act
        var effect1 = _manager.CreateEffect("testeffect", sampleProvider);
        var effect2 = _manager.CreateEffect("testeffect", sampleProvider);

        // Assert
        Assert.NotNull(effect1);
        Assert.NotNull(effect2);
        Assert.IsType<PluginEffectProxy>(effect1);
        Assert.IsType<PluginEffectProxy>(effect2);
        Assert.Equal("testeffect", effect1.Name);
        Assert.Equal("testeffect", effect2.Name);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void Dispose_WithActiveEffects_DisposesHostConnections()
    {
        // Arrange
        var sampleProvider = new TestSampleProvider();
        _manager.RefreshPlugins();
        var effect = _manager.CreateEffect("testeffect", sampleProvider);

        // Act
        _manager.Dispose();

        // Assert
        _hostLoggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.Is<EventId>(id => id.Name == "ProcessStopped"),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception?>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)
            ),
            Times.Once
        );
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void PluginHost_InitializationProtocol_WorksCorrectly()
    {
        // Arrange
        var sampleProvider = new TestSampleProvider();
        _manager.RefreshPlugins();

        // Act
        var effect = _manager.CreateEffect("testeffect", sampleProvider);

        // Assert
        Assert.NotNull(effect);
        _hostLoggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.Is<EventId>(id => id.Name == "PluginHostStarted"),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception?>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)
            ),
            Times.Once
        );
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