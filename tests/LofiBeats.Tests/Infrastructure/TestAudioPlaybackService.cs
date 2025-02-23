using LofiBeats.Core.Effects;
using LofiBeats.Core.Models;
using LofiBeats.Core.Playback;
using LofiBeats.Core.Telemetry;
using Microsoft.Extensions.Logging;
using NAudio.Wave;
using Moq;
using LofiBeats.Core.PluginApi;

namespace LofiBeats.Tests.Infrastructure;

/// <summary>
/// Test double for IAudioPlaybackService that simulates audio playback without requiring audio hardware
/// </summary>
public class TestAudioPlaybackService : IAudioPlaybackService
{
    private PlaybackState _state = PlaybackState.Stopped;
    private float _volume = 1.0f;
    private string _currentStyle = "basic";
    private ISampleProvider? _currentSource;
    private readonly Dictionary<string, IAudioEffect> _effects = new();
    private readonly ILogger<BeatPatternSampleProvider> _logger;
    private readonly UserSampleRepository _userSampleRepository;
    private readonly TelemetryTracker _telemetryTracker;

    public TestAudioPlaybackService()
    {
        // Set up mocked dependencies
        var loggerMock = new Mock<ILogger<BeatPatternSampleProvider>>();
        var userSampleLoggerMock = new Mock<ILogger<UserSampleRepository>>();
        var telemetryLoggerMock = new Mock<ILogger<TelemetryTracker>>();

        _logger = loggerMock.Object;
        _userSampleRepository = new UserSampleRepository(userSampleLoggerMock.Object);
        _telemetryTracker = new TelemetryTracker(
            new NullTelemetryService(),
            telemetryLoggerMock.Object);
    }

    public ISampleProvider? CurrentSource => _currentSource;
    
    public string CurrentStyle
    {
        get => _currentStyle;
        set => _currentStyle = value ?? throw new ArgumentNullException(nameof(value));
    }

    public float CurrentVolume => _volume;

    public void SetVolume(float level)
    {
        if (level < 0 || level > 1)
            throw new ArgumentOutOfRangeException(nameof(level));
        _volume = level;
    }

    public void SetSource(ISampleProvider source)
    {
        _currentSource = source;
    }

    public void StartPlayback()
    {
        _state = PlaybackState.Playing;
    }

    public void StopPlayback()
    {
        _state = PlaybackState.Stopped;
        _currentSource = null;
        _effects.Clear();
    }

    public void StopWithEffect(IAudioEffect effect)
    {
        // Simulate effect completion after a short delay
        Task.Delay(100).ContinueWith(_ => StopPlayback());
    }

    public void PausePlayback()
    {
        if (_state == PlaybackState.Playing)
            _state = PlaybackState.Paused;
    }

    public void ResumePlayback()
    {
        if (_state == PlaybackState.Paused)
            _state = PlaybackState.Playing;
    }

    public PlaybackState GetPlaybackState() => _state;

    public void AddEffect(IAudioEffect effect)
    {
        if (_currentSource != null)
        {
            _effects[effect.Name] = effect;
        }
    }

    public void RemoveEffect(string effectName)
    {
        _effects.Remove(effectName);
    }

    public void CrossfadeToPattern(BeatPattern pattern, float duration)
    {
        // Simulate crossfade by changing source after delay
        Task.Delay((int)(duration * 1000)).ContinueWith(_ =>
        {
            _currentSource = new BeatPatternSampleProvider(
                pattern,
                _logger,
                _userSampleRepository,
                _telemetryTracker);
            _state = PlaybackState.Playing;
        });
    }

    public Preset GetCurrentPreset()
    {
        return new Preset
        {
            Name = "Test Preset",
            Style = _currentStyle,
            Volume = _volume,
            Effects = _effects.Keys.ToList()
        };
    }

    public void ApplyPreset(Preset preset, IEffectFactory effectFactory)
    {
        ArgumentNullException.ThrowIfNull(preset);
        ArgumentNullException.ThrowIfNull(effectFactory);

        preset.Validate();

        _currentStyle = preset.Style;
        SetVolume(preset.Volume);

        var existingEffects = _effects.Keys.ToList();
        foreach (var effectName in existingEffects)
        {
            RemoveEffect(effectName);
        }

        if (_currentSource != null)
        {
            foreach (var effectName in preset.Effects)
            {
                var effect = effectFactory.CreateEffect(effectName, _currentSource);
                AddEffect(effect);
            }
        }
    }
} 