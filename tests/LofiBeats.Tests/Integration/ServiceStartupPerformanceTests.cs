using LofiBeats.Cli;
using Microsoft.Extensions.Logging;
using Moq;
using System.Diagnostics;
using Xunit.Abstractions;

namespace LofiBeats.Tests.Integration;

public class ServiceStartupPerformanceTests : IDisposable
{
    private readonly ITestOutputHelper _output;
    private readonly Mock<ILogger<ServiceConnectionHelper>> _loggerMock;
    private readonly ServiceConnectionHelper _helper;
    private readonly Stopwatch _totalStopwatch = new();
    private readonly Dictionary<string, TimeSpan> _operationTimings = new();

    public ServiceStartupPerformanceTests(ITestOutputHelper output)
    {
        _output = output;
        _loggerMock = new Mock<ILogger<ServiceConnectionHelper>>();
        
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
            var formatter = invocation.Arguments[4] as Func<object, Exception?, string>;
            
            if (formatter != null)
            {
                var message = formatter(state, exception);
                _output.WriteLine($"{logLevel}: [{eventId}] {message}");
            }
        }));

        _helper = new ServiceConnectionHelper(_loggerMock.Object);
    }

    public void Dispose()
    {
        // Ensure service is shut down after each test
        _helper.ShutdownServiceAsync().Wait();
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
            
            // Ensure service is stopped before each measurement
            await _helper.ShutdownServiceAsync();
            await Task.Delay(1000); // Give time for full shutdown
            
            results.Add(await MeasureStartupOperations());
            
            // Log individual run results
            foreach (var (operation, timing) in results[i])
            {
                _output.WriteLine($"{operation}: {timing.TotalMilliseconds:F1}ms");
            }
        }

        // Calculate and log averages
        _output.WriteLine("\nAverage timings across all runs:");
        var operations = results[0].Keys;
        foreach (var operation in operations)
        {
            var average = results.Average(r => r[operation].TotalMilliseconds);
            _output.WriteLine($"{operation}: {average:F1}ms");
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

        // Create a proxy ServiceConnectionHelper that measures individual operations
        var helper = new ServiceConnectionHelper(
            new TimingLogger(_loggerMock.Object, operationStopwatch, timings));

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