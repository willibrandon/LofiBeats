using LofiBeats.Core.BeatGeneration;
using LofiBeats.Core.Effects;
using LofiBeats.Core.Playback;

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
api.MapPost("/play", async (IAudioPlaybackService playback, IBeatGenerator generator) =>
{
    var pattern = generator.GeneratePattern();
    var beatSource = new BeatPatternSampleProvider(pattern, app.Logger);
    playback.SetSource(beatSource);
    playback.StartPlayback();
    return Results.Ok(new { message = "Playback started", pattern = pattern });
});

// Stop endpoint
api.MapPost("/stop", (IAudioPlaybackService playback) =>
{
    playback.StopPlayback();
    return Results.Ok(new { message = "Playback stopped" });
});

// Volume endpoint
api.MapPost("/volume", (IAudioPlaybackService playback, float level) =>
{
    if (level < 0 || level > 1)
        return Results.BadRequest(new { error = "Volume must be between 0.0 and 1.0" });

    playback.SetVolume(level);
    return Results.Ok(new { message = $"Volume set to {level:F2}" });
});

// Effect endpoint
api.MapPost("/effect", async (IAudioPlaybackService playback, IEffectFactory effectFactory, string name, bool enable) =>
{
    if (enable)
    {
        var currentSource = playback.CurrentSource;
        if (currentSource == null)
            return Results.BadRequest(new { error = "No audio source is currently playing" });

        var effect = effectFactory.CreateEffect(name, currentSource);
        if (effect == null)
            return Results.BadRequest(new { error = $"Unknown effect: {name}" });

        playback.AddEffect(effect);
        return Results.Ok(new { message = $"{name} effect enabled" });
    }
    else
    {
        playback.RemoveEffect(name);
        return Results.Ok(new { message = $"{name} effect disabled" });
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
    return Results.Ok(new { message = "Service shutting down..." });
});

// Configure to run on port 5000
app.Urls.Add("http://localhost:5000");

app.Run();
