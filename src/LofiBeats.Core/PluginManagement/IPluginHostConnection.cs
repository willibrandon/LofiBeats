namespace LofiBeats.Core.PluginManagement;

/// <summary>
/// Interface for managing communication with a plugin host process.
/// </summary>
public interface IPluginHostConnection : IDisposable
{
    /// <summary>
    /// Sends a message to the plugin host process and waits for a response.
    /// </summary>
    Task<T?> SendMessageAsync<T>(object message, CancellationToken cancellationToken = default);
} 