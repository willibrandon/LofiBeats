using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;

namespace LofiBeats.Cli;

public class ServiceConnectionHelper
{
    private readonly ILogger<ServiceConnectionHelper> _logger;
    private readonly HttpClient _httpClient;
    private readonly string _serviceUrl;
    private readonly string _servicePath;

    public ServiceConnectionHelper(ILogger<ServiceConnectionHelper> logger, string serviceUrl = "http://localhost:5000", string? servicePath = null)
    {
        _logger = logger;
        _httpClient = new HttpClient();
        _serviceUrl = serviceUrl;
        _servicePath = servicePath ?? GetDefaultServicePath();
    }

    public async Task EnsureServiceRunningAsync()
    {
        // 1. Check if service is running
        if (await IsServiceRunningAsync())
        {
            _logger.LogDebug("LofiBeats service is already running.");
            return;
        }

        // 2. Not running => Start service process
        StartServiceProcess();

        // 3. Wait until it responds or timeout
        const int maxRetries = 10;
        for (int i = 0; i < maxRetries; i++)
        {
            await Task.Delay(500); // half a second
            if (await IsServiceRunningAsync())
            {
                _logger.LogInformation("LofiBeats service started successfully.");
                return;
            }
            _logger.LogDebug($"Waiting for service to start (attempt {i + 1}/{maxRetries})...");
        }

        throw new Exception("Unable to start LofiBeats service after multiple retries.");
    }

    private async Task<bool> IsServiceRunningAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_serviceUrl}/healthz");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    private void StartServiceProcess()
    {
        _logger.LogInformation("Attempting to start LofiBeats service...");

        var servicePath = _servicePath;
        var workingDirectory = Path.GetDirectoryName(servicePath);

        if (string.IsNullOrEmpty(workingDirectory))
        {
            throw new Exception("Could not determine working directory for service.");
        }

        var psi = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"exec \"{servicePath}\"",
            UseShellExecute = false, // Don't use shell execute to keep in same window
            CreateNoWindow = true, // Don't create a new window
            WorkingDirectory = workingDirectory,
            RedirectStandardOutput = true, // Capture output
            RedirectStandardError = true // Capture errors
        };

        try
        {
            var process = Process.Start(psi);
            if (process == null)
            {
                throw new Exception("Failed to start process - Process.Start returned null");
            }

            // Log output and errors asynchronously
            process.OutputDataReceived += (sender, e) => 
            {
                if (!string.IsNullOrEmpty(e.Data))
                    _logger.LogDebug($"Service: {e.Data}");
            };
            process.ErrorDataReceived += (sender, e) => 
            {
                if (!string.IsNullOrEmpty(e.Data))
                    _logger.LogError($"Service Error: {e.Data}");
            };

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            _logger.LogInformation($"Started service from: {servicePath}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to start LofiBeats service.");
            throw new Exception($"Failed to start LofiBeats service: {ex.Message}", ex);
        }
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
        try
        {
            await _httpClient.PostAsync($"{_serviceUrl}/api/lofi/shutdown", null);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error while shutting down service. It may have already been stopped.");
        }
    }
} 