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
        
        // Register the telemetry service
        services.AddSingleton<ITelemetryService, LocalFileTelemetryService>();
        
        // Register the telemetry tracker
        services.AddSingleton<TelemetryTracker>();

        return services;
    }
} 