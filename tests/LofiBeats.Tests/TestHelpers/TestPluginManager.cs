using LofiBeats.Core.Configuration;
using LofiBeats.Core.PluginManagement;
using Microsoft.Extensions.Logging;

namespace LofiBeats.Tests.TestHelpers;

/// <summary>
/// A test version of PluginManager that allows tracking RefreshPlugins calls.
/// </summary>
public class TestPluginManager : PluginManager
{
    private Action? _onRefreshPlugins;

    public TestPluginManager(
        ILogger<PluginManager> logger,
        ILoggerFactory loggerFactory,
        IPluginLoader loader,
        PluginSettings settings,
        Action onRefreshPlugins)
        : base(logger, loggerFactory, loader, settings)
    {
        _onRefreshPlugins = onRefreshPlugins;
    }

    public override void RefreshPlugins()
    {
        _onRefreshPlugins?.Invoke();
        base.RefreshPlugins();
    }
} 