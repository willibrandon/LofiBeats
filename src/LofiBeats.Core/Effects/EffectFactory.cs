using NAudio.Wave;
using Microsoft.Extensions.Logging;

namespace LofiBeats.Core.Effects;

public interface IEffectFactory
{
    IAudioEffect CreateEffect(string effectName, ISampleProvider source);
}

public class EffectFactory : IEffectFactory
{
    private readonly ILogger<EffectFactory> _logger;
    private readonly ILoggerFactory _loggerFactory;

    public EffectFactory(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
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
            _ => throw new ArgumentException($"Unknown effect: {effectName}", nameof(effectName))
        };
    }
} 