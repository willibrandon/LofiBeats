using LofiBeats.Core.PluginApi;
using LofiBeats.PluginHost.Models;
using Microsoft.Extensions.Logging;
using NAudio.Wave;
using System.Diagnostics;
using System.Text.Json;
using System.Text;

namespace LofiBeats.Core.PluginManagement
{
    /// <summary>
    /// Manages the discovery, loading, and instantiation of plugin audio effects.
    /// </summary>
    public class PluginManager : IPluginManager, IDisposable
    {
        private readonly ILogger<PluginManager> _logger;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IPluginLoader _loader;
        private readonly Dictionary<string, Type> _registeredEffects = [];
        private readonly Dictionary<string, (string Description, string Version, string Author)> _effectMetadata = [];
        private readonly Dictionary<string, PluginHostConnection> _hostConnections = [];
        private bool _isDisposed;

        public bool OutOfProcessEnabled { get; set; } = true;

        public PluginManager(ILogger<PluginManager> logger, ILoggerFactory loggerFactory, IPluginLoader loader)
        {
            _logger = logger;
            _loggerFactory = loggerFactory;
            _loader = loader;
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
                    // Try to find it in the test output directory
                    var testPluginHostPath = Path.Combine(
                        Path.GetDirectoryName(typeof(PluginManager).Assembly.Location) ?? baseDir,
                        "LofiBeats.PluginHost.dll"
                    );
                    _logger.LogInformation("Looking for plugin host in test directory: {TestPluginHostPath}", testPluginHostPath);

                    if (File.Exists(testPluginHostPath))
                    {
                        pluginHostPath = testPluginHostPath;
                        _logger.LogInformation("Found plugin host in test directory");
                    }
                    else
                    {
                        _logger.LogError("Plugin host not found at {Path} or {TestPath}", pluginHostPath, testPluginHostPath);
                        throw new InvalidOperationException($"Plugin host not found at {pluginHostPath} or {testPluginHostPath}");
                    }
                }

                _logger.LogInformation("Starting plugin host from {Path} for plugin {Plugin}", pluginHostPath, pluginPath);

                var startInfo = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = $"exec \"{pluginHostPath}\" --plugin-assembly \"{pluginPath}\"",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = Path.GetDirectoryName(pluginHostPath) ?? baseDir // Set working directory to help with assembly loading
                };

                _logger.LogInformation("Starting process with command: dotnet {Args}", startInfo.Arguments);

                var process = Process.Start(startInfo) 
                    ?? throw new InvalidOperationException("Failed to start plugin host process");

                var startTime = DateTime.UtcNow;
                var timeout = TimeSpan.FromSeconds(5);
                string? startupMessage = null;
                string? effectsLoadedMessage = null;
                var outputLines = new List<string>();
                var outputEvent = new AutoResetEvent(false);

                process.OutputDataReceived += (sender, e) =>
                {
                    if (e.Data == null) return;

                    lock (outputLines)
                    {
                        outputLines.Add(e.Data);
                        _logger.LogInformation("Plugin host output: {Line}", e.Data);

                        if (e.Data.Contains("[STATUS] PluginHost started"))
                        {
                            startupMessage = e.Data;
                        }
                        else if (e.Data.Contains("[STATUS] Loaded") && e.Data.Contains("effect type(s)"))
                        {
                            effectsLoadedMessage = e.Data;
                        }

                        if (startupMessage != null && effectsLoadedMessage != null)
                        {
                            outputEvent.Set();
                        }
                    }
                };

                process.ErrorDataReceived += (sender, e) =>
                {
                    if (e.Data != null)
                    {
                        _logger.LogError("Plugin host error: {Error}", e.Data);
                    }
                };

                // Create the connection first so it can set up its output handling
                connection = new PluginHostConnection(process, _loggerFactory.CreateLogger<PluginHostConnection>());
                _hostConnections[effectName] = connection;

                // Wait for startup messages or timeout
                var success = outputEvent.WaitOne(timeout);

                if (process.HasExited)
                {
                    var standardOutput = string.Join("\n", outputLines);
                    _logger.LogError("Plugin host process exited with code {Code}. Output: {Output}", 
                        process.ExitCode, standardOutput);
                    throw new InvalidOperationException(
                        $"Plugin host process exited prematurely. Exit code: {process.ExitCode}. " +
                        $"Output: {standardOutput}");
                }

                if (!success)
                {
                    var standardOutput = string.Join("\n", outputLines);
                    _logger.LogError("Plugin host startup timeout. Output: {Output}", standardOutput);
                    process.Kill();
                    throw new InvalidOperationException(
                        $"Plugin host failed to start: Timeout waiting for startup messages. " +
                        $"Output: {standardOutput}");
                }

                _logger.LogInformation("Plugin host started successfully with output:\n{Output}", string.Join("\n", outputLines));

                // Initialize the plugin host with a basic handshake
                var initPayload = JsonDocument.Parse(JsonSerializer.Serialize(new { action = "init" })).RootElement;
                var response = connection.SendMessageAsync<PluginResponse>(new PluginMessage
                {
                    Action = "init",
                    Payload = initPayload
                }).GetAwaiter().GetResult();

                if (response?.Status != "ok")
                {
                    throw new InvalidOperationException($"Failed to initialize plugin host: {response?.Message}");
                }

                _logger.LogInformation("Plugin host initialized successfully");
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

        public void Dispose()
        {
            if (_isDisposed) return;
            _isDisposed = true;

            foreach (var connection in _hostConnections.Values)
            {
                connection.Dispose();
            }
            _hostConnections.Clear();
        }
    }
} 