using LofiBeats.Cli.Commands;
using LofiBeats.Core.PluginManagement;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using LofiBeats.Cli;

namespace LofiBeats.Tests.Commands;

[Collection("Command Tests")]
public class EffectCommandTests : IDisposable
{
    private readonly Mock<ILogger<CommandLineInterface>> _loggerMock;
    private readonly Mock<ILoggerFactory> _loggerFactoryMock;
    private readonly Mock<IConfiguration> _configMock;
    private readonly Mock<HttpMessageHandler> _mockHandler;
    private readonly HttpClient _httpClient;
    private readonly CommandLineInterface _cli;
    private readonly StringWriter _consoleOutput;
    private readonly TextWriter _originalConsoleOut;

    public EffectCommandTests()
    {
        _loggerMock = new Mock<ILogger<CommandLineInterface>>();
        _loggerFactoryMock = new Mock<ILoggerFactory>();
        _configMock = new Mock<IConfiguration>();
        _mockHandler = new Mock<HttpMessageHandler>();

        // Setup HTTP client
        _httpClient = new HttpClient(_mockHandler.Object)
        {
            BaseAddress = new Uri("http://localhost:5001")
        };

        // Setup configuration
        _configMock
            .Setup(x => x["ServiceUrl"])
            .Returns("http://localhost:5001");

        // Setup logger factory
        _loggerFactoryMock
            .Setup(x => x.CreateLogger(It.IsAny<string>()))
            .Returns(_loggerMock.Object);

        // Create ServiceConnectionHelper with mocked HTTP client
        var helper = new ServiceConnectionHelper(
            _loggerFactoryMock.Object.CreateLogger<ServiceConnectionHelper>(),
            _configMock.Object,
            serviceUrl: "http://localhost:5001",
            httpClient: _httpClient);

        // Create CLI with mocked dependencies
        _cli = new CommandLineInterface(
            _loggerMock.Object,
            _loggerFactoryMock.Object,
            _configMock.Object);

        // Capture console output
        _originalConsoleOut = Console.Out;
        _consoleOutput = new StringWriter();
        Console.SetOut(_consoleOutput);

        // Setup default mock for EnsureServiceRunningAsync
        _mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.RequestUri!.PathAndQuery == "/healthz"),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task EffectList_ShowsBuiltInEffects()
    {
        // Arrange
        _mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri!.PathAndQuery == "/api/lofi/effect/list"),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("[]") // Empty array for no plugins
            });

        // Act
        await _cli.ExecuteAsync(["effect", "list"]);
        var output = _consoleOutput.ToString();

        // Assert
        Assert.Contains("Built-in effects:", output);
        var knownEffects = new[] { "vinyl", "reverb", "lowpass", "tapeflutter" };
        foreach (var effect in knownEffects)
        {
            Assert.Contains(effect, output);
        }
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task EffectList_WithPlugins_ShowsAllEffects()
    {
        // Arrange
        var pluginEffects = new[]
        {
            new PluginEffectInfo 
            { 
                Name = "testeffect", 
                Description = "A test effect", 
                Version = "1.0.0", 
                Author = "Test Author" 
            }
        };

        _mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri!.PathAndQuery == "/api/lofi/effect/list"),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(pluginEffects))
            });

        // Act
        await _cli.ExecuteAsync(["effect", "list"]);
        var output = _consoleOutput.ToString();

        // Assert - Check built-in effects
        Assert.Contains("Built-in effects:", output);
        var knownEffects = new[] { "vinyl", "reverb", "lowpass", "tapeflutter" };
        foreach (var effect in knownEffects)
        {
            Assert.Contains(effect, output);
        }

        // Assert - Check built-in effects
        Assert.Contains("Built-in effects:", output);
        Assert.Contains("testeffect", output);
        Assert.Contains("A test effect", output);
        Assert.Contains("1.0.0", output);
        Assert.Contains("Test Author", output);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task EffectList_ServiceDown_HandlesError()
    {
        // Arrange
        _mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri!.PathAndQuery == "/api/lofi/effect/list"),
                ItExpr.IsAny<CancellationToken>()
            )
            .ThrowsAsync(new HttpRequestException("Connection refused"));

        // Act
        await _cli.ExecuteAsync(["effect", "list"]);
        var output = _consoleOutput.ToString();

        // Assert
        Assert.Contains("Built-in effects:", output); // Should still show built-in effects
        Assert.Contains("Error fetching plugin effects:", output);
        Assert.Contains("Please ensure the LofiBeats service is running", output);
    }

    public void Dispose()
    {
        Console.SetOut(_originalConsoleOut);
        _consoleOutput.Dispose();
        _httpClient.Dispose();
    }
} 