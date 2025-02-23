using LofiBeats.Core.PluginApi;
using Microsoft.Extensions.Logging;
using NAudio.Wave;

namespace LofiBeats.Core.PluginManagement
{
    /// <summary>
    /// Manages the discovery, loading, and instantiation of plugin audio effects.
    /// </summary>
    public class PluginManager
    {
        private readonly ILogger<PluginManager> _logger;
        private readonly IPluginLoader _loader;
        private readonly Dictionary<string, Type> _registeredEffects = new();

        // High-performance structured logging
        private static readonly Action<ILogger, string, Exception?> _logDuplicateEffect =
            LoggerMessage.Define<string>(LogLevel.Warning, new EventId(1, "DuplicateEffect"),
                "Duplicate effect name found: {EffectName}");

        private static readonly Action<ILogger, string, string, string, string, Exception?> _logEffectRegistered =
            LoggerMessage.Define<string, string, string, string>(LogLevel.Information, new EventId(2, "EffectRegistered"),
                "Plugin effect registered: {EffectName} -> {Type} (v{Version} by {Author})");

        private static readonly Action<ILogger, string, Exception?> _logEffectNotFound =
            LoggerMessage.Define<string>(LogLevel.Warning, new EventId(3, "EffectNotFound"),
                "No plugin effect found with name {Name}");

        private static readonly Action<ILogger, string, Exception?> _logInstantiationFailed =
            LoggerMessage.Define<string>(LogLevel.Warning, new EventId(4, "InstantiationFailed"),
                "Failed to instantiate plugin effect {Name}");

        private static readonly Action<ILogger, string, Exception> _logInstantiationError =
            LoggerMessage.Define<string>(LogLevel.Error, new EventId(5, "InstantiationError"),
                "Error instantiating plugin effect {Name}");

        private static readonly Action<ILogger, string, Exception?> _logMissingAttribute =
            LoggerMessage.Define<string>(LogLevel.Warning, new EventId(6, "MissingAttribute"),
                "Plugin effect type {Type} is missing [PluginEffectName] attribute, skipping");

        public PluginManager(ILogger<PluginManager> logger, IPluginLoader loader)
        {
            _logger = logger;
            _loader = loader;
            RefreshPlugins();
        }

        /// <summary>
        /// Refresh the plugin list by scanning the plugin directory again.
        /// </summary>
        public virtual void RefreshPlugins()
        {
            _registeredEffects.Clear();
            var effectTypes = _loader.LoadEffectTypes();
            foreach (var type in effectTypes)
            {
                try
                {
                    if (Activator.CreateInstance(type) is IAudioEffect instance)
                    {
                        _registeredEffects[instance.Name.ToLowerInvariant()] = type;
                    }
                }
                catch
                {
                    // Skip effects that can't be instantiated
                }
            }
        }

        /// <summary>
        /// Lists plugin-based effect names discovered.
        /// </summary>
        public IEnumerable<string> GetEffectNames() => _registeredEffects.Keys;

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
                var instance = Activator.CreateInstance(effectType) as IAudioEffect;
                if (instance != null)
                {
                    instance.SetSource(source);
                }
                return instance;
            }
            catch
            {
                return null;
            }
        }
    }
} 