using LofiBeats.Core.BeatGeneration;
using LofiBeats.Core.Effects;
using LofiBeats.Core.Models;
using LofiBeats.Core.Playback;
using LofiBeats.Core.PluginManagement;
using LofiBeats.Core.Scheduling;
using LofiBeats.Core.Telemetry;
using LofiBeats.Core.WebSocket;
using LofiBeats.Service.WebSocket;
using Microsoft.AspNetCore.Http.Json;
using System.Buffers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LofiBeats.Service;

public partial class Program
{
    // Singleton JsonSerializerOptions for reuse
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = false // Optimize for performance over readability
    };

    // High-performance structured logging using LoggerMessage.Define
    private static readonly Action<ILogger, Exception?> _logUnhandledException =
        LoggerMessage.Define(LogLevel.Critical, new EventId(1000, "UnhandledException"),
            "Unhandled exception occurred in application");

    private static readonly Action<ILogger, bool, string?, Exception?> _logTelemetryConfig =
        LoggerMessage.Define<bool, string?>(LogLevel.Information, new EventId(1001, "TelemetryConfig"),
            "Telemetry configuration: Seq enabled: {SeqEnabled}, Server URL: {SeqUrl}");

    private static readonly Action<ILogger, Exception?> _logNoTelemetryConfig =
        LoggerMessage.Define(LogLevel.Warning, new EventId(1002, "NoTelemetryConfig"),
            "No telemetry configuration found, using defaults");

    private static readonly Action<ILogger, Exception> _logWebSocketError =
        LoggerMessage.Define(LogLevel.Error, new EventId(100, "WebSocketError"),
            "Error handling WebSocket connection");

    private static readonly Action<ILogger, Exception?> _logWebSocketRequest =
        LoggerMessage.Define(LogLevel.Information, new EventId(101, "WebSocketRequest"),
            "Received WebSocket request");

    private static readonly Action<ILogger, Exception?> _logWebSocketUpgraded =
        LoggerMessage.Define(LogLevel.Information, new EventId(102, "WebSocketUpgraded"),
            "WebSocket connection upgraded successfully");

    private static readonly Action<ILogger, string, Exception?> _logWebSocketHeaders =
        LoggerMessage.Define<string>(LogLevel.Debug, new EventId(103, "WebSocketHeaders"),
            "WebSocket request headers: {Headers}");

    static Program()
    {
        // Set up global exception handling with structured logging
        AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
        {
            var ex = args.ExceptionObject as Exception;
            var logger = LoggerFactory.Create(logging => logging.AddConsole()).CreateLogger("GlobalExceptionHandler");
            _logUnhandledException(logger, ex);

            // Ensure the error is written to console in case logging fails
            Console.Error.WriteLine($"FATAL ERROR: {ex?.Message}");
            Console.Error.WriteLine(ex?.StackTrace);

            Environment.Exit(1);
        };

        // Optimize thread pool settings for audio processing
        ThreadPool.SetMinThreads(Environment.ProcessorCount * 2, Environment.ProcessorCount);
    }

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configure services with minimal startup work
        builder.Services.AddOpenApi();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddHealthChecks();

        // Configure telemetry with structured logging
        var telemetryConfig = builder.Configuration.GetSection("Telemetry").Get<TelemetryConfiguration>();
        var logger = LoggerFactory.Create(logging => logging.AddConsole()).CreateLogger("Program");

        if (telemetryConfig != null)
        {
            _logTelemetryConfig(logger, telemetryConfig.EnableSeq, telemetryConfig.SeqServerUrl, null);
            builder.Services.AddLofiTelemetry(telemetryConfig);
        }
        else
        {
            _logNoTelemetryConfig(logger, null);
            builder.Services.AddLofiTelemetry(new TelemetryConfiguration
            {
                EnableLocalFile = true,
                EnableSeq = false
            });
        }

        // Configure resource pooling for audio buffers
        builder.Services.AddSingleton(ArrayPool<byte>.Shared);

        // Register core services as singletons for performance
        builder.Services.AddSingleton<IAudioPlaybackService, AudioPlaybackService>();
        builder.Services.AddSingleton<IBeatGeneratorFactory, BeatGeneratorFactory>();
        builder.Services.AddSingleton<IPluginLoader, PluginLoader>();
        builder.Services.AddSingleton<PluginManager>();
        builder.Services.AddSingleton<PluginWatcher>();
        builder.Services.AddSingleton<IEffectFactory, EffectFactory>();
        builder.Services.AddSingleton<PlaybackScheduler>();
        builder.Services.AddSingleton<UserSampleRepository>();
        builder.Services.AddSingleton<TelemetryTracker>();
        builder.Services.AddSingleton<IWebSocketBroadcaster, WebSocketHandler>();
        builder.Services.AddSingleton<IWebSocketHandler, WebSocketHandler>();
        builder.Services.AddSingleton<WebSocketHandler>();
        builder.Services.AddHostedService<RealTimeMetricsService>();
        builder.Services.AddHostedService<PluginWatcherService>();

        // Configure JSON options for better performance
        builder.Services.Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.SerializerOptions.WriteIndented = false;
        });

        // Add HttpContextAccessor for WebSocket authentication
        builder.Services.AddHttpContextAccessor();

        // Add WebSocket configuration
        builder.Services.Configure<WebSocketConfiguration>(builder.Configuration.GetSection("WebSocket"));

        // Register WebSocket command handler
        builder.Services.AddSingleton(sp =>
        {
            var handler = sp.GetRequiredService<WebSocketHandler>();
            return new WebSocketCommandHandler(
                sp.GetRequiredService<ILogger<WebSocketCommandHandler>>(),
                sp.GetRequiredService<IAudioPlaybackService>(),
                sp.GetRequiredService<IBeatGeneratorFactory>(),
                sp.GetRequiredService<IEffectFactory>(),
                sp.GetRequiredService<PlaybackScheduler>(),
                sp.GetRequiredService<UserSampleRepository>(),
                sp.GetRequiredService<TelemetryTracker>(),
                handler.BroadcastEventAsync
            );
        });

        var app = builder.Build();

        // Get telemetry tracker for API endpoints
        var telemetryTracker = app.Services.GetRequiredService<TelemetryTracker>();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.MapHealthChecks("/healthz");

        // Configure WebSocket options
        app.UseWebSockets(new WebSocketOptions
        {
            KeepAliveInterval = TimeSpan.FromSeconds(30)
        });

        // Add WebSocket endpoint
        app.Map("/ws/lofi", builder =>
        {
            builder.Use(async (HttpContext context, RequestDelegate next) =>
            {
                if (!context.WebSockets.IsWebSocketRequest)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync("Not a WebSocket request");
                    return;
                }

                var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
                _logWebSocketRequest(logger, null);

                // Log headers for debugging
                var headersList = context.Request.Headers.Select(h => $"{h.Key}={string.Join(";", h.Value.ToArray())}").ToArray();
                var headers = string.Join(", ", headersList);
                _logWebSocketHeaders(logger, headers, null);

                try
                {
                    using var scope = app.Services.CreateScope();
                    var handler = scope.ServiceProvider.GetRequiredService<WebSocketHandler>();
                    var socket = await context.WebSockets.AcceptWebSocketAsync();
                    _logWebSocketUpgraded(logger, null);
                    await handler.HandleClientAsync(socket, context.RequestAborted);
                }
                catch (Exception ex)
                {
                    _logWebSocketError(logger, ex);
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync("WebSocket error occurred");
                }
            });
        });

        // Define API endpoints
        var api = app.MapGroup("/api/lofi");

        // Schedule stop endpoint
        api.MapPost("/schedule-stop", async (IAudioPlaybackService playback, IEffectFactory effectFactory,
                                     PlaybackScheduler scheduler, bool tapeStop, string delay) =>
        {
            telemetryTracker.TrackEvent(TelemetryConstants.Events.PlaybackScheduled, new Dictionary<string, string>
            {
                { TelemetryConstants.Properties.StopDelay, delay },
                { TelemetryConstants.Properties.UseEffect, tapeStop.ToString() }
            });

            var timespan = DelayParser.ParseDelay(delay);
            if (timespan == null)
            {
                return Results.BadRequest(new { error = $"Invalid delay format: {delay}" });
            }

            var totalMs = (int)timespan.Value.TotalMilliseconds;
            var description = $"Stop playback{(tapeStop ? " with tape effect" : "")} at {DateTime.Now + timespan.Value:HH:mm:ss}";

            var id = await Task.Run(() => scheduler.ScheduleStopAction(totalMs, () =>
            {
                if (tapeStop)
                {
                    var currentSource = playback.CurrentSource;
                    if (currentSource != null)
                    {
                        var effect = effectFactory.CreateEffect("tapestop", currentSource);
                        playback.StopWithEffect(effect);
                    }
                    else
                    {
                        playback.StopPlayback();
                    }
                }
                else
                {
                    playback.StopPlayback();
                }
            }, description));

            return Results.Json(new { message = $"Scheduled stop in {timespan.Value.TotalSeconds:F1} seconds", actionId = id }, JsonOptions);
        });

        // Schedule play endpoint
        api.MapPost("/schedule-play", async (IBeatGeneratorFactory factory, IAudioPlaybackService playback,
                                     PlaybackScheduler scheduler, ILogger<Program> logger, UserSampleRepository userSamples,
                                     string style = "basic", string delay = "0s") =>
        {
            telemetryTracker.TrackEvent(TelemetryConstants.Events.PlaybackScheduled, new Dictionary<string, string>
            {
                { TelemetryConstants.Properties.BeatStyle, style },
                { TelemetryConstants.Properties.StopDelay, delay }
            });

            var timespan = DelayParser.ParseDelay(delay);
            if (timespan == null)
            {
                return Results.BadRequest(new { error = $"Invalid delay format: {delay}" });
            }

            var totalMs = (int)timespan.Value.TotalMilliseconds;
            var id = await Task.Run(() => scheduler.ScheduleAction(totalMs, () =>
            {
                var generator = factory.GetGenerator(style);
                var pattern = generator.GeneratePattern();
                var beatSource = new BeatPatternSampleProvider(pattern, logger, userSamples, telemetryTracker);
                playback.SetSource(beatSource);
                playback.CurrentStyle = style;
                playback.StartPlayback();
            }));

            return Results.Ok(new { message = $"Scheduled {style} beat to play in {timespan.Value.TotalSeconds:F1} seconds", actionId = id });
        });

        // Generate endpoint
        api.MapPost("/generate", (IBeatGeneratorFactory factory, string style = "basic", int? bpm = null, string? key = null) =>
        {
            telemetryTracker.TrackEvent(TelemetryConstants.Events.BeatGenerated, new Dictionary<string, string>
            {
                { TelemetryConstants.Properties.BeatStyle, style },
                { TelemetryConstants.Properties.BeatTempo, bpm?.ToString() ?? "default" }
            });

            // Validate and normalize key
            string normalizedKey = "C"; // Default key
            if (!string.IsNullOrEmpty(key))
            {
                if (!KeyHelper.IsValidKey(key, out var validKey))
                {
                    return Results.Json(new { error = $"Invalid key '{key}'" }, JsonOptions, statusCode: 400);
                }
                normalizedKey = validKey;
            }

            var generator = factory.GetGenerator(style);
            var pattern = generator.GeneratePattern(bpm, normalizedKey);

            return Results.Json(new { message = "Pattern generated", pattern }, JsonOptions);
        });

        // Play endpoint
        api.MapPost("/play", async (HttpContext context, IBeatGeneratorFactory factory, IAudioPlaybackService playback, 
            UserSampleRepository userSamples, TelemetryTracker telemetryTracker) =>
        {
            var style = context.Request.Query["style"].FirstOrDefault() ?? "basic";
            var bpmStr = context.Request.Query["bpm"].FirstOrDefault();
            var key = context.Request.Query["key"].FirstOrDefault();
            var transition = context.Request.Query["transition"].FirstOrDefault() ?? "immediate";
            var xfadeDurationStr = context.Request.Query["xfadeDuration"].FirstOrDefault();

            telemetryTracker.TrackEvent(TelemetryConstants.Events.PlaybackStarted, new Dictionary<string, string>
            {
                { TelemetryConstants.Properties.BeatStyle, style },
                { TelemetryConstants.Properties.BeatTempo, bpmStr ?? "default" }
            });

            // Validate and normalize key
            string normalizedKey = "C"; // Default key
            if (!string.IsNullOrEmpty(key))
            {
                if (!KeyHelper.IsValidKey(key, out var validKey))
                {
                    return Results.Json(new { error = $"Invalid key '{key}'" }, JsonOptions, statusCode: 400);
                }
                normalizedKey = validKey;
            }

            // Parse BPM if provided
            int? bpm = null;
            if (!string.IsNullOrEmpty(bpmStr) && int.TryParse(bpmStr, out var parsedBpm))
            {
                if (parsedBpm < 60 || parsedBpm > 140)
                {
                    return Results.Json(new { error = "BPM must be between 60 and 140" }, JsonOptions, statusCode: 400);
                }
                bpm = parsedBpm;
            }

            // Parse crossfade duration if provided
            double xfadeDuration = 2.0;
            if (!string.IsNullOrEmpty(xfadeDurationStr) && double.TryParse(xfadeDurationStr, out var parsedDuration))
            {
                if (parsedDuration < 0.1 || parsedDuration > 10.0)
                {
                    return Results.Json(new { error = "Crossfade duration must be between 0.1 and 10.0 seconds" }, JsonOptions, statusCode: 400);
                }
                xfadeDuration = parsedDuration;
            }

            var generator = factory.GetGenerator(style);
            if (bpm.HasValue) generator.SetBPM(bpm.Value);

            var pattern = await Task.Run(() => generator.GeneratePattern(bpm, normalizedKey));

            if (transition == "crossfade")
            {
                playback.CrossfadeToPattern(pattern, (float)xfadeDuration);
            }
            else
            {
                var provider = new BeatPatternSampleProvider(
                    pattern,
                    app.Services.GetRequiredService<ILogger<BeatPatternSampleProvider>>(),
                    userSamples,
                    telemetryTracker);
                playback.SetSource(provider);
            }

            playback.CurrentStyle = style;
            playback.StartPlayback();

            return Results.Json(new { message = "Playback started", pattern }, JsonOptions);
        });

        // Stop endpoint
        api.MapPost("/stop", (IAudioPlaybackService playback, IEffectFactory effectFactory, bool tapestop = false) =>
        {
            telemetryTracker.TrackEvent(TelemetryConstants.Events.PlaybackStopped, new Dictionary<string, string>
            {
                { "UseEffect", tapestop.ToString() }
            });

            if (tapestop)
            {
                var currentSource = playback.CurrentSource;
                if (currentSource != null)
                {
                    var effect = effectFactory.CreateEffect("tapestop", currentSource);
                    playback.StopWithEffect(effect);
                }
                else
                {
                    playback.StopPlayback();
                }
            }
            else
            {
                playback.StopPlayback();
            }

            return Results.Json(new { message = "Playback stopped" }, JsonOptions);
        });

        // Pause endpoint
        api.MapPost("/pause", (IAudioPlaybackService playback) =>
        {
            var state = playback.GetPlaybackState();
            if (state != NAudio.Wave.PlaybackState.Playing)
            {
                return Results.Json(new { error = "No active playback to pause" }, JsonOptions, statusCode: 400);
            }

            telemetryTracker.TrackEvent(TelemetryConstants.Events.PlaybackPaused);
            playback.PausePlayback();
            return Results.Json(new { message = "Playback paused" }, JsonOptions);
        });

        // Resume endpoint
        api.MapPost("/resume", (IAudioPlaybackService playback) =>
        {
            var state = playback.GetPlaybackState();
            if (state != NAudio.Wave.PlaybackState.Paused)
            {
                return Results.Json(new { error = "Playback is not paused" }, JsonOptions, statusCode: 400);
            }

            telemetryTracker.TrackEvent(TelemetryConstants.Events.PlaybackResumed);
            playback.ResumePlayback();
            return Results.Json(new { message = "Playback resumed" }, JsonOptions);
        });

        // Volume endpoint
        api.MapPost("/volume", (IAudioPlaybackService playback, float level) =>
        {
            if (level < 0 || level > 1)
                return Results.Json(new { error = "Volume must be between 0.0 and 1.0" }, JsonOptions, statusCode: 400);

            telemetryTracker.TrackMetric("Audio.Volume", level);
            playback.SetVolume(level);
            return Results.Json(new { message = $"Volume set to {level:F2}" }, JsonOptions);
        });

        // Effect endpoint
        api.MapPost("/effect", (IAudioPlaybackService playback, IEffectFactory effectFactory, PluginManager pluginManager, string name, bool enable) =>
        {
            if (enable)
            {
                var currentSource = playback.CurrentSource;
                if (currentSource == null)
                    return Results.Json(new { error = "No audio source is currently playing" }, JsonOptions, statusCode: 400);

                // Try creating the effect (will work for both built-in and plugin effects)
                var effect = effectFactory.CreateEffect(name, currentSource) ?? 
                           pluginManager.CreateEffect(name, currentSource);

                if (effect == null)
                    return Results.Json(new { error = $"Unknown effect: {name}" }, JsonOptions, statusCode: 400);

                telemetryTracker.TrackEvent(TelemetryConstants.Events.EffectAdded, new Dictionary<string, string>
                {
                    { TelemetryConstants.Properties.EffectName, name }
                });

                playback.AddEffect(effect);
                return Results.Json(new { message = $"{name} effect enabled" }, JsonOptions);
            }
            else
            {
                telemetryTracker.TrackEvent(TelemetryConstants.Events.EffectRemoved, new Dictionary<string, string>
                {
                    { TelemetryConstants.Properties.EffectName, name }
                });

                playback.RemoveEffect(name);
                return Results.Json(new { message = $"{name} effect disabled" }, JsonOptions);
            }
        });

        // List effects endpoint
        api.MapGet("/effect/list", (PluginManager pluginManager) =>
        {
            var pluginEffects = pluginManager.GetEffectNames()
                .Select(name => new
                {
                    Name = name,
                    Description = "Plugin effect",
                    Version = "1.0.0",
                    Author = "Plugin Author"
                })
                .ToArray();

            return Results.Json(pluginEffects, JsonOptions);
        });

        // Get current preset endpoint
        api.MapGet("/preset/current", (IAudioPlaybackService playback) =>
        {
            var preset = playback.GetCurrentPreset();
            return Results.Json(preset, JsonOptions);
        });

        // Apply preset endpoint
        api.MapPost("/preset/apply", (IAudioPlaybackService playback, IEffectFactory effectFactory, Preset preset) =>
        {
            try
            {
                preset.Validate();

                telemetryTracker.TrackEvent(TelemetryConstants.Events.PresetLoaded, new Dictionary<string, string>
                {
                    { TelemetryConstants.Properties.PreferredBeatStyle, preset.Style },
                    { TelemetryConstants.Properties.PreferredEffects, string.Join(",", preset.Effects) }
                });

                playback.ApplyPreset(preset, effectFactory);
                return Results.Json(new { message = $"Preset '{preset.Name}' applied successfully" }, JsonOptions);
            }
            catch (ArgumentException ex)
            {
                return Results.Json(new { error = ex.Message }, JsonOptions, statusCode: 400);
            }
        });

        // Shutdown endpoint
        api.MapPost("/shutdown", async () =>
        {
            telemetryTracker.TrackEvent(TelemetryConstants.Events.ApplicationStopped);
            await telemetryTracker.TrackApplicationStop();

            _ = Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(500);
                    Environment.Exit(0);
                }
                catch
                {
                    Environment.Exit(1);
                }
            });

            return Results.Json(new { message = "Service shutting down..." }, JsonOptions);
        });

        // Configure to run on port 5001 since 5000 is used by ControlCenter
        app.Urls.Add("http://localhost:5001");

        app.Run();
    }
}

// Make the Program class public for testing
public partial class Program { }
