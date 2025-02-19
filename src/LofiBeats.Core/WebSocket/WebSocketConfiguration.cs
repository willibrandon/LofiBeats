namespace LofiBeats.Core.WebSocket;

/// <summary>
/// Configuration options for WebSocket connections.
/// </summary>
public sealed record WebSocketConfiguration
{
    /// <summary>
    /// The endpoint path for WebSocket connections (e.g., "/ws/lofi").
    /// </summary>
    public string EndpointPath { get; init; } = "/ws/lofi";

    /// <summary>
    /// The keep-alive interval for WebSocket connections in seconds.
    /// </summary>
    public int KeepAliveIntervalSeconds { get; init; } = 30;

    /// <summary>
    /// The maximum message size in bytes (default: 32KB).
    /// </summary>
    public int MaxMessageSize { get; init; } = 32 * 1024;

    /// <summary>
    /// Whether to require authentication for WebSocket connections.
    /// </summary>
    public bool RequireAuthentication { get; init; }

    /// <summary>
    /// The authentication token to use if RequireAuthentication is true.
    /// This should be overridden in production with a secure value.
    /// </summary>
    public string? AuthToken { get; init; }

    /// <summary>
    /// The maximum number of concurrent WebSocket connections allowed.
    /// </summary>
    public int MaxConcurrentConnections { get; init; } = 100;

    /// <summary>
    /// The rate limit for messages per client per second.
    /// </summary>
    public int MessageRateLimit { get; init; } = 10;

    /// <summary>
    /// Validates the configuration values.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when configuration values are invalid.</exception>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(EndpointPath))
            throw new ArgumentException("EndpointPath cannot be empty", nameof(EndpointPath));

        if (!EndpointPath.StartsWith('/'))
            throw new ArgumentException("EndpointPath must start with /", nameof(EndpointPath));

        if (KeepAliveIntervalSeconds < 1)
            throw new ArgumentException("KeepAliveIntervalSeconds must be positive", nameof(KeepAliveIntervalSeconds));

        if (MaxMessageSize < 1024)
            throw new ArgumentException("MaxMessageSize must be at least 1KB", nameof(MaxMessageSize));

        if (MaxConcurrentConnections < 1)
            throw new ArgumentException("MaxConcurrentConnections must be positive", nameof(MaxConcurrentConnections));

        if (MessageRateLimit < 1)
            throw new ArgumentException("MessageRateLimit must be positive", nameof(MessageRateLimit));

        if (RequireAuthentication && string.IsNullOrWhiteSpace(AuthToken))
            throw new ArgumentException("AuthToken must be set when RequireAuthentication is true", nameof(AuthToken));
    }
} 