using System.Net.WebSockets;

namespace LofiBeats.Core.WebSocket;

/// <summary>
/// Handles WebSocket connections and message routing.
/// </summary>
public interface IWebSocketHandler
{
    /// <summary>
    /// Handles a new WebSocket client connection.
    /// </summary>
    /// <param name="socket">The WebSocket instance for the client</param>
    /// <param name="cancellationToken">Token to cancel the operation</param>
    /// <returns>A task representing the connection handling</returns>
    Task HandleClientAsync(System.Net.WebSockets.WebSocket socket, CancellationToken cancellationToken);

    /// <summary>
    /// Broadcasts an event to all connected clients.
    /// </summary>
    /// <param name="action">The event action name</param>
    /// <param name="payload">The event payload</param>
    /// <param name="cancellationToken">Token to cancel the operation</param>
    /// <returns>A task representing the broadcast operation</returns>
    Task BroadcastEventAsync(string action, object payload, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the current number of connected clients.
    /// </summary>
    int ConnectedClientCount { get; }
} 