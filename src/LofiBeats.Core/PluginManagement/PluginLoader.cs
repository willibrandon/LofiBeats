using LofiBeats.Core.Effects;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace LofiBeats.Core.PluginManagement;

/// <summary>
/// Loads audio effect plugins from the plugin directory by scanning for assemblies
/// containing types that implement IAudioEffect.
/// </summary>
public class PluginLoader : IPluginLoader
{
    private readonly ILogger<PluginLoader> _logger;
    private readonly string _pluginDirectory;

    // High-performance structured logging
    private static readonly Action<ILogger, string, Exception?> _logPluginDirectoryNotFound =
        LoggerMessage.Define<string>(LogLevel.Warning, new EventId(1, "PluginDirectoryNotFound"),
            "Plugin directory does not exist: {Dir}");

    private static readonly Action<ILogger, string, Exception> _logPluginLoadError =
        LoggerMessage.Define<string>(LogLevel.Error, new EventId(2, "PluginLoadError"),
            "Failed to load plugin assembly {DllPath}");

    private static readonly Action<ILogger, string, int, Exception?> _logPluginTypesFound =
        LoggerMessage.Define<string, int>(LogLevel.Information, new EventId(3, "PluginTypesFound"),
            "Found {Count} effect types in assembly {DllPath}");

    /// <summary>
    /// Initializes a new instance of the PluginLoader.
    /// </summary>
    /// <param name="logger">Logger for capturing diagnostic information.</param>
    public PluginLoader(ILogger<PluginLoader> logger)
    {
        _logger = logger;
        _pluginDirectory = PluginPathHelper.EnsurePluginDirectoryExists();
    }

    /// <summary>
    /// Loads and returns all types implementing IAudioEffect from plugin assemblies.
    /// </summary>
    /// <returns>A collection of types that implement IAudioEffect.</returns>
    public IEnumerable<Type> LoadEffectTypes()
    {
        var effectTypes = new List<Type>();

        if (!Directory.Exists(_pluginDirectory))
        {
            _logPluginDirectoryNotFound(_logger, _pluginDirectory, null);
            return effectTypes;
        }

        // Load all .dll assemblies from the plugin directory
        foreach (var dllPath in Directory.EnumerateFiles(_pluginDirectory, "*.dll"))
        {
            try
            {
                // Load the assembly
                var assembly = Assembly.LoadFrom(dllPath);

                // Find all types implementing IAudioEffect
                var types = assembly.GetTypes()
                    .Where(t => typeof(IAudioEffect).IsAssignableFrom(t) && !t.IsAbstract && t.IsClass)
                    .ToArray();

                if (types.Length > 0)
                {
                    _logPluginTypesFound(_logger, dllPath, types.Length, null);
                    effectTypes.AddRange(types);
                }
            }
            catch (Exception ex)
            {
                _logPluginLoadError(_logger, dllPath, ex);
            }
        }

        return effectTypes;
    }
} 