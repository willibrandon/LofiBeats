using LofiBeats.Core.PluginManagement;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit.Abstractions;

namespace LofiBeats.Tests.PluginManagement;

/// <summary>
/// Collection definition to prevent parallel execution of plugin watcher tests
/// since they deal with file system events.
/// </summary>
[CollectionDefinition("Plugin Watcher Tests", DisableParallelization = true)]
public class PluginWatcherTestCollection : ICollectionFixture<PluginWatcherTestFixture> { }

public class PluginWatcherTestFixture : IDisposable
{
    public string BaseTestDirectory { get; }

    public PluginWatcherTestFixture()
    {
        // Create a unique base directory for all tests
        BaseTestDirectory = Path.Combine(
            Path.GetTempPath(),
            "LofiBeatsTests",
            "PluginWatcher",
            Guid.NewGuid().ToString());
        
        Directory.CreateDirectory(BaseTestDirectory);
    }

    public void Dispose()
    {
        try
        {
            if (Directory.Exists(BaseTestDirectory))
            {
                Directory.Delete(BaseTestDirectory, true);
            }
        }
        catch
        {
            // Ignore cleanup errors
        }
    }
}

[Collection("Plugin Watcher Tests")]
public class PluginWatcherTests : IDisposable
{
    private readonly string _testPluginDir;
    private readonly Mock<ILogger<PluginWatcher>> _loggerMock;
    private readonly Mock<IPluginLoader> _pluginLoaderMock;
    private readonly Mock<PluginManager> _pluginManagerMock;
    private readonly PluginWatcher _watcher;
    private volatile int _refreshCallCount;
    private readonly SemaphoreSlim _refreshSignal;
    private readonly PluginWatcherTestFixture _fixture;
    private readonly ITestOutputHelper _output;

    public PluginWatcherTests(PluginWatcherTestFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;
        _testPluginDir = Path.Combine(_fixture.BaseTestDirectory, Guid.NewGuid().ToString());
        _loggerMock = new Mock<ILogger<PluginWatcher>>();
        _pluginLoaderMock = new Mock<IPluginLoader>();
        _pluginManagerMock = new Mock<PluginManager>(Mock.Of<ILogger<PluginManager>>(), _pluginLoaderMock.Object);
        _refreshCallCount = 0;
        _refreshSignal = new SemaphoreSlim(0);

        // Setup logging
        _loggerMock.Setup(x => x.Log(
            It.IsAny<LogLevel>(),
            It.IsAny<EventId>(),
            It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()))
            .Callback<LogLevel, EventId, object, Exception?, Delegate>((level, id, state, ex, formatter) =>
            {
                var message = formatter.DynamicInvoke(state, ex) as string;
                _output.WriteLine($"[{level}] {message}");
            });

        // Setup the mock to track when RefreshPlugins is called
        _pluginManagerMock.Setup(x => x.RefreshPlugins())
            .Callback(() =>
            {
                Interlocked.Increment(ref _refreshCallCount);
                _output.WriteLine($"RefreshPlugins called (count: {_refreshCallCount})");
                try { _refreshSignal.Release(); } catch { }
            });

        // Create a clean test directory
        Directory.CreateDirectory(_testPluginDir);
        _output.WriteLine($"Created test directory: {_testPluginDir}");

        // Create the watcher with the test directory
        _watcher = new PluginWatcher(_loggerMock.Object, _pluginManagerMock.Object, _testPluginDir);
    }

    public void Dispose()
    {
        _output.WriteLine("Disposing test...");
        try { _watcher.StopWatching(); } catch (Exception ex) { _output.WriteLine($"Error stopping watcher: {ex}"); }
        try { _watcher.Dispose(); } catch (Exception ex) { _output.WriteLine($"Error disposing watcher: {ex}"); }
        try { _refreshSignal.Dispose(); } catch (Exception ex) { _output.WriteLine($"Error disposing signal: {ex}"); }

        try
        {
            if (Directory.Exists(_testPluginDir))
            {
                Directory.Delete(_testPluginDir, true);
                _output.WriteLine($"Deleted test directory: {_testPluginDir}");
            }
        }
        catch (Exception ex)
        {
            _output.WriteLine($"Error cleaning up directory: {ex}");
        }
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task StartWatching_DetectsNewPlugin()
    {
        // Arrange
        _refreshCallCount = 0;
        _output.WriteLine("Starting test: StartWatching_DetectsNewPlugin");

        // Act
        _watcher.StartWatching();
        await Task.Delay(100); // Let the watcher initialize
        _output.WriteLine("Watcher started");

        // Create a test plugin file
        var testPluginPath = Path.Combine(_testPluginDir, "test.dll");
        await File.WriteAllBytesAsync(testPluginPath, []);
        _output.WriteLine($"Created test file: {testPluginPath}");

        // Wait for the file system watcher to detect the change
        var signaled = await _refreshSignal.WaitAsync(TimeSpan.FromSeconds(2));
        var finalCount = _refreshCallCount;
        _output.WriteLine($"Wait completed. Signaled: {signaled}, RefreshCallCount: {finalCount}");

        // Assert
        Assert.True(finalCount > 0, $"RefreshPlugins should have been called at least once (actual: {finalCount})");
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task StartWatching_DetectsPluginDeletion()
    {
        // Arrange
        _refreshCallCount = 0;
        _output.WriteLine("Starting test: StartWatching_DetectsPluginDeletion");

        // Start watching first
        _watcher.StartWatching();
        await Task.Delay(100); // Let the watcher initialize
        _output.WriteLine("Watcher started");

        // Create a test plugin file
        var testPluginPath = Path.Combine(_testPluginDir, "test.dll");
        await File.WriteAllBytesAsync(testPluginPath, []);
        _output.WriteLine($"Created test file: {testPluginPath}");
        await Task.Delay(100); // Let the file system settle

        // Delete the file
        File.Delete(testPluginPath);
        _output.WriteLine($"Deleted test file: {testPluginPath}");

        // Wait for the file system watcher to detect the change
        var signaled = await _refreshSignal.WaitAsync(TimeSpan.FromSeconds(2));
        var finalCount = _refreshCallCount;
        _output.WriteLine($"Wait completed. Signaled: {signaled}, RefreshCallCount: {finalCount}");

        // Assert
        Assert.True(finalCount > 0, $"RefreshPlugins should have been called at least once (actual: {finalCount})");
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task StartWatching_DetectsPluginChanges()
    {
        // Arrange
        _refreshCallCount = 0;
        _output.WriteLine("Starting test: StartWatching_DetectsPluginChanges");

        // Start watching first
        _watcher.StartWatching();
        await Task.Delay(100); // Let the watcher initialize
        _output.WriteLine("Watcher started");

        // Create a test plugin file
        var testPluginPath = Path.Combine(_testPluginDir, "test.dll");
        await File.WriteAllBytesAsync(testPluginPath, []);
        _output.WriteLine($"Created test file: {testPluginPath}");
        await Task.Delay(100); // Let the file system settle

        // Modify the file
        await File.WriteAllBytesAsync(testPluginPath, [1, 2, 3]);
        _output.WriteLine($"Modified test file: {testPluginPath}");

        // Wait for the file system watcher to detect the change
        var signaled = await _refreshSignal.WaitAsync(TimeSpan.FromSeconds(2));
        var finalCount = _refreshCallCount;
        _output.WriteLine($"Wait completed. Signaled: {signaled}, RefreshCallCount: {finalCount}");

        // Assert
        Assert.True(finalCount > 0, $"RefreshPlugins should have been called at least once (actual: {finalCount})");
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task StopWatching_StopsDetectingChanges()
    {
        // Arrange
        _refreshCallCount = 0;
        _output.WriteLine("Starting test: StopWatching_StopsDetectingChanges");

        // Act
        _watcher.StartWatching();
        await Task.Delay(100); // Let the watcher initialize
        _output.WriteLine("Watcher started");
        _watcher.StopWatching();
        _output.WriteLine("Watcher stopped");

        // Create a test plugin file
        var testPluginPath = Path.Combine(_testPluginDir, "test.dll");
        await File.WriteAllBytesAsync(testPluginPath, []);
        _output.WriteLine($"Created test file: {testPluginPath}");

        // Wait a bit to ensure no events are triggered
        await Task.Delay(500);
        var finalCount = _refreshCallCount;
        _output.WriteLine($"Wait completed. RefreshCallCount: {finalCount}");

        // Assert
        Assert.True(finalCount == 0, $"RefreshPlugins should not have been called after stopping (actual: {finalCount})");
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task Dispose_StopsWatching()
    {
        // Arrange
        _refreshCallCount = 0;
        _output.WriteLine("Starting test: Dispose_StopsWatching");

        // Act
        _watcher.StartWatching();
        await Task.Delay(100); // Let the watcher initialize
        _output.WriteLine("Watcher started");
        _watcher.Dispose();
        _output.WriteLine("Watcher disposed");

        // Create a test plugin file
        var testPluginPath = Path.Combine(_testPluginDir, "test.dll");
        await File.WriteAllBytesAsync(testPluginPath, []);
        _output.WriteLine($"Created test file: {testPluginPath}");

        // Wait a bit to ensure no events are triggered
        await Task.Delay(500);
        var finalCount = _refreshCallCount;
        _output.WriteLine($"Wait completed. RefreshCallCount: {finalCount}");

        // Assert
        Assert.True(finalCount == 0, $"RefreshPlugins should not have been called after disposal (actual: {finalCount})");
    }
} 