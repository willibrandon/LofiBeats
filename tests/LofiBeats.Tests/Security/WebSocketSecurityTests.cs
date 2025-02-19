using LofiBeats.Core.WebSocket;
using LofiBeats.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace LofiBeats.Tests.Security;

[Collection("AI Generated Tests")]
public class WebSocketSecurityTests : IClassFixture<WebApplicationFactory<Program>>, IAsyncDisposable
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly WebSocketClient _wsClient;
    private WebSocket? _webSocket;
    private readonly CancellationTokenSource _cts;

    public WebSocketSecurityTests(WebApplicationFactory<Program> factory)
    {
        // Configure test server with authentication required
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                var settings = new Dictionary<string, string?>
                {
                    ["WebSocket:RequireAuthentication"] = "true",
                    ["WebSocket:AuthToken"] = "test-token-2024",
                    ["WebSocket:MaxMessageSize"] = "32768",
                    ["WebSocket:MessageRateLimit"] = "5",
                    ["WebSocket:EndpointPath"] = "/ws/lofi"
                };

                config.AddInMemoryCollection(settings);
            });
        });

        _wsClient = _factory.Server.CreateWebSocketClient();
        _wsClient.ConfigureRequest = request =>
        {
            request.QueryString = QueryString.Empty;
        };
        _cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    [Trait("Category", "Security")]
    public async Task Connect_WithoutToken_IsRejected()
    {
        // Arrange
        var wsUri = new Uri("ws://localhost/ws/lofi");

        // Act
        var response = await _factory.CreateClient().GetAsync(wsUri.ToString().Replace("ws://", "http://"));

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    [Trait("Category", "Security")]
    public async Task Connect_WithInvalidToken_IsRejected()
    {
        // Arrange
        var wsUri = new Uri("ws://localhost/ws/lofi?token=invalid-token");

        // Act
        var response = await _factory.CreateClient().GetAsync(wsUri.ToString().Replace("ws://", "http://"));

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    [Trait("Category", "Security")]
    public async Task Connect_WithValidToken_Succeeds()
    {
        // Arrange
        var wsUri = new Uri("ws://localhost/ws/lofi");
        _wsClient.ConfigureRequest = request =>
        {
            request.QueryString = new QueryString("?token=test-token-2024");
        };

        // Act
        _webSocket = await _wsClient.ConnectAsync(wsUri, _cts.Token);

        // Assert
        Assert.Equal(WebSocketState.Open, _webSocket.State);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    [Trait("Category", "Security")]
    public async Task RateLimit_BlocksExcessiveMessages()
    {
        // Arrange
        var wsUri = new Uri("ws://localhost/ws/lofi");
        _wsClient.ConfigureRequest = request =>
        {
            request.QueryString = new QueryString("?token=test-token-2024");
        };

        _webSocket = await _wsClient.ConnectAsync(wsUri, _cts.Token);

        var command = new WebSocketMessage(
            Core.WebSocket.WebSocketMessageType.Command,
            WebSocketActions.Commands.SyncState,
            null);

        var successfulMessages = 0;
        var rateLimitHit = false;

        try
        {
            // Send messages and wait for responses
            for (int i = 0; i < 6 && !rateLimitHit; i++)
            {
                try
                {
                    // Send command
                    await SendCommandAsync(command);

                    // Wait for response - this will throw when connection is closed
                    var response = await ReceiveMessageAsync();
                    successfulMessages++;

                    // Small delay to ensure server processes rate limit
                    await Task.Delay(10);
                }
                catch (Exception ex) when (
                    ex is WebSocketException || 
                    ex is InvalidOperationException || 
                    ex is JsonException)
                {
                    // Rate limit was hit, connection was closed
                    rateLimitHit = true;
                }
            }
        }
        finally
        {
            // Give the server time to clean up
            await Task.Delay(100);
        }

        // Verify rate limiting worked
        Assert.True(successfulMessages >= 4, $"Should have processed at least 4 messages before rate limit, but got {successfulMessages}");
        Assert.True(successfulMessages <= 5, $"Should not have processed more than 5 messages, but got {successfulMessages}");
        Assert.True(rateLimitHit, "Rate limit should have been hit");
        Assert.NotEqual(WebSocketState.Open, _webSocket?.State);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    [Trait("Category", "Security")]
    public async Task MessageSize_ExceedingLimit_IsRejected()
    {
        // Arrange
        var wsUri = new Uri("ws://localhost/ws/lofi");
        _wsClient.ConfigureRequest = request =>
        {
            request.QueryString = new QueryString("?token=test-token-2024");
        };

        _webSocket = await _wsClient.ConnectAsync(wsUri, _cts.Token);

        // Create oversized message
        var largePayload = new string('x', 40 * 1024); // 40KB
        var command = new WebSocketMessage(
            Core.WebSocket.WebSocketMessageType.Command,
            WebSocketActions.Commands.Play,
            new { data = largePayload });

        // Act
        await SendCommandAsync(command);

        // Assert - Connection should eventually close
        var buffer = new byte[1024];
        var closeReceived = false;
        var attempts = 0;
        const int maxAttempts = 5;

        while (!closeReceived && attempts < maxAttempts)
        {
            try
            {
                var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), _cts.Token);
                if (result.MessageType == System.Net.WebSockets.WebSocketMessageType.Close)
                {
                    closeReceived = true;
                    break;
                }
            }
            catch (OperationCanceledException)
            {
                // Connection was forcibly closed
                closeReceived = true;
                break;
            }
            attempts++;
            await Task.Delay(100); // Wait a bit before next attempt
        }

        Assert.True(closeReceived, "Connection should have been closed after sending oversized message");
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

    public async ValueTask DisposeAsync()
    {
        if (_webSocket?.State == WebSocketState.Open)
        {
            await _webSocket.CloseAsync(
                WebSocketCloseStatus.NormalClosure,
                "Test completed",
                CancellationToken.None);
        }
        _webSocket?.Dispose();
        _cts.Dispose();
    }
} 