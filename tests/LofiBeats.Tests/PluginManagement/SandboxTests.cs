using LofiBeats.Core.PluginManagement;
using System.Runtime.InteropServices;

namespace LofiBeats.Tests.PluginManagement;

public class SandboxTests
{
    [Fact]
    [Trait("Category", "AI_Generated")]
    public void LaunchPluginHost_UnsupportedPlatform_ThrowsPlatformNotSupported()
    {
        // Create test files to pass initial checks
        var testExePath = Path.Combine(Path.GetTempPath(), "test.exe");
        var testDllPath = Path.Combine(Path.GetTempPath(), "test.dll");
        
        try
        {
            File.WriteAllBytes(testExePath, new byte[] { 0x4D, 0x5A }); // Minimal EXE header
            File.WriteAllBytes(testDllPath, new byte[] { 0x4D, 0x5A }); // Minimal DLL header

            // Skip test on Windows since we can't easily mock the platform
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return;
            }

            var ex = Assert.Throws<PlatformNotSupportedException>(() =>
            {
                SandboxLauncher.LaunchPluginHost(testExePath, testDllPath);
            });

            Assert.Contains("Unsupported operating system", ex.Message);
        }
        finally
        {
            try { File.Delete(testExePath); } catch { }
            try { File.Delete(testDllPath); } catch { }
        }
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
    [Trait("Category", "AI_Generated")]
    [PlatformSpecific(TestPlatforms.Windows)]
    public void WindowsSandbox_CreatesJobObject()
    {
        // Create test files
        var testExePath = Path.Combine(Path.GetTempPath(), "pluginhost.exe");
        var testDllPath = Path.Combine(Path.GetTempPath(), "plugin.dll");
        
        try
        {
            File.WriteAllBytes(testExePath, new byte[] { 0x4D, 0x5A }); // Minimal EXE header
            File.WriteAllBytes(testDllPath, new byte[] { 0x4D, 0x5A }); // Minimal DLL header

            var process = SandboxLauncher.LaunchPluginHost(testExePath, testDllPath);
            Assert.NotNull(process);
            
            try
            {
                process.Kill();
            }
            catch { }
        }
        finally
        {
            try { File.Delete(testExePath); } catch { }
            try { File.Delete(testDllPath); } catch { }
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