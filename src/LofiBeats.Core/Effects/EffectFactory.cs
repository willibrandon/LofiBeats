using NAudio.Wave;
using Microsoft.Extensions.Logging;
using LofiBeats.Core.PluginManagement;
using LofiBeats.Core.PluginApi;

namespace LofiBeats.Core.Effects;

public class EffectFactory(ILoggerFactory loggerFactory, PluginManager pluginManager) : IEffectFactory
{
    private readonly ILogger<EffectFactory> _logger = loggerFactory.CreateLogger<EffectFactory>();
    private readonly ILoggerFactory _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
    private readonly PluginManager _pluginManager = pluginManager ?? throw new ArgumentNullException(nameof(pluginManager));

    public IAudioEffect CreateEffect(string effectName, ISampleProvider source)
    {
        ArgumentNullException.ThrowIfNull(source);

        _logger.LogInformation("Creating effect: {EffectName}", effectName);

        return effectName.ToLower() switch
        {
            "vinyl" => new VinylCrackleEffect(source, _loggerFactory.CreateLogger<VinylCrackleEffect>()),
            "lowpass" => new LowPassFilterEffect(source, _loggerFactory.CreateLogger<LowPassFilterEffect>()),
            "reverb" => new ReverbEffect(source, _loggerFactory.CreateLogger<ReverbEffect>()),
            "tapeflutter" => new TapeFlutterAndHissEffect(source, _loggerFactory.CreateLogger<TapeFlutterAndHissEffect>()),
            "tapestop" => new TapeStopEffect(source, _loggerFactory.CreateLogger<TapeStopEffect>()),
            _ => CreatePluginEffect(effectName, source)
        };
    }

    private IAudioEffect CreatePluginEffect(string effectName, ISampleProvider source)
    {
        var effect = _pluginManager.CreateEffect(effectName, source)
            ?? throw new ArgumentException($"Unknown effect: {effectName}", nameof(effectName));
        
        return effect;
    }
} 