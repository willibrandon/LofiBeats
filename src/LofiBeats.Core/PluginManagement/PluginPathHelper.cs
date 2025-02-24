using System.Runtime.InteropServices;

namespace LofiBeats.Core.PluginManagement;

public static class PluginPathHelper
{
    public static string GetPluginDirectory()
    {
        // OS detection logic (similar to user samples)
        string basePath;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(basePath, "LofiBeats", "Plugins");
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            return Path.Combine(home, "Library", "Application Support", "LofiBeats", "Plugins");
        }
        else
        {
            // Linux or others
            string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            return Path.Combine(home, ".local", "share", "LofiBeats", "Plugins");
        }
    }

    public static string EnsurePluginDirectoryExists()
    {
        var dir = GetPluginDirectory();
        Directory.CreateDirectory(dir);
        return dir;
    }
}
