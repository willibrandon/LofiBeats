using LofiBeats.Core.Models;
using LofiBeats.Core.Scheduling;
using LofiBeats.Core.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace LofiBeats.Cli.Commands;

public class CommandLineInterface : IDisposable
{
    private readonly RootCommand _rootCommand;
    private readonly ILogger<CommandLineInterface> _logger;
    private readonly ServiceConnectionHelper _serviceHelper;
    private readonly PlaybackScheduler _scheduler;

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

    private static readonly Action<ILogger, string, TimeSpan, Exception?> _logSchedulingStop =
        LoggerMessage.Define<string, TimeSpan>(LogLevel.Information, new EventId(16, "SchedulingStop"), "Scheduling stop with {Effect} in {Delay}");

    private static readonly Action<ILogger, Guid, Exception> _logScheduledStopError =
        LoggerMessage.Define<Guid>(LogLevel.Error, new EventId(17, "ScheduledStopError"), "Error executing scheduled stop {ActionId}");

    private static readonly Action<ILogger, Exception?> _logExecutingStartCommand =
        LoggerMessage.Define(LogLevel.Information, new EventId(19, "ExecutingStartCommand"), "Executing start command");

    private static readonly Action<ILogger, Exception?> _logServiceStarted =
        LoggerMessage.Define(LogLevel.Information, new EventId(20, "ServiceStarted"), "LofiBeats service started successfully");

    private static readonly Action<ILogger, string, Exception?> _logExecutingSavePreset =
        LoggerMessage.Define<string>(LogLevel.Information, new EventId(21, "ExecutingSavePreset"), "Saving preset to {FilePath}");

    private static readonly Action<ILogger, string, Exception?> _logExecutingLoadPreset =
        LoggerMessage.Define<string>(LogLevel.Information, new EventId(22, "ExecutingLoadPreset"), "Loading preset from {FilePath}");

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

    public CommandLineInterface(ILogger<CommandLineInterface> logger, ILoggerFactory loggerFactory, IConfiguration configuration)
    {
        _logger = logger;
        _serviceHelper = new ServiceConnectionHelper(
            loggerFactory.CreateLogger<ServiceConnectionHelper>(),
            configuration);
        _scheduler = new PlaybackScheduler(loggerFactory.CreateLogger<PlaybackScheduler>());

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

        // Add preset command
        var presetCommand = CreatePresetCommand();
        _rootCommand.AddCommand(presetCommand);

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

        var generateBpmOption = new Option<int?>(
            "--bpm",
            "Tempo in beats per minute (BPM). Default is style-dependent.");
        generateBpmOption.AddValidator(result =>
        {
            var value = result.GetValueOrDefault<int?>();
            if (value.HasValue && (value < 60 || value > 140))
            {
                result.ErrorMessage = "BPM must be between 60 and 140";
            }
        });
        generateCommand.AddOption(generateBpmOption);

        generateCommand.SetHandler(async (string style, int? bpm) =>
        {
            _logExecutingGenerateCommand(_logger, null);
            try
            {
                Console.Write("Generating beat pattern... ");
                ShowSpinner("Generating beat pattern", 1500); // Show progress for 1.5 seconds

                var response = await _serviceHelper.SendCommandAsync(HttpMethod.Post, $"generate?style={Uri.EscapeDataString(style)}&bpm={bpm}");
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
        }, generateStyleOption, generateBpmOption);
        _rootCommand.AddCommand(generateCommand);

        // Add play command
        var playCommand = new Command("play", "Plays a new lofi beat");
        playCommand.Description = "Plays a new lofi beat with the specified style.\n\n" +
                                "Options:\n" +
                                "  --style    Beat style (basic, jazzy, chillhop, hiphop)\n" +
                                "  --bpm      Tempo in beats per minute (BPM)\n" +
                                "  --after    Specify a delay (e.g. '10m' or '30s') before starting\n\n" +
                                "Examples:\n" +
                                "  play                     Play with default style\n" +
                                "  play --style=chillhop    Play chillhop style\n" +
                                "  play --style=jazzy --bpm=85  Play jazzy style at 85 BPM\n" +
                                "  play --after 5m          Start playing in 5 minutes\n" +
                                "  play --style=hiphop --after 30s  Play hiphop style in 30 seconds";

        var styleOption = new Option<string>("--style", () => "basic", "Beat style (basic, jazzy, chillhop, hiphop)");
        var bpmOption = new Option<int?>(
            "--bpm",
            "Tempo in beats per minute (BPM). Default is style-dependent.");
        bpmOption.AddValidator(result =>
        {
            var value = result.GetValueOrDefault<int?>();
            if (value.HasValue && (value < 60 || value > 140))
            {
                result.ErrorMessage = "BPM must be between 60 and 140";
            }
        });

        var afterOption = new Option<string?>(
            "--after",
            description: "Specify a delay (e.g. '10m' or '30s') before starting");

        playCommand.AddOption(styleOption);
        playCommand.AddOption(bpmOption);
        playCommand.AddOption(afterOption);

        playCommand.SetHandler(async (string style, int? bpm, string? afterValue) =>
        {
            _logExecutingPlayCommand(_logger, null);
            try
            {
                // If --after is specified, schedule the play instead of immediate
                if (!string.IsNullOrEmpty(afterValue))
                {
                    var delay = DelayParser.ParseDelay(afterValue);
                    if (delay == null)
                    {
                        Console.WriteLine($"Invalid delay format: {afterValue}");
                        Console.WriteLine("Use formats like '10m' for minutes, '30s' for seconds, or '2h' for hours.");
                        return;
                    }

                    // Schedule the play call
                    await SchedulePlay(style, bpm, delay.Value);
                }
                else
                {
                    // immediate play
                    await StartPlayback(style, bpm);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }, styleOption, bpmOption, afterOption);
        _rootCommand.AddCommand(playCommand);

        // Add stop command
        var stopCommand = new Command("stop", "Stops audio playback");
        stopCommand.Description = "Stops the currently playing audio.\n\n" +
                                "Options:\n" +
                                "  --tapestop    Gradually slow down the audio like a tape machine powering off\n" +
                                "  --after       Specify a delay (e.g. '10m' or '30s') before stopping\n\n" +
                                "Examples:\n" +
                                "  stop              Stop playback immediately\n" +
                                "  stop --tapestop   Stop with tape slow-down effect\n" +
                                "  stop --after 10m  Stop after 10 minutes\n" +
                                "  stop --after 30s --tapestop  Stop with tape effect after 30 seconds";

        var tapeStopOpt = new Option<bool>(
            name: "--tapestop",
            getDefaultValue: () => false,
            description: "Gradually slow pitch to zero before stopping");
        stopCommand.AddOption(tapeStopOpt);

        var afterOpt = new Option<string?>(
            name: "--after",
            description: "Specify a delay (e.g. '10m' or '30s') before stopping");
        stopCommand.AddOption(afterOpt);

        stopCommand.SetHandler(async (bool tapeStop, string? afterValue) =>
        {
            _logExecutingStopCommand(_logger, null);
            try
            {
                // If --after is specified, schedule the stop instead of immediate
                if (!string.IsNullOrEmpty(afterValue))
                {
                    var delay = DelayParser.ParseDelay(afterValue);
                    if (delay == null)
                    {
                        Console.WriteLine($"Invalid delay format: {afterValue}");
                        Console.WriteLine("Use formats like '10m' for minutes, '30s' for seconds, or '2h' for hours.");
                        return;
                    }

                    // Schedule the stop call
                    await ScheduleStop(tapeStop, delay.Value);
                }
                else
                {
                    // immediate stop
                    await StopPlayback(tapeStop);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }, tapeStopOpt, afterOpt);
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

        // Add start command
        var startCommand = new Command("start", "Starts the LofiBeats service");
        startCommand.Description = "Starts the LofiBeats service if it's not already running.\n\n" +
                                 "This command is useful when you want to:\n" +
                                 "1. Start the service explicitly before using other commands\n" +
                                 "2. Restart the service after a shutdown\n" +
                                 "3. Ensure the service is running without executing any playback commands";
        
        startCommand.SetHandler(async () =>
        {
            _logExecutingStartCommand(_logger, null);
            try
            {
                Console.Write("Starting LofiBeats service... " + Environment.NewLine);
                ShowSpinner("Starting LofiBeats service", 1000);
                
                // EnsureServiceRunningAsync will start the service if it's not running
                await _serviceHelper.EnsureServiceRunningAsync();
                _logServiceStarted(_logger, null);
                Console.WriteLine("LofiBeats service is now running.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting service: {ex.Message}");
            }
        });
        _rootCommand.AddCommand(startCommand);

        // Add shutdown command
        var shutdownCommand = new Command("shutdown", "Shuts down the LofiBeats service");
        shutdownCommand.SetHandler(async () =>
        {
            _logExecutingShutdownCommand(_logger, null);
            try
            {
                Console.Write("Shutting down service... " + Environment.NewLine);
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
            var version = typeof(CommandLineInterface).Assembly.GetName().Version;
            Console.WriteLine($"LofiBeats CLI v{version?.ToString(3)}");
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
                    Console.WriteLine("  generate [--style=<style>] [--bpm=<60-140>] - Generate a new beat pattern");
                    Console.WriteLine("  play [--style=<style>] [--bpm=<60-140>]    - Play a new lofi beat");
                    Console.WriteLine("  stop [--tapestop]                          - Stop audio playback");
                    Console.WriteLine("  pause                                      - Pause audio playback");
                    Console.WriteLine("  resume                                     - Resume audio playback");
                    Console.WriteLine("  effect <name> [--enable=true|false]       - Manage effects");
                    Console.WriteLine("  volume --level=<0.0-1.0>                  - Adjust master volume");
                    Console.WriteLine("  version                                   - Display version information");
                    Console.WriteLine("  update                                    - Update to latest version");
                    Console.WriteLine("  help                                      - Show this help message");
                    Console.WriteLine("  exit                                      - Exit interactive mode\n");
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

        // Add schedule command
        var scheduleCommand = new Command("schedule", "Manage scheduled actions");
        scheduleCommand.Description = "List or cancel scheduled playback actions.\n\n" +
                                    "Subcommands:\n" +
                                    "  list     List all scheduled actions\n" +
                                    "  cancel   Cancel a scheduled action by ID\n\n" +
                                    "Examples:\n" +
                                    "  schedule list              Show all scheduled actions\n" +
                                    "  schedule cancel <id>       Cancel a specific scheduled action";

        var listCommand = new Command("list", "List all scheduled actions");
        listCommand.SetHandler(() =>
        {
            var actions = _scheduler.GetScheduledActions();
            if (actions.Count == 0)
            {
                Console.WriteLine("No scheduled actions.");
                return;
            }

            Console.WriteLine("Scheduled actions:");
            foreach (var (id, description) in actions)
            {
                Console.WriteLine($"  {id}: {description}");
            }
        });
        scheduleCommand.AddCommand(listCommand);

        var cancelCommand = new Command("cancel", "Cancel a scheduled action");
        var idArg = new Argument<string>("action-id", "The ID of the action to cancel");
        cancelCommand.AddArgument(idArg);
        cancelCommand.SetHandler((string actionId) =>
        {
            if (!Guid.TryParse(actionId, out var id))
            {
                Console.WriteLine($"Invalid action ID format: {actionId}");
                return;
            }

            if (_scheduler.CancelAction(id))
            {
                Console.WriteLine($"Cancelled action {id}");
            }
            else
            {
                Console.WriteLine($"No action found with ID {id}");
            }
        }, idArg);
        scheduleCommand.AddCommand(cancelCommand);

        _rootCommand.AddCommand(scheduleCommand);

        // Update help examples in the root command
        _rootCommand.Description = "A command-line application for generating and playing lofi beats\n\n" +
            "Examples:\n" +
            "  Start the service:            lofi start\n" +
            "  Generate a jazzy beat:        lofi generate --style=jazzy\n" +
            "  Generate with custom BPM:     lofi generate --style=chillhop --bpm=85\n" +
            "  Play with a specific style:   lofi play --style=chillhop\n" +
            "  Play with custom BPM:         lofi play --style=basic --bpm=75\n" +
            "  Enable vinyl effect:          lofi effect vinyl\n" +
            "  Adjust volume:                lofi volume --level=0.8\n" +
            "  Interactive mode:             lofi interactive\n" +
            "  Update to latest version:     lofi update\n" +
            "  Shutdown the service:         lofi shutdown\n\n" +
            "For more information about a command, run: lofi help <command>";

        _logCommandsConfigured(_logger, null);
    }

    private Command CreatePresetCommand()
    {
        var presetCommand = new Command("preset", "Manage LofiBeats presets (style, volume, effects)");
        presetCommand.Description = "Save and load preset configurations including style, volume, and effects.\n\n" +
                                  "Subcommands:\n" +
                                  "  save <file>    Save current settings to a preset file\n" +
                                  "  load <file>    Load settings from a preset file\n\n" +
                                  "Examples:\n" +
                                  "  preset save mypreset.json     Save current settings\n" +
                                  "  preset load mypreset.json     Load saved settings\n" +
                                  "  preset save presets/jazz.json Save to a subdirectory";

        // 'save' subcommand
        var saveCommand = new Command("save", "Save the current playback settings to a preset file");
        var saveFileArg = new Argument<string>("file", "Path to the preset file (e.g., mypreset.json)");
        saveCommand.AddArgument(saveFileArg);
        saveCommand.SetHandler(async (string file) =>
        {
            _logExecutingSavePreset(_logger, file, null);
            await HandleSavePreset(file);
        }, saveFileArg);

        // 'load' subcommand
        var loadCommand = new Command("load", "Load preset settings from a file");
        var loadFileArg = new Argument<string>("file", "Path to the preset file (e.g., mypreset.json)");
        loadCommand.AddArgument(loadFileArg);
        loadCommand.SetHandler(async (string file) =>
        {
            _logExecutingLoadPreset(_logger, file, null);
            await HandleLoadPreset(file);
        }, loadFileArg);

        presetCommand.AddCommand(saveCommand);
        presetCommand.AddCommand(loadCommand);

        return presetCommand;
    }

    private async Task HandleSavePreset(string filePath)
    {
        try
        {
            Console.Write("Saving preset... ");
            ShowSpinner("Saving preset", 500);

            // Get current preset from service
            var response = await _serviceHelper.SendCommandAsync(HttpMethod.Get, "preset/current");
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<ApiResponse>();
                Console.WriteLine($"Error getting current preset: {error?.Error ?? "Unknown error"}");
                return;
            }

            var preset = await response.Content.ReadFromJsonAsync<Preset>();
            if (preset == null)
            {
                Console.WriteLine("Error: Failed to get current preset state");
                return;
            }

            // Save to file
            PresetStorage.SavePreset(filePath, preset);
            Console.WriteLine($"Preset saved to {filePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving preset: {ex.Message}");
            Console.WriteLine("Please ensure the LofiBeats service is running and try again.");
        }
    }

    private async Task HandleLoadPreset(string filePath)
    {
        try
        {
            if (!PresetStorage.PresetExists(filePath))
            {
                Console.WriteLine($"Error: Preset file not found: {filePath}");
                return;
            }

            Console.Write("Loading preset... ");
            ShowSpinner("Loading preset", 500);

            // Load from file
            var preset = PresetStorage.LoadPreset(filePath);
            if (preset == null)
            {
                Console.WriteLine($"Error: Failed to load preset from {filePath}");
                return;
            }

            // Apply preset through service
            var response = await _serviceHelper.SendCommandAsync(
                HttpMethod.Post,
                "preset/apply",
                preset);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<ApiResponse>();
                Console.WriteLine($"Error applying preset: {error?.Error ?? "Unknown error"}");
                return;
            }

            Console.WriteLine($"Preset '{preset.Name}' loaded successfully:");
            Console.WriteLine($"  Style: {preset.Style}");
            Console.WriteLine($"  Volume: {preset.Volume:P0}");
            Console.WriteLine($"  Effects: {(preset.Effects.Count > 0 ? string.Join(", ", preset.Effects) : "none")}");
            Console.WriteLine("\nTo start playback with these settings, run:");
            Console.WriteLine($"  lofi play");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading preset: {ex.Message}");
            Console.WriteLine("Please ensure the LofiBeats service is running and try again.");
        }
    }

    public async Task<int> ExecuteAsync(string[] args, CancellationToken cancellationToken = default)
    {
        _logExecutingCommand(_logger, string.Join(" ", args), null);
        return await _rootCommand.InvokeAsync(args);
    }

    public void Dispose()
    {
        (_serviceHelper as IDisposable)?.Dispose();
        _scheduler.Dispose();
    }

    private async Task ScheduleStop(bool tapeStop, TimeSpan delay)
    {
        _logSchedulingStop(_logger, tapeStop ? "tape stop" : "immediate stop", delay, null);
        var totalMs = (int)delay.TotalMilliseconds;

        // Create a TaskCompletionSource to wait for the action to complete
        var tcs = new TaskCompletionSource();

        // Create a closure to capture the action ID
        Guid actionId = Guid.Empty;
        var callback = new Action(() =>
        {
            Task.Run(async () =>
            {
                try
                {
                    await StopPlayback(tapeStop);
                    tcs.SetResult(); // Signal completion
                }
                catch (Exception ex)
                {
                    _logScheduledStopError(_logger, actionId, ex);
                    tcs.SetException(ex); // Signal error
                }
            });
        });

        // Schedule the actual stop call with a descriptive name
        var description = $"Stop playback{(tapeStop ? " with tape effect" : "")} at {DateTime.Now + delay:HH:mm:ss}";
        actionId = _scheduler.ScheduleAction(totalMs, callback, description);

        Console.WriteLine($"Playback will stop{(tapeStop ? " with tape effect" : "")} in {delay.TotalSeconds:F1} seconds.");
        Console.WriteLine($"Action ID: {actionId}");
        
        // Show a progress indicator while waiting
        var startTime = DateTime.Now;
        var endTime = startTime + delay;
        
        while (DateTime.Now < endTime && !tcs.Task.IsCompleted)
        {
            var remaining = endTime - DateTime.Now;
            Console.Write($"\rTime remaining: {remaining.TotalSeconds:F1}s    ");
            await Task.Delay(100); // Update every 100ms
        }
        Console.WriteLine(); // Clear the progress line

        // Wait for the action to complete
        await tcs.Task;
    }

    private async Task StopPlayback(bool tapeStop)
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

    private async Task SchedulePlay(string style, int? bpm, TimeSpan delay)
    {
        _logSchedulingStop(_logger, $"play {style} at {bpm} BPM in {delay.TotalSeconds} seconds", delay, null);
        var totalMs = (int)delay.TotalMilliseconds;

        // Create a TaskCompletionSource to wait for the action to complete
        var tcs = new TaskCompletionSource();

        // Create a closure to capture the action ID
        Guid actionId = Guid.Empty;
        var callback = new Action(() =>
        {
            Task.Run(async () =>
            {
                try
                {
                    await StartPlayback(style, bpm);
                    tcs.SetResult(); // Signal completion
                }
                catch (Exception ex)
                {
                    _logScheduledStopError(_logger, actionId, ex);
                    tcs.SetException(ex); // Signal error
                }
            });
        });

        // Schedule the actual play call with a descriptive name
        var description = $"Play {style} style{(bpm.HasValue ? $" at {bpm} BPM" : "")} at {DateTime.Now + delay:HH:mm:ss}";
        actionId = _scheduler.ScheduleAction(totalMs, callback, description);

        Console.WriteLine($"Playback will start in {delay.TotalSeconds:F1} seconds.");
        Console.WriteLine($"Action ID: {actionId}");
        
        // Show a progress indicator while waiting
        var startTime = DateTime.Now;
        var endTime = startTime + delay;
        
        while (DateTime.Now < endTime && !tcs.Task.IsCompleted)
        {
            var remaining = endTime - DateTime.Now;
            Console.Write($"\rTime remaining: {remaining.TotalSeconds:F1}s    ");
            await Task.Delay(100); // Update every 100ms
        }
        Console.WriteLine(); // Clear the progress line

        // Wait for the action to complete
        await tcs.Task;
    }

    private async Task StartPlayback(string style, int? bpm)
    {
        try
        {
            // If no style is specified (i.e. it's the default "basic"), get the current style from the service
            if (style == "basic")
            {
                var currentResponse = await _serviceHelper.SendCommandAsync(HttpMethod.Get, "preset/current");
                if (currentResponse.IsSuccessStatusCode)
                {
                    var currentPreset = await currentResponse.Content.ReadFromJsonAsync<Preset>();
                    if (currentPreset != null)
                    {
                        style = currentPreset.Style;
                    }
                }
            }

            Console.Write($"Starting playback with {style} style{(bpm.HasValue ? $" at {bpm} BPM" : "")}... ");
            ShowSpinner("Starting playback", 1000);

            var response = await _serviceHelper.SendCommandAsync(HttpMethod.Post, $"play?style={Uri.EscapeDataString(style)}&bpm={bpm}");
            var result = await response.Content.ReadFromJsonAsync<PlayResponse>();
            if (result?.Pattern != null)
            {
                Console.WriteLine($"Playing new {style} beat pattern: {result.Pattern}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error starting playback: {ex.Message}");
            Console.WriteLine("Please ensure the LofiBeats service is running and try again.");
        }
    }
}
