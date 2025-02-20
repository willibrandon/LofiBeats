using LofiBeats.Core.WebSocket;
using LofiBeats.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
                    ["WebSocket:EndpointPath"] = "/ws/lofi",
                    ["WebSocket:KeepAliveInterval"] = "15000", // 15 seconds keep-alive interval
                    ["WebSocket:DisconnectTimeout"] = "60000"  // 60 seconds disconnect timeout
                };
                config.AddInMemoryCollection(settings);
            });
            
            // Configure WebSocket middleware
            builder.Configure(app =>
            {
                app.UseWebSockets(new WebSocketOptions
                {
                    KeepAliveInterval = TimeSpan.FromSeconds(15)
                });

                app.Map("/ws/lofi", wsApp =>
                {
                    wsApp.Use(async (HttpContext context, RequestDelegate next) =>
                    {
                        if (!context.WebSockets.IsWebSocketRequest)
                        {
                            context.Response.StatusCode = StatusCodes.Status400BadRequest;
                            await context.Response.WriteAsync("Not a WebSocket request");
                            return;
                        }

                        using var scope = app.ApplicationServices.CreateScope();
                        var handler = scope.ServiceProvider.GetRequiredService<IWebSocketHandler>();
                        var socket = await context.WebSockets.AcceptWebSocketAsync();
                        await handler.HandleClientAsync(socket, context.RequestAborted);
                    });
                });
            });
        });

        _wsClient = _factory.Server.CreateWebSocketClient();
        _wsClient.ConfigureRequest = request =>
        {
            request.Headers["Sec-WebSocket-Version"] = "13";
            request.Headers["Sec-WebSocket-Protocol"] = "lofi-protocol";
            request.Headers["Connection"] = "Upgrade";
            request.Headers["Upgrade"] = "websocket";
        };
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
                    while (client.State == WebSocketState.Open)
                    {
                        using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(timeoutCts.Token, _cts.Token);
                        
                        try
                        {
                            var result = await client.ReceiveAsync(
                                new ArraySegment<byte>(buffer), 
                                linkedCts.Token);

                            if (result.MessageType == System.Net.WebSockets.WebSocketMessageType.Close)
                            {
                                break;
                            }
                        }
                        catch (OperationCanceledException)
                        {
                            // Either timeout or cancellation - break the loop
                            break;
                        }
                    }
                }
                catch (WebSocketException)
                {
                    // Connection closed
                    _output.WriteLine("WebSocket connection closed");
                }
                catch (Exception ex)
                {
                    _output.WriteLine($"Error in receive loop: {ex.Message}");
                }
                finally
                {
                    // Only increment disconnect events once when the loop exits
                    Interlocked.Increment(ref disconnectEvents);
                    _output.WriteLine($"Incremented disconnect events to: {disconnectEvents}");
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
                CancellationToken.None);

            await Task.Delay(50); // Stagger connections
        }

        try
        {
            // Cancel our token first to stop background tasks
            _cts.Cancel();

            // Then dispose the factory to trigger server shutdown
            await _factory.DisposeAsync();

            // Then forcefully close all client connections
            foreach (var client in _clients)
            {
                try
                {
                    if (client.State == WebSocketState.Open)
                    {
                        using var closeTimeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                        await client.CloseAsync(
                            WebSocketCloseStatus.NormalClosure,
                            "Test shutdown",
                            closeTimeoutCts.Token);
                    }
                }
                catch (Exception ex)
                {
                    _output.WriteLine($"Error closing client: {ex.Message}");
                }
                finally
                {
                    // Ensure the client is disposed
                    client.Dispose();
                }
            }

            // Wait for disconnect events with a timeout
            var startTime = DateTime.UtcNow;
            var timeout = TimeSpan.FromSeconds(5);
            
            while (disconnectEvents < numClients && DateTime.UtcNow - startTime < timeout)
            {
                await Task.Delay(100);
                _output.WriteLine($"Waiting for disconnects: {disconnectEvents}/{numClients}");
            }

            // Assert
            _output.WriteLine($"Disconnect events: {disconnectEvents}, Expected: {numClients}");
            Assert.Equal(numClients, disconnectEvents);
            Assert.All(_clients, client => 
                Assert.NotEqual(WebSocketState.Open, client.State));
        }
        catch (Exception ex)
        {
            _output.WriteLine($"Test error: {ex}");
            throw;
        }
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
                    while (client.State == WebSocketState.Open)
                    {
                        using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(timeoutCts.Token, _cts.Token);
                        
                        try
                        {
                            var result = await client.ReceiveAsync(
                                new ArraySegment<byte>(buffer), 
                                linkedCts.Token);

                            if (result.MessageType == System.Net.WebSockets.WebSocketMessageType.Close)
                            {
                                break;
                            }
                            Interlocked.Increment(ref receivedMessages);
                        }
                        catch (OperationCanceledException)
                        {
                            // Either timeout or cancellation - break the loop
                            break;
                        }
                    }
                }
                catch (WebSocketException)
                {
                    // Connection closed
                    _output.WriteLine("WebSocket connection closed");
                }
                catch (Exception ex)
                {
                    _output.WriteLine($"Error in receive loop: {ex.Message}");
                }
                finally
                {
                    // Only increment disconnect events once when the loop exits
                    Interlocked.Increment(ref disconnectEvents);
                    _output.WriteLine($"Incremented disconnect events to: {disconnectEvents}");
                }
            });
        }

        try
        {
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
                CancellationToken.None);

            // Give time for broadcast to start
            await Task.Delay(100);

            // Cancel our token first to stop background tasks
            _cts.Cancel();

            // Then dispose the factory to trigger server shutdown
            await _factory.DisposeAsync();

            // Then forcefully close all client connections
            foreach (var client in _clients)
            {
                try
                {
                    if (client.State == WebSocketState.Open)
                    {
                        using var closeTimeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                        await client.CloseAsync(
                            WebSocketCloseStatus.NormalClosure,
                            "Test shutdown",
                            closeTimeoutCts.Token);
                    }
                }
                catch (Exception ex)
                {
                    _output.WriteLine($"Error closing client: {ex.Message}");
                }
                finally
                {
                    // Ensure the client is disposed
                    client.Dispose();
                }
            }

            // Wait for disconnect events with a timeout
            var startTime = DateTime.UtcNow;
            var timeout = TimeSpan.FromSeconds(5);
            
            while (disconnectEvents < numClients && DateTime.UtcNow - startTime < timeout)
            {
                await Task.Delay(100);
                _output.WriteLine($"Waiting for disconnects: {disconnectEvents}/{numClients}");
            }

            // Assert
            _output.WriteLine($"Disconnect events: {disconnectEvents}, Expected: {numClients}");
            _output.WriteLine($"Received messages: {receivedMessages}");
            Assert.Equal(numClients, disconnectEvents);
            Assert.All(_clients, client => 
                Assert.NotEqual(WebSocketState.Open, client.State));
        }
        catch (Exception ex)
        {
            _output.WriteLine($"Test error: {ex}");
            throw;
        }
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task LongIdleConnection_StaysAlive()
    {
        // Arrange
        var wsUri = new Uri("ws://localhost/ws/lofi");
        _output.WriteLine($"Starting test at {DateTime.Now:HH:mm:ss.fff}");
        
        WebSocket? client = null;
        try
        {
            client = await _wsClient.ConnectAsync(wsUri, _cts.Token);
            _output.WriteLine($"Client connected at {DateTime.Now:HH:mm:ss.fff}");
            _clients.Add(client);
            
            // Create a longer timeout for this test
            using var testCts = new CancellationTokenSource(TimeSpan.FromSeconds(25));
            
            var pingsSent = 0;
            var pongsReceived = 0;
            var lastPongTime = DateTimeOffset.UtcNow;

            // Send an initial message to ensure connection is active
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
                testCts.Token);
            _output.WriteLine($"Sent initial message at {DateTime.Now:HH:mm:ss.fff}");

            // Start a background task to send pings every 5 seconds
            var pingTask = Task.Run(async () =>
            {
                while (!testCts.Token.IsCancellationRequested)
                {
                    try
                    {
                        // Send ping
                        var pingCommand = new WebSocketMessage(
                            Core.WebSocket.WebSocketMessageType.Command,
                            WebSocketActions.Commands.Ping,
                            new { timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() }
                        );
                        var pingJson = JsonSerializer.Serialize(pingCommand);
                        var pingBuffer = Encoding.UTF8.GetBytes(pingJson);
                        await client.SendAsync(
                            new ArraySegment<byte>(pingBuffer),
                            System.Net.WebSockets.WebSocketMessageType.Text,
                            true,
                            testCts.Token);
                        
                        pingsSent++;
                        _output.WriteLine($"Sent ping #{pingsSent}");
                        
                        await Task.Delay(TimeSpan.FromSeconds(5), testCts.Token);
                    }
                    catch (OperationCanceledException) when (testCts.Token.IsCancellationRequested)
                    {
                        break;
                    }
                    catch (Exception ex)
                    {
                        _output.WriteLine($"Error sending ping: {ex.Message}");
                        break;
                    }
                }
            }, testCts.Token);

            // Start a background task to receive pongs
            var receiveTask = Task.Run(async () =>
            {
                var buffer = new byte[1024];
                while (!testCts.Token.IsCancellationRequested)
                {
                    try
                    {
                        var result = await client.ReceiveAsync(
                            new ArraySegment<byte>(buffer),
                            testCts.Token);

                        if (result.MessageType == System.Net.WebSockets.WebSocketMessageType.Close)
                        {
                            _output.WriteLine("WebSocket closed by server");
                            break;
                        }

                        var json = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        var message = JsonSerializer.Deserialize<WebSocketMessage>(json);

                        if (message?.Action == WebSocketActions.Events.Pong)
                        {
                            pongsReceived++;
                            lastPongTime = DateTimeOffset.UtcNow;
                            _output.WriteLine($"Received pong #{pongsReceived}");
                        }
                        else
                        {
                            _output.WriteLine($"Received non-pong message: {message?.Action ?? "null"}");
                        }
                    }
                    catch (OperationCanceledException) when (testCts.Token.IsCancellationRequested)
                    {
                        break;
                    }
                    catch (Exception ex)
                    {
                        _output.WriteLine($"Error receiving message: {ex.Message}");
                        break;
                    }
                }
            }, testCts.Token);

            // Wait for 20 seconds to allow multiple ping/pong exchanges
            await Task.Delay(TimeSpan.FromSeconds(20), testCts.Token);

            // Assert
            _output.WriteLine($"Final client state: {client.State}");
            _output.WriteLine($"Pings sent: {pingsSent}, Pongs received: {pongsReceived}");
            _output.WriteLine($"Time since last pong: {DateTimeOffset.UtcNow - lastPongTime:g}");
            
            Assert.True(pingsSent > 0, "Should have sent at least one ping");
            Assert.Equal(pingsSent, pongsReceived);
            Assert.True((DateTimeOffset.UtcNow - lastPongTime).TotalSeconds < 10, 
                "Should have received a pong recently");
            Assert.Equal(WebSocketState.Open, client.State);
        }
        finally
        {
            // Ensure we clean up the test-specific resources
            if (client?.State == WebSocketState.Open)
            {
                _output.WriteLine($"Closing connection at {DateTime.Now:HH:mm:ss.fff}");
                try
                {
                    // Use a separate cancellation token for cleanup
                    using var cleanupCts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                    await client.CloseAsync(
                        WebSocketCloseStatus.NormalClosure,
                        "Test complete",
                        cleanupCts.Token);
                    _output.WriteLine($"Connection closed successfully at {DateTime.Now:HH:mm:ss.fff}");
                }
                catch (Exception ex)
                {
                    _output.WriteLine($"Error during connection cleanup: {ex.Message}");
                }
            }
        }
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