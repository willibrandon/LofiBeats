using System;
using System.Collections.Generic;

namespace LofiBeats.Core.PluginManagement;

/// <summary>
/// Interface for loading audio effect plugins from the plugin directory.
/// </summary>
public interface IPluginLoader
{
    /// <summary>
    /// Loads and returns all types implementing IAudioEffect from plugin assemblies.
    /// </summary>
    /// <returns>A collection of types that implement IAudioEffect.</returns>
    IEnumerable<Type> LoadEffectTypes();
} 