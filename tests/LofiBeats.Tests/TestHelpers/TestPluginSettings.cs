using LofiBeats.Core.Configuration;

namespace LofiBeats.Tests.TestHelpers;

/// <summary>
/// Helper class for creating test plugin settings.
/// </summary>
public static class TestPluginSettings
{
    /// <summary>
    /// Creates a new instance of PluginSettings with default test values.
    /// </summary>
    /// <param name="runOutOfProcess">Whether to run plugins out of process. Defaults to true.</param>
    /// <returns>A new PluginSettings instance.</returns>
    public static PluginSettings CreateDefault(bool runOutOfProcess = true)
    {
        return new PluginSettings
        {
            RunOutOfProcess = runOutOfProcess,
            HostStartupTimeoutSeconds = 5,
            MaxRestartAttempts = 3,
            AutoRestartFailedHosts = true,
            RestartDelayMilliseconds = 1000
        };
    }
} 