using NAudio.Wave;
using Microsoft.Extensions.Logging;
using LofiBeats.Core.PluginManagement;

namespace LofiBeats.Core.Effects;

public interface IEffectFactory
{
    IAudioEffect CreateEffect(string effectName, ISampleProvider source);
}

public class EffectFactory : IEffectFactory
{
    private readonly ILogger<EffectFactory> _logger;
    private readonly ILoggerFactory _loggerFactory;
    private readonly PluginManager _pluginManager;

    public EffectFactory(ILoggerFactory loggerFactory, PluginManager pluginManager)
    {
        _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        _pluginManager = pluginManager ?? throw new ArgumentNullException(nameof(pluginManager));
        _logger = loggerFactory.CreateLogger<EffectFactory>();
    }

    public IAudioEffect CreateEffect(string effectName, ISampleProvider source)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));

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
        var effect = _pluginManager.CreateEffect(effectName, source);
        if (effect == null)
        {
            throw new ArgumentException($"Unknown effect: {effectName}", nameof(effectName));
        }
        return effect;
    }
} 