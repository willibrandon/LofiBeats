using Microsoft.Extensions.Logging;

namespace LofiBeats.Core.PluginManagement;

/// <summary>
/// Watches the plugin directory for changes and triggers plugin refresh when needed.
/// </summary>
public sealed class PluginWatcher : IDisposable
{
    private readonly ILogger<PluginWatcher> _logger;
    private readonly PluginManager _pluginManager;
    private readonly string? _overrideDirectory;
    private FileSystemWatcher? _watcher;
    private bool _isDisposed;
    private FileSystemEventHandler? _onCreated;
    private FileSystemEventHandler? _onDeleted;
    private FileSystemEventHandler? _onChanged;
    private ErrorEventHandler? _onError;
    private readonly Lock _lock = new();

    // High-performance structured logging
    private static readonly Action<ILogger, string, Exception?> _logStartingWatch =
        LoggerMessage.Define<string>(LogLevel.Information, new EventId(1, "StartingWatch"),
            "Starting plugin directory watch on {Directory}");

    private static readonly Action<ILogger, string, Exception?> _logPluginAdded =
        LoggerMessage.Define<string>(LogLevel.Information, new EventId(2, "PluginAdded"),
            "New plugin detected: {FileName}");

    private static readonly Action<ILogger, string, Exception?> _logPluginRemoved =
        LoggerMessage.Define<string>(LogLevel.Information, new EventId(3, "PluginRemoved"),
            "Plugin removed: {FileName}");

    private static readonly Action<ILogger, string, Exception?> _logPluginChanged =
        LoggerMessage.Define<string>(LogLevel.Information, new EventId(4, "PluginChanged"),
            "Plugin changed: {FileName}");

    private static readonly Action<ILogger, Exception?> _logStoppingWatch =
        LoggerMessage.Define(LogLevel.Information, new EventId(5, "StoppingWatch"),
            "Stopping plugin directory watch");

    private static readonly Action<ILogger, Exception> _logWatcherError =
        LoggerMessage.Define(LogLevel.Error, new EventId(6, "WatcherError"),
            "Error in plugin watcher");

    public PluginWatcher(ILogger<PluginWatcher> logger, PluginManager pluginManager, string? overrideDirectory = null)
    {
        _logger = logger;
        _pluginManager = pluginManager;
        _overrideDirectory = overrideDirectory;
    }

    /// <summary>
    /// Starts watching the plugin directory for changes.
    /// </summary>
    public void StartWatching()
    {
        lock (_lock)
        {
            if (_isDisposed) throw new ObjectDisposedException(nameof(PluginWatcher));
            if (_watcher != null) return; // Already watching

            string dir = _overrideDirectory ?? PluginPathHelper.GetPluginDirectory();
            Directory.CreateDirectory(dir); // Ensure directory exists
            _logStartingWatch(_logger, dir, null);

            _watcher = new FileSystemWatcher(dir, "*.dll")
            {
                NotifyFilter = NotifyFilters.FileName 
                    | NotifyFilters.LastWrite 
                    | NotifyFilters.CreationTime
                    | NotifyFilters.Size,
                IncludeSubdirectories = false,
                EnableRaisingEvents = false // Don't enable until we've wired up events
            };

            // Use local functions to ensure proper error handling
            _onCreated = (sender, e) =>
            {
                if (_isDisposed) return;
                try
                {
                    lock (_lock)
                    {
                        if (_isDisposed) return;
                        _logPluginAdded(_logger, e.Name ?? string.Empty, null);
                        _pluginManager.RefreshPlugins();
                    }
                }
                catch (Exception ex)
                {
                    _logWatcherError(_logger, ex);
                }
            };

            _onDeleted = (sender, e) =>
            {
                if (_isDisposed) return;
                try
                {
                    lock (_lock)
                    {
                        if (_isDisposed) return;
                        _logPluginRemoved(_logger, e.Name ?? string.Empty, null);
                        _pluginManager.RefreshPlugins();
                    }
                }
                catch (Exception ex)
                {
                    _logWatcherError(_logger, ex);
                }
            };

            _onChanged = (sender, e) =>
            {
                if (_isDisposed) return;
                try
                {
                    lock (_lock)
                    {
                        if (_isDisposed) return;
                        _logPluginChanged(_logger, e.Name ?? string.Empty, null);
                        _pluginManager.RefreshPlugins();
                    }
                }
                catch (Exception ex)
                {
                    _logWatcherError(_logger, ex);
                }
            };

            _onError = (sender, e) =>
            {
                _logWatcherError(_logger, e.GetException());
            };

            // Wire up events
            _watcher.Created += _onCreated;
            _watcher.Deleted += _onDeleted;
            _watcher.Changed += _onChanged;
            _watcher.Error += _onError;

            // Start watching
            _watcher.EnableRaisingEvents = true;
        }
    }

    /// <summary>
    /// Stops watching the plugin directory.
    /// </summary>
    public void StopWatching()
    {
        lock (_lock)
        {
            if (_watcher == null) return;

            _logStoppingWatch(_logger, null);
            _watcher.EnableRaisingEvents = false;

            // Remove event handlers
            if (_onCreated != null) _watcher.Created -= _onCreated;
            if (_onDeleted != null) _watcher.Deleted -= _onDeleted;
            if (_onChanged != null) _watcher.Changed -= _onChanged;
            if (_onError != null) _watcher.Error -= _onError;

            _watcher.Dispose();
            _watcher = null;

            // Clear handlers
            _onCreated = null;
            _onDeleted = null;
            _onChanged = null;
            _onError = null;
        }
    }

    public void Dispose()
    {
        lock (_lock)
        {
            if (_isDisposed) return;
            _isDisposed = true;

            if (_watcher != null)
            {
                StopWatching();
            }
        }
    }
} 