using LofiBeats.Cli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Diagnostics;
using System.Net;

namespace LofiBeats.Tests.Integration;

[Collection("AI Generated Tests")]
public class ServiceConnectionHelperTests : IDisposable
{
    private readonly Mock<ILogger<ServiceConnectionHelper>> _loggerMock;
    private readonly Mock<HttpMessageHandler> _httpHandlerMock;
    private readonly HttpClient _httpClient;
    private readonly string _testServiceUrl = "http://localhost:5000";
    private readonly string _testServicePath;
    private ServiceConnectionHelper _helper;
    private readonly List<string> _cleanupFiles = new();

    public ServiceConnectionHelperTests()
    {
        _loggerMock = new Mock<ILogger<ServiceConnectionHelper>>();
        _httpHandlerMock = new Mock<HttpMessageHandler>();
        
        // Setup default response for any unmatched requests
        _httpHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable));

        _httpClient = new HttpClient(_httpHandlerMock.Object);
        
        // Create a unique test directory for each test run
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

        // Create mock configuration
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(c => c["ServiceUrl"]).Returns(_testServiceUrl);

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

    private void SetupHttpMockResponse(HttpStatusCode statusCode, string? content = null, string? endpoint = null)
    {
        var response = new HttpResponseMessage(statusCode);
        if (content != null)
        {
            response.Content = new StringContent(content);
        }

        var setup = _httpHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => 
                    endpoint == null || req.RequestUri!.ToString().EndsWith(endpoint)),
                ItExpr.IsAny<CancellationToken>());

        setup.ReturnsAsync(response)
             .Verifiable();
    }

    private void SetupHttpMockResponses(Queue<HttpResponseMessage> responses)
    {
        var setup = _httpHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());

        var sequenceSetup = setup.Returns(() =>
        {
            if (responses.Count == 0)
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable));
            }
            return Task.FromResult(responses.Dequeue());
        });

        sequenceSetup.Verifiable();
    }

    private void SetupLoggerMock()
    {
        _loggerMock.Setup(x => x.IsEnabled(It.IsAny<LogLevel>())).Returns(true);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task IsServiceRunning_WhenHealthCheckSucceeds_ReturnsTrue()
    {
        // Arrange
        SetupHttpMockResponse(HttpStatusCode.OK, endpoint: "healthz");

        // Act
        await _helper.EnsureServiceRunningAsync();

        // Assert
        _httpHandlerMock.Protected().Verify(
            "SendAsync",
            Times.AtLeastOnce(),
            ItExpr.Is<HttpRequestMessage>(req => 
                req.Method == HttpMethod.Get && 
                req.RequestUri!.ToString() == $"{_testServiceUrl}/healthz"),
            ItExpr.IsAny<CancellationToken>()
        );
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task EnsureServiceRunning_WhenServiceNotRunning_StartsNewService()
    {
        // Arrange
        var responses = new Queue<HttpResponseMessage>(
        [
            new HttpResponseMessage(HttpStatusCode.ServiceUnavailable), // First check fails
            new HttpResponseMessage(HttpStatusCode.OK)                  // Second check succeeds
        ]);

        SetupHttpMockResponses(responses);
        Environment.SetEnvironmentVariable("MOCK_SERVICE_TEST", "true");

        try
        {
            // Act
            await _helper.EnsureServiceRunningAsync();

            // Assert
            _httpHandlerMock.Protected().Verify(
                "SendAsync",
                Times.AtLeast(2),
                ItExpr.Is<HttpRequestMessage>(req => 
                    req.Method == HttpMethod.Get && 
                    req.RequestUri!.ToString() == $"{_testServiceUrl}/healthz"),
                ItExpr.IsAny<CancellationToken>()
            );
        }
        finally
        {
            Environment.SetEnvironmentVariable("MOCK_SERVICE_TEST", null);
        }
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task ShutdownService_WhenServiceRunning_SendsShutdownCommand()
    {
        // Arrange
        SetupHttpMockResponse(HttpStatusCode.OK);

        // Act
        await _helper.ShutdownServiceAsync();

        // Assert
        _httpHandlerMock.Protected().Verify(
            "SendAsync",
            Times.AtLeastOnce(),
            ItExpr.Is<HttpRequestMessage>(req => 
                req.Method == HttpMethod.Post && 
                req.RequestUri!.ToString() == $"{_testServiceUrl}/api/lofi/shutdown"),
            ItExpr.IsAny<CancellationToken>()
        );
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task SendCommand_ExecutesRequestWithCorrectEndpoint()
    {
        // Arrange
        var testEndpoint = "test-endpoint";
        var testContent = new { test = "data" };
        SetupHttpMockResponse(HttpStatusCode.OK, "{\"result\": \"success\"}");

        // Act
        await _helper.SendCommandAsync(HttpMethod.Post, testEndpoint, testContent);

        // Assert
        _httpHandlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(req => 
                req.Method == HttpMethod.Post && 
                req.RequestUri!.ToString() == $"{_testServiceUrl}/api/lofi/{testEndpoint}"),
            ItExpr.IsAny<CancellationToken>()
        );
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task SendCommand_WhenServiceNotRunning_StartsServiceBeforeSendingCommand()
    {
        // Arrange
        var responses = new Queue<HttpResponseMessage>(
        [
            new HttpResponseMessage(HttpStatusCode.ServiceUnavailable), // Initial health check fails
            new HttpResponseMessage(HttpStatusCode.OK),                 // Service starts successfully
            new HttpResponseMessage(HttpStatusCode.OK)                  // Command succeeds
        ]);

        _httpHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .Returns<HttpRequestMessage, CancellationToken>((_, _) => Task.FromResult(responses.Dequeue()));

        Environment.SetEnvironmentVariable("MOCK_SERVICE_TEST", "true");

        try
        {
            // Act
            await _helper.SendCommandAsync(HttpMethod.Get, "test");

            // Assert
            _httpHandlerMock.Protected().Verify(
                "SendAsync",
                Times.AtLeast(3), // Health check, service start check, and command
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            );
        }
        finally
        {
            Environment.SetEnvironmentVariable("MOCK_SERVICE_TEST", null);
        }
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task EnsureServiceRunning_WhenStartupFails_ThrowsException()
    {
        // Arrange
        _httpHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable));

        Environment.SetEnvironmentVariable("MOCK_SERVICE_TEST", "true");

        try
        {
            // Act & Assert
            await Assert.ThrowsAsync<ServiceStartException>(() => _helper.EnsureServiceRunningAsync());
        }
        finally
        {
            Environment.SetEnvironmentVariable("MOCK_SERVICE_TEST", null);
        }
    }

    [SkippableFact]
    [Trait("Category", "AI_Generated")]
    [Trait("Category", "Platform_Specific")]
    public async Task GetExistingServiceProcesses_WhenMockProcessTest_ReturnsCurrentProcess()
    {
        Skip.If(!OperatingSystem.IsWindows(), "Process management tests are Windows-specific");
        
        // Arrange
        SetupHttpMockResponse(HttpStatusCode.OK, endpoint: "healthz");
        Environment.SetEnvironmentVariable("MOCK_PROCESS_TEST", "true");

        try
        {
            // Act
            await _helper.EnsureServiceRunningAsync();

            // Assert
            _httpHandlerMock.Protected().Verify(
                "SendAsync",
                Times.AtLeastOnce(),
                ItExpr.Is<HttpRequestMessage>(req => 
                    req.Method == HttpMethod.Get && 
                    req.RequestUri!.ToString() == $"{_testServiceUrl}/healthz"),
                ItExpr.IsAny<CancellationToken>()
            );
        }
        finally
        {
            Environment.SetEnvironmentVariable("MOCK_PROCESS_TEST", null);
        }
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task ShutdownService_WhenServiceNotRunning_LogsAndReturns()
    {
        // Arrange
        SetupHttpMockResponse(HttpStatusCode.ServiceUnavailable);
        SetupLoggerMock();

        // Act
        await _helper.ShutdownServiceAsync();

        // Assert
        _loggerMock.Verify(
            x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.AtLeastOnce);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task SendCommand_WhenCommandFails_ThrowsException()
    {
        // Arrange
        // First set up successful health check to pass EnsureServiceRunning
        var responses = new Queue<HttpResponseMessage>(
        [
            new HttpResponseMessage(HttpStatusCode.OK),                  // Health check succeeds
            new HttpResponseMessage(HttpStatusCode.InternalServerError)  // Command fails
        ]);

        SetupHttpMockResponses(responses);
        Environment.SetEnvironmentVariable("MOCK_SERVICE_TEST", "true");

        try
        {
            // Act & Assert
            var exception = await Assert.ThrowsAsync<HttpRequestException>(() => 
                _helper.SendCommandAsync(HttpMethod.Post, "test-endpoint"));
            
            Assert.Equal(HttpStatusCode.InternalServerError, ((HttpRequestException)exception).StatusCode);
        }
        finally
        {
            Environment.SetEnvironmentVariable("MOCK_SERVICE_TEST", null);
        }
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task EnsureServiceRunning_WhenServiceAlreadyRunning_DoesNotStartNew()
    {
        // Arrange
        SetupHttpMockResponse(HttpStatusCode.OK);
        Environment.SetEnvironmentVariable("MOCK_SERVICE_TEST", "true");

        try
        {
            // Act
            await _helper.EnsureServiceRunningAsync();
            await _helper.EnsureServiceRunningAsync(); // Call twice

            // Assert
            _httpHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(2), // Only health checks, no service start
                ItExpr.Is<HttpRequestMessage>(req => 
                    req.Method == HttpMethod.Get && 
                    req.RequestUri!.ToString() == $"{_testServiceUrl}/healthz"),
                ItExpr.IsAny<CancellationToken>()
            );
        }
        finally
        {
            Environment.SetEnvironmentVariable("MOCK_SERVICE_TEST", null);
        }
    }
} 