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
public class WebSocketConcurrentCommandTests : IClassFixture<WebApplicationFactory<Program>>, IAsyncDisposable
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly WebSocketClient _wsClient;
    private readonly ConcurrentDictionary<string, WebSocket> _clients;
    private readonly CancellationTokenSource _cts;
    private readonly ITestOutputHelper _output;
    private bool _disposed;

    public WebSocketConcurrentCommandTests(WebApplicationFactory<Program> factory, ITestOutputHelper output)
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
        _clients = new ConcurrentDictionary<string, WebSocket>();
        _cts = new CancellationTokenSource();
        _cts.CancelAfter(TimeSpan.FromSeconds(30));
    }

    [SkippableFact]
    [Trait("Category", "AI_Generated")]
    [Trait("Category", "Performance")]
    public async Task SimultaneousStopStart_ShouldHandleGracefully()
    {
        Skip.If(Environment.GetEnvironmentVariable("CI") != null, 
            "Performance tests are skipped in CI environment");

        // Arrange
        const int numClients = 10;
        const int iterations = 5;
        var wsUri = new Uri("ws://localhost/ws/lofi");
        var successfulCommands = 0;
        var failedCommands = 0;
        var responseLatencies = new ConcurrentBag<TimeSpan>();
        var receivedEvents = new ConcurrentDictionary<string, int>();
        var clientStates = new ConcurrentDictionary<string, string>();

        // Create connections
        for (int i = 0; i < numClients; i++)
        {
            var client = await _wsClient.ConnectAsync(wsUri, _cts.Token);
            _clients.TryAdd(i.ToString(), client);
            clientStates.TryAdd(i.ToString(), "stopped");
            await Task.Delay(50, _cts.Token); // Stagger connections
        }

        try
        {
            // First start all clients playing
            var initialPlayTasks = _clients.Select(async kvp =>
            {
                try
                {
                    await SendCommandAndCollectEvents(kvp.Value,
                        WebSocketActions.Commands.Play,
                        new PlayCommandPayload("jazzy", 90),
                        receivedEvents);
                    clientStates[kvp.Key] = "playing";
                    Interlocked.Increment(ref successfulCommands);
                }
                catch (Exception ex)
                {
                    _output.WriteLine($"Initial play failed for client {kvp.Key}: {ex.Message}");
                    Interlocked.Increment(ref failedCommands);
                }
            });
            await Task.WhenAll(initialPlayTasks);

            // Act - Send alternating stop/start commands from all clients simultaneously
            var sw = Stopwatch.StartNew();
            var tasks = new List<Task>();

            foreach (var kvp in _clients)
            {
                var clientId = kvp.Key;
                var client = kvp.Value;
                
                tasks.Add(Task.Run(async () =>
                {
                    for (int i = 0; i < iterations && !_cts.Token.IsCancellationRequested; i++)
                    {
                        try
                        {
                            var currentState = clientStates[clientId];
                            var cmdSw = Stopwatch.StartNew();

                            if (currentState == "playing")
                            {
                                // Send stop command
                                await SendCommandAndCollectEvents(client, 
                                    WebSocketActions.Commands.Stop, 
                                    new StopCommandPayload(true),
                                    receivedEvents);
                                clientStates[clientId] = "stopped";
                            }
                            else
                            {
                                // Send start command
                                await SendCommandAndCollectEvents(client,
                                    WebSocketActions.Commands.Play,
                                    new PlayCommandPayload("jazzy", 90),
                                    receivedEvents);
                                clientStates[clientId] = "playing";
                            }

                            cmdSw.Stop();
                            responseLatencies.Add(cmdSw.Elapsed);
                            Interlocked.Increment(ref successfulCommands);

                            await Task.Delay(10, _cts.Token);
                        }
                        catch (Exception ex)
                        {
                            _output.WriteLine($"Command failed for client {clientId}: {ex.Message}");
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
            _output.WriteLine("Event counts:");
            foreach (var evt in receivedEvents)
            {
                _output.WriteLine($"  {evt.Key}: {evt.Value}");
            }
            _output.WriteLine("Final client states:");
            foreach (var state in clientStates)
            {
                _output.WriteLine($"  Client {state.Key}: {state.Value}");
            }

            // Assert
            var expectedCommands = numClients * (iterations + 1); // +1 for initial play
            Assert.True(successfulCommands > expectedCommands * 0.95, 
                $"Expected at least 95% success rate, got {(double)successfulCommands/expectedCommands:P2}");
            Assert.True(avgLatency < 100, 
                $"Average latency {avgLatency:F2}ms exceeds 100ms threshold");
            Assert.True(maxLatency < 500, 
                $"Max latency {maxLatency:F2}ms exceeds 500ms threshold");
            Assert.True(failedCommands == 0, 
                $"Expected no failed commands, got {failedCommands}");

            // Verify event counts
            var startedCount = receivedEvents.GetValueOrDefault(WebSocketActions.Events.PlaybackStarted);
            var stoppedCount = receivedEvents.GetValueOrDefault(WebSocketActions.Events.PlaybackStopped);
            
            _output.WriteLine($"Started events: {startedCount}, Stopped events: {stoppedCount}");
            
            Assert.True(Math.Abs(startedCount - stoppedCount) <= numClients, 
                $"Large discrepancy between start ({startedCount}) and stop ({stoppedCount}) events");
            Assert.True(startedCount >= numClients * iterations * 0.95,
                $"Not enough PlaybackStarted events. Expected at least {numClients * iterations * 0.95}, got {startedCount}");
        }
        finally
        {
            await CleanupClientsAsync();
        }
    }

    [SkippableFact]
    [Trait("Category", "AI_Generated")]
    [Trait("Category", "Performance")]
    public async Task RapidStartStopToggle_ShouldMaintainConsistentState()
    {
        Skip.If(Environment.GetEnvironmentVariable("CI") != null, 
            "Performance tests are skipped in CI environment");

        // Arrange
        const int numClients = 5;
        const int togglesPerClient = 20;
        var wsUri = new Uri("ws://localhost/ws/lofi");
        var receivedEvents = new ConcurrentDictionary<string, int>();
        var finalStates = new ConcurrentDictionary<string, string>();

        // Create connections
        for (int i = 0; i < numClients; i++)
        {
            var client = await _wsClient.ConnectAsync(wsUri, _cts.Token);
            _clients.TryAdd(i.ToString(), client);
            await Task.Delay(50, _cts.Token); // Stagger connections
        }

        try
        {
            // Act - Rapidly toggle playback state
            var tasks = _clients.Select(kvp => Task.Run(async () =>
            {
                var clientId = kvp.Key;
                var client = kvp.Value;
                var lastCommand = "";

                for (int i = 0; i < togglesPerClient && !_cts.Token.IsCancellationRequested; i++)
                {
                    try
                    {
                        // Alternate between play and stop
                        if (i % 2 == 0)
                        {
                            await SendCommandAndCollectEvents(client,
                                WebSocketActions.Commands.Play,
                                new PlayCommandPayload("jazzy", 90),
                                receivedEvents);
                            lastCommand = "play";
                        }
                        else
                        {
                            await SendCommandAndCollectEvents(client,
                                WebSocketActions.Commands.Stop,
                                new StopCommandPayload(true),
                                receivedEvents);
                            lastCommand = "stop";
                        }

                        await Task.Delay(5, _cts.Token); // Minimal delay between toggles
                    }
                    catch (Exception ex)
                    {
                        _output.WriteLine($"Client {clientId} toggle {i} failed: {ex.Message}");
                    }
                }

                // Record final command for this client
                finalStates.TryAdd(clientId, lastCommand);
            }, _cts.Token));

            await Task.WhenAll(tasks);

            // Log event counts
            _output.WriteLine("Event counts:");
            foreach (var evt in receivedEvents)
            {
                _output.WriteLine($"  {evt.Key}: {evt.Value}");
            }

            // Log final states
            _output.WriteLine("Final states:");
            foreach (var state in finalStates)
            {
                _output.WriteLine($"  Client {state.Key}: {state.Value}");
            }

            // Assert
            var startedCount = receivedEvents.GetValueOrDefault(WebSocketActions.Events.PlaybackStarted);
            var stoppedCount = receivedEvents.GetValueOrDefault(WebSocketActions.Events.PlaybackStopped);

            // The difference between start and stop events should be small
            Assert.True(Math.Abs(startedCount - stoppedCount) <= numClients,
                $"Large discrepancy between start ({startedCount}) and stop ({stoppedCount}) events");

            // Each client should have received responses for most commands
            var expectedEventsPerClient = togglesPerClient;
            var minExpectedTotal = (int)(numClients * expectedEventsPerClient * 0.9); // Allow 10% tolerance
            Assert.True(startedCount + stoppedCount >= minExpectedTotal,
                $"Expected at least {minExpectedTotal} total events, got {startedCount + stoppedCount}");
        }
        finally
        {
            await CleanupClientsAsync();
        }
    }

    private async Task SendCommandAndCollectEvents(
        WebSocket client,
        string action,
        object payload,
        ConcurrentDictionary<string, int> eventCounts)
    {
        if (client.State != WebSocketState.Open)
            throw new InvalidOperationException("WebSocket not connected");

        var command = new WebSocketMessage(
            Core.WebSocket.WebSocketMessageType.Command,
            action,
            payload);

        var json = JsonSerializer.Serialize(command);
        var buffer = Encoding.UTF8.GetBytes(json);

        await client.SendAsync(
            new ArraySegment<byte>(buffer),
            System.Net.WebSockets.WebSocketMessageType.Text,
            true,
            _cts.Token);

        // Wait for command acknowledgment and state change event
        var responseBuffer = new byte[4096];
        var receivedCount = 0;
        var receivedTypes = new HashSet<string>();
        var startTime = DateTime.UtcNow;
        var timeout = TimeSpan.FromMilliseconds(200);

        // Keep receiving until we get both command ack and state change, or timeout
        while (receivedTypes.Count < 2 && DateTime.UtcNow - startTime < timeout)
        {
            try
            {
                using var receiveTimeout = new CancellationTokenSource(TimeSpan.FromMilliseconds(50));
                using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token, receiveTimeout.Token);

                var result = await client.ReceiveAsync(
                    new ArraySegment<byte>(responseBuffer),
                    linkedCts.Token);

                if (result.MessageType == System.Net.WebSockets.WebSocketMessageType.Close)
                    throw new InvalidOperationException("WebSocket closed unexpectedly");

                var response = Encoding.UTF8.GetString(responseBuffer, 0, result.Count);
                var message = JsonSerializer.Deserialize<WebSocketMessage>(response);

                if (message != null)
                {
                    receivedCount++;
                    
                    // Only count playback events
                    if (message.Action == WebSocketActions.Events.PlaybackStarted || 
                        message.Action == WebSocketActions.Events.PlaybackStopped)
                    {
                        eventCounts.AddOrUpdate(
                            message.Action,
                            1,
                            (_, count) => count + 1);

                        _output?.WriteLine($"Received event: {message.Action}");
                    }
                    else
                    {
                        _output?.WriteLine($"Received non-playback event: {message.Action}");
                    }

                    // Track unique message types for completion check
                    receivedTypes.Add(message.Action);

                    // If we got both command ack and state change, we can stop
                    if (receivedTypes.Count >= 2)
                        break;
                }
            }
            catch (OperationCanceledException) when (DateTime.UtcNow - startTime >= timeout)
            {
                // Normal timeout, break the loop
                break;
            }
            catch (Exception ex)
            {
                _output?.WriteLine($"Error receiving message: {ex.Message}");
                if (receivedCount == 0)
                    throw; // Only throw if we haven't received any messages
                break;
            }
        }

        if (receivedCount == 0)
        {
            throw new InvalidOperationException("No response received from server");
        }
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
    }
} 