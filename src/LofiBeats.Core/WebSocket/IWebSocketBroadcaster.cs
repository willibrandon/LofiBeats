namespace LofiBeats.Core.WebSocket;

/// <summary>
/// Interface for broadcasting WebSocket events to connected clients.
/// </summary>
public interface IWebSocketBroadcaster
{
    /// <summary>
    /// Broadcasts an event to all connected clients.
    /// </summary>
    /// <param name="action">The event action name</param>
    /// <param name="payload">The event payload</param>
    /// <param name="cancellationToken">Token to cancel the operation</param>
    /// <returns>A task representing the broadcast operation</returns>
    Task BroadcastEventAsync(string action, object payload, CancellationToken cancellationToken);
} 