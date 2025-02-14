using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using LofiBeats.Cli.Commands;
using LofiBeats.Core.BeatGeneration;
using LofiBeats.Core.Effects;
using LofiBeats.Core.Playback;

namespace LofiBeats.Cli;

public static class Startup
{
    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .AddCommandLine(args);
            })
            .ConfigureServices((context, services) =>
            {
                // Register our CLI interface
                services.AddSingleton<CommandLineInterface>();

                // Register beat generation services
                services.AddSingleton<IBeatGenerator, BasicLofiBeatGenerator>();

                // Register audio effect services
                services.AddSingleton<IEffectFactory, EffectFactory>();

                // Register audio playback service
                services.AddSingleton<IAudioPlaybackService, AudioPlaybackService>();
            })
            .ConfigureLogging((context, logging) =>
            {
                logging.ClearProviders();
                logging.AddConfiguration(context.Configuration.GetSection("Logging"));
                logging.AddConsole();
                logging.AddDebug();

                // Set minimum log level from configuration
                if (Enum.TryParse<LogLevel>(context.Configuration["Logging:LogLevel:Default"], out var defaultLevel))
                {
                    logging.SetMinimumLevel(defaultLevel);
                }
            });
    }
} 