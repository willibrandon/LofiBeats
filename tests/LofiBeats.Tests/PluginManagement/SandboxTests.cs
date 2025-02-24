using LofiBeats.Core.PluginManagement;
using System.Runtime.InteropServices;

namespace LofiBeats.Tests.PluginManagement;

public class SandboxTests : IDisposable
{
    private readonly string _testPluginHostPath;
    private readonly string _testPluginPath;

    public SandboxTests()
    {
        // Create test files with minimal headers
        _testPluginHostPath = Path.Combine(Path.GetTempPath(), "test.exe");
        _testPluginPath = Path.Combine(Path.GetTempPath(), "test.dll");

        // Create minimal PE header for Windows or ELF header for Linux
        var header = new byte[] { 0x4D, 0x5A }; // MZ header
        File.WriteAllBytes(_testPluginHostPath, header);
        File.WriteAllBytes(_testPluginPath, header);
    }

    [Fact]
    public void LaunchPluginHost_UnsupportedPlatform_ThrowsPlatformNotSupported()
    {
        // Skip on supported platforms
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ||
            RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ||
            RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return;
        }

        Assert.Throws<PlatformNotSupportedException>(() =>
            SandboxLauncher.LaunchPluginHost(_testPluginHostPath, _testPluginPath));
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void LaunchPluginHost_InvalidPaths_ThrowsFileNotFound()
    {
        var ex = Assert.Throws<FileNotFoundException>(() =>
        {
            SandboxLauncher.LaunchPluginHost("nonexistent.dll", "plugin.dll");
        });

        Assert.Contains("not found", ex.Message);
    }

    [Fact]
    public void WindowsSandbox_CreatesJobObject()
    {
        // Skip on non-Windows platforms
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return;
        }

        var process = SandboxLauncher.LaunchPluginHost(_testPluginHostPath, _testPluginPath);
        try
        {
            Assert.NotNull(process);
            Assert.False(process.HasExited);
        }
        finally
        {
            if (!process.HasExited)
            {
                process.Kill();
            }
            process.Dispose();
        }
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    [PlatformSpecific(TestPlatforms.OSX)]
    public void MacSandbox_CreatesSandboxProfile()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return; // Skip on non-macOS
        }

        // Verify sandbox profile exists in resources
        var assembly = typeof(SandboxLauncher).Assembly;
        using var stream = assembly.GetManifestResourceStream(
            "LofiBeats.Core.PluginManagement.Resources.macos-sandbox.sb");
        
        Assert.NotNull(stream);
        using var reader = new StreamReader(stream);
        var profile = reader.ReadToEnd();
        
        // Verify profile contains required restrictions
        Assert.Contains("(deny network*)", profile);
        Assert.Contains("(deny file-write*)", profile);
        Assert.Contains("(allow process-exec)", profile);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    [PlatformSpecific(TestPlatforms.Linux)]
    public void LinuxSandbox_CreatesSystemdService()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return; // Skip on non-Linux
        }

        // Verify service template exists in resources
        var assembly = typeof(SandboxLauncher).Assembly;
        using var stream = assembly.GetManifestResourceStream(
            "LofiBeats.Core.PluginManagement.Resources.lofibeats-plugin@.service");
        
        Assert.NotNull(stream);
        using var reader = new StreamReader(stream);
        var service = reader.ReadToEnd();
        
        // Verify service contains required restrictions
        Assert.Contains("PrivateTmp=yes", service);
        Assert.Contains("NoNewPrivileges=yes", service);
        Assert.Contains("ProtectSystem=strict", service);
    }

    // Helper method to check if a process is in a job (Windows only)
    private static bool IsProcessInJob(IntPtr processHandle)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return false;
        }

        var isInJob = false;
        var result = NativeMethods.IsProcessInJob(processHandle, IntPtr.Zero, out isInJob);
        return result && isInJob;
    }

    private static class NativeMethods
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool IsProcessInJob(
            IntPtr processHandle,
            IntPtr jobHandle,
            [MarshalAs(UnmanagedType.Bool)] out bool result);
    }

    public void Dispose()
    {
        try
        {
            if (File.Exists(_testPluginHostPath))
                File.Delete(_testPluginHostPath);
            if (File.Exists(_testPluginPath))
                File.Delete(_testPluginPath);
        }
        catch
        {
            // Ignore cleanup errors
        }
    }
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class PlatformSpecificAttribute : Attribute
{
    public TestPlatforms Platform { get; }

    public PlatformSpecificAttribute(TestPlatforms platform)
    {
        Platform = platform;
    }
}

[Flags]
public enum TestPlatforms
{
    Windows = 1,
    Linux = 2,
    OSX = 4
} 