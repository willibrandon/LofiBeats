using LofiBeats.Cli.Commands;
using LofiBeats.Core.Configuration;
using LofiBeats.Core.Effects;
using LofiBeats.Core.PluginManagement;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LofiBeats.Cli;

public static class Startup
{
    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                var env = hostingContext.HostingEnvironment;
                
                config.SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("cli.appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"cli.appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .AddCommandLine(args);
            })
            .ConfigureServices((context, services) =>
            {
                // Configure settings
                var pluginSettings = context.Configuration.GetSection("PluginSettings").Get<PluginSettings>()
                    ?? new PluginSettings();
                services.AddSingleton(pluginSettings);

                // Register plugin services
                services.AddSingleton<IPluginLoader, PluginLoader>();
                services.AddSingleton<PluginManager>();
                services.AddSingleton<PluginWatcher>();
                services.AddSingleton<IEffectFactory, EffectFactory>();

                // Register our CLI interface with its dependencies
                services.AddSingleton<CommandLineInterface>(sp =>
                {
                    var logger = sp.GetRequiredService<ILogger<CommandLineInterface>>();
                    var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
                    var configuration = sp.GetRequiredService<IConfiguration>();
                    return new CommandLineInterface(logger, loggerFactory, configuration);
                });
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