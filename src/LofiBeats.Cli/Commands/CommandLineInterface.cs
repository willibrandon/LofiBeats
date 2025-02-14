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
        var playCommand = new Command("play", "Starts audio playback");
        playCommand.SetHandler(() =>
        {
            _logger.LogInformation("Executing play command");
            
            // Create and set up the audio chain
            var testTone = new AutoDisposingSampleProvider(new TestTone());
            _playbackService.SetSource(testTone);
            
            // Add vinyl effect to the chain
            var effect = _effectFactory.CreateEffect("vinyl", testTone);
            if (effect != null)
            {
                _playbackService.AddEffect(effect);
            }
            
            _playbackService.StartPlayback();
            Console.WriteLine("Playing test tone with vinyl effect... Press Enter to stop");
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
                var testTone = new AutoDisposingSampleProvider(new TestTone());
                var newEffect = _effectFactory.CreateEffect(name, testTone);

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