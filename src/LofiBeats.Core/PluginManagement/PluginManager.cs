using LofiBeats.Core.Effects;
using Microsoft.Extensions.Logging;
using NAudio.Wave;
using System.Reflection;

namespace LofiBeats.Core.PluginManagement
{
    /// <summary>
    /// Manages the discovery, loading, and instantiation of plugin audio effects.
    /// </summary>
    public class PluginManager
    {
        private readonly ILogger<PluginManager> _logger;
        private readonly IPluginLoader _loader;
        private readonly Dictionary<string, (Type Type, PluginEffectNameAttribute Metadata)> _registeredEffects = new();

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
            foreach (var t in effectTypes)
            {
                var nameAttr = t.GetCustomAttribute<PluginEffectNameAttribute>();
                if (nameAttr == null)
                {
                    _logMissingAttribute(_logger, t.FullName ?? t.Name, null);
                    continue;
                }

                string effectName = nameAttr.Name.ToLowerInvariant();

                // Avoid collisions with built-in effect names
                // Possibly skip if name matches a built-in effect
                if (_registeredEffects.ContainsKey(effectName))
                {
                    _logDuplicateEffect(_logger, effectName, null);
                    continue;
                }

                _registeredEffects[effectName] = (t, nameAttr);
                _logEffectRegistered(_logger, effectName, t.FullName ?? t.Name, nameAttr.Version, nameAttr.Author, null);
            }
        }

        /// <summary>
        /// Lists plugin-based effect names discovered.
        /// </summary>
        public IEnumerable<string> GetEffectNames() => _registeredEffects.Keys;

        /// <summary>
        /// Gets metadata for a specific effect.
        /// </summary>
        public PluginEffectNameAttribute? GetEffectMetadata(string effectName)
        {
            var key = effectName.ToLowerInvariant();
            return _registeredEffects.TryGetValue(key, out var effect) ? effect.Metadata : null;
        }

        /// <summary>
        /// Gets all registered effects with their metadata.
        /// </summary>
        public IEnumerable<(string Name, PluginEffectNameAttribute Metadata)> GetEffectsWithMetadata()
        {
            return _registeredEffects.Select(kvp => (kvp.Key, kvp.Value.Metadata));
        }

        /// <summary>
        /// Instantiates a new IAudioEffect plugin by name.
        /// </summary>
        public virtual IAudioEffect? CreateEffect(string effectName, ISampleProvider source)
        {
            ArgumentNullException.ThrowIfNull(source);

            var key = effectName.ToLowerInvariant();
            if (!_registeredEffects.TryGetValue(key, out var effect))
            {
                _logEffectNotFound(_logger, effectName, null);
                return null;
            }

            // Instantiate
            try
            {
                // We need a parameterless constructor or something that takes `ISampleProvider`
                // If your plugin effect requires different constructor arguments, adapt here
                var instance = Activator.CreateInstance(effect.Type) as IAudioEffect;
                if (instance != null)
                {
                    instance.SetSource(source);
                }
                else
                {
                    _logInstantiationFailed(_logger, effectName, null);
                }

                return instance;
            }
            catch (Exception ex)
            {
                _logInstantiationError(_logger, effectName, ex);
                return null;
            }
        }
    }
} 