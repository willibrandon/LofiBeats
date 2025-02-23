using LofiBeats.Core.PluginApi;
using NAudio.Wave;

namespace LofiBeats.Core.Effects;

public interface IEffectFactory
{
    IAudioEffect CreateEffect(string effectName, ISampleProvider source);
}