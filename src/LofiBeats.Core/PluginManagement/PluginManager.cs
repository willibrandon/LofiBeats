using LofiBeats.Core.Configuration;
using LofiBeats.Core.PluginApi;
using LofiBeats.PluginHost.Models;
using Microsoft.Extensions.Logging;
using NAudio.Wave;
using System.Diagnostics;
using System.Text.Json;

namespace LofiBeats.Core.PluginManagement;

/// <summary>
/// Manages the discovery, loading, and instantiation of plugin audio effects.
/// </summary>
public class PluginManager : IPluginManager, IDisposable
{
    private readonly ILogger<PluginManager> _logger;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IPluginLoader _loader;
    private readonly PluginSettings _settings;
    private readonly Dictionary<string, Type> _registeredEffects = [];
    private readonly Dictionary<string, (string Description, string Version, string Author)> _effectMetadata = [];
    private readonly Dictionary<string, PluginHostConnection> _hostConnections = [];
    private readonly Dictionary<string, int> _restartAttempts = [];
    private bool _isDisposed;

    // High-performance structured logging
    private static readonly Action<ILogger, string, Exception?> _logHostStartupFailed =
        LoggerMessage.Define<string>(LogLevel.Error, new EventId(1, "HostStartupFailed"),
            "Plugin host startup failed for effect {EffectName}");

    private static readonly Action<ILogger, string, int, Exception?> _logHostRestarting =
        LoggerMessage.Define<string, int>(LogLevel.Warning, new EventId(2, "HostRestarting"),
            "Restarting plugin host for effect {EffectName} (attempt {Attempt})");

    private static readonly Action<ILogger, string, Exception?> _logHostMaxRestartAttempts =
        LoggerMessage.Define<string>(LogLevel.Error, new EventId(3, "HostMaxRestartAttempts"),
            "Maximum restart attempts reached for effect {EffectName}");

    public bool OutOfProcessEnabled 
    { 
        get => _settings.RunOutOfProcess;
        set => _settings.RunOutOfProcess = value;
    }

    public PluginManager(ILogger<PluginManager> logger, ILoggerFactory loggerFactory, IPluginLoader loader, PluginSettings settings)
    {
        _logger = logger;
        _loggerFactory = loggerFactory;
        _loader = loader;
        _settings = settings;
        RefreshPlugins();
    }

    /// <summary>
    /// Refresh the plugin list by scanning the plugin directory again.
    /// </summary>
    public virtual void RefreshPlugins()
    {
        _registeredEffects.Clear();
        _effectMetadata.Clear();
        var effectTypes = _loader.LoadEffectTypes();
        foreach (var type in effectTypes)
        {
            try
            {
                if (Activator.CreateInstance(type) is IAudioEffect instance)
                {
                    var name = instance.Name.ToLowerInvariant();
                    _registeredEffects[name] = type;
                    _effectMetadata[name] = (instance.Description, instance.Version, instance.Author);
                }
            }
            catch
            {
                // Skip effects that can't be instantiated
            }
        }
    }

    /// <summary>
    /// Gets metadata for all available plugin effects.
    /// </summary>
    /// <returns>A collection of effect metadata.</returns>
    public IEnumerable<(string Name, string Description, string Version, string Author)> GetEffectMetadata()
    {
        return _effectMetadata.Select(kvp => (
            Name: kvp.Key,
            kvp.Value.Description,
            kvp.Value.Version,
            kvp.Value.Author
        ));
    }

    /// <summary>
    /// Lists plugin-based effect names discovered.
    /// </summary>
    public IEnumerable<string> GetEffectNames() => _registeredEffects.Keys;

    /// <summary>
    /// Instantiates a new IAudioEffect plugin by name.
    /// </summary>
    IAudioEffect? IPluginManager.CreateEffect(string effectName, ISampleProvider source)
    {
        return CreateEffect(effectName, source);
    }

    /// <summary>
    /// Instantiates a new IAudioEffect plugin by name.
    /// </summary>
    public virtual IAudioEffect? CreateEffect(string effectName, ISampleProvider source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var key = effectName.ToLowerInvariant();
        if (!_registeredEffects.TryGetValue(key, out var effectType))
        {
            return null;
        }

        try
        {
            if (OutOfProcessEnabled)
            {
                return CreateOutOfProcessEffect(key, effectType, source);
            }
            else
            {
                var instance = Activator.CreateInstance(effectType) as IAudioEffect;
                if (instance == null) return null;

                instance.SetSource(source);
                return instance;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create effect {EffectName}", effectName);
            return null;
        }
    }

    private PluginEffectProxy CreateOutOfProcessEffect(string effectName, Type effectType, ISampleProvider source)
    {
        // Get or create host connection for this plugin
        if (!_hostConnections.TryGetValue(effectName, out var connection))
        {
            var pluginPath = effectType.Assembly.Location;
            _logger.LogInformation("Plugin assembly location: {PluginPath}", pluginPath);

            var baseDir = AppContext.BaseDirectory;
            _logger.LogInformation("Base directory: {BaseDir}", baseDir);

            var pluginHostPath = Path.Combine(baseDir, "LofiBeats.PluginHost.dll");
            _logger.LogInformation("Looking for plugin host at: {PluginHostPath}", pluginHostPath);

            if (!File.Exists(pluginHostPath))
            {
                // Try to find it in the source location
                var sourcePluginHostPath = Path.Combine(
                    AppContext.BaseDirectory,
                    "..", "..", "..", "..",
                    "src", "LofiBeats.PluginHost", "bin", "Debug", "net9.0",
                    "LofiBeats.PluginHost.dll"
                );

                if (File.Exists(sourcePluginHostPath))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(pluginHostPath)!);
                    File.Copy(sourcePluginHostPath, pluginHostPath, true);
                }
                else
                {
                    throw new InvalidOperationException($"Plugin host not found at {pluginHostPath}");
                }
            }

            _logger.LogInformation("Starting plugin host from {Path} for plugin {Plugin}", pluginHostPath, pluginPath);

            connection = StartPluginHost(effectName, pluginPath);
        }

        // Create the effect in the plugin host
        var createPayload = JsonDocument.Parse(JsonSerializer.Serialize(new { EffectName = effectName })).RootElement;
        var createResponse = connection.SendMessageAsync<PluginResponse>(new PluginMessage
        {
            Action = "createEffect",
            Payload = createPayload
        }).GetAwaiter().GetResult();

        if (createResponse?.Status != "ok" || createResponse?.Payload == null)
        {
            throw new InvalidOperationException($"Failed to create effect in plugin host: {createResponse?.Message}");
        }

        var effectId = createResponse.Payload.Value.GetProperty("effectId").GetString()
            ?? throw new InvalidOperationException("Plugin host did not return an effect ID");

        // Create and return the proxy
        return new PluginEffectProxy(
            effectName,
            _effectMetadata[effectName].Description,
            _effectMetadata[effectName].Version,
            _effectMetadata[effectName].Author,
            effectId,
            source,
            connection,
            _loggerFactory.CreateLogger<PluginEffectProxy>()
        );
    }

    private PluginHostConnection StartPluginHost(string effectName, string pluginPath)
    {
        var outputLines = new List<string>();
        var outputEvent = new AutoResetEvent(false);
        var timeout = TimeSpan.FromSeconds(10);
        PluginHostConnection? connection = null;

        try
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = $"exec \"{GetPluginHostPath()}\" --plugin-assembly \"{pluginPath}\"",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            // Create the connection first, it will set up its own output handling
            process.Start();
            connection = new PluginHostConnection(process, _loggerFactory.CreateLogger<PluginHostConnection>());
            _hostConnections[effectName] = connection;

            // Wait for startup messages or timeout
            var success = outputEvent.WaitOne(timeout);

            if (process.HasExited)
            {
                var standardOutput = string.Join("\n", outputLines);
                _logger.LogError("Plugin host process exited with code {Code}. Output: {Output}", 
                    process.ExitCode, standardOutput);

                // Handle restart logic
                if (_settings.AutoRestartFailedHosts)
                {
                    _restartAttempts.TryGetValue(effectName, out var attempts);
                    if (attempts < _settings.MaxRestartAttempts)
                    {
                        _restartAttempts[effectName] = attempts + 1;
                        _logHostRestarting(_logger, effectName, attempts + 1, null);
                        Thread.Sleep(_settings.RestartDelayMilliseconds);
                        return StartPluginHost(effectName, pluginPath);
                    }
                    else
                    {
                        _logHostMaxRestartAttempts(_logger, effectName, null);
                    }
                }

                throw new InvalidOperationException(
                    $"Plugin host process exited prematurely. Exit code: {process.ExitCode}. " +
                    $"Output: {standardOutput}");
            }

            return connection;
        }
        catch
        {
            connection?.Dispose();
            throw;
        }
    }

    public void Dispose()
    {
        if (_isDisposed) return;

        foreach (var connection in _hostConnections.Values)
        {
            try
            {
                connection.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error disposing plugin host connection");
            }
        }

        _hostConnections.Clear();
        _restartAttempts.Clear();
        _isDisposed = true;
    }

    private static string GetPluginHostPath()
    {
        var hostPath = Path.Combine(
            AppContext.BaseDirectory,
            "LofiBeats.PluginHost.dll"
        );

        if (!File.Exists(hostPath))
        {
            throw new InvalidOperationException($"Plugin host not found at {hostPath}");
        }

        return hostPath;
    }
}
