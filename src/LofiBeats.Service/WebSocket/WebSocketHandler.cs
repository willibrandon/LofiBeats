using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using LofiBeats.Core.BeatGeneration;
using LofiBeats.Core.Effects;
using LofiBeats.Core.Models;
using LofiBeats.Core.Playback;
using LofiBeats.Core.Scheduling;
using LofiBeats.Core.Telemetry;
using LofiBeats.Core.WebSocket;

namespace LofiBeats.Service.WebSocket;

/// <summary>
/// Handles WebSocket connections and message routing.
/// </summary>
public sealed class WebSocketHandler : IWebSocketHandler, IWebSocketBroadcaster, IDisposable
{
    private readonly ILogger<WebSocketHandler> _logger;
    private readonly WebSocketConfiguration _config;
    private readonly WebSocketCommandHandler _commandHandler;
    private readonly ConcurrentDictionary<Guid, WebSocketConnection> _connections;
    private readonly SemaphoreSlim _broadcastLock;
    private bool _disposed;

    private static readonly Action<ILogger, int, Exception?> _logClientConnected =
        LoggerMessage.Define<int>(LogLevel.Information, new EventId(1, "ClientConnected"), 
            "WebSocket client connected. Total clients: {ClientCount}");

    private static readonly Action<ILogger, Guid, Exception?> _logClientDisconnected =
        LoggerMessage.Define<Guid>(LogLevel.Information, new EventId(2, "ClientDisconnected"), 
            "WebSocket client {ClientId} disconnected");

    private static readonly Action<ILogger, string, Exception?> _logMessageReceived =
        LoggerMessage.Define<string>(LogLevel.Debug, new EventId(4, "MessageReceived"), 
            "Received WebSocket message: {Message}");

    private static readonly Action<ILogger, string, Exception?> _logBroadcastEvent =
        LoggerMessage.Define<string>(LogLevel.Debug, new EventId(5, "BroadcastEvent"), 
            "Broadcasting WebSocket event: {Action}");

    private static readonly Action<ILogger, Guid, Exception> _logClientError =
        LoggerMessage.Define<Guid>(LogLevel.Error, new EventId(1, "ClientError"),
            "Error handling WebSocket client {ClientId}");

    private static readonly Action<ILogger, Guid, Exception> _logClientCloseError =
        LoggerMessage.Define<Guid>(LogLevel.Warning, new EventId(2, "ClientCloseError"),
            "Error closing WebSocket client {ClientId}");

    private static readonly Action<ILogger, Guid, string, Exception?> _logInvalidJsonMessage =
        LoggerMessage.Define<Guid, string>(LogLevel.Warning, new EventId(3, "InvalidJsonMessage"),
            "Invalid JSON message from client {ClientId}: {Error}");

    private static readonly Action<ILogger, string, int, Exception?> _logMessageBytes =
        LoggerMessage.Define<string, int>(LogLevel.Debug, new EventId(6, "MessageBytes"),
            "Message bytes: {ByteString}, Count: {Count}");

    public WebSocketHandler(
        ILogger<WebSocketHandler> logger,
        IOptions<WebSocketConfiguration> config,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _config = config.Value;
        _connections = new ConcurrentDictionary<Guid, WebSocketConnection>();
        _broadcastLock = new SemaphoreSlim(1, 1);

        // Create command handler with broadcast delegate
        _commandHandler = new WebSocketCommandHandler(
            serviceProvider.GetRequiredService<ILogger<WebSocketCommandHandler>>(),
            serviceProvider.GetRequiredService<IAudioPlaybackService>(),
            serviceProvider.GetRequiredService<IBeatGeneratorFactory>(),
            serviceProvider.GetRequiredService<IEffectFactory>(),
            serviceProvider.GetRequiredService<PlaybackScheduler>(),
            serviceProvider.GetRequiredService<UserSampleRepository>(),
            serviceProvider.GetRequiredService<TelemetryTracker>(),
            BroadcastEventAsync
        );
    }

    /// <inheritdoc />
    public int ConnectedClientCount => _connections.Count;

    /// <inheritdoc />
    public async Task HandleClientAsync(System.Net.WebSockets.WebSocket socket, CancellationToken cancellationToken)
    {
        // Check connection limit
        if (_connections.Count >= _config.MaxConcurrentConnections)
        {
            await socket.CloseAsync(WebSocketCloseStatus.PolicyViolation, 
                "Maximum connections reached", cancellationToken);
            return;
        }

        var clientId = Guid.NewGuid();
        var connection = new WebSocketConnection(socket);
        
        if (_connections.TryAdd(clientId, connection))
        {
            _logClientConnected(_logger, _connections.Count, null);
        }

        try
        {
            await ReceiveMessagesAsync(clientId, connection, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            // Normal cancellation, ignore
        }
        catch (Exception ex)
        {
            _logClientError(_logger, clientId, ex);
        }
        finally
        {
            if (_connections.TryRemove(clientId, out _))
            {
                _logClientDisconnected(_logger, clientId, null);
            }

            try
            {
                await connection.CloseAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logClientCloseError(_logger, clientId, ex);
            }
        }
    }

    /// <inheritdoc />
    public async Task BroadcastEventAsync(string action, object payload, CancellationToken cancellationToken)
    {
        if (_disposed) return;

        _logBroadcastEvent(_logger, action, null);

        var message = JsonSerializer.Serialize(new { action, payload });
        var messageBytes = Encoding.UTF8.GetBytes(message);

        await _broadcastLock.WaitAsync(cancellationToken);
        try
        {
            var tasks = _connections.Values.Select(connection =>
                connection.SendAsync(messageBytes, System.Net.WebSockets.WebSocketMessageType.Text, true, cancellationToken));
            await Task.WhenAll(tasks);
        }
        finally
        {
            _broadcastLock.Release();
        }
    }

    private async Task ReceiveMessagesAsync(Guid clientId, WebSocketConnection connection, 
        CancellationToken cancellationToken)
    {
        var buffer = new byte[32768];

        while (!cancellationToken.IsCancellationRequested)
        {
            var result = await connection.ReceiveAsync(buffer, cancellationToken);
            if (result.MessageType == System.Net.WebSockets.WebSocketMessageType.Close)
            {
                return;
            }

            if (result.MessageType != System.Net.WebSockets.WebSocketMessageType.Text)
            {
                continue;
            }

            var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
            _logMessageReceived(_logger, message, null);
            
            // Log the raw bytes for debugging
            var byteString = BitConverter.ToString(buffer, 0, result.Count);
            _logMessageBytes(_logger, byteString, result.Count, null);

            try
            {
                var command = JsonSerializer.Deserialize<WebSocketCommand>(message);
                if (command != null && command.Type == Core.WebSocket.WebSocketMessageType.Command)
                {
                    await _commandHandler.HandleCommandAsync(clientId, command.Action, 
                        command.Payload, cancellationToken);
                }
                else
                {
                    _logInvalidJsonMessage(_logger, clientId, "Invalid message type or null command", null);
                }
            }
            catch (JsonException ex)
            {
                _logInvalidJsonMessage(_logger, clientId, ex.Message, null);
            }
        }
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        _broadcastLock.Dispose();
        
        // Close all WebSocket connections
        foreach (var (id, connection) in _connections)
        {
            try
            {
                // Fire and forget close
                _ = connection.CloseAsync(CancellationToken.None);
            }
            catch
            {
                // Ignore errors during disposal
            }
        }
        
        _connections.Clear();
    }
} 