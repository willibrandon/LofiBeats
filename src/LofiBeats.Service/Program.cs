using LofiBeats.Core.BeatGeneration;
using LofiBeats.Core.Effects;
using LofiBeats.Core.Playback;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();

// Register our core services as singletons
builder.Services.AddSingleton<IAudioPlaybackService, AudioPlaybackService>();
builder.Services.AddSingleton<IBeatGenerator, BasicLofiBeatGenerator>();
builder.Services.AddSingleton<IEffectFactory, EffectFactory>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Add health check endpoint
app.MapHealthChecks("/healthz");

// Define our API endpoints
var api = app.MapGroup("/api/lofi");

// Play endpoint
api.MapPost("/play", (IAudioPlaybackService playback, IBeatGenerator generator) =>
{
    var pattern = generator.GeneratePattern();
    var beatSource = new BeatPatternSampleProvider(pattern, app.Logger);
    playback.SetSource(beatSource);
    playback.StartPlayback();
    return Results.Text(JsonSerializer.Serialize(new { message = "Playback started", pattern = pattern }), "application/json");
});

// Stop endpoint
api.MapPost("/stop", (IAudioPlaybackService playback) =>
{
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

    playback.ResumePlayback();
    return Results.Text(JsonSerializer.Serialize(new { message = "Playback resumed" }), "application/json");
});

// Volume endpoint
api.MapPost("/volume", (IAudioPlaybackService playback, float level) =>
{
    if (level < 0 || level > 1)
        return Results.Text(JsonSerializer.Serialize(new { error = "Volume must be between 0.0 and 1.0" }), "application/json", statusCode: 400);

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

        playback.AddEffect(effect);
        return Results.Text(JsonSerializer.Serialize(new { message = $"{name} effect enabled" }), "application/json");
    }
    else
    {
        playback.RemoveEffect(name);
        return Results.Text(JsonSerializer.Serialize(new { message = $"{name} effect disabled" }), "application/json");
    }
});

// Shutdown endpoint
api.MapPost("/shutdown", () =>
{
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
