using System.CommandLine;
using Microsoft.Extensions.Logging;
using LofiBeats.Core.BeatGeneration;
using LofiBeats.Core.Effects;
using LofiBeats.Core.Playback;

namespace LofiBeats.Cli.Commands;

public class CommandLineInterface
{
    private readonly RootCommand _rootCommand;
    private readonly ILogger<CommandLineInterface> _logger;
    private readonly IBeatGenerator _beatGenerator;
    private readonly IAudioPlaybackService _playbackService;
    private readonly IEffectFactory _effectFactory;

    public CommandLineInterface(
        ILogger<CommandLineInterface> logger,
        IBeatGenerator beatGenerator,
        IAudioPlaybackService playbackService,
        IEffectFactory effectFactory)
    {
        _logger = logger;
        _beatGenerator = beatGenerator;
        _playbackService = playbackService;
        _effectFactory = effectFactory;
        
        _rootCommand = new RootCommand("Lofi Beats Generator & Player CLI")
        {
            Description = "A command-line application for generating and playing lofi beats"
        };

        ConfigureCommands();
        _logger.LogInformation("CommandLineInterface initialized");
    }

    private void ConfigureCommands()
    {
        // Add generate command
        var generateCommand = new Command("generate", "Generates a new beat pattern");
        generateCommand.SetHandler(() =>
        {
            _logger.LogInformation("Executing generate command");
            var pattern = _beatGenerator.GeneratePattern();
            Console.WriteLine($"Generated pattern: {pattern}");
        });
        _rootCommand.AddCommand(generateCommand);

        // Add play command
        var playCommand = new Command("play", "Plays the current or generated beat");
        playCommand.SetHandler(() =>
        {
            _logger.LogInformation("Executing play command");
            
            // Generate a new beat pattern if none exists
            var pattern = _beatGenerator.GeneratePattern();
            Console.WriteLine($"Playing pattern: {pattern}");
            
            // Create and set up the audio chain based on the pattern
            var beatSource = new BeatPatternSampleProvider(pattern, _logger);
            _playbackService.SetSource(beatSource);
            
            // Add vinyl effect by default for that lofi feel
            var effect = _effectFactory.CreateEffect("vinyl", beatSource);
            if (effect != null)
            {
                _playbackService.AddEffect(effect);
            }
            
            _playbackService.StartPlayback();
            Console.WriteLine("Playing lofi beat... Press Enter to stop");
            Console.ReadLine(); // Wait for user input before stopping
            _playbackService.StopPlayback();
        });
        _rootCommand.AddCommand(playCommand);

        // Add stop command
        var stopCommand = new Command("stop", "Stops audio playback");
        stopCommand.SetHandler(() =>
        {
            _logger.LogInformation("Executing stop command");
            _playbackService.StopPlayback();
            Console.WriteLine("Playback stopped.");
        });
        _rootCommand.AddCommand(stopCommand);

        // Add effect command
        var effectCommand = new Command("effect", "Manage effects");
        var effectNameOpt = new Option<string>("--name", "Name of the effect to manage") { IsRequired = true };
        var enableOpt = new Option<bool>("--enable", () => true, "Whether to enable or disable the effect");

        effectCommand.AddOption(effectNameOpt);
        effectCommand.AddOption(enableOpt);

        effectCommand.SetHandler((string name, bool enable) =>
        {
            _logger.LogInformation("Executing effect command with name: {Name}, enable: {Enable}", name, enable);

            if (enable)
            {
                var currentSource = _playbackService.CurrentSource;
                if (currentSource == null)
                {
                    Console.WriteLine("No audio source is currently playing. Start playback first.");
                    return;
                }

                var newEffect = _effectFactory.CreateEffect(name, currentSource);
                if (newEffect != null)
                {
                    _playbackService.AddEffect(newEffect);
                    Console.WriteLine($"{name} effect enabled.");
                }
                else
                {
                    Console.WriteLine($"Effect '{name}' not recognized.");
                }
            }
            else
            {
                _playbackService.RemoveEffect(name);
                Console.WriteLine($"{name} effect disabled.");
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

        volumeCommand.SetHandler((float level) =>
        {
            _logger.LogInformation("Setting volume to: {Level}", level);
            _playbackService.SetVolume(level);
            Console.WriteLine($"Volume set to: {level:F2}");
        }, volumeOption);
        _rootCommand.AddCommand(volumeCommand);

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
} 