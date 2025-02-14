using System.CommandLine;

namespace LofiBeats.Cli.Commands;

public class CommandLineInterface
{
    private readonly RootCommand _rootCommand;

    public CommandLineInterface()
    {
        _rootCommand = new RootCommand("Lofi Beats Generator & Player CLI")
        {
            Description = "A command-line application for generating and playing lofi beats"
        };

        ConfigureCommands();
    }

    private void ConfigureCommands()
    {
        // Add hello command
        var helloCommand = new Command("hello", "Says hello");
        helloCommand.SetHandler(() =>
        {
            Console.WriteLine("Hello from LofiBeats CLI!");
        });
        _rootCommand.AddCommand(helloCommand);

        // Add version command
        var versionCommand = new Command("version", "Display version information");
        versionCommand.SetHandler(() =>
        {
            Console.WriteLine("LofiBeats CLI v0.1.0");
        });
        _rootCommand.AddCommand(versionCommand);
    }

    public async Task<int> ExecuteAsync(string[] args)
    {
        return await _rootCommand.InvokeAsync(args);
    }
} 