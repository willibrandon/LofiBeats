using LofiBeats.Core.PluginApi;
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

    // Configuration constants
    private const int MaxRecursionDepth = 8;  // Maximum directory depth to search (2³)
    private const int MaxPluginsPerDirectory = 64;  // Maximum plugins per directory (2⁶)
    private const int MaxTotalPlugins = 256;  // Maximum total plugins to load (2⁸)

    // High-performance structured logging
    private static readonly Action<ILogger, string, Exception?> _logPluginDirectoryNotFound =
        LoggerMessage.Define<string>(LogLevel.Warning, new EventId(1, "PluginDirectoryNotFound"),
            "Plugin directory does not exist: {Dir}");

    private static readonly Action<ILogger, string, Exception> _logPluginLoadError =
        LoggerMessage.Define<string>(LogLevel.Error, new EventId(2, "PluginLoadError"),
            "Failed to load plugin assembly {DllPath}");

    private static readonly Action<ILogger, string, Exception> _logPluginFileLocked =
        LoggerMessage.Define<string>(LogLevel.Warning, new EventId(3, "PluginFileLocked"),
            "Plugin file is locked and cannot be accessed: {DllPath}");

    private static readonly Action<ILogger, string, int, Exception?> _logPluginTypesFound =
        LoggerMessage.Define<string, int>(LogLevel.Information, new EventId(4, "PluginTypesFound"),
            "Found {Count} effect types in assembly {DllPath}");

    private static readonly Action<ILogger, string, Exception?> _logMaxPluginsExceeded =
        LoggerMessage.Define<string>(LogLevel.Warning, new EventId(5, "MaxPluginsExceeded"),
            "Maximum number of plugins exceeded in directory: {Dir}");

    private static readonly Action<ILogger, int, Exception?> _logMaxTotalPluginsExceeded =
        LoggerMessage.Define<int>(LogLevel.Warning, new EventId(6, "MaxTotalPluginsExceeded"),
            "Maximum total number of plugins ({Count}) exceeded");

    private static readonly Action<ILogger, string, int, Exception?> _logMaxDepthExceeded =
        LoggerMessage.Define<string, int>(LogLevel.Warning, new EventId(7, "MaxDepthExceeded"),
            "Maximum recursion depth ({Depth}) exceeded for directory: {Dir}");

    /// <summary>
    /// Initializes a new instance of the PluginLoader.
    /// </summary>
    /// <param name="logger">Logger for capturing diagnostic information.</param>
    /// <param name="pluginDirectory">Optional custom plugin directory path. If not specified, uses the default location.</param>
    public PluginLoader(ILogger<PluginLoader> logger, string? pluginDirectory = null)
    {
        _logger = logger;
        _pluginDirectory = pluginDirectory ?? PluginPathHelper.GetPluginDirectory();
        Directory.CreateDirectory(_pluginDirectory);
    }

    /// <summary>
    /// Loads and returns all types implementing IAudioEffect from plugin assemblies.
    /// </summary>
    /// <returns>A collection of types that implement IAudioEffect.</returns>
    public IEnumerable<Type> LoadEffectTypes()
    {
        var effectTypes = new List<Type>();

        // Ensure directory exists, recreate if necessary
        if (!Directory.Exists(_pluginDirectory))
        {
            _logPluginDirectoryNotFound(_logger, _pluginDirectory, null);
            Directory.CreateDirectory(_pluginDirectory);
        }

        try
        {
            // Use a stack to track directories to process and their depth
            var directories = new Stack<(string Path, int Depth)>();
            directories.Push((_pluginDirectory, 0));

            var totalPluginsLoaded = 0;

            while (directories.Count > 0 && totalPluginsLoaded < MaxTotalPlugins)
            {
                var (currentDir, depth) = directories.Pop();

                // Check recursion depth
                if (depth >= MaxRecursionDepth)
                {
                    _logMaxDepthExceeded(_logger, currentDir, depth, null);
                    continue;
                }

                // Process DLLs in current directory
                var pluginsInCurrentDir = 0;
                foreach (var dllPath in Directory.EnumerateFiles(currentDir, "*.dll"))
                {
                    if (pluginsInCurrentDir >= MaxPluginsPerDirectory)
                    {
                        _logMaxPluginsExceeded(_logger, currentDir, null);
                        break;
                    }

                    try
                    {
                        // Try to open the file first to check if it's locked
                        try
                        {
                            using var stream = File.Open(dllPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                        }
                        catch (IOException ex) when ((ex.HResult & 0x0000FFFF) == 32 || // ERROR_SHARING_VIOLATION
                                                   (ex.HResult & 0x0000FFFF) == 33)  // ERROR_LOCK_VIOLATION
                        {
                            _logPluginFileLocked(_logger, dllPath, ex);
                            continue;
                        }

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
                            pluginsInCurrentDir += types.Length;
                            totalPluginsLoaded += types.Length;

                            if (totalPluginsLoaded >= MaxTotalPlugins)
                            {
                                _logMaxTotalPluginsExceeded(_logger, MaxTotalPlugins, null);
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logPluginLoadError(_logger, dllPath, ex);
                    }
                }

                // Add subdirectories to the stack
                foreach (var subDir in Directory.EnumerateDirectories(currentDir))
                {
                    directories.Push((subDir, depth + 1));
                }
            }
        }
        catch (Exception ex)
        {
            _logPluginLoadError(_logger, _pluginDirectory, ex);
        }

        return effectTypes;
    }
} 