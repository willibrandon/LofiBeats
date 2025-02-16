using System.CommandLine;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using System.CommandLine.Invocation;

namespace LofiBeats.Cli.Commands;

public class CommandLineInterface : IDisposable
{
    private readonly RootCommand _rootCommand;
    private readonly ILogger<CommandLineInterface> _logger;
    private readonly ServiceConnectionHelper _serviceHelper;

    private static readonly Action<ILogger, Exception?> _logCommandLineInterfaceInitialized =
        LoggerMessage.Define(LogLevel.Information, new EventId(0, nameof(CommandLineInterface)), "CommandLineInterface initialized");

    private static readonly Action<ILogger, string, Exception?> _logExecutingCommand =
        LoggerMessage.Define<string>(LogLevel.Information, new EventId(1, "ExecutingCommand"), "Executing command with args: {Args}");

    private static readonly Action<ILogger, Exception?> _logExecutingPlayCommand =
        LoggerMessage.Define(LogLevel.Information, new EventId(2, "ExecutingPlayCommand"), "Executing play command");

    private static readonly Action<ILogger, Exception?> _logExecutingStopCommand =
        LoggerMessage.Define(LogLevel.Information, new EventId(3, "ExecutingStopCommand"), "Executing stop command");

    private static readonly Action<ILogger, Exception?> _logExecutingPauseCommand =
        LoggerMessage.Define(LogLevel.Information, new EventId(4, "ExecutingPauseCommand"), "Executing pause command");

    private static readonly Action<ILogger, Exception?> _logExecutingResumeCommand =
        LoggerMessage.Define(LogLevel.Information, new EventId(5, "ExecutingResumeCommand"), "Executing resume command");

    private static readonly Action<ILogger, string, bool, Exception?> _logExecutingEffectCommand =
        LoggerMessage.Define<string, bool>(LogLevel.Information, new EventId(6, "ExecutingEffectCommand"), "Executing effect command with name: {Name}, enable: {Enable}");

    private static readonly Action<ILogger, float, Exception?> _logSettingVolume =
        LoggerMessage.Define<float>(LogLevel.Information, new EventId(7, "SettingVolume"), "Setting volume to: {Level}");

    private static readonly Action<ILogger, Exception?> _logExecutingShutdownCommand =
        LoggerMessage.Define(LogLevel.Information, new EventId(8, "ExecutingShutdownCommand"), "Executing shutdown command");

    private static readonly Action<ILogger, Exception?> _logExecutingVersionCommand =
        LoggerMessage.Define(LogLevel.Information, new EventId(9, "ExecutingVersionCommand"), "Executing version command");

    private static readonly Action<ILogger, Exception?> _logCommandsConfigured =
        LoggerMessage.Define(LogLevel.Debug, new EventId(10, "CommandsConfigured"), "Commands configured successfully");

    private static readonly Action<ILogger, Exception?> _logExecutingGenerateCommand =
        LoggerMessage.Define(LogLevel.Information, new EventId(11, "ExecutingGenerateCommand"), "Executing generate command");

    private static readonly Action<ILogger, Exception?> _logEnteringInteractiveMode =
        LoggerMessage.Define(LogLevel.Information, new EventId(12, "EnteringInteractiveMode"), "Entering interactive mode");

    private static readonly Action<ILogger, string, Exception?> _logEffectNotFound =
        LoggerMessage.Define<string>(LogLevel.Warning, new EventId(13, "EffectNotFound"), "Effect '{Name}' not found");

    private static readonly Action<ILogger, string, Exception?> _logInvalidBeatStyle =
        LoggerMessage.Define<string>(LogLevel.Warning, new EventId(14, "InvalidBeatStyle"), "Invalid beat style '{Style}'");

    private static readonly Action<ILogger, Exception?> _logExecutingUpdateCommand =
        LoggerMessage.Define(LogLevel.Information, new EventId(15, "ExecutingUpdateCommand"), "Executing update command");

    private static readonly string[] ValidEffects = ["vinyl", "reverb", "lowpass", "tapeflutter"];
    private static readonly string[] ValidBeatStyles = ["basic", "jazzy", "chillhop", "hiphop"];

    private static void ShowSpinner(string message, int durationMs)
    {
        var spinChars = new[] { '|', '/', '-', '\\' };
        var originalLeft = Console.CursorLeft;
        var originalTop = Console.CursorTop;
        var startTime = DateTime.Now;

        while ((DateTime.Now - startTime).TotalMilliseconds < durationMs)
        {
            foreach (var spinChar in spinChars)
            {
                Console.SetCursorPosition(originalLeft, originalTop);
                Console.Write($"{message} {spinChar}");
                Thread.Sleep(100);
            }
        }
        Console.SetCursorPosition(originalLeft, originalTop);
        Console.Write(new string(' ', message.Length + 2)); // Clear the spinner
        Console.SetCursorPosition(originalLeft, originalTop);
    }

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
        _logCommandLineInterfaceInitialized(_logger, null);
    }

    private void ConfigureCommands()
    {
        // Add update command
        var updateCommand = new Command("update", "Update the CLI tool to the latest version");
        updateCommand.Description = "Updates the LofiBeats CLI tool to the latest version.\n\n" +
                                  "Since this is distributed as a .NET tool, you can update it using:\n" +
                                  "  dotnet tool update --global lofi\n\n" +
                                  "This will fetch and install the latest version from NuGet.";
        
        updateCommand.SetHandler(() =>
        {
            _logExecutingUpdateCommand(_logger, null);
            Console.WriteLine("To update the LofiBeats CLI tool, run:");
            Console.WriteLine("  dotnet tool update --global lofi");
            Console.WriteLine("\nThis will update to the latest version from NuGet.");
        });
        _rootCommand.AddCommand(updateCommand);

        // Add generate command with enhanced description and validation
        var generateCommand = new Command("generate", "Generates a new lofi beat pattern with customizable style")
        {
            Description = "Creates a new beat pattern using the specified style. Each style has unique characteristics:\n" +
                         "  - basic: Standard lofi beat pattern\n" +
                         "  - jazzy: Syncopated rhythms with jazz-inspired elements\n" +
                         "  - chillhop: Laid-back beats with hip-hop influence\n" +
                         "  - hiphop: Classic hip-hop inspired beats with boom-bap elements"
        };
        var generateStyleOption = new Option<string>(
            "--style",
            () => "basic",
            "Beat style (basic, jazzy, chillhop, hiphop)");
        generateStyleOption.AddValidator(result =>
        {
            var value = result.GetValueOrDefault<string>();
            if (!ValidBeatStyles.Contains(value, StringComparer.OrdinalIgnoreCase))
            {
                result.ErrorMessage = $"Invalid style '{value}'. Valid styles are: {string.Join(", ", ValidBeatStyles)}";
            }
        });
        generateCommand.AddOption(generateStyleOption);

        generateCommand.SetHandler(async (string style) =>
        {
            _logExecutingGenerateCommand(_logger, null);
            try
            {
                Console.Write("Generating beat pattern... ");
                ShowSpinner("Generating beat pattern", 1500); // Show progress for 1.5 seconds

                var response = await _serviceHelper.SendCommandAsync(HttpMethod.Post, $"generate?style={Uri.EscapeDataString(style)}");
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadFromJsonAsync<ApiResponse>();
                    _logInvalidBeatStyle(_logger, style, null);
                    Console.WriteLine($"Failed to generate beat pattern: {error?.Error ?? "Unknown error"}");
                    return;
                }

                var result = await response.Content.ReadFromJsonAsync<PlayResponse>();
                if (result?.Pattern != null)
                {
                    Console.WriteLine($"Generated new {style} beat pattern: {result.Pattern}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating beat pattern: {ex.Message}");
                Console.WriteLine("Please ensure the LofiBeats service is running and try again.");
            }
        }, generateStyleOption);
        _rootCommand.AddCommand(generateCommand);

        // Add play command
        var playCommand = new Command("play", "Plays a new lofi beat");
        var styleOption = new Option<string>("--style", () => "basic", "Beat style (basic, jazzy, chillhop, hiphop)");
        playCommand.AddOption(styleOption);

        playCommand.SetHandler(async (string style) =>
        {
            _logExecutingPlayCommand(_logger, null);
            try
            {
                Console.Write("Starting playback... ");
                ShowSpinner("Starting playback", 1000);

                var response = await _serviceHelper.SendCommandAsync(HttpMethod.Post, $"play?style={Uri.EscapeDataString(style)}");
                var result = await response.Content.ReadFromJsonAsync<PlayResponse>();
                if (result?.Pattern != null)
                {
                    Console.WriteLine($"Playing new {style} beat pattern: {result.Pattern}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }, styleOption);
        _rootCommand.AddCommand(playCommand);

        // Add stop command
        var stopCommand = new Command("stop", "Stops audio playback");
        stopCommand.Description = "Stops the currently playing audio.\n\n" +
                                "Options:\n" +
                                "  --tapestop    Gradually slow down the audio like a tape machine powering off\n\n" +
                                "Examples:\n" +
                                "  stop              Stop playback immediately\n" +
                                "  stop --tapestop   Stop with tape slow-down effect";
        var tapeStopOpt = new Option<bool>(
            name: "--tapestop",
            getDefaultValue: () => false,
            description: "Gradually slow pitch to zero before stopping");
        stopCommand.AddOption(tapeStopOpt);

        stopCommand.SetHandler(async (bool tapeStop) =>
        {
            _logExecutingStopCommand(_logger, null);
            try
            {
                if (tapeStop)
                {
                    Console.Write("Applying tape stop effect... ");
                    ShowSpinner("Applying tape stop effect", 2000);
                }
                else
                {
                    Console.Write("Stopping playback... ");
                    ShowSpinner("Stopping playback", 500);
                }

                var response = await _serviceHelper.SendCommandAsync(
                    HttpMethod.Post,
                    $"stop?tapestop={tapeStop}");
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
        }, tapeStopOpt);
        _rootCommand.AddCommand(stopCommand);

        // Add pause command
        var pauseCommand = new Command("pause", "Pauses audio playback");
        pauseCommand.SetHandler(async () =>
        {
            _logExecutingPauseCommand(_logger, null);
            try
            {
                Console.Write("Pausing playback... ");
                ShowSpinner("Pausing playback", 500);

                var response = await _serviceHelper.SendCommandAsync(HttpMethod.Post, "pause");
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
        });
        _rootCommand.AddCommand(pauseCommand);

        // Add resume command
        var resumeCommand = new Command("resume", "Resumes audio playback");
        resumeCommand.SetHandler(async () =>
        {
            _logExecutingResumeCommand(_logger, null);
            try
            {
                Console.Write("Resuming playback... ");
                ShowSpinner("Resuming playback", 500);

                var response = await _serviceHelper.SendCommandAsync(HttpMethod.Post, "resume");
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
        });
        _rootCommand.AddCommand(resumeCommand);

        // Add effect command
        var effectCommand = new Command("effect", "Manage audio effects");
        
        // Add effect name as a required argument instead of an option
        var effectNameArg = new Argument<string>(
            name: "effect-name",
            description: "Name of the effect to manage");

        effectCommand.AddArgument(effectNameArg);
        
        var enableOpt = new Option<bool>(
            name: "--enable",
            getDefaultValue: () => true,
            description: "Enable (true) or disable (false) the effect");

        effectCommand.AddOption(enableOpt);

        effectCommand.Description = "Enable or disable audio effects to enhance your lofi beats.\n\n" +
                         "Available effects:\n" +
                         "  - vinyl:       Adds vinyl record crackle and noise for that authentic feel\n" +
                         "  - reverb:      Adds space and atmosphere to create depth\n" +
                         "  - lowpass:     Reduces high frequencies for that warm, mellow sound\n" +
                         "  - tapeflutter: Adds wow/flutter pitch drift and tape hiss for vintage vibes\n\n" +
                         "Examples:\n" +
                         "  effect vinyl              Enable vinyl effect\n" +
                         "  effect reverb             Enable reverb effect\n" +
                         "  effect lowpass --enable=false  Disable lowpass filter\n" +
                         "  effect tapeflutter        Enable tape flutter effect";

        effectCommand.SetHandler(async (string name, bool enable) =>
        {
            if (!ValidEffects.Contains(name, StringComparer.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Error: '{name}' is not a valid effect name.");
                Console.WriteLine($"Available effects are: {string.Join(", ", ValidEffects)}");
                return;
            }

            _logExecutingEffectCommand(_logger, name, enable, null);
            try
            {
                var action = enable ? "Enabling" : "Disabling";
                Console.Write($"{action} {name} effect... ");
                ShowSpinner($"{action} {name} effect", 1000);

                var response = await _serviceHelper.SendCommandAsync(
                    HttpMethod.Post,
                    $"effect?name={Uri.EscapeDataString(name)}&enable={enable}");

                var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
                if (!response.IsSuccessStatusCode)
                {
                    _logEffectNotFound(_logger, name, null);
                    Console.WriteLine($"Error: {result?.Error ?? $"Effect '{name}' not found"}");
                    Console.WriteLine($"Available effects are: {string.Join(", ", ValidEffects)}");
                    return;
                }

                if (result?.Message != null)
                {
                    Console.WriteLine(result.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error managing effect: {ex.Message}");
                Console.WriteLine("Please ensure the LofiBeats service is running and try again.");
            }
        }, effectNameArg, enableOpt);
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
            _logSettingVolume(_logger, level, null);
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
            _logExecutingShutdownCommand(_logger, null);
            try
            {
                Console.Write("Shutting down service... ");
                ShowSpinner("Shutting down service", 1000);
                await _serviceHelper.ShutdownServiceAsync();
                Console.WriteLine("Service has been shut down.");
            }
            catch (HttpRequestException)
            {
                Console.WriteLine("Service is not running.");
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
            _logExecutingVersionCommand(_logger, null);
            Console.WriteLine("LofiBeats CLI v0.1.0");
        });
        _rootCommand.AddCommand(versionCommand);

        // Add interactive command
        var interactiveCommand = new Command("interactive", "Enters an interactive mode for real-time control");
        interactiveCommand.SetHandler(async (InvocationContext context) =>
        {
            var cancellationToken = context.GetCancellationToken();
            _logEnteringInteractiveMode(_logger, null);
            Console.WriteLine("Entering interactive mode. Type 'help' for commands, 'exit' to quit.");
            
            while (!cancellationToken.IsCancellationRequested)
            {
                if (cancellationToken.IsCancellationRequested) break;

                Console.Write("> ");
                
                // Use ReadLine with a small delay to check for cancellation
                var line = await Task.Run(() => 
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        if (Console.KeyAvailable)
                        {
                            return Console.ReadLine();
                        }
                        Thread.Sleep(100);
                    }
                    return null;
                }, cancellationToken);

                if (cancellationToken.IsCancellationRequested || line == null) break;
                
                if (string.IsNullOrWhiteSpace(line)) continue;
                if (line.Equals("exit", StringComparison.OrdinalIgnoreCase)) break;
                if (line.Equals("help", StringComparison.OrdinalIgnoreCase))
                {
                    // Show available commands
                    Console.WriteLine("\nAvailable commands:");
                    Console.WriteLine("  generate [--style=<style>] - Generate a new beat pattern");
                    Console.WriteLine("  play [--style=<style>]    - Play a new lofi beat");
                    Console.WriteLine("  stop [--tapestop]         - Stop audio playback");
                    Console.WriteLine("  pause                     - Pause audio playback");
                    Console.WriteLine("  resume                    - Resume audio playback");
                    Console.WriteLine("  effect <name> [--enable=true|false] - Manage effects");
                    Console.WriteLine("  volume --level=<0.0-1.0>  - Adjust master volume");
                    Console.WriteLine("  version                   - Display version information");
                    Console.WriteLine("  update                    - Update to latest version");
                    Console.WriteLine("  help                      - Show this help message");
                    Console.WriteLine("  exit                      - Exit interactive mode\n");
                    continue;
                }

                try
                {
                    // Split the line into args and invoke the root command
                    var args = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    await _rootCommand.InvokeAsync(args);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            Console.WriteLine("Exiting interactive mode.");
        });
        _rootCommand.AddCommand(interactiveCommand);

        // Add help examples to the root command
        _rootCommand.Description = "A command-line application for generating and playing lofi beats\n\n" +
            "Examples:\n" +
            "  Generate a jazzy beat:        lofi generate --style=jazzy\n" +
            "  Play with a specific style:   lofi play --style=chillhop\n" +
            "  Enable vinyl effect:          lofi effect vinyl\n" +
            "  Adjust volume:                lofi volume --level=0.8\n" +
            "  Interactive mode:             lofi interactive\n" +
            "  Update to latest version:     lofi update\n\n" +
            "For more information about a command, run: lofi help <command>";

        _logCommandsConfigured(_logger, null);
    }

    public async Task<int> ExecuteAsync(string[] args, CancellationToken cancellationToken = default)
    {
        _logExecutingCommand(_logger, string.Join(" ", args), null);
        return await _rootCommand.InvokeAsync(args);
    }

    public void Dispose()
    {
        (_serviceHelper as IDisposable)?.Dispose();
    }
}
