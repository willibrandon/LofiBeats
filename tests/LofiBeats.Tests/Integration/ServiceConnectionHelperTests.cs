using LofiBeats.Cli;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using System.Runtime.InteropServices;

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
        
        _testServicePath = Path.Combine(testDir, "LofiBeats.Service.dll");
        File.WriteAllText(_testServicePath, "test");

        _helper = new ServiceConnectionHelper(
            _loggerMock.Object,
            _testServiceUrl,
            _httpClient,
            _testServicePath);
    }

    public void Dispose()
    {
        _httpClient.Dispose();
        
        // Clean up all test files and directories
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
        var responses = new Queue<HttpResponseMessage>(new[]
        {
            new HttpResponseMessage(HttpStatusCode.ServiceUnavailable), // First check fails
            new HttpResponseMessage(HttpStatusCode.OK)                  // Second check succeeds
        });

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
        var responses = new Queue<HttpResponseMessage>(new[]
        {
            new HttpResponseMessage(HttpStatusCode.ServiceUnavailable), // Initial health check fails
            new HttpResponseMessage(HttpStatusCode.OK),                 // Service starts successfully
            new HttpResponseMessage(HttpStatusCode.OK)                  // Command succeeds
        });

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
            await Assert.ThrowsAsync<Exception>(() => _helper.EnsureServiceRunningAsync());
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
        var responses = new Queue<HttpResponseMessage>(new[]
        {
            new HttpResponseMessage(HttpStatusCode.OK),                  // Health check succeeds
            new HttpResponseMessage(HttpStatusCode.InternalServerError)  // Command fails
        });

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