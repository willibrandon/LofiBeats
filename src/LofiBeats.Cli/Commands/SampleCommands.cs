using System.CommandLine;
using LofiBeats.Core.Playback;
using Microsoft.Extensions.Logging;

namespace LofiBeats.Cli.Commands;

/// <summary>
/// Commands for managing user samples.
/// </summary>
public static class SampleCommands
{
    private static readonly Action<ILogger, string, string, Exception?> LogSampleRegistered =
        LoggerMessage.Define<string, string>(
            LogLevel.Information,
            new EventId(1, "SampleRegistered"),
            "Successfully registered sample '{Name}' from {Path}");

    private static readonly Action<ILogger, string, string, Exception?> LogSampleRegistrationFailed =
        LoggerMessage.Define<string, string>(
            LogLevel.Error,
            new EventId(2, "SampleRegistrationFailed"),
            "Failed to register sample '{Name}' from {Path}");

    private static readonly Action<ILogger, Exception?> LogListSamplesFailed =
        LoggerMessage.Define(
            LogLevel.Error,
            new EventId(3, "ListSamplesFailed"),
            "Failed to list samples");

    private static readonly string[] CommonDrums = ["kick", "snare", "hat", "ohat"];

    public static Command CreateSampleCommand(UserSampleRepository sampleRepository, ILogger logger)
    {
        var sampleCommand = new Command("sample", "Manage user samples");

        // Register sample command
        var registerCommand = new Command("register", "Register a new sample");
        var nameArg = new Argument<string>("name", "Name to identify the sample");
        var pathArg = new Argument<string>("path", "Path to the audio file");
        var velocityOption = new Option<int?>(
            name: "--velocity",
            description: "Optional velocity layer (0-127)",
            getDefaultValue: () => null);

        registerCommand.AddArgument(nameArg);
        registerCommand.AddArgument(pathArg);
        registerCommand.AddOption(velocityOption);

        registerCommand.SetHandler((string name, string path, int? velocity) =>
        {
            try
            {
                sampleRepository.RegisterSample(name, path, velocity);
                LogSampleRegistered(logger, name, path, null);
                Console.WriteLine($"Sample '{name}' registered successfully" + 
                    (velocity.HasValue ? $" (velocity layer: {velocity})" : ""));
            }
            catch (Exception ex)
            {
                LogSampleRegistrationFailed(logger, name, path, ex);
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }, nameArg, pathArg, velocityOption);

        // List samples command
        var listCommand = new Command("list", "List all registered samples");
        listCommand.SetHandler(() =>
        {
            try
            {
                var foundSamples = new HashSet<string>();

                foreach (var drum in CommonDrums)
                {
                    if (sampleRepository.HasSample(drum))
                    {
                        foundSamples.Add(drum);
                    }
                }

                if (foundSamples.Count == 0)
                {
                    Console.WriteLine("No samples registered.");
                }
                else
                {
                    Console.WriteLine("Registered samples:");
                    foreach (var sample in foundSamples.OrderBy(s => s))
                    {
                        Console.WriteLine($"  - {sample}");
                    }
                }
            }
            catch (Exception ex)
            {
                LogListSamplesFailed(logger, ex);
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        });

        // Add subcommands
        sampleCommand.AddCommand(registerCommand);
        sampleCommand.AddCommand(listCommand);

        return sampleCommand;
    }
} 