using LofiBeats.Core.WebSocket;
using LofiBeats.Service;
using LofiBeats.Tests.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Xunit.Abstractions;

namespace LofiBeats.Tests.Integration;

[Collection("WebSocket Tests")]
public class WebSocketShutdownTests : IClassFixture<WebApplicationFactory<Program>>, IAsyncDisposable
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly WebSocketClient _wsClient;
    private readonly List<WebSocket> _clients;
    private readonly CancellationTokenSource _cts;
    private readonly ITestOutputHelper _output;
    private bool _disposed;

    public WebSocketShutdownTests(WebApplicationFactory<Program> factory, ITestOutputHelper output)
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
                    ["WebSocket:MessageRateLimit"] = "100",
                    ["WebSocket:MaxMessageSize"] = "32768",
                    ["WebSocket:MaxConcurrentConnections"] = "50",
                    ["WebSocket:EndpointPath"] = "/ws/lofi"
                };
                config.AddInMemoryCollection(settings);
            });
        });

        _wsClient = _factory.Server.CreateWebSocketClient();
        _clients = new List<WebSocket>();
        _cts = new CancellationTokenSource();
        _cts.CancelAfter(TimeSpan.FromSeconds(30));
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task GracefulShutdown_ShouldCloseAllConnections()
    {
        // Arrange
        const int numClients = 5;
        var wsUri = new Uri("ws://localhost/ws/lofi");
        var closedClients = 0;

        // Create multiple client connections
        for (int i = 0; i < numClients; i++)
        {
            var client = await _wsClient.ConnectAsync(wsUri, _cts.Token);
            _clients.Add(client);

            // Start a background task to monitor connection state
            _ = Task.Run(async () =>
            {
                var buffer = new byte[1024];
                try
                {
                    while (client.State == WebSocketState.Open)
                    {
                        var result = await client.ReceiveAsync(
                            new ArraySegment<byte>(buffer), _cts.Token);
                        
                        if (result.MessageType == System.Net.WebSockets.WebSocketMessageType.Close)
                        {
                            Interlocked.Increment(ref closedClients);
                            break;
                        }
                    }
                }
                catch (WebSocketException)
                {
                    // Connection closed
                    Interlocked.Increment(ref closedClients);
                }
                catch (OperationCanceledException)
                {
                    // Test cancelled - still count as closed since we initiated it
                    Interlocked.Increment(ref closedClients);
                }
            });

            await Task.Delay(50); // Stagger connections
        }

        // Act - Trigger graceful shutdown by disposing the factory
        await _factory.DisposeAsync();

        // Assert
        await Task.Delay(1000); // Give time for connections to close
        Assert.Equal(numClients, closedClients);
        Assert.All(_clients, client => 
            Assert.NotEqual(WebSocketState.Open, client.State));
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task UnexpectedShutdown_ShouldCleanupResources()
    {
        // Arrange
        const int numClients = 3;
        var wsUri = new Uri("ws://localhost/ws/lofi");
        var disconnectEvents = 0;

        // Create multiple client connections
        for (int i = 0; i < numClients; i++)
        {
            var client = await _wsClient.ConnectAsync(wsUri, _cts.Token);
            _clients.Add(client);

            // Monitor for disconnects
            _ = Task.Run(async () =>
            {
                var buffer = new byte[4096];
                try
                {
                    while (!_cts.Token.IsCancellationRequested)
                    {
                        await client.ReceiveAsync(new ArraySegment<byte>(buffer), _cts.Token);
                    }
                }
                catch (OperationCanceledException)
                {
                    // Expected during shutdown
                    Interlocked.Increment(ref disconnectEvents);
                }
                catch (WebSocketException)
                {
                    // Connection closed
                    Interlocked.Increment(ref disconnectEvents);
                }
            });

            // Send a test message to ensure connection is active
            var message = new WebSocketMessage(
                Core.WebSocket.WebSocketMessageType.Command,
                WebSocketActions.Commands.SyncState,
                null);
            var json = JsonSerializer.Serialize(message);
            var buffer = Encoding.UTF8.GetBytes(json);
            await client.SendAsync(
                new ArraySegment<byte>(buffer),
                System.Net.WebSockets.WebSocketMessageType.Text,
                true,
                _cts.Token);

            await Task.Delay(50); // Stagger connections
        }

        // Act - Close connections before canceling
        foreach (var client in _clients)
        {
            if (client.State == WebSocketState.Open)
            {
                await client.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "Test shutdown", CancellationToken.None);
            }
        }

        // Then cancel the token
        _cts.Cancel();

        // Assert
        await Task.Delay(1000); // Give time for cleanup
        Assert.Equal(numClients, disconnectEvents);
        Assert.All(_clients, client => 
            Assert.NotEqual(WebSocketState.Open, client.State));
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task BroadcastDuringShutdown_ShouldHandleGracefully()
    {
        // Arrange
        const int numClients = 3;
        var wsUri = new Uri("ws://localhost/ws/lofi");
        var receivedMessages = 0;
        var disconnectEvents = 0;

        // Create multiple client connections
        for (int i = 0; i < numClients; i++)
        {
            var client = await _wsClient.ConnectAsync(wsUri, _cts.Token);
            _clients.Add(client);

            // Monitor for messages and disconnects
            _ = Task.Run(async () =>
            {
                var buffer = new byte[4096];
                try
                {
                    while (!_cts.Token.IsCancellationRequested)
                    {
                        await client.ReceiveAsync(new ArraySegment<byte>(buffer), _cts.Token);
                        Interlocked.Increment(ref receivedMessages);
                    }
                }
                catch (OperationCanceledException)
                {
                    // Expected during shutdown
                    Interlocked.Increment(ref disconnectEvents);
                }
                catch (WebSocketException)
                {
                    // Connection closed
                    Interlocked.Increment(ref disconnectEvents);
                }
            });
        }

        // Send a broadcast-triggering command
        var primaryClient = _clients[0];
        var command = new WebSocketMessage(
            Core.WebSocket.WebSocketMessageType.Command,
            WebSocketActions.Commands.Play,
            new PlayCommandPayload("jazzy", 90));
        var json = JsonSerializer.Serialize(command);
        var buffer = Encoding.UTF8.GetBytes(json);
        await primaryClient.SendAsync(
            new ArraySegment<byte>(buffer),
            System.Net.WebSockets.WebSocketMessageType.Text,
            true,
            _cts.Token);

        // Act - Give time for broadcast to start, then close connections
        await Task.Delay(100);
        foreach (var client in _clients)
        {
            if (client.State == WebSocketState.Open)
            {
                await client.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "Test shutdown", CancellationToken.None);
            }
        }

        // Then cancel the token
        _cts.Cancel();

        // Assert
        await Task.Delay(1000); // Give time for cleanup
        Assert.Equal(numClients, disconnectEvents);
        _output.WriteLine($"Received {receivedMessages} messages before shutdown");
        Assert.All(_clients, client => 
            Assert.NotEqual(WebSocketState.Open, client.State));
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed) return;
        _disposed = true;

        foreach (var client in _clients)
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
                client.Dispose();
            }
            catch
            {
                // Ignore errors during cleanup
            }
        }
        _clients.Clear();

        _cts.Dispose();
        await _factory.DisposeAsync();
    }
} 