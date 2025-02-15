using LofiBeats.Cli.Commands;
using Microsoft.Extensions.DependencyInjection;
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

    public static async Task<int> Main(string[] args)
    {
        var builder = Startup.CreateHostBuilder(args);
        using var host = builder.Build();

        try
        {
            // Get the CLI interface from the service provider
            var cli = host.Services.GetRequiredService<CommandLineInterface>();
            return await cli.ExecuteAsync(args);
        }
        catch (Exception ex)
        {
            // Get logger if possible, otherwise write to console
            var logger = host.Services.GetService<ILogger<Program>>();
            if (logger != null)
            {
                LogErrorExecutingCommand(logger, ex);
            }
            else
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }

            return 1;
        }
    }
}
