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
        private readonly Dictionary<string, Type> _registeredEffects = [];

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
                instance?.SetSource(source);
                return instance;
            }
            catch
            {
                return null;
            }
        }
    }
} 