using LofiBeats.Cli.Commands;
using LofiBeats.Core.PluginManagement;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using System.Text.Json;

namespace LofiBeats.Tests.Commands;

[Collection("Command Tests")]
public class EffectCommandTests : IDisposable
{
    private readonly Mock<ILogger<CommandLineInterface>> _loggerMock;
    private readonly Mock<ILoggerFactory> _loggerFactoryMock;
    private readonly Mock<IConfiguration> _configMock;
    private readonly CommandLineInterface _cli;
    private readonly StringWriter _consoleOutput;
    private readonly TextWriter _originalConsoleOut;

    public EffectCommandTests()
    {
        _loggerMock = new Mock<ILogger<CommandLineInterface>>();
        _loggerFactoryMock = new Mock<ILoggerFactory>();
        _configMock = new Mock<IConfiguration>();

        // Setup logger factory
        _loggerFactoryMock
            .Setup(x => x.CreateLogger(It.IsAny<string>()))
            .Returns(_loggerMock.Object);

        // Setup configuration
        _configMock
            .Setup(x => x["ServiceUrl"])
            .Returns("http://localhost:5001");

        _cli = new CommandLineInterface(_loggerMock.Object, _loggerFactoryMock.Object, _configMock.Object);

        // Capture console output
        _originalConsoleOut = Console.Out;
        _consoleOutput = new StringWriter();
        Console.SetOut(_consoleOutput);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task EffectList_ShowsBuiltInEffects()
    {
        // Act
        await _cli.ExecuteAsync(["effect", "list"]);
        var output = _consoleOutput.ToString();

        // Assert
        Assert.Contains("Built-in effects:", output);
        Assert.Contains("vinyl", output);
        Assert.Contains("reverb", output);
        Assert.Contains("lowpass", output);
        Assert.Contains("tapeflutter", output);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task EffectList_WithPlugins_ShowsAllEffects()
    {
        // Arrange
        var pluginEffects = new[]
        {
            new { Name = "testeffect", Description = "A test effect", Version = "1.0.0", Author = "Test Author" }
        };

        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler
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

        var httpClient = new HttpClient(mockHandler.Object)
        {
            BaseAddress = new Uri("http://localhost:5001")
        };

        // Create a new CLI instance with the mocked HTTP client
        var cli = new CommandLineInterface(
            _loggerMock.Object,
            _loggerFactoryMock.Object,
            _configMock.Object);

        // Act
        await cli.ExecuteAsync(["effect", "list"]);
        var output = _consoleOutput.ToString();

        // Assert
        Assert.Contains("Built-in effects:", output);
        Assert.Contains("Plugin effects:", output);
        Assert.Contains("testeffect", output);
        Assert.Contains("A test effect", output);
        Assert.Contains("1.0.0", output);
        Assert.Contains("Test Author", output);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task EffectList_ServiceDown_HandlesError()
    {
        // Act
        await _cli.ExecuteAsync(["effect", "list"]);
        var output = _consoleOutput.ToString();

        // Assert
        Assert.Contains("Built-in effects:", output); // Should still show built-in effects
        Assert.Contains("Error fetching effects:", output);
        Assert.Contains("Please ensure the LofiBeats service is running", output);
    }

    public void Dispose()
    {
        Console.SetOut(_originalConsoleOut);
        _consoleOutput.Dispose();
    }
} 