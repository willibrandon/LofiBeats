using LofiBeats.Core.Playback;
using LofiBeats.Core.Telemetry;
using LofiBeats.Core.WebSocket;
using LofiBeats.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace LofiBeats.Tests.Integration;

[Collection("AI Generated Tests")]
public class WebSocketIntegrationTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly WebSocketClient _wsClient;
    private WebSocket? _webSocket;
    private readonly CancellationTokenSource _cts;

    public WebSocketIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Testing");
            builder.ConfigureAppConfiguration((context, config) =>
            {
                var settings = new Dictionary<string, string?>
                {
                    ["WebSocket:RequireAuthentication"] = "false",
                    ["WebSocket:MessageRateLimit"] = "5",
                    ["WebSocket:MaxMessageSize"] = "32768",
                    ["WebSocket:EndpointPath"] = "/ws/lofi"
                };
                config.AddInMemoryCollection(settings);
            });

            builder.ConfigureServices(services =>
            {
                // Remove existing services and replace with test versions
                foreach (var descriptor in services.Where(d => d.ServiceType == typeof(IAudioPlaybackService)).ToList())
                {
                    services.Remove(descriptor);
                }
                services.AddSingleton<IAudioPlaybackService, TestAudioPlaybackService>();

                // Replace telemetry with mocks
                foreach (var descriptor in services.Where(d => d.ServiceType == typeof(ITelemetryService)).ToList())
                {
                    services.Remove(descriptor);
                }
                var mockTelemetry = new Mock<ITelemetryService>();
                services.AddSingleton(mockTelemetry.Object);

                foreach (var descriptor in services.Where(d => d.ServiceType == typeof(TelemetryTracker)).ToList())
                {
                    services.Remove(descriptor);
                }
                var mockLogger = new Mock<ILogger<TelemetryTracker>>();
                services.AddSingleton(new TelemetryTracker(mockTelemetry.Object, mockLogger.Object));
            });
        });

        _wsClient = _factory.Server.CreateWebSocketClient();
        _cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
    }

    private async Task<bool> WaitForServerReady()
    {
        var maxAttempts = 10;
        var delay = TimeSpan.FromMilliseconds(500);
        var attempt = 0;

        while (attempt < maxAttempts)
        {
            try
            {
                using var client = _factory.CreateDefaultClient();
                var response = await client.GetAsync("/healthz");
                if (response.IsSuccessStatusCode)
                {
                    await Task.Delay(1000);
                    return true;
                }
            }
            catch (Exception)
            {
                attempt++;
                if (attempt < maxAttempts)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(delay.TotalMilliseconds * 1.5));
                }
            }
        }

        return false;
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task Connect_WithoutAuth_Succeeds()
    {
        // Arrange
        await WaitForServerReady();

        // Act
        var wsUri = new Uri($"http://localhost:5001/ws/lofi".Replace("http://", "ws://"));
        _webSocket = await _wsClient.ConnectAsync(wsUri, _cts.Token);

        // Assert
        Assert.Equal(WebSocketState.Open, _webSocket.State);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task PlayCommand_SendsCorrectResponse()
    {
        // Arrange
        await ConnectToServer();

        var command = new WebSocketMessage(
            Core.WebSocket.WebSocketMessageType.Command,
            WebSocketActions.Commands.Play,
            new PlayCommandPayload("jazzy", 90, "immediate", 2.0));

        // Act
        await SendCommandAsync(command);
        var response = await ReceiveMessageAsync();

        // Assert
        Assert.NotNull(response);
        Assert.Equal(WebSocketActions.Events.PlaybackStarted, response.Action);
        var payload = JsonSerializer.Deserialize<PlaybackStartedPayload>(
            response.Payload?.ToString() ?? "{}");
        Assert.NotNull(payload);
        Assert.Equal("jazzy", payload.Style);
        Assert.Equal(90, payload.Bpm);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task StopCommand_SendsCorrectResponse()
    {
        // Arrange
        await ConnectToServer();

        // First start playback
        var playCommand = new WebSocketMessage(
            Core.WebSocket.WebSocketMessageType.Command,
            WebSocketActions.Commands.Play,
            new PlayCommandPayload());
        await SendCommandAsync(playCommand);
        await ReceiveMessageAsync(); // Consume play response

        // Act - Send stop command
        var stopCommand = new WebSocketMessage(
            Core.WebSocket.WebSocketMessageType.Command,
            WebSocketActions.Commands.Stop,
            new StopCommandPayload(true));
        await SendCommandAsync(stopCommand);
        var response = await ReceiveMessageAsync();

        // Assert
        Assert.NotNull(response);
        Assert.Equal(WebSocketActions.Events.PlaybackStopped, response.Action);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task VolumeCommand_UpdatesVolumeAndBroadcasts()
    {
        // Arrange
        await ConnectToServer();

        var command = new WebSocketMessage(
            Core.WebSocket.WebSocketMessageType.Command,
            WebSocketActions.Commands.Volume,
            new { level = 0.5f });

        // Act
        await SendCommandAsync(command);
        var response = await ReceiveMessageAsync();

        // Assert
        Assert.NotNull(response);
        Assert.Equal(WebSocketActions.Events.VolumeChanged, response.Action);
        var payload = JsonSerializer.Deserialize<VolumeChangedPayload>(
            response.Payload?.ToString() ?? "{}");
        Assert.NotNull(payload);
        Assert.Equal(0.5f, payload.NewVolume);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public async Task InvalidCommand_ReturnsError()
    {
        // Arrange
        await ConnectToServer();

        var command = new WebSocketMessage(
            Core.WebSocket.WebSocketMessageType.Command,
            "invalid-command",
            new { });

        // Act
        await SendCommandAsync(command);
        var response = await ReceiveMessageAsync();

        // Assert
        Assert.NotNull(response);
        Assert.Equal(WebSocketActions.Errors.UnknownCommand, response.Action);
    }

    private async Task ConnectToServer()
    {
        await WaitForServerReady();

        var wsUri = new Uri($"http://localhost:5001/ws/lofi".Replace("http://", "ws://"));
        _webSocket = await _wsClient.ConnectAsync(wsUri, _cts.Token);
        Assert.Equal(WebSocketState.Open, _webSocket.State);
    }

    private async Task SendCommandAsync(WebSocketMessage command)
    {
        if (_webSocket == null) throw new InvalidOperationException("WebSocket not connected");
        var json = JsonSerializer.Serialize(command);
        var buffer = Encoding.UTF8.GetBytes(json);
        await _webSocket.SendAsync(
            new ArraySegment<byte>(buffer),
            System.Net.WebSockets.WebSocketMessageType.Text,
            true,
            _cts.Token);
    }

    private async Task<WebSocketMessage> ReceiveMessageAsync()
    {
        if (_webSocket == null) throw new InvalidOperationException("WebSocket not connected");
        var buffer = new byte[32768];
        var result = await _webSocket.ReceiveAsync(
            new ArraySegment<byte>(buffer),
            _cts.Token);

        var json = Encoding.UTF8.GetString(buffer, 0, result.Count);
        return JsonSerializer.Deserialize<WebSocketMessage>(json)
            ?? throw new JsonException("Failed to deserialize WebSocket message");
    }

    public void Dispose()
    {
        _webSocket?.Dispose();
        _cts.Dispose();
    }
} 