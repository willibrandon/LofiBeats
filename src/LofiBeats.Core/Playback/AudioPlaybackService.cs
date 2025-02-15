using LofiBeats.Core.Effects;
using Microsoft.Extensions.Logging;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace LofiBeats.Core.Playback;

public class AudioPlaybackService : IAudioPlaybackService, IDisposable
{
    private readonly ILogger<AudioPlaybackService> _logger;
    private readonly WaveOutEvent _waveOut;
    private readonly MixingSampleProvider _mixer;
    private readonly List<IAudioEffect> _effects = new List<IAudioEffect>();
    private ISampleProvider? _currentSource;
    private bool _isPaused;  // Track if we're paused separately from _waveOut state

    public ISampleProvider? CurrentSource => _currentSource;

    public AudioPlaybackService(ILogger<AudioPlaybackService> logger)
    {
        _logger = logger;
        
        // Create the output device and mixer
        _waveOut = new WaveOutEvent();
        _mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(44100, 2))
        {
            ReadFully = true // Ensures continuous playback
        };

        // Initialize and start playing (will play silence when no inputs)
        _waveOut.Init(_mixer);
        _waveOut.Play();
        _isPaused = false;
        
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

        _isPaused = false;
        _logger.LogInformation("Playback active - mixer is continuously playing");
    }

    public void StopPlayback()
    {
        if (_currentSource != null)
        {
            _mixer.RemoveMixerInput(_currentSource);
            _currentSource = null;
        }
        _isPaused = false;
        _logger.LogInformation("Playback stopped - source removed from mixer");
    }

    public void PausePlayback()
    {
        if (!_isPaused && _currentSource != null)
        {
            _mixer.RemoveMixerInput(_currentSource);
            _isPaused = true;
            _logger.LogInformation("Playback paused.");
        }
    }

    public void ResumePlayback()
    {
        if (_isPaused && _currentSource != null)
        {
            _mixer.AddMixerInput(_currentSource);
            _isPaused = false;
            _logger.LogInformation("Playback resumed.");
        }
    }

    public PlaybackState GetPlaybackState()
    {
        if (_currentSource == null) return PlaybackState.Stopped;
        if (_isPaused) return PlaybackState.Paused;
        return PlaybackState.Playing;
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

    public void SetVolume(float volume)
    {
        // Volume range is 0.0f to 1.0f for WaveOutEvent
        _waveOut.Volume = Math.Clamp(volume, 0f, 1f);
        _logger.LogInformation($"Volume set to: {volume}");
    }

    public void Dispose()
    {
        _waveOut?.Dispose();
    }
} 