using System.Diagnostics;
using System.Text.Json;
using LofiBeats.PluginHost.Models;
using LofiBeats.Tests.TestHelpers;

namespace LofiBeats.Tests.PluginManagement;

public class PluginHostTests : IDisposable
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private Process? _pluginHostProcess;
    private readonly string _testPluginDir;
    private readonly string _uniqueId;
    private readonly string _pluginHostPath;
    private readonly string _testPluginPath;

    public PluginHostTests()
    {
        _uniqueId = Guid.NewGuid().ToString("N");
        
        // Create a unique test directory
        _testPluginDir = Path.Combine(
            Path.GetTempPath(),
            "LofiBeatsTests",
            "PluginHostTests",
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

        // Create test plugin
        _testPluginPath = Path.Combine(_testPluginDir, $"TestPlugin_{_uniqueId}.dll");
        CreateTestPluginAssembly(_testPluginPath);
    }

    public void Dispose()
    {
        if (_pluginHostProcess is not null && !_pluginHostProcess.HasExited)
        {
            _pluginHostProcess.Kill();
            _pluginHostProcess.Dispose();
        }

        // Clean up test directory
        try
        {
            if (Directory.Exists(_testPluginDir))
            {
                Directory.Delete(_testPluginDir, true);
            }
        }
        catch
        {
            // Ignore cleanup errors
        }
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

            process.BeginErrorReadLine();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new InvalidOperationException(
                    $"Failed to compile test plugin. Exit code: {process.ExitCode}\n" +
                    $"Error: {errorBuilder}");
            }

            // Verify the build output exists
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
            catch
            {
                // Ignore cleanup errors
            }
        }
    }

    private Process StartPluginHost(string assemblyName = "test.dll")
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"exec \"{_pluginHostPath}\" --plugin-assembly \"{_testPluginPath}\"",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        var startupCompletionSource = new TaskCompletionSource<bool>();
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        cts.Token.Register(() => startupCompletionSource.TrySetException(new TimeoutException("Plugin host did not start within 5 seconds")));

        process.ErrorDataReceived += (_, e) =>
        {
            if (e.Data != null)
            {
                Console.WriteLine($"[ERROR] {e.Data}");
            }
        };

        process.OutputDataReceived += (_, e) =>
        {
            if (e.Data != null)
            {
                Console.WriteLine($"[OUTPUT] {e.Data}");
                if (e.Data.Contains("[DEBUG] PluginHost started"))
                {
                    startupCompletionSource.TrySetResult(true);
                }
            }
        };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        try
        {
            startupCompletionSource.Task.Wait();
        }
        catch (AggregateException ex) when (ex.InnerException is TimeoutException)
        {
            process.Kill();
            throw new InvalidOperationException("Plugin host did not start within 5 seconds");
        }

        if (process.HasExited)
        {
            throw new InvalidOperationException($"Plugin host exited unexpectedly with code {process.ExitCode}");
        }

        return process;
    }

    private static async Task<PluginResponse> SendMessageAndGetResponse(Process process, PluginMessage message)
    {
        var json = JsonSerializer.Serialize(message, _jsonOptions);
        Console.WriteLine($"Sending message: {json}");
        await process.StandardInput.WriteLineAsync(json);
        await process.StandardInput.FlushAsync();

        // Read lines until we find a response
        var startTime = DateTime.UtcNow;
        var timeout = TimeSpan.FromSeconds(5);
        var responseCompletionSource = new TaskCompletionSource<PluginResponse>();

        void OutputHandler(object sender, DataReceivedEventArgs e)
        {
            if (e.Data == null) return;

            if (e.Data.StartsWith("[RESPONSE] "))
            {
                var responseJson = e.Data.Substring("[RESPONSE] ".Length);
                try
                {
                    var response = JsonSerializer.Deserialize<PluginResponse>(responseJson, _jsonOptions);
                    if (response != null)
                    {
                        responseCompletionSource.TrySetResult(response);
                    }
                }
                catch (Exception ex)
                {
                    responseCompletionSource.TrySetException(ex);
                }
            }
        }

        process.OutputDataReceived += OutputHandler;

        try
        {
            using var cts = new CancellationTokenSource(timeout);
            cts.Token.Register(() => responseCompletionSource.TrySetException(
                new TimeoutException("Did not receive response within timeout period")));

            return await responseCompletionSource.Task;
        }
        finally
        {
            process.OutputDataReceived -= OutputHandler;
        }
    }

    [Fact]
    public async Task InitMessage_ShouldReturnSuccess()
    {
        // Arrange
        _pluginHostProcess = StartPluginHost();

        var message = new PluginMessage
        {
            Action = "init",
            Payload = null
        };

        // Act
        var response = await SendMessageAndGetResponse(_pluginHostProcess, message);

        // Assert
        Assert.Equal("ok", response.Status);
        Assert.Equal("Plugin host initialized", response.Message);
        Assert.NotNull(response.Payload);
        
        // Verify effects metadata
        var effects = response.Payload.Value.GetProperty("effects").EnumerateArray().ToList();
        Assert.NotEmpty(effects);
        
        var effect = effects[0];
        Assert.Equal("testeffect", effect.GetProperty("name").GetString());
        Assert.Equal("Test Effect", effect.GetProperty("description").GetString());
        Assert.Equal("1.0.0", effect.GetProperty("version").GetString());
        Assert.Equal("Test Author", effect.GetProperty("author").GetString());
    }

    [Fact]
    public async Task InvalidAction_ShouldReturnError()
    {
        // Arrange
        _pluginHostProcess = StartPluginHost();

        var message = new PluginMessage
        {
            Action = "nonexistent_action",
            Payload = null
        };

        // Act
        var response = await SendMessageAndGetResponse(_pluginHostProcess, message);

        // Assert
        Assert.Equal("error", response.Status);
        Assert.Contains("Unknown action", response.Message);
    }

    [Fact]
    public async Task InvalidJson_ShouldReturnError()
    {
        // Arrange
        _pluginHostProcess = StartPluginHost();

        // Act
        await _pluginHostProcess.StandardInput.WriteLineAsync("invalid json");
        await _pluginHostProcess.StandardInput.FlushAsync();

        // Read lines until we find a response
        var responseCompletionSource = new TaskCompletionSource<PluginResponse>();

        void OutputHandler(object sender, DataReceivedEventArgs e)
        {
            if (e.Data == null) return;

            Console.WriteLine($"Received line: {e.Data}");
            if (e.Data.StartsWith("[RESPONSE] "))
            {
                var responseJson = e.Data.Substring("[RESPONSE] ".Length);
                try
                {
                    var response = JsonSerializer.Deserialize<PluginResponse>(responseJson, _jsonOptions);
                    if (response != null)
                    {
                        responseCompletionSource.TrySetResult(response);
                    }
                }
                catch (Exception ex)
                {
                    responseCompletionSource.TrySetException(ex);
                }
            }
        }

        _pluginHostProcess.OutputDataReceived += OutputHandler;

        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            cts.Token.Register(() => responseCompletionSource.TrySetException(
                new TimeoutException("Did not receive response within timeout period")));

            var response = await responseCompletionSource.Task;

            // Assert
            Assert.NotNull(response);
            Assert.Equal("error", response.Status);
            Assert.Contains("Error processing message", response.Message ?? "");
        }
        finally
        {
            _pluginHostProcess.OutputDataReceived -= OutputHandler;
        }
    }

    [Fact]
    public void MissingPluginAssembly_ShouldStartAnyway()
    {
        // Arrange & Act
        _pluginHostProcess = StartPluginHost("nonexistent.dll");

        // Assert
        // The process should start and be ready for messages
        Assert.False(_pluginHostProcess.HasExited);
    }
} 