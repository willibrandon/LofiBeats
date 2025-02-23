using LofiBeats.Core.PluginManagement;
using Microsoft.Extensions.Hosting;

namespace LofiBeats.Service;

/// <summary>
/// Hosted service that manages the plugin watcher's lifecycle.
/// </summary>
public sealed class PluginWatcherService : IHostedService
{
    private readonly PluginWatcher _watcher;

    public PluginWatcherService(PluginWatcher watcher)
    {
        _watcher = watcher;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _watcher.StartWatching();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _watcher.StopWatching();
        return Task.CompletedTask;
    }
} 