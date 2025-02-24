using LofiBeats.PluginHost.Models;
using System.Diagnostics;
using System.Text.Json;

namespace LofiBeats.Tests.PluginManagement;

public class PluginHostTests : IDisposable
{
    private Process? _pluginHostProcess;
    private readonly string _pluginHostPath;

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

        process.Start();
        // Read the initial startup message
        var startupMessage = process.StandardOutput.ReadLine();
        Assert.NotNull(startupMessage);
        Assert.Contains("PluginHost started", startupMessage);

        return process;
    }

    private static async Task<PluginResponse> SendMessageAndGetResponse(Process process, PluginMessage message)
    {
        var json = JsonSerializer.Serialize(message);
        await process.StandardInput.WriteLineAsync(json);
        await process.StandardInput.FlushAsync();

        var responseLine = await process.StandardOutput.ReadLineAsync();
        Assert.NotNull(responseLine);

        return JsonSerializer.Deserialize<PluginResponse>(responseLine)
            ?? throw new InvalidOperationException("Failed to deserialize response");
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
        var responseLine = await _pluginHostProcess.StandardOutput.ReadLineAsync();

        // Assert
        Assert.NotNull(responseLine);
        var response = JsonSerializer.Deserialize<PluginResponse>(responseLine);
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