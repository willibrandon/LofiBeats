using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Runtime.InteropServices;

namespace LofiBeats.Cli;

public class ServiceConnectionHelper
{
    private static readonly SemaphoreSlim _serviceLock = new(1, 1);

    private readonly ILogger<ServiceConnectionHelper> _logger;
    private readonly HttpClient _httpClient;
    private readonly string _serviceUrl;
    private readonly string _servicePath;
    private readonly string _pidFilePath;

    // Logging definitions remain the same as in the original code.
    private static readonly Action<ILogger, Exception?> _logServiceAlreadyRunning =
        LoggerMessage.Define(LogLevel.Debug, new EventId(1, "ServiceAlreadyRunning"), "LofiBeats service is already running.");

    private static readonly Action<ILogger, Exception?> _logFoundExistingProcesses =
        LoggerMessage.Define(LogLevel.Debug, new EventId(2, "FoundExistingProcesses"), "Found existing service processes, attempting to reuse...");

    private static readonly Action<ILogger, Exception?> _logConnectedToExistingService =
        LoggerMessage.Define(LogLevel.Debug, new EventId(3, "ConnectedToExistingService"), "Successfully connected to existing service.");

    private static readonly Action<ILogger, int, Exception?> _logCleanedUpStaleProcess =
        LoggerMessage.Define<int>(LogLevel.Debug, new EventId(4, "CleanedUpStaleProcess"), "Cleaned up stale process {ProcessId}");

    private static readonly Action<ILogger, int, Exception> _logFailedToCleanupProcess =
        LoggerMessage.Define<int>(LogLevel.Warning, new EventId(5, "FailedToCleanupProcess"), "Failed to clean up process {ProcessId}");

    private static readonly Action<ILogger, Exception?> _logServiceStartedSuccessfully =
        LoggerMessage.Define(LogLevel.Debug, new EventId(6, "ServiceStartedSuccessfully"), "LofiBeats service started successfully.");

    private static readonly Action<ILogger, int, int, Exception?> _logWaitingForServiceStart =
        LoggerMessage.Define<int, int>(LogLevel.Debug, new EventId(7, "WaitingForServiceStart"), "Waiting for service to start (attempt {Attempt}/{MaxAttempts})...");

    private static readonly Action<ILogger, int, Exception> _logErrorCheckingProcess =
        LoggerMessage.Define<int>(LogLevel.Debug, new EventId(8, "ErrorCheckingProcess"), "Error checking process {ProcessId}");

    private static readonly Action<ILogger, Exception?> _logUnsupportedOs =
        LoggerMessage.Define(LogLevel.Warning, new EventId(9, "UnsupportedOs"), "Unsupported operating system for process detection");

    private static readonly Action<ILogger, Exception?> _logStartingService =
        LoggerMessage.Define(LogLevel.Debug, new EventId(10, "StartingService"), "Attempting to start LofiBeats service...");

    private static readonly Action<ILogger, string, Exception?> _logServiceOutput =
        LoggerMessage.Define<string>(LogLevel.Debug, new EventId(11, "ServiceOutput"), "Service: {Output}");

    private static readonly Action<ILogger, string, Exception?> _logServiceError =
        LoggerMessage.Define<string>(LogLevel.Error, new EventId(12, "ServiceError"), "Service Error: {Error}");

    private static readonly Action<ILogger, string, Exception?> _logServiceStarted =
        LoggerMessage.Define<string>(LogLevel.Debug, new EventId(13, "ServiceStarted"), "Started service from: {Path}");

    private static readonly Action<ILogger, Exception> _logFailedToStartService =
        LoggerMessage.Define(LogLevel.Error, new EventId(14, "FailedToStartService"), "Failed to start LofiBeats service.");

    private static readonly Action<ILogger, Exception?> _logNoRunningService =
        LoggerMessage.Define(LogLevel.Debug, new EventId(15, "NoRunningService"), "No running service found - nothing to shut down.");

    private static readonly Action<ILogger, Exception?> _logShuttingDownService =
        LoggerMessage.Define(LogLevel.Debug, new EventId(16, "ShuttingDownService"), "Shutting down running service...");

    private static readonly Action<ILogger, Exception?> _logServiceShutdownRequested =
        LoggerMessage.Define(LogLevel.Debug, new EventId(17, "ServiceShutdownRequested"), "Service shutdown requested successfully.");

    private static readonly Action<ILogger, Exception> _logErrorShuttingDown =
        LoggerMessage.Define(LogLevel.Warning, new EventId(18, "ErrorShuttingDown"), "Error while shutting down service.");

    private static readonly Action<ILogger, string, int, Exception?> _logHealthCheckResponse =
        LoggerMessage.Define<string, int>(LogLevel.Debug, new EventId(20, "HealthCheckResponse"), "Health check response: {Status} ({StatusCode})");

    private static readonly Action<ILogger, int, Exception?> _logRetryingServiceCheck =
        LoggerMessage.Define<int>(LogLevel.Debug, new EventId(21, "RetryingServiceCheck"), "Retrying service check (attempt {Attempt})");

    private static readonly Action<ILogger, int, TimeSpan, Exception?> _logHealthCheckRetry =
        LoggerMessage.Define<int, TimeSpan>(LogLevel.Debug, new EventId(22, "HealthCheckRetry"),
            "Health check failed (attempt {RetryNumber}); retrying in {Delay}...");

    private static readonly Action<ILogger, int, Exception?> _logFoundPidFile =
        LoggerMessage.Define<int>(LogLevel.Debug, new EventId(23, "FoundPidFile"), "Found PID file with process ID {ProcessId}");

    private static readonly Action<ILogger, int, Exception?> _logPidFileCreated =
        LoggerMessage.Define<int>(LogLevel.Debug, new EventId(24, "PidFileCreated"), "Created PID file for process {ProcessId}");

    private static readonly Action<ILogger, Exception?> _logPidFileInvalid =
        LoggerMessage.Define(LogLevel.Debug, new EventId(25, "PidFileInvalid"), "PID file is invalid or process is no longer running");

    private static readonly Action<ILogger, string, string, Exception?> _logStartingServiceDetails =
        LoggerMessage.Define<string, string>(LogLevel.Debug, new EventId(26, "StartingServiceDetails"),
            "Starting service with path: '{ServicePath}' in directory: '{ServiceDirectory}'");

    private static readonly Action<ILogger, string, Exception?> _logExecutingCommand =
        LoggerMessage.Define<string>(LogLevel.Debug, new EventId(27, "ExecutingCommand"),
            "Executing command: dotnet {Arguments}");

    private static readonly Action<ILogger, string, string, string, string, Exception?> _logServiceStartupDetails =
        LoggerMessage.Define<string, string, string, string>(LogLevel.Debug, new EventId(28, "ServiceStartupDetails"),
            "[DEBUG] About to start service:\n[DEBUG]   FileName: {FileName}\n[DEBUG]   Arguments: {Arguments}\n[DEBUG]   WorkingDirectory: {WorkingDirectory}\n[DEBUG]   ServicePath: {ServicePath}");

    private static readonly Action<ILogger, Exception> _logChmodError =
        LoggerMessage.Define(LogLevel.Warning, new EventId(29, "ChmodError"), "Failed to set executable permissions");

    private static readonly Action<ILogger, int, Exception?> _logServiceExitedQuickly =
        LoggerMessage.Define<int>(LogLevel.Debug, new EventId(30, "ServiceExitedQuickly"),
            "Service exited immediately with code {ExitCode}");

    private static readonly Action<ILogger, Exception?> _logServiceStillRunning =
        LoggerMessage.Define(LogLevel.Debug, new EventId(31, "ServiceStillRunning"),
            "Service is still running after initial startup check");

    // Configuration for how long we wait to see if the process exits immediately.
    private const int QuickExitCheckMilliseconds = 800;

    public ServiceConnectionHelper(
        ILogger<ServiceConnectionHelper> logger,
        IConfiguration configuration,
        string? serviceUrl = null,
        HttpClient? httpClient = null,
        string? servicePath = null)
    {
        _logger = logger;
        _httpClient = httpClient ?? new HttpClient();

        _serviceUrl = configuration["ServiceUrl"]
                      ?? serviceUrl
                      ?? "http://localhost:5001";

        _servicePath = servicePath ?? GetDefaultServicePath();
        _pidFilePath = Path.Combine(Path.GetDirectoryName(_servicePath)!, "service.pid");
    }

    /// <summary>
    /// Ensures the LofiBeats service is running.
    /// Uses a concurrency guard to prevent multiple simultaneous starts.
    /// </summary>
    public async Task EnsureServiceRunningAsync()
    {
        await _serviceLock.WaitAsync();
        try
        {
            const int maxRetries = 20;
            const int maxStartupRetries = 1;

            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                // 1. Quick initial health check
                if (await IsServiceRunningAsync())
                {
                    _logServiceAlreadyRunning(_logger, null);
                    return;
                }

                // 2. Look for existing processes
                var existingProcesses = GetExistingServiceProcesses();
                if (existingProcesses.Count != 0)
                {
                    _logFoundExistingProcesses(_logger, null);

                    // Quick check if process is responding
                    if (await IsServiceRunningAsync())
                    {
                        _logConnectedToExistingService(_logger, null);
                        return;
                    }

                    // Clean up stale only on last attempt
                    if (attempt == maxStartupRetries)
                    {
                        foreach (var proc in existingProcesses)
                        {
                            try
                            {
                                proc.Kill();
                                proc.WaitForExit(3000); // Give it some time to exit
                                _logCleanedUpStaleProcess(_logger, proc.Id, null);
                            }
                            catch (Exception ex)
                            {
                                _logFailedToCleanupProcess(_logger, proc.Id, ex);
                            }
                        }
                    }
                }

                if (attempt < maxStartupRetries)
                {
                    _logRetryingServiceCheck(_logger, attempt, null);
                    continue;
                }

                // 3. One more health check before starting service
                var isRunning = await IsServiceRunningAsync();
                if (isRunning)
                {
                    _logServiceStartedSuccessfully(_logger, null);
                    return;
                }

                // 4. Start a new service process
                _logStartingService(_logger, null);
                await StartServiceAsync();

                // 5. Wait for service to respond
                isRunning = await IsServiceRunningAsync(attempts: 5);
                if (isRunning)
                {
                    _logServiceStartedSuccessfully(_logger, null);
                    return;
                }
            }

            // If service fails after final attempt, throw.
            throw new ServiceStartException("Unable to start LofiBeats service after multiple retries.");
        }
        finally
        {
            _serviceLock.Release();
        }
    }

    /// <summary>
    /// Attempts to read the PID file safely, returning a running Process
    /// if it matches our service. Otherwise returns null.
    /// </summary>
    private Process? GetProcessFromPidFile()
    {
        // Use a file stream with shared read to avoid partial reads when another process writes the PID file.
        FileStream? fs = null;
        try
        {
            if (!File.Exists(_pidFilePath))
                return null;

            fs = new FileStream(_pidFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var reader = new StreamReader(fs);
            fs = null; // Ownership has moved to StreamReader

            var pidContent = reader.ReadToEnd().Trim();
            if (int.TryParse(pidContent, out var pid))
            {
                _logFoundPidFile(_logger, pid, null);
                var process = Process.GetProcessById(pid);

                // Verify it's our service process
                if (!process.HasExited && IsLofiBeatsServiceProcess(process))
                {
                    return process;
                }
            }

            _logPidFileInvalid(_logger, null);
            File.Delete(_pidFilePath);
        }
        catch
        {
            // Process doesn't exist or access denied
            _logPidFileInvalid(_logger, null);
            try { File.Delete(_pidFilePath); } catch { }
        }
        finally
        {
            fs?.Dispose();
        }

        return null;
    }

    /// <summary>
    /// Determines if the given process is a LofiBeats service process.
    /// Uses OS-specific logic for command-line checks.
    /// </summary>
    private bool IsLofiBeatsServiceProcess(Process process)
    {
        try
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var wmiQuery = $"SELECT CommandLine FROM Win32_Process WHERE ProcessId = {process.Id}";
                using var searcher = new System.Management.ManagementObjectSearcher(wmiQuery);
                using var collection = searcher.Get();

                if (collection.Count == 0)
                {
                    _logErrorCheckingProcess(_logger, process.Id, new Exception("WMI query returned no results"));
                    return false;
                }

                foreach (var item in collection)
                {
                    var commandLine = item["CommandLine"]?.ToString();
                    if (commandLine == null) continue;

                    // Check for both DLL and executable references
                    if (commandLine.Contains("LofiBeats.Service.dll", StringComparison.OrdinalIgnoreCase) ||
                        commandLine.Contains("LofiBeats.Service.exe", StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }

                return false;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var cmdlinePath = $"/proc/{process.Id}/cmdline";
                if (File.Exists(cmdlinePath))
                {
                    var cmdline = File.ReadAllText(cmdlinePath);
                    return cmdline.Contains("LofiBeats.Service.dll", StringComparison.OrdinalIgnoreCase)
                        || cmdline.Contains("LofiBeats.Service", StringComparison.OrdinalIgnoreCase);
                }
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                var psi = new ProcessStartInfo
                {
                    FileName = "ps",
                    Arguments = $"-p {process.Id} -o command",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                using var ps = Process.Start(psi);
                if (ps != null)
                {
                    var output = ps.StandardOutput.ReadToEnd();
                    ps.WaitForExit();
                    return output.Contains("LofiBeats.Service.dll", StringComparison.OrdinalIgnoreCase)
                           || output.Contains("LofiBeats.Service", StringComparison.OrdinalIgnoreCase);
                }
            }
            else
            {
                _logUnsupportedOs(_logger, null);
            }
        }
        catch (Exception ex)
        {
            _logErrorCheckingProcess(_logger, process.Id, ex);
        }

        return false;
    }

    /// <summary>
    /// Enumerate existing LofiBeats service processes. Tries PID file first, then falls back to system enumeration.
    /// </summary>
    private List<Process> GetExistingServiceProcesses()
    {
        // First check the PID file
        var pidProcess = GetProcessFromPidFile();
        if (pidProcess != null)
        {
            return new List<Process> { pidProcess };
        }

        // For test environment - mock process
        if (Environment.GetEnvironmentVariable("MOCK_PROCESS_TEST") == "true")
        {
            return new List<Process> { Process.GetCurrentProcess() };
        }

        // Fall back to full process enumeration
        var processes = Process.GetProcessesByName("dotnet")
            .Where(p => p.MainWindowTitle == "")
            .Where(IsLofiBeatsServiceProcess)
            .ToList();

        // Log unsupported OS only if no results found
        if (processes.Count == 0 &&
            !RuntimeInformation.IsOSPlatform(OSPlatform.Windows) &&
            !RuntimeInformation.IsOSPlatform(OSPlatform.Linux) &&
            !RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            _logUnsupportedOs(_logger, null);
        }

        return processes;
    }

    /// <summary>
    /// Checks if the service is running by doing a quick HTTP health check.
    /// </summary>
    private async Task<bool> IsServiceRunningAsync(int attempts = 1)
    {
        var attempt = 1;
        while (attempt <= attempts)
        {
            try
            {
                // Quick check.
                using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(500));
                var response = await _httpClient.GetAsync($"{_serviceUrl}/healthz",
                        HttpCompletionOption.ResponseHeadersRead, // Do not buffer content
                        cts.Token);

                _logHealthCheckResponse(_logger,
                    response.StatusCode.ToString(),
                    (int)response.StatusCode,
                    null);

                // Consider both OK and redirect responses as success
                return response.IsSuccessStatusCode ||
                       ((int)response.StatusCode >= 300 && (int)response.StatusCode < 400);
            }
            catch (Exception ex)
            {
                // 0 used when we fail to get an actual PID in this context
                _logErrorCheckingProcess(_logger, 0, ex);
                await Task.Delay(250); // Delay before retry
            }

            attempt++;
        }

        return false;
    }

    /// <summary>
    /// Starts the service process (if not running). Copies settings, sets executable bits, etc.
    /// </summary>
    private async Task StartServiceAsync()
    {
        try
        {
            var serviceDirectory = Path.GetDirectoryName(_servicePath)!;
            var serviceFileName = Path.GetFileName(_servicePath);

            // Copy service settings if needed
            await CopyServiceSettingsAsync(serviceDirectory);

            // Log the service path and directory for debugging
            _logStartingServiceDetails(_logger, _servicePath, serviceDirectory, null);

            // Determine if we're running a published single-file executable
            bool isPublishedExecutable = !_servicePath.EndsWith(".dll", StringComparison.OrdinalIgnoreCase);

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = isPublishedExecutable ? _servicePath : "dotnet",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    WorkingDirectory = serviceDirectory
                }
            };

            // Only add the DLL path as an argument when using dotnet
            if (!isPublishedExecutable)
            {
                process.StartInfo.Arguments = $"\"{_servicePath}\"";
            }

            // Ensure the file is executable on non-Windows
            if (isPublishedExecutable && !RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                try
                {
                    var chmodPsi = new ProcessStartInfo
                    {
                        FileName = "chmod",
                        Arguments = $"+x \"{_servicePath}\"",
                        UseShellExecute = false,
                        RedirectStandardError = true
                    };
                    using var chmod = Process.Start(chmodPsi);
                    chmod?.WaitForExit();
                }
                catch (Exception ex)
                {
                    _logChmodError(_logger, ex);
                }
            }

            _logServiceStartupDetails(_logger,
                process.StartInfo.FileName,
                process.StartInfo.Arguments,
                serviceDirectory,
                _servicePath,
                null);

            process.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                    _logServiceOutput(_logger, e.Data, null);
            };
            process.ErrorDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                    _logServiceError(_logger, e.Data, null);
            };

            // Log the final command that will be executed
            _logExecutingCommand(_logger, $"{process.StartInfo.FileName} {process.StartInfo.Arguments}", null);

            // Start the process
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            // Check quickly if it exited
            bool exitedQuickly = process.WaitForExit(QuickExitCheckMilliseconds);
            if (exitedQuickly)
            {
                // Child died almost instantly; log the exit code
                _logServiceExitedQuickly(_logger, process.ExitCode, null);
                throw new ServiceStartException($"Service process exited immediately with code {process.ExitCode}.");
            }
            else
            {
                _logServiceStillRunning(_logger, null);
            }

            // Write PID file (with file-sharing turned off to avoid concurrency issues)
            using (var fs = new FileStream(_pidFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var writer = new StreamWriter(fs))
            {
                await writer.WriteAsync(process.Id.ToString());
            }
            _logPidFileCreated(_logger, process.Id, null);

            _logServiceStarted(_logger, _servicePath, null);
        }
        catch (Exception ex)
        {
            _logFailedToStartService(_logger, ex);
            throw new ServiceStartException($"Failed to start LofiBeats service: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Copies the service's appsettings files (production & development) if source is newer or doesn't exist.
    /// </summary>
    private static async Task CopyServiceSettingsAsync(string serviceDirectory)
    {
        var cliDirectory = AppContext.BaseDirectory;

        var serviceSettings = Path.Combine(cliDirectory, "service.appsettings.json");
        var serviceDevSettings = Path.Combine(cliDirectory, "service.appsettings.Development.json");

        var targetSettings = Path.Combine(serviceDirectory, "appsettings.json");
        var targetDevSettings = Path.Combine(serviceDirectory, "appsettings.Development.json");

        await Task.Run(() =>
        {
            if (File.Exists(serviceSettings) && ShouldCopySettings(serviceSettings, targetSettings))
            {
                File.Copy(serviceSettings, targetSettings, true);
            }
            if (File.Exists(serviceDevSettings) && ShouldCopySettings(serviceDevSettings, targetDevSettings))
            {
                File.Copy(serviceDevSettings, targetDevSettings, true);
            }
        });
    }

    private static bool ShouldCopySettings(string source, string destination)
    {
        if (!File.Exists(destination))
            return true;

        var sourceInfo = new FileInfo(source);
        var destInfo = new FileInfo(destination);

        return sourceInfo.Length != destInfo.Length ||
               sourceInfo.LastWriteTimeUtc > destInfo.LastWriteTimeUtc;
    }

    private static string GetDefaultServicePath()
    {
        // Get the directory where the CLI is running
        var cliDirectory = AppContext.BaseDirectory;

        // For development, try the typical bin/Debug path
        var serviceDevPath = Path.GetFullPath(Path.Combine(
            cliDirectory,           // e.g. bin/Debug/net9.0
            "..", "..", "..",       // up to src/LofiBeats.Cli
            "..", "LofiBeats.Service", // into src/LofiBeats.Service
            "bin", "Debug", "net9.0",
            "LofiBeats.Service.dll"
        ));

        if (File.Exists(serviceDevPath))
        {
            return serviceDevPath;
        }

        // For published mode, check both exe and DLL
        var publishedExe = Path.Combine(cliDirectory,
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? "LofiBeats.Service.exe"
                : "LofiBeats.Service");

        var publishedDll = Path.Combine(cliDirectory, "LofiBeats.Service.dll");

        if (File.Exists(publishedExe))
        {
            return publishedExe;
        }

        return publishedDll;
    }

    /// <summary>
    /// Sends a command to the LofiBeats service. Ensures service is running first.
    /// </summary>
    public async Task<HttpResponseMessage> SendCommandAsync(HttpMethod method, string endpoint, object? content = null)
    {
        await EnsureServiceRunningAsync();

        var requestUri = $"{_serviceUrl}/api/lofi/{endpoint.TrimStart('/')}";
        using var request = new HttpRequestMessage(method, requestUri);

        if (content != null)
        {
            request.Content = JsonContent.Create(content);
        }

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return response;
    }

    /// <summary>
    /// Requests service shutdown via HTTP endpoint, if the service is running.
    /// </summary>
    public async Task ShutdownServiceAsync()
    {
        if (!await IsServiceRunningAsync())
        {
            _logNoRunningService(_logger, null);
            return;
        }

        try
        {
            _logShuttingDownService(_logger, null);
            await _httpClient.PostAsync($"{_serviceUrl}/api/lofi/shutdown", null);
            _logServiceShutdownRequested(_logger, null);
        }
        catch (Exception ex)
        {
            _logErrorShuttingDown(_logger, ex);
            throw;
        }
    }
}
