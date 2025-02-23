using System.Text.Json;
using LofiBeats.Core.Models;

namespace LofiBeats.Core.Storage;

/// <summary>
/// Provides functionality for saving and loading presets to/from JSON files.
/// </summary>
public static class PresetStorage
{
    private static readonly JsonSerializerOptions _serializerOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    /// <summary>
    /// Saves a preset to a JSON file.
    /// </summary>
    /// <param name="filePath">The path where the preset should be saved.</param>
    /// <param name="preset">The preset to save.</param>
    /// <exception cref="ArgumentNullException">Thrown when filePath or preset is null.</exception>
    /// <exception cref="ArgumentException">Thrown when filePath is empty or invalid.</exception>
    /// <exception cref="IOException">Thrown when there's an error writing to the file.</exception>
    public static void SavePreset(string filePath, Preset preset)
    {
        ArgumentNullException.ThrowIfNull(filePath);
        ArgumentNullException.ThrowIfNull(preset);

        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path cannot be empty.", nameof(filePath));

        // Validate the preset before saving
        preset.Validate();

        try
        {
            // Ensure the directory exists
            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory))
                Directory.CreateDirectory(directory);

            // Serialize and write atomically using a temporary file
            var tempPath = Path.GetTempFileName();
            var json = JsonSerializer.Serialize(preset, _serializerOptions);
            File.WriteAllText(tempPath, json);
            
            // Atomically replace the target file
            if (File.Exists(filePath))
                File.Delete(filePath);
            File.Move(tempPath, filePath);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            throw new IOException($"Failed to save preset to {filePath}: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Loads a preset from a JSON file.
    /// </summary>
    /// <param name="filePath">The path to the preset file.</param>
    /// <returns>The loaded preset, or null if the file doesn't exist.</returns>
    /// <exception cref="ArgumentNullException">Thrown when filePath is null.</exception>
    /// <exception cref="ArgumentException">Thrown when filePath is empty or invalid.</exception>
    /// <exception cref="JsonException">Thrown when the file contains invalid JSON or cannot be deserialized.</exception>
    /// <exception cref="IOException">Thrown when there's an error reading the file.</exception>
    public static Preset? LoadPreset(string filePath)
    {
        ArgumentNullException.ThrowIfNull(filePath);

        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path cannot be empty.", nameof(filePath));

        if (!File.Exists(filePath))
            return null;

        try
        {
            var json = File.ReadAllText(filePath);
            var preset = JsonSerializer.Deserialize<Preset>(json, _serializerOptions)
                ?? throw new JsonException($"Failed to deserialize preset from {filePath}");

            // Validate the loaded preset
            preset.Validate();
            return preset;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            throw new IOException($"Failed to load preset from {filePath}: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Checks if a preset file exists at the specified path.
    /// </summary>
    /// <param name="filePath">The path to check.</param>
    /// <returns>True if the preset file exists, false otherwise.</returns>
    /// <exception cref="ArgumentNullException">Thrown when filePath is null.</exception>
    public static bool PresetExists(string filePath)
    {
        ArgumentNullException.ThrowIfNull(filePath);
        return File.Exists(filePath);
    }
} 