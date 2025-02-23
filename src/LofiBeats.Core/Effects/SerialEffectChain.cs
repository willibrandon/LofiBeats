using NAudio.Wave;
using Microsoft.Extensions.Logging;
using LofiBeats.Core.PluginApi;

namespace LofiBeats.Core.Effects;

public class SerialEffectChain : ISampleProvider
{
    private readonly ILogger<SerialEffectChain> _logger;
    private readonly ISampleProvider _baseSource;
    private readonly List<IAudioEffect> _effects = [];
    private ISampleProvider _finalProvider;

    public WaveFormat WaveFormat => _finalProvider.WaveFormat;

    public SerialEffectChain(ISampleProvider source, ILogger<SerialEffectChain> logger, ILoggerFactory loggerFactory)
    {
        _baseSource = source ?? throw new ArgumentNullException(nameof(source));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _finalProvider = _baseSource;
    }

    public void AddEffect(IAudioEffect effect)
    {
        if (effect == null) throw new ArgumentNullException(nameof(effect));

        _logger.LogInformation("Adding effect {EffectName} to chain", effect.Name);

        // The new effect's source is the current final link in the chain
        effect.SetSource(_finalProvider);

        // Now the new effect becomes the final link
        _finalProvider = effect;

        // Keep track of it
        _effects.Add(effect);
    }

    public void RemoveEffect(string effectName)
    {
        var index = _effects.FindIndex(e => e.Name.Equals(effectName, StringComparison.OrdinalIgnoreCase));
        if (index < 0)
        {
            _logger.LogWarning("Effect {EffectName} not found in chain", effectName);
            return;
        }

        _logger.LogInformation("Removing effect {EffectName} from chain", effectName);
        _effects.RemoveAt(index);

        // Rebuild the chain from scratch
        RebuildChain();
    }

    private void RebuildChain()
    {
        // Start from the base source
        var current = _baseSource;

        // Re-wire each effect in the list
        foreach (var effect in _effects)
        {
            effect.SetSource(current);
            current = effect;
        }

        _finalProvider = current;
    }

    public int Read(float[] buffer, int offset, int count)
    {
        // Just delegate to the final link in the chain
        return _finalProvider.Read(buffer, offset, count);
    }
} 