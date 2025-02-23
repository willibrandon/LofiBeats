using LofiBeats.Core.Effects;
using LofiBeats.Core.Models;
using LofiBeats.Core.PluginApi;
using LofiBeats.Core.Telemetry;
using Microsoft.Extensions.Logging;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

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
    private readonly UserSampleRepository _userSampleRepository;
    private readonly TelemetryTracker? _telemetry;
    private bool _isDisposed;
    private ISampleProvider? _currentSource;
    private SerialEffectChain? _effectChain;
    private string _currentStyle = "basic";
    private float _currentVolume = 1.0f;
    private readonly Lock _stateLock = new();

    public ISampleProvider? CurrentSource => _currentSource;

    /// <summary>
    /// Gets or sets the current beat generation style.
    /// </summary>
    public string CurrentStyle
    {
        get
        {
            lock (_stateLock)
            {
                return _currentStyle;
            }
        }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Style cannot be empty or null.", nameof(value));

            lock (_stateLock)
            {
                if (_currentStyle != value)
                {
                    _currentStyle = value;
                    _logger.LogInformation("Style changed to: {Style}", value);
                }
            }
        }
    }

    /// <summary>
    /// Gets the current volume level (range: 0.0 to 1.0).
    /// </summary>
    public float CurrentVolume
    {
        get
        {
            lock (_stateLock)
            {
                return _currentVolume;
            }
        }
        private set
        {
            lock (_stateLock)
            {
                _currentVolume = value;
            }
        }
    }

    public AudioPlaybackService(
        ILogger<AudioPlaybackService> logger, 
        ILoggerFactory loggerFactory,
        UserSampleRepository userSampleRepository,
        TelemetryTracker? telemetry = null)
    {
        _logger = logger;
        _loggerFactory = loggerFactory;
        _userSampleRepository = userSampleRepository;
        _telemetry = telemetry;
        _sources = [];
        _effects = [];
        _mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(44100, 2));
        _audioOutput = CreateAudioOutput();
        _audioOutput.Init(_mixer.ToWaveProvider());
    }

    public AudioPlaybackService(
        ILogger<AudioPlaybackService> logger, 
        ILoggerFactory loggerFactory, 
        IAudioOutput audioOutput,
        UserSampleRepository userSampleRepository,
        TelemetryTracker? telemetry = null)
    {
        _logger = logger;
        _loggerFactory = loggerFactory;
        _userSampleRepository = userSampleRepository;
        _telemetry = telemetry;
        _sources = [];
        _effects = [];
        _mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(44100, 2));
        _audioOutput = audioOutput;
        _audioOutput.Init(_mixer.ToWaveProvider());
    }

    protected virtual IAudioOutput CreateAudioOutput()
    {
        return AudioOutputFactory.CreateForCurrentPlatform(_loggerFactory);
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
                _loggerFactory.CreateLogger<SerialEffectChain>());

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
            _loggerFactory.CreateLogger<SerialEffectChain>());

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
                _loggerFactory.CreateLogger<SerialEffectChain>());

            _mixer.AddMixerInput(_effectChain);
        }

        // Add to the chain
        _effectChain.AddEffect(effect);
    }

    public void RemoveEffect(string effectName)
    {
        if (_isDisposed) return;

        if (_effects.Remove(effectName))
        {
            if (_effectChain == null) return;

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
        CurrentVolume = clampedVolume;
        _audioOutput.SetVolume(clampedVolume);
    }

    /// <summary>
    /// Gets a preset object representing the current playback state.
    /// </summary>
    /// <returns>A preset containing the current style, volume, and effects.</returns>
    public Preset GetCurrentPreset()
    {
        lock (_stateLock)
        {
            return new Preset
            {
                Name = $"Preset_{DateTime.Now:yyyyMMdd_HHmmss}",
                Style = _currentStyle,
                Volume = _currentVolume,
                Effects = [.. _effects.Keys]
            };
        }
    }

    /// <summary>
    /// Applies a preset to the current playback state.
    /// </summary>
    /// <param name="preset">The preset to apply.</param>
    /// <param name="effectFactory">Factory for creating effects from names.</param>
    /// <exception cref="ArgumentNullException">Thrown when preset or effectFactory is null.</exception>
    public void ApplyPreset(Preset preset, IEffectFactory effectFactory)
    {
        ArgumentNullException.ThrowIfNull(preset);
        ArgumentNullException.ThrowIfNull(effectFactory);

        // Validate the preset
        preset.Validate();

        lock (_stateLock)
        {
            if (_isDisposed) return;

            // 1. Set style
            CurrentStyle = preset.Style;

            // 2. Set volume
            SetVolume(preset.Volume);

            // Only proceed with effects if we have a current source
            if (_currentSource != null)
            {
                // 3. Clear existing effects
                var existingEffects = _effects.Keys.ToList();
                foreach (var effectName in existingEffects)
                {
                    RemoveEffect(effectName);
                }

                // 4. Add new effects from preset
                foreach (var effectName in preset.Effects)
                {
                    try
                    {
                        var effect = effectFactory.CreateEffect(effectName, _currentSource);
                        AddEffect(effect);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to create effect {EffectName} from preset", effectName);
                    }
                }
            }
            else
            {
                _logger.LogInformation("No active source - effects from preset will be applied when playback starts");
            }
        }
    }

    /// <summary>
    /// Crossfades from the current pattern to a new pattern over the specified duration.
    /// </summary>
    /// <param name="newPattern">The new beat pattern to fade to.</param>
    /// <param name="crossfadeDuration">Duration of the crossfade in seconds.</param>
    public void CrossfadeToPattern(BeatPattern newPattern, float crossfadeDuration)
    {
        lock (_stateLock)
        {
            var oldSource = _currentSource;
            if (oldSource == null)
            {
                // If nothing is playing, just set the new pattern directly.
                var provider = CreateProviderForPattern(newPattern);
                SetSource(provider);
                StartPlayback();
                return;
            }

            // Get the current BeatPatternSampleProvider
            if (oldSource is not BeatPatternSampleProvider oldBeatProvider)
            {
                _logger.LogWarning("Cannot perform synchronized crossfade - current source is not a BeatPatternSampleProvider");
                // Fall back to immediate transition
                var provider = CreateProviderForPattern(newPattern);
                SetSource(provider);
                StartPlayback();
                return;
            }

            _logger.LogInformation("Starting synchronized crossfade from {OldStyle} to {NewStyle} over {Duration}s", 
                _currentStyle, newPattern.DrumSequence[0], crossfadeDuration);

            // Create the new provider
            var newBeatProvider = CreateProviderForPattern(newPattern);

            // Wait for the next bar start before beginning crossfade
            Task.Run(async () =>
            {
                try
                {
                    // Wait for up to 2 bars for a good transition point
                    var maxWaitMs = (int)(2 * 4 * (60000f / oldBeatProvider.Pattern.BPM));
                    var startTime = DateTime.UtcNow;

                    while (!oldBeatProvider.IsAtBarStart())
                    {
                        if ((DateTime.UtcNow - startTime).TotalMilliseconds > maxWaitMs)
                        {
                            _logger.LogWarning("Could not find bar start for sync - starting crossfade immediately");
                            break;
                        }
                        await Task.Delay(1);
                    }

                    lock (_stateLock)
                    {
                        if (_isDisposed) return;

                        // Sync the new provider with the current beat position
                        newBeatProvider.SyncWith(oldBeatProvider);

                        // Create crossfade objects
                        var xfadeManager = new CrossfadeManager(crossfadeDuration);
                        xfadeManager.BeginCrossfade();

                        // Create crossfade provider
                        var crossfadeProvider = new CrossfadeSampleProvider(
                            oldBeatProvider, newBeatProvider, xfadeManager
                        );

                        // Remove old source from the mixer
                        _mixer.RemoveAllMixerInputs();

                        // Add crossfade provider
                        _mixer.AddMixerInput(crossfadeProvider);

                        // Keep track of the crossfade provider to prevent premature disposal
                        var currentCrossfadeProvider = crossfadeProvider;

                        // Start a background task to watch for crossfade completion
                        Task.Run(async () =>
                        {
                            try 
                            {
                                while (!xfadeManager.IsCrossfadeComplete())
                                {
                                    await Task.Delay(100);
                                }

                                _logger.LogInformation("Crossfade complete, switching to new pattern");

                                // Crossfade done
                                lock (_stateLock)
                                {
                                    if (_isDisposed) return;

                                    // Remove crossfader from mixer
                                    _mixer.RemoveAllMixerInputs();

                                    // Add the new provider as the main source
                                    _currentSource = newBeatProvider;
                                    _mixer.AddMixerInput(_currentSource);

                                    // Update style
                                    _currentStyle = newPattern.DrumSequence.Length > 0 ? newPattern.DrumSequence[0] : "basic";

                                    // Now it's safe to dispose the crossfade provider
                                    currentCrossfadeProvider.Dispose();
                                }

                                // Telemetry event
                                _telemetry?.TrackEvent(TelemetryConstants.Events.PlaybackStarted, new Dictionary<string, string>
                                {
                                    { TelemetryConstants.Properties.BeatStyle, _currentStyle },
                                    { TelemetryConstants.Properties.BeatTempo, newPattern.BPM.ToString() }
                                });
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Error during crossfade completion");
                            }
                        });

                        // Telemetry event for crossfade start
                        _telemetry?.TrackEvent(TelemetryConstants.Events.PlaybackTransition, new Dictionary<string, string>
                        {
                            { TelemetryConstants.Properties.OldStyle, _currentStyle },
                            { TelemetryConstants.Properties.NewStyle, newPattern.DrumSequence[0] },
                            { TelemetryConstants.Properties.OldBPM, oldBeatProvider.Pattern.BPM.ToString() },
                            { TelemetryConstants.Properties.NewBPM, newPattern.BPM.ToString() },
                            { TelemetryConstants.Properties.CrossfadeDuration, crossfadeDuration.ToString("F1") },
                            { TelemetryConstants.Properties.BeatsPerBar, BeatPatternSampleProvider.GetBeatsPerBarForStyle(newPattern.DrumSequence[0]).ToString() },
                            { TelemetryConstants.Properties.TransitionType, "crossfade" }
                        });

                        // Ensure playback is running
                        StartPlayback();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during crossfade initialization");
                }
            });
        }
    }

    /// <summary>
    /// Creates a BeatPatternSampleProvider for the given pattern.
    /// </summary>
    /// <param name="pattern">The beat pattern to create a provider for.</param>
    /// <returns>A configured sample provider for the pattern.</returns>
    private BeatPatternSampleProvider CreateProviderForPattern(BeatPattern pattern)
    {
        ArgumentNullException.ThrowIfNull(pattern);

        var provider = new BeatPatternSampleProvider(
            pattern,
            _loggerFactory.CreateLogger<BeatPatternSampleProvider>(),
            _userSampleRepository,
            _telemetry ?? new TelemetryTracker(
                new NullTelemetryService(),
                _loggerFactory.CreateLogger<TelemetryTracker>()));

        return provider;
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
