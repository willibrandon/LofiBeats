using Microsoft.Extensions.Logging;

namespace LofiBeats.Core.Configuration;

/// <summary>
/// Configuration settings for the plugin system.
/// </summary>
public class PluginSettings
{
    /// <summary>
    /// Gets or sets whether plugins should run in separate processes.
    /// </summary>
    public bool RunOutOfProcess { get; set; } = true;

    /// <summary>
    /// Gets or sets the timeout in seconds for plugin host startup.
    /// </summary>
    public int HostStartupTimeoutSeconds { get; set; } = 5;

    /// <summary>
    /// Gets or sets the maximum number of restart attempts for failed hosts.
    /// </summary>
    public int MaxRestartAttempts { get; set; } = 3;

    /// <summary>
    /// Gets or sets whether to automatically restart failed hosts.
    /// </summary>
    public bool AutoRestartFailedHosts { get; set; } = true;

    /// <summary>
    /// Gets or sets the delay in milliseconds between restart attempts.
    /// </summary>
    public int RestartDelayMilliseconds { get; set; } = 1000;
} 