using LofiBeats.Core.PluginManagement;
using System.Runtime.InteropServices;

namespace LofiBeats.Tests.PluginManagement;

[Collection("Plugin Tests")]
public class PluginPathHelperTests
{
    [SkippableFact]
    [Trait("Category", "AI_Generated")]
    public void GetPluginDirectory_OnWindows_ReturnsCorrectPath()
    {
        Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Windows), "Test only runs on Windows");
        
        var expected = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "LofiBeats",
            "Plugins");
        
        var actual = PluginPathHelper.GetPluginDirectory();
        
        Assert.Equal(expected, actual);
    }

    [SkippableFact]
    [Trait("Category", "AI_Generated")]
    public void GetPluginDirectory_OnMacOS_ReturnsCorrectPath()
    {
        Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.OSX), "Test only runs on macOS");
        
        var expected = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "Library",
            "Application Support",
            "LofiBeats",
            "Plugins");
        
        var actual = PluginPathHelper.GetPluginDirectory();
        
        Assert.Equal(expected, actual);
    }

    [SkippableFact]
    [Trait("Category", "AI_Generated")]
    public void GetPluginDirectory_OnLinux_ReturnsCorrectPath()
    {
        Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Linux), "Test only runs on Linux");
        
        var expected = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".local",
            "share",
            "LofiBeats",
            "Plugins");
        
        var actual = PluginPathHelper.GetPluginDirectory();
        
        Assert.Equal(expected, actual);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void EnsurePluginDirectoryExists_CreatesDirectory()
    {
        // Get the directory path
        var dir = PluginPathHelper.GetPluginDirectory();
        
        // Ensure directory doesn't exist
        if (Directory.Exists(dir))
        {
            Directory.Delete(dir, true);
        }
        
        // Call the method
        var result = PluginPathHelper.EnsurePluginDirectoryExists();
        
        // Verify
        Assert.True(Directory.Exists(result));
        Assert.Equal(dir, result);
        
        // Cleanup
        Directory.Delete(dir, true);
    }
} 