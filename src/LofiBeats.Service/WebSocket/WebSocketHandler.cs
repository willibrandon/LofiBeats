using LofiBeats.Core.WebSocket;
using Microsoft.Extensions.Options;
using System.Buffers;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace LofiBeats.Service.WebSocket;

/// <summary>
/// Handles WebSocket connections and message routing.
/// </summary>
public sealed class WebSocketHandler : IWebSocketHandler, IDisposable
{
    private readonly ConcurrentDictionary<Guid, System.Net.WebSockets.WebSocket> _clients = new();
    private readonly ILogger<WebSocketHandler> _logger;
    private readonly WebSocketConfiguration _config;
    private readonly IServiceProvider _services;
    private readonly SemaphoreSlim _broadcastLock = new(1, 1);
    private readonly ArrayPool<byte> _arrayPool = ArrayPool<byte>.Shared;
    private bool _disposed;

    private static readonly Action<ILogger, int, Exception?> _logClientConnected =
        LoggerMessage.Define<int>(LogLevel.Information, new EventId(1, "ClientConnected"), 
            "WebSocket client connected. Total clients: {ClientCount}");

    private static readonly Action<ILogger, Guid, Exception?> _logClientDisconnected =
        LoggerMessage.Define<Guid>(LogLevel.Information, new EventId(2, "ClientDisconnected"), 
            "WebSocket client {ClientId} disconnected");

    private static readonly Action<ILogger, string, Exception?> _logMessageReceived =
        LoggerMessage.Define<string>(LogLevel.Debug, new EventId(3, "MessageReceived"), 
            "Received WebSocket message: {Message}");

    private static readonly Action<ILogger, string, Exception?> _logBroadcastEvent =
        LoggerMessage.Define<string>(LogLevel.Debug, new EventId(4, "BroadcastEvent"), 
            "Broadcasting WebSocket event: {Action}");

    private static readonly Action<ILogger, Guid, Exception> _logClientError =
        LoggerMessage.Define<Guid>(LogLevel.Error, new EventId(5, "ClientError"), 
            "Error handling WebSocket client {ClientId}");

    private static readonly Action<ILogger, Guid, Exception> _logClientCloseError =
        LoggerMessage.Define<Guid>(LogLevel.Warning, new EventId(6, "ClientCloseError"), 
            "Error closing WebSocket for client {ClientId}");

    private static readonly Action<ILogger, Guid, Exception> _logInvalidJsonMessage =
        LoggerMessage.Define<Guid>(LogLevel.Warning, new EventId(7, "InvalidJsonMessage"), 
            "Invalid JSON message from client {ClientId}");

    public WebSocketHandler(
        ILogger<WebSocketHandler> logger,
        IOptions<WebSocketConfiguration> config,
        IServiceProvider services)
    {
        _logger = logger;
        _config = config.Value;
        _services = services;
    }

    /// <inheritdoc />
    public int ConnectedClientCount => _clients.Count;

    /// <inheritdoc />
    public async Task HandleClientAsync(System.Net.WebSockets.WebSocket socket, CancellationToken cancellationToken)
    {
        // Check connection limit
        if (_clients.Count >= _config.MaxConcurrentConnections)
        {
            await socket.CloseAsync(WebSocketCloseStatus.PolicyViolation, 
                "Maximum connections reached", cancellationToken);
            return;
        }

        var clientId = Guid.NewGuid();
        _clients[clientId] = socket;
        _logClientConnected(_logger, _clients.Count, null);

        try
        {
            await ReceiveMessagesAsync(clientId, socket, cancellationToken);
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
            _clients.TryRemove(clientId, out _);
            _logClientDisconnected(_logger, clientId, null);

            try
            {
                // Only try to close if not already closing/closed
                if (socket.State is WebSocketState.Open or WebSocketState.CloseReceived)
                {
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, 
                        "Closing", cancellationToken);
                }
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

        var message = new WebSocketMessage(Core.WebSocket.WebSocketMessageType.Event, action, payload);
        var json = JsonSerializer.Serialize(message);

        // Get buffer from pool
        var maxSize = Encoding.UTF8.GetMaxByteCount(json.Length);
        var buffer = _arrayPool.Rent(maxSize);
        var bytesWritten = Encoding.UTF8.GetBytes(json, buffer);

        try
        {
            // Ensure only one broadcast at a time to prevent memory spikes
            await _broadcastLock.WaitAsync(cancellationToken);
            try
            {
                var tasks = _clients.Values
                    .Where(ws => ws.State == WebSocketState.Open)
                    .Select(ws => ws.SendAsync(
                        new ArraySegment<byte>(buffer, 0, bytesWritten),
                        System.Net.WebSockets.WebSocketMessageType.Text,
                        true,
                        cancellationToken));

                await Task.WhenAll(tasks);
            }
            finally
            {
                _broadcastLock.Release();
            }
        }
        finally
        {
            // Return buffer to pool
            _arrayPool.Return(buffer);
        }
    }

    private async Task ReceiveMessagesAsync(Guid clientId, System.Net.WebSockets.WebSocket socket, 
        CancellationToken cancellationToken)
    {
        var buffer = _arrayPool.Rent(_config.MaxMessageSize);
        try
        {
            while (socket.State == WebSocketState.Open && !cancellationToken.IsCancellationRequested)
            {
                var result = await socket.ReceiveAsync(
                    new ArraySegment<byte>(buffer), cancellationToken);

                if (result.CloseStatus.HasValue)
                {
                    break;
                }

                if (result.Count > _config.MaxMessageSize)
                {
                    await socket.CloseAsync(WebSocketCloseStatus.MessageTooBig,
                        "Message exceeds size limit", cancellationToken);
                    break;
                }

                var json = Encoding.UTF8.GetString(buffer, 0, result.Count);
                _logMessageReceived(_logger, json, null);

                await ProcessMessageAsync(clientId, json, cancellationToken);
            }
        }
        finally
        {
            _arrayPool.Return(buffer);
        }
    }

    private async Task ProcessMessageAsync(Guid clientId, string json, CancellationToken cancellationToken)
    {
        try
        {
            var message = JsonSerializer.Deserialize<WebSocketMessage>(json);
            if (message == null) return;

            // TODO: Implement command routing based on message.Type and message.Action
            // This will be implemented in the next chunk
        }
        catch (JsonException ex)
        {
            _logInvalidJsonMessage(_logger, clientId, ex);
            if (_clients.TryGetValue(clientId, out var socket))
            {
                var errorMessage = new WebSocketMessage(
                    Core.WebSocket.WebSocketMessageType.Error,
                    WebSocketActions.Errors.InvalidPayload,
                    new ErrorPayload("Invalid JSON message format"));

                var errorJson = JsonSerializer.Serialize(errorMessage);
                var buffer = _arrayPool.Rent(Encoding.UTF8.GetMaxByteCount(errorJson.Length));
                try
                {
                    var bytesWritten = Encoding.UTF8.GetBytes(errorJson, buffer);
                    await socket.SendAsync(
                        new ArraySegment<byte>(buffer, 0, bytesWritten),
                        System.Net.WebSockets.WebSocketMessageType.Text,
                        true,
                        cancellationToken);
                }
                finally
                {
                    _arrayPool.Return(buffer);
                }
            }
        }
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        _broadcastLock.Dispose();
        
        // Close all WebSocket connections
        foreach (var socket in _clients.Values)
        {
            try
            {
                // Fire and forget close
                _ = socket.CloseAsync(WebSocketCloseStatus.EndpointUnavailable, 
                    "Server shutting down", CancellationToken.None);
            }
            catch
            {
                // Ignore errors during disposal
            }
        }
        
        _clients.Clear();
    }
} 