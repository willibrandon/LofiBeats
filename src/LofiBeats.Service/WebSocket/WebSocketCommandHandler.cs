using LofiBeats.Core.BeatGeneration;
using LofiBeats.Core.Effects;
using LofiBeats.Core.Playback;
using LofiBeats.Core.Scheduling;
using LofiBeats.Core.Telemetry;
using LofiBeats.Core.WebSocket;
using System.Text.Json;

namespace LofiBeats.Service.WebSocket;

/// <summary>
/// Handles processing of WebSocket commands by routing them to appropriate services.
/// </summary>
public sealed class WebSocketCommandHandler
{
    private readonly ILogger<WebSocketCommandHandler> _logger;
    private readonly IAudioPlaybackService _playback;
    private readonly IBeatGeneratorFactory _beatFactory;
    private readonly IEffectFactory _effectFactory;
    private readonly PlaybackScheduler _scheduler;
    private readonly UserSampleRepository _userSamples;
    private readonly TelemetryTracker _telemetry;
    private readonly Func<string, object, CancellationToken, Task> _broadcast;

    private static readonly Action<ILogger, string, string, Exception?> _logCommandReceived =
        LoggerMessage.Define<string, string>(LogLevel.Debug, new EventId(1, "CommandReceived"),
            "Received WebSocket command: {Action} with payload: {Payload}");

    private static readonly Action<ILogger, string, Exception?> _logUnknownCommand =
        LoggerMessage.Define<string>(LogLevel.Warning, new EventId(2, "UnknownCommand"),
            "Unknown WebSocket command: {Action}");

    private static readonly Action<ILogger, string, string, Exception?> _logInvalidPayload =
        LoggerMessage.Define<string, string>(LogLevel.Warning, new EventId(3, "InvalidPayload"),
            "Invalid payload for command {Action}: {Error}");

    private static readonly Action<ILogger, string, Exception> _logCommandError =
        LoggerMessage.Define<string>(LogLevel.Error, new EventId(4, "CommandError"),
            "Error processing command {Action}");

    public WebSocketCommandHandler(
        ILogger<WebSocketCommandHandler> logger,
        IAudioPlaybackService playback,
        IBeatGeneratorFactory beatFactory,
        IEffectFactory effectFactory,
        PlaybackScheduler scheduler,
        UserSampleRepository userSamples,
        TelemetryTracker telemetry,
        Func<string, object, CancellationToken, Task> broadcast)
    {
        _logger = logger;
        _playback = playback;
        _beatFactory = beatFactory;
        _effectFactory = effectFactory;
        _scheduler = scheduler;
        _userSamples = userSamples;
        _telemetry = telemetry;
        _broadcast = broadcast;
    }

    /// <summary>
    /// Handles a WebSocket command message.
    /// </summary>
    /// <param name="clientId">The ID of the client sending the command</param>
    /// <param name="action">The command action</param>
    /// <param name="payload">The command payload</param>
    /// <param name="cancellationToken">Token to cancel the operation</param>
    public async Task HandleCommandAsync(Guid clientId, string action, JsonElement payload, 
        CancellationToken cancellationToken)
    {
        _logCommandReceived(_logger, action, payload.ToString(), null);

        try
        {
            switch (action)
            {
                case WebSocketActions.Commands.Play:
                    await HandlePlayCommandAsync(payload, cancellationToken);
                    break;

                case WebSocketActions.Commands.Stop:
                    await HandleStopCommandAsync(payload, cancellationToken);
                    break;

                case WebSocketActions.Commands.Volume:
                    await HandleVolumeCommandAsync(payload, cancellationToken);
                    break;

                case WebSocketActions.Commands.AddEffect:
                    await HandleAddEffectCommandAsync(payload, cancellationToken);
                    break;

                case WebSocketActions.Commands.RemoveEffect:
                    await HandleRemoveEffectCommandAsync(payload, cancellationToken);
                    break;

                case WebSocketActions.Commands.SyncState:
                    await HandleSyncStateCommandAsync(cancellationToken);
                    break;

                case WebSocketActions.Commands.Ping:
                    await HandlePingCommandAsync(clientId, cancellationToken);
                    break;

                default:
                    _logUnknownCommand(_logger, action, null);
                    await SendErrorAsync(clientId, WebSocketActions.Errors.UnknownCommand,
                        $"Unknown command: {action}", cancellationToken);
                    break;
            }
        }
        catch (JsonException ex)
        {
            _logInvalidPayload(_logger, action, ex.Message, null);
            await SendErrorAsync(clientId, WebSocketActions.Errors.InvalidPayload,
                $"Invalid payload for {action}: {ex.Message}", cancellationToken);
        }
        catch (Exception ex)
        {
            _logCommandError(_logger, action, ex);
            await SendErrorAsync(clientId, "command-error",
                $"Error processing {action}: {ex.Message}", cancellationToken);
        }
    }

    private async Task HandlePlayCommandAsync(JsonElement payload, CancellationToken cancellationToken)
    {
        var command = JsonSerializer.Deserialize<PlayCommandPayload>(payload.GetRawText())
            ?? throw new JsonException("Invalid play command payload");

        // Validate and normalize key if provided
        string normalizedKey = "C"; // Default key
        if (!string.IsNullOrEmpty(command.Key))
        {
            if (!KeyHelper.IsValidKey(command.Key, out var validKey))
            {
                throw new ArgumentException($"Invalid key '{command.Key}'");
            }
            normalizedKey = validKey;
        }

        var generator = _beatFactory.GetGenerator(command.Style);
        var pattern = generator.GeneratePattern(command.Bpm);
        pattern.Key = normalizedKey;

        // TODO: Transposition logic will be added in a later chunk

        if (command.Transition == "crossfade")
        {
            _playback.CrossfadeToPattern(pattern, (float)command.XfadeDuration);
        }
        else
        {
            var provider = new BeatPatternSampleProvider(pattern, _logger, _userSamples, _telemetry);
            _playback.SetSource(provider);
        }

        _playback.CurrentStyle = command.Style;
        _playback.StartPlayback();

        // Broadcast playback started event with key
        await _broadcast(
            WebSocketActions.Events.PlaybackStarted,
            new PlaybackStartedPayload(command.Style, pattern.BPM, pattern.Key),
            cancellationToken);
    }

    private async Task HandleStopCommandAsync(JsonElement payload, CancellationToken cancellationToken)
    {
        var command = JsonSerializer.Deserialize<StopCommandPayload>(payload.GetRawText())
            ?? throw new JsonException("Invalid stop command payload");

        if (command.TapeStop)
        {
            var currentSource = _playback.CurrentSource;
            if (currentSource != null)
            {
                var effect = _effectFactory.CreateEffect("tapestop", currentSource);
                _playback.StopWithEffect(effect);
            }
            else
            {
                _playback.StopPlayback();
            }
        }
        else
        {
            _playback.StopPlayback();
        }

        // Broadcast playback stopped event
        await _broadcast(
            WebSocketActions.Events.PlaybackStopped,
            new { tapestop = command.TapeStop },
            cancellationToken);
    }

    private async Task HandleVolumeCommandAsync(JsonElement payload, CancellationToken cancellationToken)
    {
        var level = payload.TryGetProperty("level", out var lv) ? (float)lv.GetDouble() : 1.0f;
        if (level < 0f || level > 1f)
        {
            throw new ArgumentException("Volume level must be between 0.0 and 1.0");
        }

        _playback.SetVolume(level);

        // Broadcast volume changed event
        await _broadcast(
            WebSocketActions.Events.VolumeChanged,
            new VolumeChangedPayload(level),
            cancellationToken);
    }

    private async Task HandleAddEffectCommandAsync(JsonElement payload, CancellationToken cancellationToken)
    {
        var name = payload.GetProperty("name").GetString()
            ?? throw new JsonException("Effect name is required");

        var currentSource = _playback.CurrentSource
            ?? throw new InvalidOperationException("No audio source is currently playing");

        var effect = _effectFactory.CreateEffect(name, currentSource)
            ?? throw new ArgumentException($"Unknown effect: {name}");

        _playback.AddEffect(effect);

        // Broadcast effect added event
        await _broadcast(
            WebSocketActions.Events.EffectAdded,
            new { name },
            cancellationToken);
    }

    private async Task HandleRemoveEffectCommandAsync(JsonElement payload, CancellationToken cancellationToken)
    {
        var name = payload.GetProperty("name").GetString()
            ?? throw new JsonException("Effect name is required");

        _playback.RemoveEffect(name);

        // Broadcast effect removed event
        await _broadcast(
            WebSocketActions.Events.EffectRemoved,
            new { name },
            cancellationToken);
    }

    private async Task HandleSyncStateCommandAsync(CancellationToken cancellationToken)
    {
        var state = new
        {
            isPlaying = _playback.GetPlaybackState() == NAudio.Wave.PlaybackState.Playing,
            style = _playback.CurrentStyle,
            volume = _playback.CurrentVolume,
            effects = _playback.GetCurrentPreset().Effects
        };

        // Send current state as a sync event
        await _broadcast(
            "sync-state",
            state,
            cancellationToken);
    }

    private async Task HandlePingCommandAsync(Guid clientId, CancellationToken cancellationToken)
    {
        // Simply respond with a pong event
        await _broadcast(
            WebSocketActions.Events.Pong,
            new { timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() },
            cancellationToken);
    }

    private async Task SendErrorAsync(Guid clientId, string errorAction, string message, 
        CancellationToken cancellationToken)
    {
        await _broadcast(
            errorAction,
            new ErrorPayload(message),
            cancellationToken);
    }
} 