using System.Diagnostics;
using System.Runtime.InteropServices;

namespace LofiBeats.Core.PluginManagement;

/// <summary>
/// Provides OS-specific sandboxing for plugin host processes.
/// </summary>
public static class SandboxLauncher
{
    private const string MacOSSandboxProfile = "LofiBeats.Core.PluginManagement.Resources.macos-sandbox.sb";
    private const string LinuxServiceTemplate = "LofiBeats.Core.PluginManagement.Resources.lofibeats-plugin@.service";

    private static void LogWarning(string message, Exception? ex = null)
    {
        Console.WriteLine($"[Warning] {message}");
        if (ex != null)
        {
            Console.WriteLine($"[Warning] Exception: {ex.Message}");
        }
    }

    /// <summary>
    /// Launches the Plugin Host process with sandbox restrictions based on OS.
    /// </summary>
    /// <param name="pluginHostPath">The full path to the PluginHost executable (or DLL to run with dotnet).</param>
    /// <param name="pluginAssemblyPath">The full path to the plugin assembly to load.</param>
    /// <param name="startInfo">Optional ProcessStartInfo to customize the process launch.</param>
    /// <returns>A Process object representing the launched sandboxed Plugin Host.</returns>
    public static Process LaunchPluginHost(
        string pluginHostPath, 
        string pluginAssemblyPath,
        ProcessStartInfo? startInfo = null)
    {
        // Check files first - these must be checked before any platform-specific code
        if (!File.Exists(pluginHostPath))
        {
            throw new FileNotFoundException("Plugin host executable not found", pluginHostPath);
        }
        if (!File.Exists(pluginAssemblyPath))
        {
            throw new FileNotFoundException("Plugin assembly not found", pluginAssemblyPath);
        }

        // Platform checks
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return LaunchWindowsSandbox(pluginHostPath, pluginAssemblyPath, startInfo);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return LaunchLinuxSandbox(pluginHostPath, pluginAssemblyPath, startInfo);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return LaunchMacOSSandbox(pluginHostPath, pluginAssemblyPath, startInfo);
        }
        else
        {
            throw new PlatformNotSupportedException("Unsupported operating system for sandboxed plugin execution");
        }
    }

    private static Process LaunchWindowsSandbox(string pluginHostPath, string pluginAssemblyPath, ProcessStartInfo? startInfo = null)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            throw new PlatformNotSupportedException("Windows sandbox is only supported on Windows");
        }

        var jobName = $"LofiBeatsPlugin_{Guid.NewGuid():N}";
        var job = Windows.CreateJobObject(IntPtr.Zero, jobName);
        
        if (job == IntPtr.Zero)
        {
            var error = Marshal.GetLastWin32Error();
            throw new InvalidOperationException($"Failed to create job object: {error}");
        }

        try
        {
            var basicInfo = new Windows.JOBOBJECT_BASIC_LIMIT_INFORMATION
            {
                LimitFlags = Windows.JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE | 
                            Windows.JOB_OBJECT_LIMIT_DIE_ON_UNHANDLED_EXCEPTION |
                            Windows.JOB_OBJECT_LIMIT_ACTIVE_PROCESS,
                ActiveProcessLimit = 1,
                PriorityClass = 0x20 // NORMAL_PRIORITY_CLASS
            };

            var extendedInfo = new Windows.JOBOBJECT_EXTENDED_LIMIT_INFORMATION
            {
                BasicLimitInformation = basicInfo
            };

            var length = Marshal.SizeOf(typeof(Windows.JOBOBJECT_EXTENDED_LIMIT_INFORMATION));
            var extendedInfoPtr = Marshal.AllocHGlobal(length);

            try
            {
                Marshal.StructureToPtr(extendedInfo, extendedInfoPtr, false);

                if (!Windows.SetInformationJobObject(job, Windows.JobObjectInfoType.ExtendedLimitInformation,
                    extendedInfoPtr, (uint)length))
                {
                    var error = Marshal.GetLastWin32Error();
                    throw new InvalidOperationException($"Failed to set job object information: {error}");
                }

                startInfo ??= new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = $"exec \"{pluginHostPath}\" --plugin-assembly \"{pluginAssemblyPath}\"",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                var process = Process.Start(startInfo)
                    ?? throw new InvalidOperationException("Failed to start plugin host process");

                if (!Windows.AssignProcessToJobObject(job, process.Handle))
                {
                    var error = Marshal.GetLastWin32Error();
                    process.Kill();
                    throw new InvalidOperationException($"Failed to assign process to job object: {error}");
                }

                process.Exited += (sender, args) =>
                {
                    try
                    {
                        Windows.TerminateJobObject(job, 0);
                        Windows.CloseHandle(job);
                    }
                    catch (Exception ex)
                    {
                        LogWarning($"Failed to cleanup job object on process exit: {jobName}", ex);
                    }
                };
                process.EnableRaisingEvents = true;

                return process;
            }
            finally
            {
                Marshal.FreeHGlobal(extendedInfoPtr);
            }
        }
        catch
        {
            Windows.CloseHandle(job);
            throw;
        }
    }

    private static Process LaunchLinuxSandbox(string pluginHostPath, string pluginAssemblyPath, ProcessStartInfo? startInfo = null)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            throw new PlatformNotSupportedException("Linux sandbox is only supported on Linux");
        }

        // Use a simpler approach without systemd for Docker compatibility
        startInfo ??= new ProcessStartInfo
        {
            FileName = "nice",
            Arguments = $"-n 19 dotnet exec \"{pluginHostPath}\" --plugin-assembly \"{pluginAssemblyPath}\"",
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        var process = Process.Start(startInfo)
            ?? throw new InvalidOperationException("Failed to start plugin host process");

        // Set resource limits using ulimit if available
        try
        {
            var pid = process.Id;
            Process.Start(new ProcessStartInfo
            {
                FileName = "bash",
                Arguments = $"-c \"ulimit -t 3600 -v 1048576 -n 256 && prlimit --pid={pid} --nofile=256:256\"",
                UseShellExecute = false,
                CreateNoWindow = true
            })?.WaitForExit();
        }
        catch (Exception ex)
        {
            LogWarning("Failed to set resource limits", ex);
        }

        return process;
    }

    private static Process LaunchMacOSSandbox(string pluginHostPath, string pluginAssemblyPath, ProcessStartInfo? startInfo = null)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            throw new PlatformNotSupportedException("macOS sandbox is only supported on macOS");
        }

        startInfo ??= new ProcessStartInfo
        {
            FileName = "sandbox-exec",
            Arguments = $"-p '(version 1) (allow default) (deny file-write*) (allow file-write* (subpath \"{Path.GetTempPath()}\"))' dotnet exec \"{pluginHostPath}\" --plugin-assembly \"{pluginAssemblyPath}\"",
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        return Process.Start(startInfo)
            ?? throw new InvalidOperationException("Failed to start sandboxed plugin host process");
    }

    private static class Windows
    {
        public const uint JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE = 0x2000;
        public const uint JOB_OBJECT_LIMIT_DIE_ON_UNHANDLED_EXCEPTION = 0x400;
        public const uint JOB_OBJECT_LIMIT_ACTIVE_PROCESS = 0x8;
        public const uint JOB_OBJECT_LIMIT_JOB_MEMORY = 0x200;
        public const uint JOB_OBJECT_LIMIT_JOB_TIME = 0x4;
        public const uint JOB_OBJECT_LIMIT_PROCESS_MEMORY = 0x100;
        public const uint JOB_OBJECT_SECURITY_ONLY_TOKEN = 0x2000;
        public const uint JOB_OBJECT_UILIMIT_NONE = 0x00000000;
        public const uint JOB_OBJECT_UILIMIT_HANDLES = 0x00000001;
        public const uint JOB_OBJECT_UILIMIT_READCLIPBOARD = 0x00000002;
        public const uint JOB_OBJECT_UILIMIT_WRITECLIPBOARD = 0x00000004;
        public const uint JOB_OBJECT_UILIMIT_SYSTEMPARAMETERS = 0x00000008;
        public const uint JOB_OBJECT_UILIMIT_DISPLAYSETTINGS = 0x00000010;
        public const uint JOB_OBJECT_UILIMIT_GLOBALATOMS = 0x00000020;
        public const uint JOB_OBJECT_UILIMIT_DESKTOP = 0x00000040;
        public const uint JOB_OBJECT_UILIMIT_EXITWINDOWS = 0x00000080;

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr CreateJobObject(IntPtr lpJobAttributes, string name);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetInformationJobObject(IntPtr job, JobObjectInfoType infoType,
            IntPtr lpJobObjectInfo, uint cbJobObjectInfoLength);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool AssignProcessToJobObject(IntPtr job, IntPtr process);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr handle);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool TerminateJobObject(IntPtr handle, uint exitCode);

        public enum JobObjectInfoType
        {
            AssociateCompletionPortInformation = 7,
            BasicLimitInformation = 2,
            BasicUIRestrictions = 4,
            EndOfJobTimeInformation = 6,
            ExtendedLimitInformation = 9,
            SecurityLimitInformation = 5,
            GroupInformation = 11
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct JOBOBJECT_BASIC_LIMIT_INFORMATION
        {
            public Int64 PerProcessUserTimeLimit;
            public Int64 PerJobUserTimeLimit;
            public UInt32 LimitFlags;
            public UIntPtr MinimumWorkingSetSize;
            public UIntPtr MaximumWorkingSetSize;
            public UInt32 ActiveProcessLimit;
            public UIntPtr Affinity;
            public UInt32 PriorityClass;
            public UInt32 SchedulingClass;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct IO_COUNTERS
        {
            public UInt64 ReadOperationCount;
            public UInt64 WriteOperationCount;
            public UInt64 OtherOperationCount;
            public UInt64 ReadTransferCount;
            public UInt64 WriteTransferCount;
            public UInt64 OtherTransferCount;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct JOBOBJECT_EXTENDED_LIMIT_INFORMATION
        {
            public JOBOBJECT_BASIC_LIMIT_INFORMATION BasicLimitInformation;
            public IO_COUNTERS IoInfo;
            public UIntPtr ProcessMemoryLimit;
            public UIntPtr JobMemoryLimit;
            public UIntPtr PeakProcessMemoryUsed;
            public UIntPtr PeakJobMemoryUsed;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SECURITY_ATTRIBUTES
        {
            public int nLength;
            public IntPtr lpSecurityDescriptor;
            public bool bInheritHandle;
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr CreateJobObject(
            [In] ref SECURITY_ATTRIBUTES jobAttributes,
            string name);
    }
}

