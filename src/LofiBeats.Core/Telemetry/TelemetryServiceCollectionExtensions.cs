using Microsoft.Extensions.DependencyInjection;

namespace LofiBeats.Core.Telemetry;

/// <summary>
/// Extension methods for configuring telemetry services.
/// </summary>
public static class TelemetryServiceCollectionExtensions
{
    /// <summary>
    /// Adds telemetry services to the service collection with default configuration.
    /// </summary>
    /// <param name="services">The service collection to add telemetry to.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddLofiTelemetry(this IServiceCollection services)
    {
        return services.AddLofiTelemetry(new TelemetryConfiguration());
    }

    /// <summary>
    /// Adds telemetry services to the service collection with custom configuration.
    /// </summary>
    /// <param name="services">The service collection to add telemetry to.</param>
    /// <param name="configuration">The telemetry configuration to use.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddLofiTelemetry(
        this IServiceCollection services,
        TelemetryConfiguration configuration)
    {
        // Register the configuration
        services.AddSingleton(configuration);

        // Register telemetry services based on configuration
        if (configuration.EnableLocalFile)
        {
            services.AddSingleton<LocalFileTelemetryService>();
        }

        if (configuration.EnableSeq)
        {
            services.AddSingleton<SeqTelemetryService>();
        }

        // Register the composite telemetry service that combines all enabled services
        services.AddSingleton<ITelemetryService>(sp =>
        {
            var services = new List<ITelemetryService>();

            if (configuration.EnableLocalFile)
            {
                services.Add(sp.GetRequiredService<LocalFileTelemetryService>());
            }

            if (configuration.EnableSeq)
            {
                services.Add(sp.GetRequiredService<SeqTelemetryService>());
            }

            return services.Count == 1 
                ? services[0] 
                : new CompositeTelemetryService(services);
        });

        // Register the telemetry tracker
        services.AddSingleton<TelemetryTracker>();

        return services;
    }
} 