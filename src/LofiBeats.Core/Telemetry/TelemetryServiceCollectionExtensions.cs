using Microsoft.Extensions.DependencyInjection;

namespace LofiBeats.Core.Telemetry;

/// <summary>
/// Extension methods for configuring telemetry services.
/// </summary>
public static class TelemetryServiceCollectionExtensions
{
    /// <summary>
    /// Adds telemetry services to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add telemetry to.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddLofiTelemetry(this IServiceCollection services)
    {
        // Register the local file telemetry service as the default implementation
        services.AddSingleton<ITelemetryService, LocalFileTelemetryService>();
        
        // Register the telemetry tracker
        services.AddSingleton<TelemetryTracker>();

        return services;
    }
} 