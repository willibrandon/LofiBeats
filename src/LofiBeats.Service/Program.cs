using LofiBeats.Core.BeatGeneration;
using LofiBeats.Core.Effects;
using LofiBeats.Core.Playback;
using LofiBeats.Core.Telemetry;
using System.Text.Json;
using Microsoft.Extensions.Logging;

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
    // Log the configuration values
    builder.Services.AddLogging(logging =>
    {
        logging.AddConsole();
        logging.AddDebug();
    });
    var logger = LoggerFactory.Create(logging => logging.AddConsole()).CreateLogger("Program");
    logger.LogInformation("Telemetry Configuration: EnableSeq={EnableSeq}, SeqServerUrl={SeqServerUrl}", 
        telemetryConfig.EnableSeq, telemetryConfig.SeqServerUrl);

    builder.Services.AddLofiTelemetry(telemetryConfig);
}
else
{
    var logger = LoggerFactory.Create(logging => logging.AddConsole()).CreateLogger("Program");
    logger.LogWarning("No telemetry configuration found in appsettings.json, using defaults");

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

var app = builder.Build();

// Get telemetry tracker for API endpoints
var telemetryTracker = app.Services.GetRequiredService<TelemetryTracker>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Remove HTTPS redirection since this is a local service
// app.UseHttpsRedirection();

// Add health check endpoint
app.MapHealthChecks("/healthz");

// Define our API endpoints
var api = app.MapGroup("/api/lofi");

// Generate endpoint
api.MapPost("/generate", (IBeatGeneratorFactory factory, string style = "basic") =>
{
    telemetryTracker.TrackEvent(TelemetryConstants.Events.BeatGenerated, new Dictionary<string, string>
    {
        { TelemetryConstants.Properties.BeatStyle, style }
    });

    var generator = factory.GetGenerator(style);
    var pattern = generator.GeneratePattern();
    return Results.Text(JsonSerializer.Serialize(new { message = "Pattern generated", pattern = pattern }), "application/json");
});

// Play endpoint
api.MapPost("/play", (IAudioPlaybackService playback, IBeatGeneratorFactory factory, string style = "basic") =>
{
    telemetryTracker.TrackEvent(TelemetryConstants.Events.PlaybackStarted, new Dictionary<string, string>
    {
        { TelemetryConstants.Properties.BeatStyle, style }
    });

    var generator = factory.GetGenerator(style);
    var pattern = generator.GeneratePattern();
    var beatSource = new BeatPatternSampleProvider(pattern, app.Logger);
    playback.SetSource(beatSource);
    playback.StartPlayback();
    return Results.Text(JsonSerializer.Serialize(new { message = "Playback started", pattern = pattern }), "application/json");
});

// Stop endpoint
api.MapPost("/stop", (IAudioPlaybackService playback) =>
{
    telemetryTracker.TrackEvent(TelemetryConstants.Events.PlaybackStopped);
    playback.StopPlayback();
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

    Task.Run(async () =>
    {
        await Task.Delay(500); // Small delay to allow response to be sent
        Environment.Exit(0);
    });
    return Results.Text(JsonSerializer.Serialize(new { message = "Service shutting down..." }), "application/json");
});

// Configure to run on port 5000
app.Urls.Add("http://localhost:5000");

app.Run();

// Make the Program class public for testing
public partial class Program { }
