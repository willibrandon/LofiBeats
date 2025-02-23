namespace LofiBeats.Core.PluginApi;

/// <summary>
/// Attribute to specify the name and metadata for a plugin audio effect.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class PluginEffectNameAttribute : Attribute
{
    /// <summary>
    /// Gets the unique name of the effect.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets or sets a description of what the effect does.
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

    /// <summary>
    /// Initializes a new instance of the <see cref="PluginEffectNameAttribute"/> class.
    /// </summary>
    /// <param name="name">The unique name of the effect.</param>
    /// <exception cref="ArgumentException">Thrown when name is null or empty.</exception>
    public PluginEffectNameAttribute(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Effect name cannot be null or empty.", nameof(name));
        }

        Name = name;
    }
} 