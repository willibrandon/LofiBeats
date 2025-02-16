using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using System.Diagnostics;
using System.Net.Http.Json;

namespace LofiBeats.Cli;

public class ServiceConnectionHelper
{
    private readonly ILogger<ServiceConnectionHelper> _logger;
    private readonly HttpClient _httpClient;
    private readonly string _serviceUrl;
    private readonly string _servicePath;
    private readonly AsyncRetryPolicy<bool> _healthCheckPolicy;
    private readonly string _pidFilePath;

    private static readonly Action<ILogger, Exception?> _logServiceAlreadyRunning =
        LoggerMessage.Define(LogLevel.Debug, new EventId(1, "ServiceAlreadyRunning"), "LofiBeats service is already running.");

    private static readonly Action<ILogger, Exception?> _logFoundExistingProcesses =
        LoggerMessage.Define(LogLevel.Information, new EventId(2, "FoundExistingProcesses"), "Found existing service processes, attempting to reuse...");

    private static readonly Action<ILogger, Exception?> _logConnectedToExistingService =
        LoggerMessage.Define(LogLevel.Information, new EventId(3, "ConnectedToExistingService"), "Successfully connected to existing service.");

    private static readonly Action<ILogger, int, Exception?> _logCleanedUpStaleProcess =
        LoggerMessage.Define<int>(LogLevel.Debug, new EventId(4, "CleanedUpStaleProcess"), "Cleaned up stale process {ProcessId}");

    private static readonly Action<ILogger, int, Exception> _logFailedToCleanupProcess =
        LoggerMessage.Define<int>(LogLevel.Warning, new EventId(5, "FailedToCleanupProcess"), "Failed to clean up process {ProcessId}");

    private static readonly Action<ILogger, Exception?> _logServiceStartedSuccessfully =
        LoggerMessage.Define(LogLevel.Information, new EventId(6, "ServiceStartedSuccessfully"), "LofiBeats service started successfully.");

    private static readonly Action<ILogger, int, int, Exception?> _logWaitingForServiceStart =
        LoggerMessage.Define<int, int>(LogLevel.Debug, new EventId(7, "WaitingForServiceStart"), "Waiting for service to start (attempt {Attempt}/{MaxAttempts})...");

    private static readonly Action<ILogger, int, Exception> _logErrorCheckingProcess =
        LoggerMessage.Define<int>(LogLevel.Debug, new EventId(8, "ErrorCheckingProcess"), "Error checking process {ProcessId}");

    private static readonly Action<ILogger, Exception?> _logUnsupportedOs =
        LoggerMessage.Define(LogLevel.Warning, new EventId(9, "UnsupportedOs"), "Unsupported operating system for process detection");

    private static readonly Action<ILogger, Exception?> _logStartingService =
        LoggerMessage.Define(LogLevel.Information, new EventId(10, "StartingService"), "Attempting to start LofiBeats service...");

    private static readonly Action<ILogger, string, Exception?> _logServiceOutput =
        LoggerMessage.Define<string>(LogLevel.Debug, new EventId(11, "ServiceOutput"), "Service: {Output}");

    private static readonly Action<ILogger, string, Exception?> _logServiceError =
        LoggerMessage.Define<string>(LogLevel.Error, new EventId(12, "ServiceError"), "Service Error: {Error}");

    private static readonly Action<ILogger, string, Exception?> _logServiceStarted =
        LoggerMessage.Define<string>(LogLevel.Information, new EventId(13, "ServiceStarted"), "Started service from: {Path}");

    private static readonly Action<ILogger, Exception> _logFailedToStartService =
        LoggerMessage.Define(LogLevel.Error, new EventId(14, "FailedToStartService"), "Failed to start LofiBeats service.");

    private static readonly Action<ILogger, Exception?> _logNoRunningService =
        LoggerMessage.Define(LogLevel.Information, new EventId(15, "NoRunningService"), "No running service found - nothing to shut down.");

    private static readonly Action<ILogger, Exception?> _logShuttingDownService =
        LoggerMessage.Define(LogLevel.Information, new EventId(16, "ShuttingDownService"), "Shutting down running service...");

    private static readonly Action<ILogger, Exception?> _logServiceShutdownRequested =
        LoggerMessage.Define(LogLevel.Information, new EventId(17, "ServiceShutdownRequested"), "Service shutdown requested successfully.");

    private static readonly Action<ILogger, Exception> _logErrorShuttingDown =
        LoggerMessage.Define(LogLevel.Warning, new EventId(18, "ErrorShuttingDown"), "Error while shutting down service.");

    private static readonly Action<ILogger, string, int, Exception?> _logHealthCheckResponse =
        LoggerMessage.Define<string, int>(LogLevel.Debug, new EventId(20, "HealthCheckResponse"), "Health check response: {Status} ({StatusCode})");

    private static readonly Action<ILogger, int, Exception?> _logRetryingServiceCheck =
        LoggerMessage.Define<int>(LogLevel.Debug, new EventId(21, "RetryingServiceCheck"), "Retrying service check (attempt {Attempt})");

    private static readonly Action<ILogger, int, TimeSpan, Exception?> _logHealthCheckRetry =
        LoggerMessage.Define<int, TimeSpan>(LogLevel.Debug, new EventId(22, "HealthCheckRetry"), 
            "Health check failed, retrying in {RetryNumber} with delay {Delay}...");

    private static readonly Action<ILogger, int, Exception?> _logFoundPidFile =
        LoggerMessage.Define<int>(LogLevel.Debug, new EventId(23, "FoundPidFile"), "Found PID file with process ID {ProcessId}");

    private static readonly Action<ILogger, int, Exception?> _logPidFileCreated =
        LoggerMessage.Define<int>(LogLevel.Debug, new EventId(24, "PidFileCreated"), "Created PID file for process {ProcessId}");

    private static readonly Action<ILogger, Exception?> _logPidFileInvalid =
        LoggerMessage.Define(LogLevel.Debug, new EventId(25, "PidFileInvalid"), "PID file is invalid or process is no longer running");

    public ServiceConnectionHelper(
        ILogger<ServiceConnectionHelper> logger, 
        string serviceUrl = "http://localhost:5000", 
        HttpClient? httpClient = null,
        string? servicePath = null)
    {
        _logger = logger;
        _httpClient = httpClient ?? new HttpClient();
        _serviceUrl = serviceUrl;
        _servicePath = servicePath ?? GetDefaultServicePath();
        _pidFilePath = Path.Combine(Path.GetDirectoryName(_servicePath)!, "service.pid");

        // Configure Polly retry policy for health checks
        _healthCheckPolicy = Policy<bool>
            .Handle<HttpRequestException>()
            .Or<TaskCanceledException>()
            .WaitAndRetryAsync(
                retryCount: 6, // Maximum of ~6.4 seconds total (100ms, 200ms, 400ms, 800ms, 1600ms, 3200ms)
                sleepDurationProvider: retryAttempt => 
                {
                    var delay = TimeSpan.FromMilliseconds(Math.Min(100 * Math.Pow(2, retryAttempt - 1), 3200));
                    _logHealthCheckRetry(_logger, retryAttempt, delay, null);
                    return delay;
                }
            );
    }

    public async Task EnsureServiceRunningAsync()
    {
        const int maxStartupRetries = 3;

        for (int attempt = 1; attempt <= maxStartupRetries; attempt++)
        {
            // 1. Check if service is running using exponential backoff
            if (await _healthCheckPolicy.ExecuteAsync(IsServiceRunningAsync))
            {
                _logServiceAlreadyRunning(_logger, null);
                return;
            }

            // 2. Look for existing processes
            var existingProcesses = GetExistingServiceProcesses();
            if (existingProcesses.Count != 0)
            {
                _logFoundExistingProcesses(_logger, null);
                
                // Check if process is responding
                if (await _healthCheckPolicy.ExecuteAsync(IsServiceRunningAsync))
                {
                    _logConnectedToExistingService(_logger, null);
                    return;
                }

                // Only clean up processes on the last attempt
                if (attempt == maxStartupRetries)
                {
                    foreach (var proc in existingProcesses)
                    {
                        try
                        {
                            proc.Kill();
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

            // 3. Start new service process
            _logStartingService(_logger, null);
            await StartServiceAsync();

            // 4. Wait for service to respond using exponential backoff
            if (await _healthCheckPolicy.ExecuteAsync(IsServiceRunningAsync))
            {
                _logServiceStartedSuccessfully(_logger, null);
                return;
            }

            throw new Exception("Unable to start LofiBeats service after multiple retries.");
        }
    }

    private Process? GetProcessFromPidFile()
    {
        try
        {
            if (!File.Exists(_pidFilePath))
                return null;

            var pidContent = File.ReadAllText(_pidFilePath).Trim();
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

        return null;
    }

    private bool IsLofiBeatsServiceProcess(Process process)
    {
        try
        {
            if (OperatingSystem.IsWindows())
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
                    if (commandLine?.Contains("LofiBeats.Service.dll") == true)
                    {
                        return true;
                    }
                }

                return false;
            }
            else if (OperatingSystem.IsLinux())
            {
                var cmdline = File.ReadAllText($"/proc/{process.Id}/cmdline");
                return cmdline.Contains("LofiBeats.Service.dll");
            }
            else if (OperatingSystem.IsMacOS())
            {
                var psi = new ProcessStartInfo
                {
                    FileName = "ps",
                    Arguments = $"-p {process.Id} -o command",
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                };
                var ps = Process.Start(psi);
                if (ps != null)
                {
                    var output = ps.StandardOutput.ReadToEnd();
                    ps.WaitForExit();
                    return output.Contains("LofiBeats.Service.dll");
                }
            }
        }
        catch (Exception ex)
        {
            _logErrorCheckingProcess(_logger, process.Id, ex);
        }

        return false;
    }

    private List<Process> GetExistingServiceProcesses()
    {
        // First check the PID file
        var pidProcess = GetProcessFromPidFile();
        if (pidProcess != null)
        {
            return new List<Process> { pidProcess };
        }

        // For test environment
        if (Environment.GetEnvironmentVariable("MOCK_PROCESS_TEST") == "true")
        {
            return new List<Process> { Process.GetCurrentProcess() };
        }

        // Fall back to full process enumeration
        var processes = Process.GetProcessesByName("dotnet")
            .Where(p => p.MainWindowTitle == "")
            .Where(IsLofiBeatsServiceProcess)
            .ToList();

        // Only log unsupported OS if we're not on a supported platform
        if (processes.Count == 0 && !OperatingSystem.IsWindows() && !OperatingSystem.IsLinux() && !OperatingSystem.IsMacOS())
        {
            _logUnsupportedOs(_logger, null);
        }

        return processes;
    }

    private async Task<bool> IsServiceRunningAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_serviceUrl}/healthz");
            _logHealthCheckResponse(_logger, response.StatusCode.ToString(), (int)response.StatusCode, null);

            // Consider both OK and redirect responses as successful
            return response.IsSuccessStatusCode || 
                   (int)response.StatusCode is >= 300 and < 400;
        }
        catch (Exception ex)
        {
            _logErrorCheckingProcess(_logger, 0, ex);
            return false;
        }
    }

    private async Task StartServiceAsync()
    {
        try
        {
            var serviceDirectory = Path.GetDirectoryName(_servicePath)!;

            // Copy service appsettings if they don't exist
            await CopyServiceSettingsAsync(serviceDirectory);

            // Start the service process
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = $"\"{_servicePath}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    WorkingDirectory = serviceDirectory
                }
            };

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

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            // Write PID file
            File.WriteAllText(_pidFilePath, process.Id.ToString());
            _logPidFileCreated(_logger, process.Id, null);

            _logServiceStarted(_logger, _servicePath, null);
        }
        catch (Exception ex)
        {
            _logFailedToStartService(_logger, ex);
            throw new Exception($"Failed to start LofiBeats service: {ex.Message}", ex);
        }
    }

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
        
        // Service DLL should be in the same directory
        var servicePath = Path.Combine(cliDirectory, "LofiBeats.Service.dll");

        if (!File.Exists(servicePath))
        {
            // For development, try looking in the service's output directory
            var serviceDevPath = Path.GetFullPath(Path.Combine(
                cliDirectory, // bin/Debug/net9.0
                "..", "..", "..", // back to src/LofiBeats.Cli
                "..", "LofiBeats.Service", // to src/LofiBeats.Service
                "bin", "Debug", "net9.0",
                "LofiBeats.Service.dll"
            ));

            if (File.Exists(serviceDevPath))
            {
                return serviceDevPath;
            }
        }

        return servicePath;
    }

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

    public async Task ShutdownServiceAsync()
    {
        // Check if service is running first
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