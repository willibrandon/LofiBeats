using LofiBeats.Core.WebSocket;
using LofiBeats.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Xunit.Abstractions;

namespace LofiBeats.Tests.Performance;

[Collection("WebSocket Tests")]
public class WebSocketBackpressureTests : IClassFixture<WebApplicationFactory<Program>>, IAsyncDisposable
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly WebSocketClient _wsClient;
    private readonly ConcurrentDictionary<string, WebSocket> _clients;
    private readonly CancellationTokenSource _cts;
    private readonly ITestOutputHelper _output;
    private bool _disposed;

    public WebSocketBackpressureTests(WebApplicationFactory<Program> factory, ITestOutputHelper output)
    {
        _output = output;
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Testing");
            builder.ConfigureAppConfiguration((context, config) =>
            {
                var settings = new Dictionary<string, string?>
                {
                    ["WebSocket:RequireAuthentication"] = "false",
                    ["WebSocket:MessageRateLimit"] = "100", // High rate limit for backpressure testing
                    ["WebSocket:MaxMessageSize"] = "32768",
                    ["WebSocket:MaxConcurrentConnections"] = "50",
                    ["WebSocket:EndpointPath"] = "/ws/lofi"
                };
                config.AddInMemoryCollection(settings);
            });
        });

        _wsClient = _factory.Server.CreateWebSocketClient();
        _clients = new ConcurrentDictionary<string, WebSocket>();
        _cts = new CancellationTokenSource();
        _cts.CancelAfter(TimeSpan.FromSeconds(30));
    }

    [SkippableFact]
    [Trait("Category", "AI_Generated")]
    [Trait("Category", "Performance")]
    public async Task HighFrequencyCommands_ShouldNotDegrade()
    {
        Skip.If(Environment.GetEnvironmentVariable("CI") != null, 
            "Performance tests are skipped in CI environment");

        // Arrange
        const int numClients = 10;
        const int commandsPerClient = 20;
        const int totalCommands = numClients * commandsPerClient;
        var wsUri = new Uri("ws://localhost/ws/lofi");
        var successfulCommands = 0;
        var failedCommands = 0;
        var responseLatencies = new ConcurrentBag<TimeSpan>();

        // Create connections
        for (int i = 0; i < numClients; i++)
        {
            var client = await _wsClient.ConnectAsync(wsUri, _cts.Token);
            _clients.TryAdd(i.ToString(), client);
            await Task.Delay(50, _cts.Token); // Stagger connections
        }

        try
        {
            // Act - Send rapid commands from all clients simultaneously
            var sw = Stopwatch.StartNew();
            var tasks = new List<Task>();

            foreach (var client in _clients.Values)
            {
                tasks.Add(Task.Run(async () =>
                {
                    for (int i = 0; i < commandsPerClient && !_cts.Token.IsCancellationRequested; i++)
                    {
                        try
                        {
                            var cmdSw = Stopwatch.StartNew();
                            await SendAndVerifyCommand(client);
                            cmdSw.Stop();
                            
                            responseLatencies.Add(cmdSw.Elapsed);
                            Interlocked.Increment(ref successfulCommands);
                            await Task.Delay(10, _cts.Token); // Small delay between commands
                        }
                        catch (Exception ex)
                        {
                            _output.WriteLine($"Command failed: {ex.Message}");
                            Interlocked.Increment(ref failedCommands);
                        }
                    }
                }, _cts.Token));
            }

            await Task.WhenAll(tasks);
            sw.Stop();

            // Calculate metrics
            var totalTime = sw.Elapsed;
            var avgLatency = responseLatencies.Average(l => l.TotalMilliseconds);
            var maxLatency = responseLatencies.Max(l => l.TotalMilliseconds);
            var commandsPerSecond = successfulCommands / totalTime.TotalSeconds;

            // Log metrics
            _output.WriteLine($"Total time: {totalTime.TotalSeconds:F2}s");
            _output.WriteLine($"Commands per second: {commandsPerSecond:F2}");
            _output.WriteLine($"Average latency: {avgLatency:F2}ms");
            _output.WriteLine($"Max latency: {maxLatency:F2}ms");
            _output.WriteLine($"Successful commands: {successfulCommands}");
            _output.WriteLine($"Failed commands: {failedCommands}");

            // Assert
            Assert.True(successfulCommands > totalCommands * 0.95, 
                $"Expected at least 95% success rate, got {(double)successfulCommands/totalCommands:P2}");
            Assert.True(avgLatency < 100, 
                $"Average latency {avgLatency:F2}ms exceeds 100ms threshold");
            Assert.True(maxLatency < 500, 
                $"Max latency {maxLatency:F2}ms exceeds 500ms threshold");
            Assert.True(failedCommands == 0, 
                $"Expected no failed commands, got {failedCommands}");
        }
        finally
        {
            await CleanupClientsAsync();
        }
    }

    [SkippableFact]
    [Trait("Category", "AI_Generated")]
    [Trait("Category", "Performance")]
    public async Task SlowClient_ShouldNotAffectOthers()
    {
        Skip.If(Environment.GetEnvironmentVariable("CI") != null, 
            "Performance tests are skipped in CI environment");

        // Arrange
        const int normalClients = 5;
        var wsUri = new Uri("ws://localhost/ws/lofi");
        var normalLatencies = new ConcurrentBag<TimeSpan>();

        // Create one slow client that delays processing
        var slowClient = await _wsClient.ConnectAsync(wsUri, _cts.Token);
        _clients.TryAdd("slow", slowClient);

        // Create normal clients
        for (int i = 0; i < normalClients; i++)
        {
            var client = await _wsClient.ConnectAsync(wsUri, _cts.Token);
            _clients.TryAdd(i.ToString(), client);
        }

        try
        {
            // Start a task that makes the slow client slow to process messages
            var slowClientTask = Task.Run(async () =>
            {
                var buffer = new byte[4096];
                while (!_cts.Token.IsCancellationRequested)
                {
                    try
                    {
                        var result = await slowClient.ReceiveAsync(
                            new ArraySegment<byte>(buffer), _cts.Token);
                        await Task.Delay(100, _cts.Token); // Simulate slow processing
                    }
                    catch when (_cts.Token.IsCancellationRequested)
                    {
                        break;
                    }
                }
            }, _cts.Token);

            // Act - Send commands from normal clients and measure their latency
            var tasks = _clients.Where(c => c.Key != "slow").Select(c => Task.Run(async () =>
            {
                for (int i = 0; i < 10 && !_cts.Token.IsCancellationRequested; i++)
                {
                    var sw = Stopwatch.StartNew();
                    await SendAndVerifyCommand(c.Value);
                    sw.Stop();
                    normalLatencies.Add(sw.Elapsed);
                    await Task.Delay(50, _cts.Token);
                }
            }, _cts.Token));

            await Task.WhenAll(tasks);

            // Calculate metrics
            var avgLatency = normalLatencies.Average(l => l.TotalMilliseconds);
            var maxLatency = normalLatencies.Max(l => l.TotalMilliseconds);

            // Log metrics
            _output.WriteLine($"Average latency for normal clients: {avgLatency:F2}ms");
            _output.WriteLine($"Max latency for normal clients: {maxLatency:F2}ms");

            // Assert
            Assert.True(avgLatency < 100, 
                $"Average latency {avgLatency:F2}ms exceeds 100ms threshold");
            Assert.True(maxLatency < 500, 
                $"Max latency {maxLatency:F2}ms exceeds 500ms threshold");
        }
        finally
        {
            await CleanupClientsAsync();
        }
    }

    private async Task SendAndVerifyCommand(WebSocket client)
    {
        if (client.State != WebSocketState.Open) 
            throw new InvalidOperationException("WebSocket not connected");

        // Send a simple command
        var command = new WebSocketMessage(
            Core.WebSocket.WebSocketMessageType.Command,
            WebSocketActions.Commands.SyncState,
            null);

        var json = JsonSerializer.Serialize(command);
        var buffer = Encoding.UTF8.GetBytes(json);
        
        await client.SendAsync(
            new ArraySegment<byte>(buffer),
            System.Net.WebSockets.WebSocketMessageType.Text,
            true,
            _cts.Token);

        // Wait for response
        var responseBuffer = new byte[4096];
        var result = await client.ReceiveAsync(
            new ArraySegment<byte>(responseBuffer),
            _cts.Token);

        if (result.MessageType == System.Net.WebSockets.WebSocketMessageType.Close)
            throw new InvalidOperationException("WebSocket closed unexpectedly");

        // Verify we got a valid response
        var response = Encoding.UTF8.GetString(responseBuffer, 0, result.Count);
        var message = JsonSerializer.Deserialize<WebSocketMessage>(response);
        
        Assert.NotNull(message);
        Assert.NotEqual(WebSocketActions.Errors.UnknownCommand, message.Action);
    }

    private async Task CleanupClientsAsync()
    {
        foreach (var client in _clients.Values)
        {
            try
            {
                if (client.State == WebSocketState.Open)
                {
                    using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));
                    await client.CloseAsync(
                        WebSocketCloseStatus.NormalClosure,
                        "Test completed",
                        cts.Token);
                }
            }
            catch
            {
                // Ignore errors during cleanup
            }
        }
        _clients.Clear();
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed) return;
        _disposed = true;

        if (!_cts.IsCancellationRequested)
        {
            _cts.Cancel();
        }

        await CleanupClientsAsync();
        _cts.Dispose();
        await _factory.DisposeAsync();

        GC.SuppressFinalize(this);
    }
} 