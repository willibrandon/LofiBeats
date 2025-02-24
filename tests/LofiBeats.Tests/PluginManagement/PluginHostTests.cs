using LofiBeats.PluginHost.Models;
using System.Diagnostics;
using System.Text.Json;

namespace LofiBeats.Tests.PluginManagement;

public class PluginHostTests : IDisposable
{
    private Process? _pluginHostProcess;
    private readonly string _pluginHostPath;
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = false
    };

    public PluginHostTests()
    {
        // Get the path to the PluginHost executable
        _pluginHostPath = Path.Combine(
            AppContext.BaseDirectory,
            "..", "..", "..", "..", "..",
            "src", "LofiBeats.PluginHost", "bin", "Debug", "net9.0", "LofiBeats.PluginHost.dll"
        );
    }

    public void Dispose()
    {
        if (_pluginHostProcess is not null && !_pluginHostProcess.HasExited)
        {
            _pluginHostProcess.Kill();
            _pluginHostProcess.Dispose();
        }
    }

    private Process StartPluginHost(string pluginAssemblyPath)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"exec \"{_pluginHostPath}\" --plugin-assembly \"{pluginAssemblyPath}\"",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        var errorOutput = new List<string>();
        process.ErrorDataReceived += (sender, e) =>
        {
            if (e.Data != null)
            {
                errorOutput.Add(e.Data);
            }
        };

        process.Start();
        process.BeginErrorReadLine();

        // Read lines until we find the startup message or timeout
        var startTime = DateTime.UtcNow;
        var timeout = TimeSpan.FromSeconds(5);
        var startupFound = false;
        var allOutput = new List<string>();

        while (DateTime.UtcNow - startTime < timeout)
        {
            var line = process.StandardOutput.ReadLine();
            if (line == null)
            {
                break;
            }

            allOutput.Add(line);
            if (line.Contains("[DEBUG] PluginHost started"))
            {
                startupFound = true;
                break;
            }
        }

        // If startup message wasn't found, log all output for debugging
        if (!startupFound)
        {
            Console.WriteLine("Plugin host output:");
            foreach (var line in allOutput)
            {
                Console.WriteLine($"  {line}");
            }

            if (errorOutput.Count > 0)
            {
                Console.WriteLine("Plugin host error output:");
                foreach (var line in errorOutput)
                {
                    Console.WriteLine($"  {line}");
                }
            }
        }

        Assert.True(startupFound, "Plugin host did not output startup message within timeout period");
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

        while (DateTime.UtcNow - startTime < timeout)
        {
            var line = await process.StandardOutput.ReadLineAsync();
            if (line == null)
            {
                break;
            }

            Console.WriteLine($"Received line: {line}");
            if (line.StartsWith("[RESPONSE] "))
            {
                var responseJson = line.Substring("[RESPONSE] ".Length);
                return JsonSerializer.Deserialize<PluginResponse>(responseJson, _jsonOptions)
                    ?? throw new InvalidOperationException("Failed to deserialize response");
            }
        }

        throw new TimeoutException("Did not receive response within timeout period");
    }

    [Fact]
    public async Task InitMessage_ShouldReturnSuccess()
    {
        // Arrange
        _pluginHostProcess = StartPluginHost("test.dll");

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
    }

    [Fact]
    public async Task InvalidAction_ShouldReturnError()
    {
        // Arrange
        _pluginHostProcess = StartPluginHost("test.dll");

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
        _pluginHostProcess = StartPluginHost("test.dll");

        // Act
        await _pluginHostProcess.StandardInput.WriteLineAsync("invalid json");
        await _pluginHostProcess.StandardInput.FlushAsync();

        // Read lines until we find a response
        var startTime = DateTime.UtcNow;
        var timeout = TimeSpan.FromSeconds(5);
        var response = default(PluginResponse);

        while (DateTime.UtcNow - startTime < timeout)
        {
            var line = await _pluginHostProcess.StandardOutput.ReadLineAsync();
            if (line == null)
            {
                break;
            }

            Console.WriteLine($"Received line: {line}");
            if (line.StartsWith("[RESPONSE] "))
            {
                var responseJson = line.Substring("[RESPONSE] ".Length);
                response = JsonSerializer.Deserialize<PluginResponse>(responseJson, _jsonOptions);
                break;
            }
        }

        // Assert
        Assert.NotNull(response);
        Assert.Equal("error", response.Status);
        Assert.Contains("Error processing message", response.Message ?? "");
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