using System.CommandLine;
using Microsoft.Extensions.Logging;
using LofiBeats.Core.BeatGeneration;

namespace LofiBeats.Cli.Commands;

public class CommandLineInterface
{
    private readonly RootCommand _rootCommand;
    private readonly ILogger<CommandLineInterface> _logger;
    private readonly IBeatGenerator _beatGenerator;

    public CommandLineInterface(
        ILogger<CommandLineInterface> logger,
        IBeatGenerator beatGenerator)
    {
        _logger = logger;
        _beatGenerator = beatGenerator;
        _rootCommand = new RootCommand("Lofi Beats Generator & Player CLI")
        {
            Description = "A command-line application for generating and playing lofi beats"
        };

        ConfigureCommands();
        _logger.LogInformation("CommandLineInterface initialized");
    }

    private void ConfigureCommands()
    {
        // Add hello command
        var helloCommand = new Command("hello", "Says hello");
        helloCommand.SetHandler(() =>
        {
            _logger.LogInformation("Executing hello command");
            Console.WriteLine("Hello from LofiBeats CLI!");
        });
        _rootCommand.AddCommand(helloCommand);

        // Add version command
        var versionCommand = new Command("version", "Display version information");
        versionCommand.SetHandler(() =>
        {
            _logger.LogInformation("Executing version command");
            Console.WriteLine("LofiBeats CLI v0.1.0");
        });
        _rootCommand.AddCommand(versionCommand);

        // Add generate command
        var generateCommand = new Command("generate", "Generates a new beat pattern");
        generateCommand.SetHandler(() =>
        {
            _logger.LogInformation("Executing generate command");
            var pattern = _beatGenerator.GeneratePattern();
            Console.WriteLine($"Generated pattern: {pattern}");
        });
        _rootCommand.AddCommand(generateCommand);

        _logger.LogDebug("Commands configured successfully");
    }

    public async Task<int> ExecuteAsync(string[] args)
    {
        _logger.LogInformation("Executing command with args: {Args}", string.Join(" ", args));
        return await _rootCommand.InvokeAsync(args);
    }
} 