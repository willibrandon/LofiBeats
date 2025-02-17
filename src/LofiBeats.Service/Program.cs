using LofiBeats.Core.BeatGeneration;
using LofiBeats.Core.Effects;
using LofiBeats.Core.Playback;
using LofiBeats.Core.Scheduling;
using LofiBeats.Core.Telemetry;
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
                                     PlaybackScheduler scheduler, ILogger<Program> logger, string style = "basic", string delay = "0s") =>
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
                var beatSource = new BeatPatternSampleProvider(pattern, logger);
                playback.SetSource(beatSource);
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
        api.MapPost("/play", (HttpContext context, IAudioPlaybackService playback, IBeatGeneratorFactory factory) =>
        {
            var style = context.Request.Query["style"].FirstOrDefault() ?? "basic";
            int? bpm = null;
            if (context.Request.Query.TryGetValue("bpm", out var bpmValue) && !string.IsNullOrEmpty(bpmValue))
            {
                if (int.TryParse(bpmValue, out var parsedBpm))
                {
                    bpm = parsedBpm;
                }
            }

            telemetryTracker.TrackEvent(TelemetryConstants.Events.PlaybackStarted, new Dictionary<string, string>
            {
                { TelemetryConstants.Properties.BeatStyle, style },
                { TelemetryConstants.Properties.BeatTempo, bpm?.ToString() ?? "default" }
            });

            var generator = factory.GetGenerator(style);
            var pattern = generator.GeneratePattern(bpm);
            var beatSource = new BeatPatternSampleProvider(pattern, app.Logger);
            playback.SetSource(beatSource);
            playback.StartPlayback();
            return Results.Text(JsonSerializer.Serialize(new { message = "Playback started", pattern = pattern }), "application/json");
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

        // Configure to run on port 5000
        app.Urls.Add("http://localhost:5000");

        app.Run();
    }
}

// Make the Program class public for testing
public partial class Program { }
