using LofiBeats.Cli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Diagnostics;
using System.Net;
using Xunit.Abstractions;

namespace LofiBeats.Tests.Integration;

public class ServiceStartupPerformanceTests : IDisposable
{
    private readonly ITestOutputHelper _output;
    private readonly Mock<ILogger<ServiceConnectionHelper>> _loggerMock;
    private readonly Mock<HttpMessageHandler> _httpHandlerMock;
    private readonly HttpClient _httpClient;
    private readonly string _testServicePath;
    private readonly ServiceConnectionHelper _helper;
    private readonly List<string> _cleanupFiles = [];

    public ServiceStartupPerformanceTests(ITestOutputHelper output)
    {
        _output = output;
        _loggerMock = new Mock<ILogger<ServiceConnectionHelper>>();
        _httpHandlerMock = new Mock<HttpMessageHandler>();
        
        // Enable all log levels
        _loggerMock.Setup(x => x.IsEnabled(It.IsAny<LogLevel>())).Returns(true);
        
        // Setup logger to output to test console
        _loggerMock.Setup(x => x.Log(
            It.IsAny<LogLevel>(),
            It.IsAny<EventId>(),
            It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()
        )).Callback(new InvocationAction(invocation =>
        {
            var logLevel = (LogLevel)invocation.Arguments[0];
            var eventId = (EventId)invocation.Arguments[1];
            var state = invocation.Arguments[2];
            var exception = (Exception?)invocation.Arguments[3];

            if (invocation.Arguments[4] is Func<object, Exception?, string> formatter)
            {
                var message = formatter(state, exception);
                _output.WriteLine($"{logLevel}: [{eventId}] {message}");
            }
        }));

        // Create a unique test directory
        var testDir = Path.Combine(Path.GetTempPath(), "LofiBeatsTests", Guid.NewGuid().ToString());
        Directory.CreateDirectory(testDir);
        _cleanupFiles.Add(testDir);
        
        // Copy the actual service files instead of creating an empty one
        var sourceServicePath = Path.Combine(
            AppContext.BaseDirectory,
            "LofiBeats.Service.dll");
        
        _testServicePath = Path.Combine(testDir, "LofiBeats.Service.dll");
        
        // Copy service and its dependencies
        if (File.Exists(sourceServicePath))
        {
            var serviceTestDir = Path.GetDirectoryName(_testServicePath)!;
            
            // Copy all DLL files from the source directory
            foreach (var file in Directory.GetFiles(Path.GetDirectoryName(sourceServicePath)!, "*.dll"))
            {
                var fileName = Path.GetFileName(file);
                File.Copy(file, Path.Combine(serviceTestDir, fileName), true);
            }
            
            // Copy runtime config
            var runtimeConfig = Path.ChangeExtension(sourceServicePath, ".runtimeconfig.json");
            if (File.Exists(runtimeConfig))
            {
                File.Copy(runtimeConfig, Path.ChangeExtension(_testServicePath, ".runtimeconfig.json"));
            }
            
            // Copy deps file
            var depsFile = Path.ChangeExtension(sourceServicePath, ".deps.json");
            if (File.Exists(depsFile))
            {
                File.Copy(depsFile, Path.ChangeExtension(_testServicePath, ".deps.json"));
            }
        }
        else
        {
            // Fallback for when running tests without build
            File.WriteAllText(_testServicePath, "test");
        }

        // Setup HTTP mock with instant responses
        _httpHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable));

        _httpClient = new HttpClient(_httpHandlerMock.Object);

        // Create mock configuration
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(c => c["ServiceUrl"]).Returns("http://localhost:5000");

        _helper = new ServiceConnectionHelper(
            _loggerMock.Object,
            configurationMock.Object,
            null,
            _httpClient,
            _testServicePath);
    }

    public void Dispose()
    {
        try
        {
            // Try graceful shutdown first
            _helper.ShutdownServiceAsync().Wait(TimeSpan.FromSeconds(5));
        }
        catch
        {
            // Ignore shutdown errors
        }

        _httpClient.Dispose();
        
        // Find and kill any remaining service processes
        try
        {
            var serviceName = Path.GetFileNameWithoutExtension(_testServicePath);
            foreach (var proc in Process.GetProcessesByName("dotnet")
                .Where(p => p.MainWindowTitle == "" && IsLofiBeatsServiceProcess(p)))
            {
                try
                {
                    if (!proc.HasExited)
                    {
                        proc.Kill(true);
                        proc.WaitForExit(1000);
                    }
                }
                catch
                {
                    // Ignore process cleanup errors
                }
                finally
                {
                    proc.Dispose();
                }
            }
        }
        catch
        {
            // Ignore process enumeration errors
        }
        
        // Clean up test files
        foreach (var path in _cleanupFiles)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                else if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
            }
            catch
            {
                // Ignore cleanup errors
            }
        }
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

                foreach (var item in collection)
                {
                    var commandLine = item["CommandLine"]?.ToString();
                    if (commandLine?.Contains(_testServicePath) == true)
                    {
                        return true;
                    }
                }
            }
            else if (OperatingSystem.IsLinux())
            {
                var cmdline = File.ReadAllText($"/proc/{process.Id}/cmdline");
                return cmdline.Contains(_testServicePath);
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
                    return output.Contains(_testServicePath);
                }
            }
        }
        catch
        {
            // Ignore process check errors
        }
        return false;
    }

    [Fact]
    public async Task MeasureColdStartupTime()
    {
        // Run multiple cold starts to get consistent measurements
        const int numberOfRuns = 3;
        var results = new List<Dictionary<string, TimeSpan>>();

        for (int i = 0; i < numberOfRuns; i++)
        {
            _output.WriteLine($"\nRun {i + 1} of {numberOfRuns}:");

            // Setup HTTP responses for this run
            var responses = new Queue<HttpResponseMessage>(
            [
                new HttpResponseMessage(HttpStatusCode.ServiceUnavailable), // Initial health check
                new HttpResponseMessage(HttpStatusCode.ServiceUnavailable), // After process check
                new HttpResponseMessage(HttpStatusCode.OK)                  // After service start
            ]);

            _httpHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Returns<HttpRequestMessage, CancellationToken>((_, _) => 
                    Task.FromResult(responses.Count > 0 
                        ? responses.Dequeue() 
                        : new HttpResponseMessage(HttpStatusCode.OK)));
            
            // Ensure service is completely stopped before each measurement
            await _helper.ShutdownServiceAsync();
            
            // Wait for the service to fully stop and verify
            await Task.Delay(1000); // Give time for shutdown to complete
            
            // Kill any remaining service processes
            var serviceName = Path.GetFileNameWithoutExtension(_testServicePath);
            foreach (var proc in Process.GetProcessesByName("dotnet")
                .Where(p => p.MainWindowTitle == "" && IsLofiBeatsServiceProcess(p)))
            {
                try
                {
                    if (!proc.HasExited)
                    {
                        proc.Kill(true);
                        proc.WaitForExit(3000);
                    }
                }
                catch
                {
                    // Ignore process cleanup errors
                }
                finally
                {
                    proc.Dispose();
                }
            }
            
            results.Add(await MeasureStartupOperations());
            
            // Log individual run results
            foreach (var (operation, timing) in results[i])
            {
                _output.WriteLine($"{operation}: {timing.TotalMilliseconds:F1}ms");
            }

            // Verify the number of HTTP requests
            _httpHandlerMock.Protected().Verify(
                "SendAsync",
                Times.AtLeast(3),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        // Calculate and log averages
        _output.WriteLine("\nAverage timings across all runs:");
        var operations = results.SelectMany(r => r.Keys).Distinct();
        foreach (var operation in operations)
        {
            var timings = results.Where(r => r.ContainsKey(operation))
                                .Select(r => r[operation].TotalMilliseconds)
                                .ToList();
            
            if (timings.Count > 0)
            {
                var average = timings.Average();
                _output.WriteLine($"{operation}: {average:F1}ms");
            }
            else
            {
                _output.WriteLine($"{operation}: No measurements");
            }
        }

        // Add some basic assertions to catch significant regressions
        var avgTotalTime = results.Average(r => r["Total Startup"].TotalMilliseconds);
        Assert.True(avgTotalTime < 5000, $"Average startup time ({avgTotalTime:F1}ms) exceeded 5 seconds");
    }

    private async Task<Dictionary<string, TimeSpan>> MeasureStartupOperations()
    {
        var timings = new Dictionary<string, TimeSpan>();
        var totalStopwatch = Stopwatch.StartNew();
        var operationStopwatch = new Stopwatch();

        // Create mock configuration
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(c => c["ServiceUrl"]).Returns("http://localhost:5000");

        // Create a proxy ServiceConnectionHelper that measures individual operations
        var helper = new ServiceConnectionHelper(
            new TimingLogger(_loggerMock.Object, operationStopwatch, timings),
            configurationMock.Object,
            null,
            _httpClient,
            _testServicePath);

        // Measure total startup time
        operationStopwatch.Start();
        await helper.EnsureServiceRunningAsync();
        operationStopwatch.Stop();
        totalStopwatch.Stop();

        timings["Total Startup"] = totalStopwatch.Elapsed;
        return timings;
    }

    private sealed class TimingLogger : ILogger<ServiceConnectionHelper>
    {
        private readonly ILogger<ServiceConnectionHelper> _innerLogger;
        private readonly Stopwatch _stopwatch;
        private readonly Dictionary<string, TimeSpan> _timings;
        private string _currentOperation = string.Empty;

        public TimingLogger(
            ILogger<ServiceConnectionHelper> logger,
            Stopwatch stopwatch,
            Dictionary<string, TimeSpan> timings)
        {
            _innerLogger = logger;
            _stopwatch = stopwatch;
            _timings = timings;
            
            // Start timing the initial health check
            _currentOperation = "Health Check";
            _stopwatch.Start();
        }

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            var message = formatter(state, exception);
            
            // Track timing based on log messages
            switch (eventId.Name)
            {
                case "StartingService":
                    RecordTiming("Health Check");
                    _currentOperation = "Process Enumeration";
                    _stopwatch.Restart();
                    break;
                
                case "FoundExistingProcesses":
                    RecordTiming("Process Enumeration");
                    _currentOperation = "Process Cleanup";
                    _stopwatch.Restart();
                    break;

                case "ServiceStartedSuccessfully":
                    RecordTiming(_currentOperation);
                    _currentOperation = "Service Initialization";
                    _stopwatch.Restart();
                    break;
                
                case "ServiceAlreadyRunning":
                    RecordTiming("Health Check");
                    break;
            }

            _innerLogger.Log(logLevel, eventId, state, exception, formatter);
        }

        private void RecordTiming(string operation)
        {
            if (_currentOperation != string.Empty)
            {
                _stopwatch.Stop();
                if (_timings.TryGetValue(operation, out var existingTiming))
                {
                    _timings[operation] = existingTiming.Add(_stopwatch.Elapsed);
                }
                else
                {
                    _timings[operation] = _stopwatch.Elapsed;
                }
            }
        }

        public bool IsEnabled(LogLevel logLevel) => true;
        
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull 
            => _innerLogger.BeginScope(state);
    }
} 