namespace LofiBeats.Core.PluginManagement;

/// <summary>
/// Represents metadata about a plugin effect.
/// </summary>
public class PluginEffectInfo
{
    /// <summary>
    /// Gets or sets the unique name of the effect.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of what the effect does.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the version of the effect.
    /// </summary>
    public string Version { get; set; } = "1.0.0";

    /// <summary>
    /// Gets or sets the author of the effect.
    /// </summary>
    public string Author { get; set; } = string.Empty;
} 