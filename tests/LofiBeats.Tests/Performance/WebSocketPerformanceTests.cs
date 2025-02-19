using LofiBeats.Core.WebSocket;
using LofiBeats.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.TestHost;

namespace LofiBeats.Tests.Performance;

[Collection("AI Generated Tests")]
public class WebSocketPerformanceTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly WebSocketClient _wsClient;
    private readonly ConcurrentDictionary<string, WebSocket> _clients;
    private readonly CancellationTokenSource _cts;
    private bool _disposed;

    public WebSocketPerformanceTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Testing");
            builder.ConfigureAppConfiguration((context, config) =>
            {
                var settings = new Dictionary<string, string?>
                {
                    ["WebSocket:RequireAuthentication"] = "false",
                    ["WebSocket:MessageRateLimit"] = "100",
                    ["WebSocket:MaxMessageSize"] = "32768",
                    ["WebSocket:MaxConcurrentConnections"] = "200",
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

    private async Task<bool> WaitForServerReady()
    {
        using var client = _factory.CreateDefaultClient();
        var maxAttempts = 10;
        var attempt = 0;

        while (attempt < maxAttempts && !_cts.Token.IsCancellationRequested)
        {
            try
            {
                var response = await client.GetAsync("/healthz");
                if (response.IsSuccessStatusCode)
                {
                    await Task.Delay(1000, _cts.Token);
                    return true;
                }
            }
            catch when (!_cts.Token.IsCancellationRequested)
            {
                attempt++;
                if (attempt < maxAttempts)
                {
                    await Task.Delay(500 * attempt, _cts.Token);
                }
            }
        }

        return false;
    }

    [SkippableFact]
    [Trait("Category", "AI_Generated")]
    [Trait("Category", "Performance")]
    public async Task ConcurrentConnections_HandledEfficiently()
    {
        Skip.If(Environment.GetEnvironmentVariable("CI") != null, 
            "Performance tests are skipped in CI environment");

        // Arrange
        Assert.True(await WaitForServerReady(), "Server failed to start");
        const int numConnections = 50;
        var wsUri = new Uri($"ws://localhost:5001/ws/lofi");

        var process = Process.GetCurrentProcess();
        var initialMemory = process.WorkingSet64;
        var initialCpu = process.TotalProcessorTime;
        var startTime = DateTime.UtcNow;

        try
        {
            // Act - Create connections
            var connectTasks = new List<Task>();
            for (int i = 0; i < numConnections && !_cts.Token.IsCancellationRequested; i++)
            {
                connectTasks.Add(CreateAndConnectClientAsync(wsUri, i.ToString()));
                await Task.Delay(10, _cts.Token);
            }
            await Task.WhenAll(connectTasks);

            // Send test messages from each client
            var messageTasks = new List<Task>();
            for (int round = 0; round < 5 && !_cts.Token.IsCancellationRequested; round++)
            {
                foreach (var client in _clients.Values)
                {
                    messageTasks.Add(SendTestMessageAsync(client));
                }
                await Task.Delay(100, _cts.Token);
            }
            await Task.WhenAll(messageTasks);

            // Calculate metrics
            var endTime = DateTime.UtcNow;
            var duration = endTime - startTime;
            var finalMemory = process.WorkingSet64;
            var finalCpu = process.TotalProcessorTime;
            var memoryDeltaMB = (finalMemory - initialMemory) / 1024.0 / 1024.0;
            var cpuTimeDelta = (finalCpu - initialCpu).TotalSeconds;

            // Assert
            Assert.True(memoryDeltaMB < 200, 
                $"Memory usage increased by {memoryDeltaMB:F2}MB, which exceeds the 200MB threshold");
            
            Assert.True(cpuTimeDelta < 10, 
                $"CPU time increased by {cpuTimeDelta:F2} seconds, which exceeds the 10 second threshold");

            var activeConnections = _clients.Count(c => c.Value.State == WebSocketState.Open);
            Assert.Equal(numConnections, activeConnections);
        }
        finally
        {
            await CleanupClientsAsync();
        }
    }

    [SkippableFact]
    [Trait("Category", "AI_Generated")]
    [Trait("Category", "Performance")]
    public async Task MessageBroadcast_HandledEfficiently()
    {
        Skip.If(Environment.GetEnvironmentVariable("CI") != null, 
            "Performance tests are skipped in CI environment");

        // Arrange
        Assert.True(await WaitForServerReady(), "Server failed to start");
        const int numConnections = 20;
        var wsUri = new Uri($"ws://localhost:5001/ws/lofi");

        try
        {
            // Create connections
            for (int i = 0; i < numConnections && !_cts.Token.IsCancellationRequested; i++)
            {
                await CreateAndConnectClientAsync(wsUri, i.ToString());
                await Task.Delay(10, _cts.Token);
            }

            var process = Process.GetCurrentProcess();
            var initialMemory = process.WorkingSet64;
            var startTime = DateTime.UtcNow;

            // Act - Send a message that triggers broadcast
            var primarySocket = _clients.First().Value;
            var command = new WebSocketMessage(
                Core.WebSocket.WebSocketMessageType.Command,
                WebSocketActions.Commands.Play,
                new PlayCommandPayload("jazzy", 90));

            await SendCommandAsync(primarySocket, command);

            // Wait for broadcasts
            await Task.Delay(1000, _cts.Token);

            // Calculate metrics
            var endTime = DateTime.UtcNow;
            var duration = endTime - startTime;
            var finalMemory = process.WorkingSet64;
            var memoryDeltaMB = (finalMemory - initialMemory) / 1024.0 / 1024.0;

            // Assert
            Assert.True(duration.TotalMilliseconds < 2000, 
                $"Broadcast took {duration.TotalMilliseconds:F2}ms, which exceeds the 2000ms threshold");
            
            Assert.True(memoryDeltaMB < 50, 
                $"Memory usage increased by {memoryDeltaMB:F2}MB during broadcast, which exceeds the 50MB threshold");

            var activeConnections = _clients.Count(c => c.Value.State == WebSocketState.Open);
            Assert.Equal(numConnections, activeConnections);
        }
        finally
        {
            await CleanupClientsAsync();
        }
    }

    private async Task CreateAndConnectClientAsync(Uri wsUri, string clientId)
    {
        var webSocket = await _wsClient.ConnectAsync(wsUri, _cts.Token);
        _clients.TryAdd(clientId, webSocket);

        // Start a background task to handle responses
        _ = Task.Run(async () =>
        {
            var buffer = new byte[32768];
            try
            {
                while (!_cts.Token.IsCancellationRequested && webSocket.State == WebSocketState.Open)
                {
                    var result = await webSocket.ReceiveAsync(
                        new ArraySegment<byte>(buffer), _cts.Token);
                    
                    if (result.MessageType == System.Net.WebSockets.WebSocketMessageType.Close)
                    {
                        break;
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Normal cancellation, ignore
            }
            catch (WebSocketException)
            {
                // Connection closed, ignore
            }
        });
    }

    private static async Task SendTestMessageAsync(WebSocket client)
    {
        if (client.State != WebSocketState.Open) return;

        var command = new WebSocketMessage(
            Core.WebSocket.WebSocketMessageType.Command,
            WebSocketActions.Commands.SyncState,
            null);
        await SendCommandAsync(client, command);
    }

    private static async Task SendCommandAsync(WebSocket client, WebSocketMessage command)
    {
        if (client.State != WebSocketState.Open) return;

        var json = JsonSerializer.Serialize(command);
        var buffer = Encoding.UTF8.GetBytes(json);
        await client.SendAsync(
            new ArraySegment<byte>(buffer),
            System.Net.WebSockets.WebSocketMessageType.Text,
            true,
            CancellationToken.None);
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

    public void Dispose()
    {
        if (_disposed) return;
        
        if (!_cts.IsCancellationRequested)
        {
            _cts.Cancel();
        }
        
        CleanupClientsAsync().Wait(TimeSpan.FromSeconds(2));
        
        _cts.Dispose();
        _disposed = true;
    }
} 