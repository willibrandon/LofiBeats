using LofiBeats.Core.Effects;
using Microsoft.Extensions.Logging;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace LofiBeats.Core.Playback;

public class AudioPlaybackService : IAudioPlaybackService, IDisposable
{
    private readonly ILogger<AudioPlaybackService> _logger;
    private readonly ILoggerFactory _loggerFactory;
    private readonly WaveOutEvent _waveOut;
    private readonly MixingSampleProvider _mixer;
    private ISampleProvider? _currentSource;
    private SerialEffectChain? _effectChain;
    private bool _isPaused;

    public ISampleProvider? CurrentSource => _currentSource;

    public AudioPlaybackService(ILogger<AudioPlaybackService> logger, ILoggerFactory loggerFactory)
    {
        _logger = logger;
        _loggerFactory = loggerFactory;

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

        // Remove existing inputs
        if (_currentSource != null)
        {
            _mixer.RemoveMixerInput(_currentSource);
        }
        if (_effectChain != null)
        {
            _mixer.RemoveMixerInput(_effectChain);
        }

        _currentSource = convertedSource;

        // Create a new effect chain with the current source
        _effectChain = new SerialEffectChain(_currentSource, _loggerFactory.CreateLogger<SerialEffectChain>(), _loggerFactory);
        _mixer.AddMixerInput(_effectChain);

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
        // Remove the effect chain if present
        if (_effectChain != null)
        {
            _mixer.RemoveMixerInput(_effectChain);
            _effectChain = null;
        }

        // Remove the current source
        _currentSource = null;

        _isPaused = false;
        _logger.LogInformation("Playback stopped - all sources and effects removed from mixer");
    }

    public void PausePlayback()
    {
        if (!_isPaused && _effectChain != null)
        {
            _mixer.RemoveMixerInput(_effectChain);
            _isPaused = true;
            _logger.LogInformation("Playback paused.");
        }
    }

    public void ResumePlayback()
    {
        if (_isPaused && _effectChain != null)
        {
            _mixer.AddMixerInput(_effectChain);
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
        if (_effectChain == null)
        {
            if (_currentSource == null)
            {
                _logger.LogWarning("Cannot add effect - no audio source set");
                return;
            }
            _effectChain = new SerialEffectChain(_currentSource, _loggerFactory.CreateLogger<SerialEffectChain>(), _loggerFactory);
            _mixer.AddMixerInput(_effectChain);
        }
        else
        {
            // Temporarily remove the chain from the mixer
            _mixer.RemoveMixerInput(_effectChain);
        }

        _effectChain.AddEffect(effect);
        
        // Re-add the chain to the mixer if it's not paused
        if (!_isPaused)
        {
            _mixer.AddMixerInput(_effectChain);
        }
        
        _logger.LogInformation($"{effect.Name} effect added.");
    }

    public void RemoveEffect(string effectName)
    {
        if (_effectChain != null)
        {
            _effectChain.RemoveEffect(effectName);
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
