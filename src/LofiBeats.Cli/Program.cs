using LofiBeats.Cli.Commands;
using LofiBeats.Core.Telemetry;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LofiBeats.Cli;

public class Program
{
    private static readonly Action<ILogger, Exception> LogErrorExecutingCommand = LoggerMessage.Define(
        LogLevel.Error,
        new EventId(
            1,
            nameof(LogErrorExecutingCommand)),
        "An error occurred while executing the command");

    private static readonly CancellationTokenSource _cts = new();

    public static async Task<int> Main(string[] args)
    {
        // Set up Ctrl+C handling
        Console.CancelKeyPress += (sender, e) =>
        {
            Console.WriteLine("\nShutting down gracefully...");
            e.Cancel = true; // Prevent abrupt termination
            _cts.Cancel();
        };

        // Set up global exception handling
        AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
        {
            var ex = args.ExceptionObject as Exception;
            Console.Error.WriteLine($"FATAL ERROR: {ex?.Message}");
            Console.Error.WriteLine(ex?.StackTrace);
            Environment.Exit(1);
        };

        IHost? host = null;
        try
        {
            // Use the Startup class to create and configure the host
            host = Startup.CreateHostBuilder(args).Build();

            // Get the CLI interface from the service provider
            var cli = host.Services.GetRequiredService<CommandLineInterface>();
            return await cli.ExecuteAsync(args, _cts.Token);
        }
        catch (OperationCanceledException)
        {
            // Get telemetry if possible
            if (host != null)
            {
                var telemetry = host.Services.GetService<TelemetryTracker>();
                if (telemetry != null)
                {
                    await telemetry.TrackApplicationStop();
                }
            }
            return 0;
        }
        catch (Exception ex)
        {
            // Get logger and telemetry if possible
            if (host != null)
            {
                var logger = host.Services.GetService<ILogger<Program>>();
                var telemetry = host.Services.GetService<TelemetryTracker>();

                if (logger != null)
                {
                    LogErrorExecutingCommand(logger, ex);
                }
                else
                {
                    Console.Error.WriteLine($"Error: {ex.Message}");
                }

                // Track the exception in telemetry if available
                telemetry?.TrackError(ex, "CLI Command Execution");
            }
            else
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }

            return 1;
        }
        finally
        {
            host?.Dispose();
        }
    }
}
