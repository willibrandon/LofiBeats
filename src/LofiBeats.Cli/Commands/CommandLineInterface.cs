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

    private static readonly string[] ValidEffects = ["vinyl", "reverb", "lowpass"];
    private static readonly string[] ValidBeatStyles = ["basic", "jazzy", "chillhop"];

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
        // Add generate command with enhanced description and validation
        var generateCommand = new Command("generate", "Generates a new lofi beat pattern with customizable style")
        {
            Description = "Creates a new beat pattern using the specified style. Each style has unique characteristics:\n" +
                         "  - basic: Standard lofi beat pattern\n" +
                         "  - jazzy: Syncopated rhythms with jazz-inspired elements\n" +
                         "  - chillhop: Laid-back beats with hip-hop influence"
        };
        var generateStyleOption = new Option<string>(
            "--style",
            () => "basic",
            "Beat style (basic, jazzy, chillhop)");
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
        var styleOption = new Option<string>("--style", () => "basic", "Beat style (basic, jazzy, chillhop)");
        playCommand.AddOption(styleOption);

        playCommand.SetHandler(async (string style) =>
        {
            _logExecutingPlayCommand(_logger, null);
            try
            {
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
        stopCommand.SetHandler(async () =>
        {
            _logExecutingStopCommand(_logger, null);
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

        // Add pause command
        var pauseCommand = new Command("pause", "Pauses audio playback");
        pauseCommand.SetHandler(async () =>
        {
            _logExecutingPauseCommand(_logger, null);
            try
            {
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
                         "  - vinyl:   Adds vinyl record crackle and noise for that authentic feel\n" +
                         "  - reverb:  Adds space and atmosphere to create depth\n" +
                         "  - lowpass: Reduces high frequencies for that warm, mellow sound\n\n" +
                         "Examples:\n" +
                         "  effect vinyl              Enable vinyl effect\n" +
                         "  effect reverb             Enable reverb effect\n" +
                         "  effect lowpass --enable=false  Disable lowpass filter";

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
                Console.Write($"{(enable ? "Enabling" : "Disabling")} {name} effect... ");
                ShowSpinner($"{(enable ? "Enabling" : "Disabling")} {name} effect", 1000);

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
                await _serviceHelper.ShutdownServiceAsync();
            }
            catch (HttpRequestException)
            {
                // Don't show technical error for connection issues
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
        interactiveCommand.SetHandler(async () =>
        {
            _logEnteringInteractiveMode(_logger, null);
            Console.WriteLine("Entering interactive mode. Type 'help' for commands, 'exit' to quit.");
            
            while (true)
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                
                if (string.IsNullOrWhiteSpace(line)) continue;
                if (line.Equals("exit", StringComparison.OrdinalIgnoreCase)) break;
                if (line.Equals("help", StringComparison.OrdinalIgnoreCase))
                {
                    // Show available commands
                    Console.WriteLine("\nAvailable commands:");
                    Console.WriteLine("  generate [--style=<style>] - Generate a new beat pattern");
                    Console.WriteLine("  play [--style=<style>]    - Play a new lofi beat");
                    Console.WriteLine("  stop                      - Stop audio playback");
                    Console.WriteLine("  pause                     - Pause audio playback");
                    Console.WriteLine("  resume                    - Resume audio playback");
                    Console.WriteLine("  effect --name=<name> [--enable=true|false] - Manage effects");
                    Console.WriteLine("  volume --level=<0.0-1.0>  - Adjust master volume");
                    Console.WriteLine("  version                   - Display version information");
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
            "  Generate a jazzy beat:        lofi-beats generate --style=jazzy\n" +
            "  Play with a specific style:   lofi-beats play --style=chillhop\n" +
            "  Enable vinyl effect:          lofi-beats effect --name=vinyl --enable\n" +
            "  Adjust volume:                lofi-beats volume --level=0.8\n" +
            "  Interactive mode:             lofi-beats interactive";

        _logCommandsConfigured(_logger, null);
    }

    public async Task<int> ExecuteAsync(string[] args)
    {
        _logExecutingCommand(_logger, string.Join(" ", args), null);
        return await _rootCommand.InvokeAsync(args);
    }

    public void Dispose()
    {
        (_serviceHelper as IDisposable)?.Dispose();
    }
}
