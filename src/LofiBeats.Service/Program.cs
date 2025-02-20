using LofiBeats.Core.BeatGeneration;
using LofiBeats.Core.Effects;
using LofiBeats.Core.Models;
using LofiBeats.Core.Playback;
using LofiBeats.Core.Scheduling;
using LofiBeats.Core.Telemetry;
using LofiBeats.Core.WebSocket;
using LofiBeats.Service.WebSocket;
using System.Text.Json;

namespace LofiBeats.Service;

public partial class Program
{
    static Program()
    {
        // Set up global exception handling
        AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
        {
            var ex = args.ExceptionObject as Exception;
            var logger = LoggerFactory.Create(logging => logging.AddConsole()).CreateLogger("GlobalExceptionHandler");
            LoggerMessages.LogUnhandledException(logger, ex?.Message, ex);
            
            // Ensure the error is written to console in case logging fails
            Console.Error.WriteLine($"FATAL ERROR: {ex?.Message}");
            Console.Error.WriteLine(ex?.StackTrace);
            
            // Force exit with error code
            Environment.Exit(1);
        };
    }

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

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddHealthChecks();

        // Configure telemetry from settings
        var telemetryConfig = builder.Configuration.GetSection("Telemetry").Get<TelemetryConfiguration>();
        if (telemetryConfig != null)
        {
            builder.Services.AddLogging(logging =>
            {
                logging.AddConsole();
                logging.AddDebug();
            });
            var logger = LoggerFactory.Create(logging => logging.AddConsole()).CreateLogger("Program");
            LoggerMessages.LogTelemetryConfig(logger, telemetryConfig.EnableSeq, telemetryConfig.SeqServerUrl, null);

            builder.Services.AddLofiTelemetry(telemetryConfig);
        }
        else
        {
            var logger = LoggerFactory.Create(logging => logging.AddConsole()).CreateLogger("Program");
            LoggerMessages.LogNoTelemetryConfig(logger, null);

            // Fallback to default configuration if settings are missing
            builder.Services.AddLofiTelemetry(new TelemetryConfiguration
            {
                EnableLocalFile = true,
                EnableSeq = false // Disable Seq by default if no configuration is provided
            });
        }

        // Register our core services as singletons
        builder.Services.AddSingleton<IAudioPlaybackService, AudioPlaybackService>();
        builder.Services.AddSingleton<IBeatGeneratorFactory, BeatGeneratorFactory>();
        builder.Services.AddSingleton<IEffectFactory, EffectFactory>();
        builder.Services.AddSingleton<PlaybackScheduler>();
        builder.Services.AddSingleton<UserSampleRepository>();
        builder.Services.AddSingleton<TelemetryTracker>();

        // Add HttpContextAccessor for WebSocket authentication
        builder.Services.AddHttpContextAccessor();

        // Add WebSocket configuration
        builder.Services.Configure<WebSocketConfiguration>(builder.Configuration.GetSection("WebSocket"));

        // Register WebSocket services
        builder.Services.AddSingleton<IWebSocketBroadcaster, WebSocketHandler>();
        builder.Services.AddSingleton<IWebSocketHandler, WebSocketHandler>();
        builder.Services.AddSingleton<WebSocketHandler>();
        builder.Services.AddHostedService<RealTimeMetricsService>();
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

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        // Add health check endpoint
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

        // Define our API endpoints
        var api = app.MapGroup("/api/lofi");

        // Schedule stop endpoint
        api.MapPost("/schedule-stop", (IAudioPlaybackService playback, IEffectFactory effectFactory, 
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

            // Schedule the stop
            var totalMs = (int)timespan.Value.TotalMilliseconds;
            var description = $"Stop playback{(tapeStop ? " with tape effect" : "")} at {DateTime.Now + timespan.Value:HH:mm:ss}";
            var id = scheduler.ScheduleStopAction(totalMs, () =>
            {
                // In callback: call the service's stop logic
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
            }, description);

            return Results.Ok(new { message = $"Scheduled stop in {timespan.Value.TotalSeconds:F1} seconds", actionId = id });
        });

        // Schedule play endpoint
        api.MapPost("/schedule-play", (IBeatGeneratorFactory factory, IAudioPlaybackService playback,
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

            // Schedule the play action
            var totalMs = (int)timespan.Value.TotalMilliseconds;
            var id = scheduler.ScheduleAction(totalMs, () =>
            {
                var generator = factory.GetGenerator(style);
                var pattern = generator.GeneratePattern();
                var beatSource = new BeatPatternSampleProvider(pattern, logger, userSamples, telemetryTracker);
                playback.SetSource(beatSource);
                playback.CurrentStyle = style;
                playback.StartPlayback();
            });

            return Results.Ok(new { message = $"Scheduled {style} beat to play in {timespan.Value.TotalSeconds:F1} seconds", actionId = id });
        });

        // Generate endpoint
        api.MapPost("/generate", (IBeatGeneratorFactory factory, string style = "basic", int? bpm = null) =>
        {
            telemetryTracker.TrackEvent(TelemetryConstants.Events.BeatGenerated, new Dictionary<string, string>
            {
                { TelemetryConstants.Properties.BeatStyle, style },
                { TelemetryConstants.Properties.BeatTempo, bpm?.ToString() ?? "default" }
            });

            var generator = factory.GetGenerator(style);
            var pattern = generator.GeneratePattern(bpm);
            return Results.Text(JsonSerializer.Serialize(new { message = "Pattern generated", pattern = pattern }), "application/json");
        });

        // Play endpoint
        api.MapPost("/play", (HttpContext context, IAudioPlaybackService playback, IBeatGeneratorFactory factory, UserSampleRepository userSamples, TelemetryTracker telemetryTracker) =>
        {
            var style = context.Request.Query["style"].FirstOrDefault() ?? "basic";
            var transition = context.Request.Query["transition"].FirstOrDefault() ?? "immediate";
            var xfadeDuration = double.TryParse(context.Request.Query["xfadeDuration"].FirstOrDefault(), out var val) ? val : 2.0;
            
            // Parse BPM
            int? bpm = null;
            if (context.Request.Query.TryGetValue("bpm", out var bpmValue) && !string.IsNullOrEmpty(bpmValue))
            {
                if (int.TryParse(bpmValue, out var parsedBpm))
                {
                    bpm = parsedBpm;
                }
            }

            // Track playback started event
            telemetryTracker.TrackEvent(TelemetryConstants.Events.PlaybackStarted, new Dictionary<string, string>
            {
                { TelemetryConstants.Properties.BeatStyle, style },
                { TelemetryConstants.Properties.BeatTempo, bpm?.ToString() ?? "default" }
            });

            var generator = factory.GetGenerator(style);
            if (bpm.HasValue)
            {
                generator.SetBPM(bpm.Value);
            }
            var pattern = generator.GeneratePattern();

            // Add user samples to pattern
            for (int i = 0; i < pattern.DrumSequence.Length; i++)
            {
                var drum = pattern.DrumSequence[i];
                if (userSamples.HasSample(drum))
                {
                    pattern.UserSampleSteps[i] = drum;
                }
            }

            if (transition == "crossfade")
            {
                playback.CrossfadeToPattern(pattern, (float)xfadeDuration);
            }
            else
            {
                // Create and set the provider for immediate transition
                var provider = new BeatPatternSampleProvider(
                    pattern,
                    app.Services.GetRequiredService<ILogger<BeatPatternSampleProvider>>(),
                    userSamples,
                    telemetryTracker);
                playback.SetSource(provider);
            }

            playback.CurrentStyle = style;
            playback.StartPlayback();

            return Results.Ok(new { message = "Playback started", pattern });
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

            return Results.Text(JsonSerializer.Serialize(new { message = "Playback stopped" }), "application/json");
        });

        // Pause endpoint
        api.MapPost("/pause", (IAudioPlaybackService playback) =>
        {
            var state = playback.GetPlaybackState();
            if (state != NAudio.Wave.PlaybackState.Playing)
            {
                return Results.Text(JsonSerializer.Serialize(new { error = "No active playback to pause" }), "application/json", statusCode: 400);
            }

            telemetryTracker.TrackEvent(TelemetryConstants.Events.PlaybackPaused);
            playback.PausePlayback();
            return Results.Text(JsonSerializer.Serialize(new { message = "Playback paused" }), "application/json");
        });

        // Resume endpoint
        api.MapPost("/resume", (IAudioPlaybackService playback) =>
        {
            var state = playback.GetPlaybackState();
            if (state != NAudio.Wave.PlaybackState.Paused)
            {
                return Results.Text(JsonSerializer.Serialize(new { error = "Playback is not paused" }), "application/json", statusCode: 400);
            }

            telemetryTracker.TrackEvent(TelemetryConstants.Events.PlaybackResumed);
            playback.ResumePlayback();
            return Results.Text(JsonSerializer.Serialize(new { message = "Playback resumed" }), "application/json");
        });

        // Volume endpoint
        api.MapPost("/volume", (IAudioPlaybackService playback, float level) =>
        {
            if (level < 0 || level > 1)
                return Results.Text(JsonSerializer.Serialize(new { error = "Volume must be between 0.0 and 1.0" }), "application/json", statusCode: 400);

            telemetryTracker.TrackMetric("Audio.Volume", level);
            playback.SetVolume(level);
            return Results.Text(JsonSerializer.Serialize(new { message = $"Volume set to {level:F2}" }), "application/json");
        });

        // Effect endpoint
        api.MapPost("/effect", (IAudioPlaybackService playback, IEffectFactory effectFactory, string name, bool enable) =>
        {
            if (enable)
            {
                var currentSource = playback.CurrentSource;
                if (currentSource == null)
                    return Results.Text(JsonSerializer.Serialize(new { error = "No audio source is currently playing" }), "application/json", statusCode: 400);

                var effect = effectFactory.CreateEffect(name, currentSource);
                if (effect == null)
                    return Results.Text(JsonSerializer.Serialize(new { error = $"Unknown effect: {name}" }), "application/json", statusCode: 400);

                telemetryTracker.TrackEvent(TelemetryConstants.Events.EffectAdded, new Dictionary<string, string>
                {
                    { TelemetryConstants.Properties.EffectName, name }
                });

                playback.AddEffect(effect);
                return Results.Text(JsonSerializer.Serialize(new { message = $"{name} effect enabled" }), "application/json");
            }
            else
            {
                telemetryTracker.TrackEvent(TelemetryConstants.Events.EffectRemoved, new Dictionary<string, string>
                {
                    { TelemetryConstants.Properties.EffectName, name }
                });

                playback.RemoveEffect(name);
                return Results.Text(JsonSerializer.Serialize(new { message = $"{name} effect disabled" }), "application/json");
            }
        });

        // Get current preset endpoint
        api.MapGet("/preset/current", (IAudioPlaybackService playback) =>
        {
            var preset = playback.GetCurrentPreset();
            return Results.Ok(preset);
        });

        // Apply preset endpoint
        api.MapPost("/preset/apply", (IAudioPlaybackService playback, IEffectFactory effectFactory, Preset preset) =>
        {
            try
            {
                // Validate the preset
                preset.Validate();

                // Track preset application
                telemetryTracker.TrackEvent(TelemetryConstants.Events.PresetLoaded, new Dictionary<string, string>
                {
                    { TelemetryConstants.Properties.PreferredBeatStyle, preset.Style },
                    { TelemetryConstants.Properties.PreferredEffects, string.Join(",", preset.Effects) }
                });

                // Apply the preset
                playback.ApplyPreset(preset, effectFactory);
                return Results.Ok(new { message = $"Preset '{preset.Name}' applied successfully" });
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        // Shutdown endpoint
        api.MapPost("/shutdown", async () =>
        {
            telemetryTracker.TrackEvent(TelemetryConstants.Events.ApplicationStopped);
            await telemetryTracker.TrackApplicationStop();

            // Use a separate task for shutdown, but await the delay
            _ = Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(500); // Small delay to allow response to be sent
                    Environment.Exit(0);
                }
                catch
                {
                    Environment.Exit(1); // Exit with error if shutdown task fails
                }
            });
            
            return Results.Text(JsonSerializer.Serialize(new { message = "Service shutting down..." }), "application/json");
        });

        // Configure to run on port 5001 since 5000 is used by ControlCenter
        app.Urls.Add("http://localhost:5001");

        app.Run();
    }
}

// Make the Program class public for testing
public partial class Program { }
