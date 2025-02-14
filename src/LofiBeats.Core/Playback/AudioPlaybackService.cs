using LofiBeats.Core.Effects;
using Microsoft.Extensions.Logging;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace LofiBeats.Core.Playback;

public class AudioPlaybackService : IAudioPlaybackService, IDisposable
{
    private readonly ILogger<AudioPlaybackService> _logger;
    private WaveOutEvent _waveOut;
    private MixingSampleProvider _mixer;
    private List<IAudioEffect> _effects = new List<IAudioEffect>();

    public AudioPlaybackService(ILogger<AudioPlaybackService> logger)
    {
        _logger = logger;
        _waveOut = new WaveOutEvent();

        _mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(44100, 2))
        {
            ReadFully = true
        };
        _waveOut.Init(_mixer);
    }

    public void StartPlayback()
    {
        if (_waveOut.PlaybackState != PlaybackState.Playing)
        {
            _waveOut.Play();
            _logger.LogInformation("Playback started.");
        }
    }

    public void StopPlayback()
    {
        _waveOut.Stop();
        _logger.LogInformation("Playback stopped.");
    }

    public void AddEffect(IAudioEffect effect)
    {
        _effects.Add(effect);
        _mixer.AddMixerInput(effect);
        _logger.LogInformation($"{effect.Name} effect added.");
    }

    public void RemoveEffect(string effectName)
    {
        var effectToRemove = _effects.Find(e => e.Name.Equals(effectName, StringComparison.OrdinalIgnoreCase));
        if (effectToRemove != null)
        {
            _mixer.RemoveMixerInput(effectToRemove);
            _effects.Remove(effectToRemove);
            _logger.LogInformation($"{effectName} effect removed.");
        }
    }

    public void Dispose()
    {
        _waveOut?.Dispose();
    }
} 