using System;
using System.IO;
using Xunit;

namespace LofiBeats.Tests.PluginManagement;

[CollectionDefinition("Plugin Tests")]
public class PluginTestCollection : ICollectionFixture<PluginTestFixture> { }

public class PluginTestFixture : IDisposable
{
    public string TestPluginDirectory { get; }

    public PluginTestFixture()
    {
        // Create a unique test directory
        TestPluginDirectory = Path.Combine(
            Path.GetTempPath(),
            "LofiBeatsTests",
            "Plugins",
            Guid.NewGuid().ToString());
        
        Directory.CreateDirectory(TestPluginDirectory);
    }

    public void Dispose()
    {
        try
        {
            if (Directory.Exists(TestPluginDirectory))
            {
                // Give the runtime a chance to release file handles
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Thread.Sleep(100);

                Directory.Delete(TestPluginDirectory, true);
            }
        }
        catch
        {
            // Ignore cleanup errors in test fixture
        }

        GC.SuppressFinalize(this);
    }
} 