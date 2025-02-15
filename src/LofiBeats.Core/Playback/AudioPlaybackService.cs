using Microsoft.Extensions.Logging;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using LofiBeats.Core.Effects;

namespace LofiBeats.Core.Playback;

/// <summary>
/// Service for managing audio playback and effects.
/// </summary>
public class AudioPlaybackService : IAudioPlaybackService, IDisposable
{
    private readonly ILogger<AudioPlaybackService> _logger;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IAudioOutput _audioOutput;
    private readonly MixingSampleProvider _mixer;
    private readonly Dictionary<Guid, ISampleProvider> _sources;
    private readonly Dictionary<string, IAudioEffect> _effects;
    private bool _isDisposed;
    private ISampleProvider? _currentSource;
    private SerialEffectChain? _effectChain;

    public ISampleProvider? CurrentSource => _currentSource;

    public AudioPlaybackService(ILogger<AudioPlaybackService> logger, ILoggerFactory loggerFactory)
    {
        _logger = logger;
        _loggerFactory = loggerFactory;
        _sources = new Dictionary<Guid, ISampleProvider>();
        _effects = new Dictionary<string, IAudioEffect>();
        _mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(44100, 2));
        _audioOutput = CreateAudioOutput();
        _audioOutput.Init(_mixer.ToWaveProvider());
    }

    protected virtual IAudioOutput CreateAudioOutput()
    {
        return AudioOutputFactory.CreateForCurrentPlatform(_loggerFactory);
    }

    public AudioPlaybackService(ILogger<AudioPlaybackService> logger, ILoggerFactory loggerFactory, IAudioOutput audioOutput)
    {
        _logger = logger;
        _loggerFactory = loggerFactory;
        _sources = new Dictionary<Guid, ISampleProvider>();
        _effects = new Dictionary<string, IAudioEffect>();
        _mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(44100, 2));
        _audioOutput = audioOutput;
        _audioOutput.Init(_mixer.ToWaveProvider());
    }

    public void SetSource(ISampleProvider source)
    {
        if (_isDisposed) return;

        // Remove existing source if any
        if (_currentSource != null)
        {
            _mixer.RemoveMixerInput(_currentSource);
        }

        // Ensure stereo output
        var stereoSource = source.WaveFormat.Channels == 1
            ? new MonoToStereoSampleProvider(source)
            : source;

        _currentSource = stereoSource;

        // If we have effects, recreate the chain
        if (_effects.Count > 0)
        {
            _effectChain = new SerialEffectChain(
                _currentSource,
                _loggerFactory.CreateLogger<SerialEffectChain>(),
                _loggerFactory);
            foreach (var effect in _effects.Values)
            {
                _effectChain.AddEffect(effect);
            }
            _mixer.AddMixerInput(_effectChain);
        }
        else
        {
            _mixer.AddMixerInput(_currentSource);
        }

        // Start playback automatically
        _audioOutput.Play();
    }

    public void StartPlayback()
    {
        if (_isDisposed) return;

        if (_currentSource == null)
        {
            // Add a test tone if no source is available
            var testTone = new SignalGenerator(44100, 1)
            {
                Gain = 0.2,
                Frequency = 440,
                Type = SignalGeneratorType.Sin
            };

            SetSource(testTone);
        }

        _audioOutput.Play();
    }

    public void StopPlayback()
    {
        if (_isDisposed) return;
        _audioOutput.Stop();
        _currentSource = null;
        _effectChain = null;
        _mixer.RemoveAllMixerInputs();
    }

    public void StopWithEffect(IAudioEffect effect)
    {
        if (_isDisposed) return;
        if (_currentSource == null) return;

        // Create a new chain just for the stop effect
        var stopChain = new SerialEffectChain(
            _currentSource,
            _loggerFactory.CreateLogger<SerialEffectChain>(),
            _loggerFactory);
        stopChain.AddEffect(effect);

        // Remove existing inputs
        _mixer.RemoveAllMixerInputs();
        
        // Add the stop chain
        _mixer.AddMixerInput(stopChain);

        // The effect (e.g. TapeStopEffect) will signal when it's done
        // by returning 0 samples, at which point playback will stop
    }

    public void PausePlayback()
    {
        if (_isDisposed) return;
        _audioOutput.Pause();
    }

    public void ResumePlayback()
    {
        if (_isDisposed) return;
        _audioOutput.Play();
    }

    public PlaybackState GetPlaybackState()
    {
        if (_isDisposed) return PlaybackState.Stopped;
        return _audioOutput.PlaybackState;
    }

    public void AddEffect(IAudioEffect effect)
    {
        if (_isDisposed) return;
        if (_currentSource == null) return;

        // Remove existing effect with same name if any
        RemoveEffect(effect.Name);

        // Add to effects dictionary
        _effects.Add(effect.Name, effect);

        // If this is our first effect, create the chain
        if (_effectChain == null)
        {
            _mixer.RemoveMixerInput(_currentSource);
            _effectChain = new SerialEffectChain(
                _currentSource,
                _loggerFactory.CreateLogger<SerialEffectChain>(),
                _loggerFactory);
            _mixer.AddMixerInput(_effectChain);
        }

        // Add to the chain
        _effectChain.AddEffect(effect);
    }

    public void RemoveEffect(string effectName)
    {
        if (_isDisposed) return;
        if (_effectChain == null) return;

        if (_effects.Remove(effectName))
        {
            _effectChain.RemoveEffect(effectName);

            // If no more effects, remove the chain
            if (_effects.Count == 0 && _currentSource != null)
            {
                _mixer.RemoveMixerInput(_effectChain);
                _effectChain = null;
                _mixer.AddMixerInput(_currentSource);
            }
        }
    }

    public void SetVolume(float volume)
    {
        if (_isDisposed) return;
        // Clamp volume between 0 and 1
        float clampedVolume = Math.Max(0f, Math.Min(1f, volume));
        _audioOutput.SetVolume(clampedVolume);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (disposing)
            {
                _audioOutput.Dispose();
            }
            _isDisposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
