using System.CommandLine;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;

namespace LofiBeats.Cli.Commands;

public class CommandLineInterface : IDisposable
{
    private readonly RootCommand _rootCommand;
    private readonly ILogger<CommandLineInterface> _logger;
    private readonly ServiceConnectionHelper _serviceHelper;

    public CommandLineInterface(ILogger<CommandLineInterface> logger, ILoggerFactory loggerFactory)
    {
        _logger = logger;
        _serviceHelper = new ServiceConnectionHelper(
            loggerFactory.CreateLogger<ServiceConnectionHelper>());
        
        _rootCommand = new RootCommand("Lofi Beats Generator & Player CLI")
        {
            Description = "A command-line application for generating and playing lofi beats"
        };

        ConfigureCommands();
        _logger.LogInformation("CommandLineInterface initialized");
    }

    private void ConfigureCommands()
    {
        // Add play command
        var playCommand = new Command("play", "Plays a new lofi beat");
        playCommand.SetHandler(async () =>
        {
            _logger.LogInformation("Executing play command");
            try
            {
                var response = await _serviceHelper.SendCommandAsync(HttpMethod.Post, "play");
                var result = await response.Content.ReadFromJsonAsync<PlayResponse>();
                if (result?.Pattern != null)
                {
                    Console.WriteLine($"Playing new beat pattern: {result.Pattern}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        });
        _rootCommand.AddCommand(playCommand);

        // Add stop command
        var stopCommand = new Command("stop", "Stops audio playback");
        stopCommand.SetHandler(async () =>
        {
            _logger.LogInformation("Executing stop command");
            try
            {
                var response = await _serviceHelper.SendCommandAsync(HttpMethod.Post, "stop");
                var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
                if (result?.Message != null)
                {
                    Console.WriteLine(result.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        });
        _rootCommand.AddCommand(stopCommand);

        // Add effect command
        var effectCommand = new Command("effect", "Manage effects");
        var effectNameOpt = new Option<string>("--name", "Name of the effect to manage") { IsRequired = true };
        var enableOpt = new Option<bool>("--enable", () => true, "Whether to enable or disable the effect");

        effectCommand.AddOption(effectNameOpt);
        effectCommand.AddOption(enableOpt);

        effectCommand.SetHandler(async (string name, bool enable) =>
        {
            _logger.LogInformation("Executing effect command with name: {Name}, enable: {Enable}", name, enable);
            try
            {
                var response = await _serviceHelper.SendCommandAsync(
                    HttpMethod.Post,
                    $"effect?name={Uri.EscapeDataString(name)}&enable={enable}");
                
                var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error: {result?.Error}");
                    return;
                }

                if (result?.Message != null)
                {
                    Console.WriteLine(result.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }, effectNameOpt, enableOpt);
        _rootCommand.AddCommand(effectCommand);

        // Add volume command
        var volumeCommand = new Command("volume", "Adjusts master volume");
        var volumeOption = new Option<float>(
            "--level",
            description: "Volume level between 0.0 and 1.0",
            getDefaultValue: () => 1.0f);
        volumeOption.AddValidator(result =>
        {
            var value = result.GetValueOrDefault<float>();
            if (value < 0f || value > 1f)
            {
                result.ErrorMessage = "Volume level must be between 0.0 and 1.0";
            }
        });
        volumeCommand.AddOption(volumeOption);

        volumeCommand.SetHandler(async (float level) =>
        {
            _logger.LogInformation("Setting volume to: {Level}", level);
            try
            {
                var response = await _serviceHelper.SendCommandAsync(
                    HttpMethod.Post,
                    $"volume?level={level}");
                
                var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error: {result?.Error}");
                    return;
                }

                if (result?.Message != null)
                {
                    Console.WriteLine(result.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }, volumeOption);
        _rootCommand.AddCommand(volumeCommand);

        // Add shutdown command
        var shutdownCommand = new Command("shutdown", "Shuts down the LofiBeats service");
        shutdownCommand.SetHandler(async () =>
        {
            _logger.LogInformation("Executing shutdown command");
            try
            {
                await _serviceHelper.ShutdownServiceAsync();
                Console.WriteLine("Service shutdown requested successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        });
        _rootCommand.AddCommand(shutdownCommand);

        // Add version command
        var versionCommand = new Command("version", "Display version information");
        versionCommand.SetHandler(() =>
        {
            _logger.LogInformation("Executing version command");
            Console.WriteLine("LofiBeats CLI v0.1.0");
        });
        _rootCommand.AddCommand(versionCommand);

        _logger.LogDebug("Commands configured successfully");
    }

    public async Task<int> ExecuteAsync(string[] args)
    {
        _logger.LogInformation("Executing command with args: {Args}", string.Join(" ", args));
        return await _rootCommand.InvokeAsync(args);
    }

    public void Dispose()
    {
        (_serviceHelper as IDisposable)?.Dispose();
    }
} 