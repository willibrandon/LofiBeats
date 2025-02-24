using LofiBeats.Core.PluginApi;
using NAudio.Wave;

namespace LofiBeats.Core.PluginManagement;

/// <summary>
/// Interface for managing plugin discovery, loading, and instantiation.
/// </summary>
public interface IPluginManager
{
    /// <summary>
    /// Gets metadata for all available plugin effects.
    /// </summary>
    /// <returns>A collection of effect metadata.</returns>
    IEnumerable<(string Name, string Description, string Version, string Author)> GetEffectMetadata();

    /// <summary>
    /// Lists plugin-based effect names discovered.
    /// </summary>
    IEnumerable<string> GetEffectNames();

    /// <summary>
    /// Instantiates a new IAudioEffect plugin by name.
    /// </summary>
    IAudioEffect? CreateEffect(string effectName, ISampleProvider source);

    /// <summary>
    /// Refresh the plugin list by scanning the plugin directory again.
    /// </summary>
    void RefreshPlugins();
} 