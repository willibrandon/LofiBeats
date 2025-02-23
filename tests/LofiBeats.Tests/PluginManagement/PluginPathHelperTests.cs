using LofiBeats.Core.PluginManagement;
using System.Runtime.InteropServices;
using System.IO;
using System;

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
        // Create a unique test directory path
        var testRoot = Path.Combine(Path.GetTempPath(), "LofiBeatsTests", Guid.NewGuid().ToString());
        var testPluginDir = Path.Combine(testRoot, "Plugins");
        
        try
        {
            // Call the method and verify
            var result = PluginPathHelper.EnsurePluginDirectoryExists();
            Assert.True(Directory.Exists(result));
            
            // Verify we can write to the directory
            var testFile = Path.Combine(result, "test.txt");
            File.WriteAllText(testFile, "test");
            Assert.True(File.Exists(testFile));
            
            // Clean up test file
            File.Delete(testFile);
        }
        finally
        {
            // Clean up test directory if we created it
            try
            {
                if (Directory.Exists(testRoot))
                {
                    Directory.Delete(testRoot, true);
                }
            }
            catch
            {
                // Ignore cleanup errors in tests
            }
        }
    }
} 