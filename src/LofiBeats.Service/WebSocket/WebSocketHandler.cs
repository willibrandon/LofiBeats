using LofiBeats.Core.BeatGeneration;
using LofiBeats.Core.Effects;
using LofiBeats.Core.Playback;
using LofiBeats.Core.Scheduling;
using LofiBeats.Core.Telemetry;
using LofiBeats.Core.WebSocket;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

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
    private readonly ConcurrentDictionary<Guid, ClientRateLimit> _rateLimits;
    private readonly SemaphoreSlim _broadcastLock;
    private readonly IHttpContextAccessor _httpContextAccessor;
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

    private static readonly Action<ILogger, Guid, Exception?> _logRateLimitExceeded =
        LoggerMessage.Define<Guid>(LogLevel.Warning, new EventId(7, "RateLimitExceeded"),
            "Rate limit exceeded for client {ClientId}");

    private static readonly Action<ILogger, Guid, Exception?> _logAuthenticationFailed =
        LoggerMessage.Define<Guid>(LogLevel.Warning, new EventId(8, "AuthenticationFailed"),
            "Authentication failed for client {ClientId}");

    private static readonly Action<ILogger, Guid, int, Exception?> _logMessageSizeExceeded =
        LoggerMessage.Define<Guid, int>(LogLevel.Warning, new EventId(9, "MessageSizeExceeded"),
            "Message size limit exceeded for client {ClientId}. Size: {Size} bytes");

    public WebSocketHandler(
        ILogger<WebSocketHandler> logger,
        IOptions<WebSocketConfiguration> config,
        IServiceProvider serviceProvider,
        IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _config = config.Value;
        _connections = new ConcurrentDictionary<Guid, WebSocketConnection>();
        _rateLimits = new ConcurrentDictionary<Guid, ClientRateLimit>();
        _broadcastLock = new SemaphoreSlim(1, 1);
        _httpContextAccessor = httpContextAccessor;

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
            await socket.CloseOutputAsync(WebSocketCloseStatus.PolicyViolation, 
                "Maximum connections reached", cancellationToken);
            return;
        }

        var clientId = Guid.NewGuid();

        // Authenticate client if required
        if (_config.RequireAuthentication)
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
            {
                _logAuthenticationFailed(_logger, clientId, null);
                await socket.CloseOutputAsync(WebSocketCloseStatus.PolicyViolation, 
                    "Authentication required", cancellationToken);
                return;
            }

            var token = context.Request.Query["token"].ToString();
            if (string.IsNullOrEmpty(token) || token != _config.AuthToken)
            {
                _logAuthenticationFailed(_logger, clientId, null);
                await socket.CloseOutputAsync(WebSocketCloseStatus.PolicyViolation, 
                    "Authentication required", cancellationToken);
                return;
            }
        }

        var connection = new WebSocketConnection(socket);
        
        if (_connections.TryAdd(clientId, connection))
        {
            _logClientConnected(_logger, _connections.Count, null);
            _rateLimits.TryAdd(clientId, new ClientRateLimit(_config.MessageRateLimit));
        }

        try
        {
            await ReceiveMessagesAsync(clientId, connection, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            // Normal cancellation, try to close gracefully
            try
            {
                await connection.CloseAsync(WebSocketCloseStatus.NormalClosure, "Server shutdown", CancellationToken.None);
            }
            catch (Exception ex)
            {
                _logClientCloseError(_logger, clientId, ex);
            }
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
            _rateLimits.TryRemove(clientId, out _);

            try
            {
                // Use a new token to ensure we can close even if canceled
                await connection.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed", CancellationToken.None);
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

        // 1. Acquire lock briefly, just to snapshot current connections
        await _broadcastLock.WaitAsync(cancellationToken);
        WebSocketConnection[] snapshot;
        try
        {
            snapshot = _connections.Values.ToArray();
        }
        finally
        {
            _broadcastLock.Release();
        }

        // 2. Send to all connections outside the lock
        var tasks = snapshot.Select(conn =>
            conn.SendAsync(messageBytes, System.Net.WebSockets.WebSocketMessageType.Text, true, cancellationToken));
        await Task.WhenAll(tasks);
    }

    private async Task ReceiveMessagesAsync(Guid clientId, WebSocketConnection connection, 
        CancellationToken cancellationToken)
    {
        var buffer = new byte[_config.MaxMessageSize];
        var rateLimit = _rateLimits[clientId];

        while (!cancellationToken.IsCancellationRequested)
        {
            // Check rate limit
            if (!rateLimit.CheckLimit())
            {
                _logRateLimitExceeded(_logger, clientId, null);
                await connection.CloseAsync(cancellationToken);
                return;
            }

            var result = await connection.ReceiveAsync(buffer, cancellationToken);
            if (result.MessageType == System.Net.WebSockets.WebSocketMessageType.Close)
            {
                return;
            }

            if (result.MessageType != System.Net.WebSockets.WebSocketMessageType.Text)
            {
                continue;
            }

            // Check message size
            if (result.Count > _config.MaxMessageSize)
            {
                _logMessageSizeExceeded(_logger, clientId, result.Count, null);
                await connection.CloseAsync(cancellationToken);
                return;
            }

            var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
            _logMessageReceived(_logger, message, null);
            
            // Log the raw bytes for debugging
            var byteString = BitConverter.ToString(buffer, 0, result.Count);
            _logMessageBytes(_logger, byteString, result.Count, null);

            try
            {
                var command = JsonSerializer.Deserialize<WebSocketMessage>(message);
                if (command != null && command.Type == Core.WebSocket.WebSocketMessageType.Command)
                {
                    await _commandHandler.HandleCommandAsync(clientId, command.Action, 
                        JsonSerializer.SerializeToElement(command.Payload), cancellationToken);
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
        _rateLimits.Clear();
    }
}

/// <summary>
/// Tracks rate limiting for a client
/// </summary>
internal sealed class ClientRateLimit
{
    private readonly int _maxMessagesPerSecond;
    private readonly Queue<DateTime> _messageTimestamps;
    private readonly object _lock = new();

    public ClientRateLimit(int maxMessagesPerSecond)
    {
        _maxMessagesPerSecond = maxMessagesPerSecond;
        _messageTimestamps = new Queue<DateTime>();
    }

    public bool CheckLimit()
    {
        lock (_lock)
        {
            var now = DateTime.UtcNow;
            var cutoff = now.AddSeconds(-1);

            // Remove timestamps older than 1 second
            while (_messageTimestamps.Count > 0 && _messageTimestamps.Peek() < cutoff)
            {
                _messageTimestamps.Dequeue();
            }

            // Check if we're under the limit
            if (_messageTimestamps.Count < _maxMessagesPerSecond)
            {
                _messageTimestamps.Enqueue(now);
                return true;
            }

            return false;
        }
    }
} 