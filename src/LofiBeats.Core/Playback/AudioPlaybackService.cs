using LofiBeats.Core.Effects;
using Microsoft.Extensions.Logging;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace LofiBeats.Core.Playback;

public class AudioPlaybackService : IAudioPlaybackService, IDisposable
{
    private readonly ILogger<AudioPlaybackService> _logger;
    private readonly IWavePlayer _outputDevice;
    private readonly MixingSampleProvider _mixer;
    private readonly List<IAudioEffect> _effects = new List<IAudioEffect>();
    private ISampleProvider? _currentSource;

    public AudioPlaybackService(ILogger<AudioPlaybackService> logger)
    {
        _logger = logger;
        
        // Create the output device and mixer
        _outputDevice = new WaveOutEvent();
        _mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(44100, 2))
        {
            ReadFully = true // Ensures continuous playback
        };

        // Initialize and start playing (will play silence when no inputs)
        _outputDevice.Init(_mixer);
        _outputDevice.Play();
        
        _logger.LogInformation("AudioPlaybackService initialized with continuous playback");
    }

    public void SetSource(ISampleProvider source)
    {
        // Convert mono to stereo if needed
        var convertedSource = ConvertToRightChannelCount(source);

        if (_currentSource != null)
        {
            _mixer.RemoveMixerInput(_currentSource);
        }

        _currentSource = convertedSource;
        _mixer.AddMixerInput(convertedSource);
        _logger.LogInformation("Audio source set and added to mixer");
    }

    private ISampleProvider ConvertToRightChannelCount(ISampleProvider input)
    {
        if (input.WaveFormat.Channels == _mixer.WaveFormat.Channels)
        {
            return input;
        }
        if (input.WaveFormat.Channels == 1 && _mixer.WaveFormat.Channels == 2)
        {
            return new MonoToStereoSampleProvider(input);
        }
        throw new NotImplementedException("Not yet implemented this channel count conversion");
    }

    public void StartPlayback()
    {
        if (_currentSource == null)
        {
            _logger.LogWarning("No audio source set. Adding test tone.");
            SetSource(new TestTone());
        }

        _logger.LogInformation("Playback active - mixer is continuously playing");
    }

    public void StopPlayback()
    {
        if (_currentSource != null)
        {
            _mixer.RemoveMixerInput(_currentSource);
            _currentSource = null;
        }
        _logger.LogInformation("Playback stopped - source removed from mixer");
    }

    public void AddEffect(IAudioEffect effect)
    {
        var convertedEffect = ConvertToRightChannelCount(effect);
        _effects.Add(effect);
        _mixer.AddMixerInput(convertedEffect);
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
        _outputDevice?.Dispose();
    }
} 