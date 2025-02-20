using System.Net.WebSockets;

namespace LofiBeats.Service.WebSocket;

/// <summary>
/// Wraps a WebSocket connection and provides convenience methods for sending and receiving messages.
/// </summary>
public sealed class WebSocketConnection
{
    private readonly System.Net.WebSockets.WebSocket _socket;

    public WebSocketConnection(System.Net.WebSockets.WebSocket socket)
    {
        _socket = socket;
    }

    public WebSocketState State => _socket.State;

    public async Task SendAsync(byte[] buffer, WebSocketMessageType messageType, bool endOfMessage, 
        CancellationToken cancellationToken)
    {
        await _socket.SendAsync(new ArraySegment<byte>(buffer), messageType, endOfMessage, cancellationToken);
    }

    public async Task<WebSocketReceiveResult> ReceiveAsync(byte[] buffer, CancellationToken cancellationToken)
    {
        return await _socket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
    }

    public async Task CloseAsync(CancellationToken cancellationToken)
    {
        await CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", cancellationToken);
    }

    public async Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
    {
        if (_socket.State == WebSocketState.Open)
        {
            await _socket.CloseAsync(closeStatus, statusDescription, cancellationToken);
        }
    }
} 