using LofiBeats.Cli.Commands;

namespace LofiBeats.Cli;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var cli = new CommandLineInterface();
        return await cli.ExecuteAsync(args);
    }
}
