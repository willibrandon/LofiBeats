using System.Text.RegularExpressions;

namespace LofiBeats.Core.Telemetry;

/// <summary>
/// Configuration settings for telemetry collection.
/// </summary>
public class TelemetryConfiguration
{
    /// <summary>
    /// Gets or sets whether telemetry collection is enabled.
    /// </summary>
    public bool IsEnabled { get; set; } = true;

    /// <summary>
    /// Gets or sets whether this is a test environment.
    /// </summary>
    public bool IsTestEnvironment { get; set; }

    /// <summary>
    /// Gets or sets whether to enable Seq logging.
    /// </summary>
    public bool EnableSeq { get; set; }

    /// <summary>
    /// Gets or sets the Seq server URL.
    /// Defaults to http://localhost:5341 if not specified.
    /// </summary>
    public string SeqServerUrl { get; set; } = "http://localhost:5341";

    /// <summary>
    /// Gets or sets the Seq API key for authentication (optional).
    /// </summary>
    public string? SeqApiKey { get; set; }

    /// <summary>
    /// Gets or sets the maximum buffer size before forcing a flush.
    /// </summary>
    public int MaxBufferSize { get; set; } = 100;

    /// <summary>
    /// Gets or sets the flush interval in minutes.
    /// </summary>
    public int FlushIntervalMinutes { get; set; } = 1;

    /// <summary>
    /// Gets or sets the maximum file size in megabytes.
    /// </summary>
    public int MaxFileSizeMB { get; set; } = 10;

    /// <summary>
    /// Gets or sets the maximum age of telemetry files in days.
    /// </summary>
    public int MaxFileAgeDays { get; set; } = 30;

    /// <summary>
    /// Gets or sets patterns for events that should be excluded from production telemetry.
    /// </summary>
    public HashSet<string> ExcludedEventPatterns { get; set; } =
    [
        "^Test",
        "^Debug",
        "^Parallel",
        "^Mock"
    ];

    /// <summary>
    /// Optional delegate to override the base path calculation.
    /// This is primarily used for testing to ensure telemetry files are created in test-specific locations.
    /// </summary>
    public Func<string>? GetBasePath { get; set; }

    /// <summary>
    /// Gets or sets whether to enable local file logging.
    /// Defaults to true for backward compatibility.
    /// </summary>
    public bool EnableLocalFile { get; set; } = true;

    /// <summary>
    /// Checks if an event should be filtered out based on configuration.
    /// </summary>
    public bool ShouldFilterEvent(string eventName)
    {
        if (IsTestEnvironment) return false;

        return ExcludedEventPatterns.Any(pattern => 
            Regex.IsMatch(eventName, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled));
    }

    /// <summary>
    /// Validates a timestamp is not in the future (with a small tolerance).
    /// </summary>
    public static bool IsValidTimestamp(DateTimeOffset timestamp)
    {
        var tolerance = TimeSpan.FromMinutes(5); // Allow for small clock differences
        return timestamp <= DateTimeOffset.UtcNow + tolerance;
    }

    /// <summary>
    /// Gets the base path for telemetry storage.
    /// </summary>
    public string GetBasePathInternal()
    {
        // If a custom path provider is set, use it
        if (GetBasePath != null)
        {
            return GetBasePath();
        }

        var basePath = IsTestEnvironment ? 
            Path.Combine(Path.GetTempPath(), "LofiBeatsTests", "Telemetry") :
            Path.Combine(
                GetDefaultAppDataPath(),
                "LofiBeats",
                "Telemetry"
            );

        return Path.Combine(basePath, IsTestEnvironment ? "Test" : "Production");
    }

    private static string GetDefaultAppDataPath()
    {
        if (OperatingSystem.IsWindows())
        {
            return Environment.GetEnvironmentVariable("LOCALAPPDATA") ?? 
                   Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        }
        
        // For macOS, use ~/Library/Application Support
        if (OperatingSystem.IsMacOS())
        {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "Library",
                "Application Support"
            );
        }
        
        // For Linux, use ~/.local/share as per XDG Base Directory spec
        return Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".local",
            "share"
        );
    }

    /// <summary>
    /// Cleans up old telemetry files.
    /// </summary>
    public void CleanupOldFiles(string basePath)
    {
        if (!Directory.Exists(basePath)) return;

        var cutoffDate = DateTime.UtcNow.AddDays(-MaxFileAgeDays);
        var files = Directory.GetFiles(basePath, "telemetry_*.json");

        foreach (var file in files)
        {
            try
            {
                var fileInfo = new FileInfo(file);
                if (fileInfo.LastWriteTimeUtc < cutoffDate || 
                    fileInfo.Length > MaxFileSizeMB * 1024 * 1024)
                {
                    File.Delete(file);
                }
            }
            catch
            {
                // Ignore errors during cleanup
            }
        }
    }
} 