using System.CommandLine;
using System.Text.Json;
using LofiBeats.PluginHost.Models;

namespace LofiBeats.PluginHost;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var pluginAssemblyOption = new Option<string>(
            name: "--plugin-assembly",
            description: "Path to the plugin assembly to load"
        );

        var rootCommand = new RootCommand("LofiBeats Plugin Host - Runs plugins in an isolated process")
        {
            pluginAssemblyOption
        };

        rootCommand.SetHandler(async (string pluginAssembly) =>
        {
            await RunPluginHost(pluginAssembly);
        }, pluginAssemblyOption);

        return await rootCommand.InvokeAsync(args);
    }

    private static async Task RunPluginHost(string pluginAssemblyPath)
    {
        Console.WriteLine($"PluginHost started. Loading assembly: {pluginAssemblyPath}");

        // Set up JSON serializer options
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = false
        };

        // Main message loop
        while (true)
        {
            try
            {
                // Read a line from STDIN
                var line = await Console.In.ReadLineAsync();
                if (line == null)
                {
                    // EOF - parent process closed the pipe
                    break;
                }

                // Parse the message
                var message = JsonSerializer.Deserialize<PluginMessage>(line, jsonOptions);
                if (message == null)
                {
                    await WriteResponse(new PluginResponse
                    {
                        Status = "error",
                        Message = "Invalid message format"
                    }, jsonOptions);
                    continue;
                }

                // Handle the message based on action
                var response = message.Action switch
                {
                    "init" => new PluginResponse
                    {
                        Status = "ok",
                        Message = "Plugin host initialized"
                    },
                    _ => new PluginResponse
                    {
                        Status = "error",
                        Message = $"Unknown action: {message.Action}"
                    }
                };

                await WriteResponse(response, jsonOptions);
            }
            catch (Exception ex)
            {
                await WriteResponse(new PluginResponse
                {
                    Status = "error",
                    Message = $"Error processing message: {ex.Message}"
                }, jsonOptions);
            }
        }
    }

    private static async Task WriteResponse(PluginResponse response, JsonSerializerOptions options)
    {
        var json = JsonSerializer.Serialize(response, options);
        await Console.Out.WriteLineAsync(json);
        await Console.Out.FlushAsync();
    }
}
