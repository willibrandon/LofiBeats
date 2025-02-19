using LofiBeats.Core.Playback;
using LofiBeats.Core.Telemetry;
using LofiBeats.Service;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace LofiBeats.Tests.Infrastructure;

/// <summary>
/// Base class for all LofiBeats API tests providing common test infrastructure
/// </summary>
public abstract class LofiBeatsTestBase : IAsyncLifetime
{
    protected readonly WebApplicationFactory<Program> Factory;
    protected readonly HttpClient Client;
    protected readonly ITestOutputHelper Output;
    protected readonly TestAudioPlaybackService AudioService;

    protected LofiBeatsTestBase(ITestOutputHelper output)
    {
        Output = output;
        AudioService = new TestAudioPlaybackService();
        
        Factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Replace real services with test doubles
                    services.RemoveAll(typeof(IAudioPlaybackService));
                    services.AddSingleton<IAudioPlaybackService>(AudioService);
                    
                    services.RemoveAll(typeof(TelemetryTracker));
                    services.AddSingleton<TelemetryTracker>(sp => 
                        new TelemetryTracker(
                            new NullTelemetryService(),
                            sp.GetRequiredService<ILogger<TelemetryTracker>>()));

                    // Configure logging
                    services.AddLogging(logging =>
                    {
                        logging.ClearProviders();
                        logging.AddConsole();
                        logging.AddDebug();
                    });
                });
            });

        Client = Factory.CreateClient();
    }

    /// <summary>
    /// Checks if the current environment has audio capabilities
    /// </summary>
    protected static bool HasAudioCapabilities()
    {
        try
        {
            using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            using var output = AudioOutputFactory.CreateForCurrentPlatform(loggerFactory);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public virtual Task InitializeAsync() => Task.CompletedTask;

    public virtual async Task DisposeAsync()
    {
        await Factory.DisposeAsync();
    }
} 