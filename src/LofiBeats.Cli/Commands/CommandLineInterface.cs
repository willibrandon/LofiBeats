using System.CommandLine;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;

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
        // Add generate command
        var generateCommand = new Command("generate", "Generates a new lofi beat pattern");
        var generateStyleOption = new Option<string>("--style", () => "basic", "Beat style (basic, jazzy, chillhop)");
        generateCommand.AddOption(generateStyleOption);

        generateCommand.SetHandler(async (string style) =>
        {
            _logExecutingGenerateCommand(_logger, null);
            try
            {
                var response = await _serviceHelper.SendCommandAsync(HttpMethod.Post, $"generate?style={Uri.EscapeDataString(style)}");
                var result = await response.Content.ReadFromJsonAsync<PlayResponse>();
                if (result?.Pattern != null)
                {
                    Console.WriteLine($"Generated new {style} beat pattern: {result.Pattern}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
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
        var effectCommand = new Command("effect", "Manage effects");
        var effectNameOpt = new Option<string>("--name", "Name of the effect to manage") { IsRequired = true };
        var enableOpt = new Option<bool>("--enable", () => true, "Whether to enable or disable the effect");

        effectCommand.AddOption(effectNameOpt);
        effectCommand.AddOption(enableOpt);

        effectCommand.SetHandler(async (string name, bool enable) =>
        {
            _logExecutingEffectCommand(_logger, name, enable, null);
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
