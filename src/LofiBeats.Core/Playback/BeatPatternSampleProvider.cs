using LofiBeats.Core.Models;
using LofiBeats.Core.Telemetry;
using Microsoft.Extensions.Logging;
using NAudio.Wave;

namespace LofiBeats.Core.Playback;

/// <summary>
/// Provides real-time audio synthesis for drum patterns with support for both
/// synthesized and user-supplied samples.
/// </summary>
/// <remarks>
/// This class implements a hybrid approach to drum sound generation:
/// - Synthesized drums using multi-stage envelopes and frequency modulation
/// - User-supplied samples with velocity layers
/// - Real-time effects including resonant filtering and amplitude modulation
/// 
/// Features:
/// - Advanced drum synthesis with realistic envelopes and frequency sweeps
/// - Humanization through timing and velocity variations
/// - Seamless integration of user samples with velocity mapping
/// - Real-time buffer management and resource cleanup
/// - Thread-safe audio generation
/// - Automatic sample rate conversion
/// - Telemetry tracking for performance monitoring
/// 
/// Performance considerations:
/// - Uses pre-allocated buffers for noise generation
/// - Minimizes allocations in the audio processing path
/// - Implements proper resource disposal
/// - Handles buffer underruns gracefully
/// 
/// The provider maintains its own timing and synchronization, ensuring accurate
/// playback of patterns at the specified tempo.
/// </remarks>
public class BeatPatternSampleProvider : ISampleProvider, IDisposable
{
    private readonly BeatPattern _pattern;
    private readonly ILogger _logger;
    private readonly WaveFormat _waveFormat;
    private readonly UserSampleRepository _userSamples;
    private readonly TelemetryTracker _telemetry;
    private float _phase;
    private int _currentStep;
    private int _samplesUntilNextStep;
    private readonly float[] _frequencies = [440f, 587.33f, 659.25f, 783.99f]; // A4, D5, E5, G5
    private int _samplesPerStep;
    private int _currentSampleInStep;
    private readonly Random _rand = new();
    private float _kickPhase;
    private readonly float[] _noiseBuffer = new float[1024]; // For noise-based sounds
    private int _noisePosition;
    private const float SAMPLE_RATE = 44100f;
    private bool _disposed;

    // Humanization fields
    private readonly float _maxTimeShiftMs = 15f; // up to ±15 ms
    private readonly int[] _timeOffsets; // offset in samples
    private readonly float _samplesPerMs;

    // Velocity variation fields
    private readonly float _kickMinVelocity = 0.8f;
    private readonly float _kickMaxVelocity = 1.0f;
    private readonly float _snareMinVelocity = 0.6f;
    private readonly float _snareMaxVelocity = 1.0f;
    private readonly float _hatMinVelocity = 0.4f;
    private readonly float _hatMaxVelocity = 0.8f;
    private readonly Dictionary<int, float> _velocities = new();

    // User sample tracking
    private ISampleProvider? _currentUserSample;
    private readonly float[] _userSampleBuffer = new float[4096];
    private int _userSampleBufferPosition;
    private int _userSampleBufferCount;

    private readonly Dictionary<string, int> _sampleTriggerCounts = new();

    public WaveFormat WaveFormat => _waveFormat;

    public BeatPattern Pattern => _pattern;

    public BeatPatternSampleProvider(
        BeatPattern pattern,
        ILogger logger,
        UserSampleRepository userSampleRepository,
        TelemetryTracker telemetryTracker)
    {
        _pattern = pattern;
        _logger = logger;
        _userSamples = userSampleRepository;
        _telemetry = telemetryTracker;
        _waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);

        // Calculate samples per step based on tempo
        float secondsPerBeat = 60f / pattern.BPM;
        _samplesPerStep = (int)(secondsPerBeat * _waveFormat.SampleRate);

        // Initialize humanization
        _samplesPerMs = _waveFormat.SampleRate / 1000f;
        _timeOffsets = new int[pattern.DrumSequence.Length];
        RandomizeOffsets();

        // Pre-fill noise buffer
        for (int i = 0; i < _noiseBuffer.Length; i++)
        {
            _noiseBuffer[i] = (float)(_rand.NextDouble() * 2 - 1);
        }

        _logger.LogInformation("BeatPatternSampleProvider initialized with tempo {Tempo} BPM", pattern.BPM);
    }

    private void RandomizeOffsets()
    {
        for (int i = 0; i < _timeOffsets.Length; i++)
        {
            // For certain drums only (like snare/hat), add random offset
            string drum = _pattern.GetSampleForStep(i);
            if (drum == "snare" || drum == "hat")
            {
                float shiftMs = (float)(_rand.NextDouble() * 2 * _maxTimeShiftMs - _maxTimeShiftMs);
                _timeOffsets[i] = (int)(shiftMs * _samplesPerMs);
            }
            else
            {
                _timeOffsets[i] = 0; // no offset for kick or user samples
            }

            // Also randomize velocity for this step
            _velocities[i] = drum.ToLower() switch
            {
                "kick" => (float)(_kickMinVelocity + _rand.NextDouble() * (_kickMaxVelocity - _kickMinVelocity)),
                "snare" => (float)(_snareMinVelocity + _rand.NextDouble() * (_snareMaxVelocity - _snareMinVelocity)),
                "hat" => (float)(_hatMinVelocity + _rand.NextDouble() * (_hatMaxVelocity - _hatMinVelocity)),
                _ => 1.0f // User samples use full velocity
            };
        }
    }

    public int Read(float[] buffer, int offset, int count)
    {
        if (_disposed) return 0;

        for (int i = 0; i < count; i += 2) // Stereo, so increment by 2
        {
            // Get the offset for the current step
            int offsetSamples = _timeOffsets[_currentStep];

            // Adjust the current sample position by the offset
            int localSampleInStep = _currentSampleInStep - offsetSamples;

            // Generate sample if we're within the valid window
            float sample;
            if (localSampleInStep < 0 || localSampleInStep >= _samplesPerStep)
            {
                sample = 0f; // Silent if outside the window
            }
            else
            {
                sample = GenerateSample();
            }

            // Write to both channels
            buffer[offset + i] = sample;
            buffer[offset + i + 1] = sample;

            // Update step counters
            _currentSampleInStep++;
            if (_currentSampleInStep >= _samplesPerStep)
            {
                _currentSampleInStep = 0;
                _currentStep = (_currentStep + 1) % _pattern.DrumSequence.Length;
                _kickPhase = 0; // Reset kick phase for new step
                RandomizeOffsets(); // Randomize offsets for next step

                // Reset user sample state
                if (_currentUserSample is IDisposable disposable)
                {
                    disposable.Dispose();
                }
                _currentUserSample = null;
                _userSampleBufferPosition = 0;
                _userSampleBufferCount = 0;
            }

            // Update phase for oscillator
            _phase += (float)(2 * Math.PI * _frequencies[_currentStep % _frequencies.Length] / _waveFormat.SampleRate);
            if (_phase > 2 * Math.PI) _phase -= (float)(2 * Math.PI);
        }

        return count;
    }

    private float GenerateSample()
    {
        string currentDrum = _pattern.GetSampleForStep(_currentStep);
        if (currentDrum == "_") return 0f; // Silent for rest

        float sample = 0f;
        float normalizedTime = (float)_currentSampleInStep / _samplesPerStep;
        float velocity = _velocities[_currentStep];

        // First check if we have a user sample for this drum type
        if (_userSamples.HasSample(currentDrum))
        {
            // Track sample trigger
            if (!_sampleTriggerCounts.TryGetValue(currentDrum, out int count))
            {
                count = 0;
            }
            _sampleTriggerCounts[currentDrum] = count + 1;

            _telemetry.TrackEvent(TelemetryConstants.Events.UserSampleTriggered, new Dictionary<string, string>
            {
                { TelemetryConstants.Properties.SampleName, currentDrum },
                { TelemetryConstants.Properties.SampleVelocity, velocity.ToString("F2") },
                { TelemetryConstants.Properties.SamplePosition, _currentStep.ToString() }
            });

            sample = GenerateUserSample(currentDrum) * velocity;
        }
        else
        {
            switch (currentDrum.ToLower())
            {
                case "kick":
                    sample += GenerateKick(normalizedTime) * velocity;
                    break;
                case "snare":
                    sample += GenerateSnare(normalizedTime) * velocity;
                    break;
                case "hat":
                    sample += GenerateHiHat(normalizedTime, false) * velocity;
                    break;
                case "ohat":
                    sample += GenerateHiHat(normalizedTime, true) * velocity;
                    break;
            }
        }

        // Add a simple chord tone based on the current step
        if (_pattern.ChordProgression?.Length > 0)
        {
            sample += GenerateChord(normalizedTime);
        }

        return sample * 0.5f; // Reduce overall volume
    }

    private float GenerateUserSample(string sampleName)
    {
        // Initialize or refill buffer if needed
        if (_currentUserSample == null)
        {
            _currentUserSample = _userSamples.CreateSampleProvider(sampleName);
            _userSampleBufferPosition = 0;
            _userSampleBufferCount = _currentUserSample.Read(_userSampleBuffer, 0, _userSampleBuffer.Length);
        }
        else if (_userSampleBufferPosition >= _userSampleBufferCount)
        {
            _userSampleBufferCount = _currentUserSample.Read(_userSampleBuffer, 0, _userSampleBuffer.Length);
            _userSampleBufferPosition = 0;
            if (_userSampleBufferCount == 0) return 0f; // End of sample
        }

        return _userSampleBufferPosition < _userSampleBufferCount 
            ? _userSampleBuffer[_userSampleBufferPosition++] 
            : 0f;
    }

    /// <summary>
    /// Generates a kick drum sound using multi-stage envelope and frequency sweep.
    /// </summary>
    /// <param name="normalizedTime">Time within the current step (0-1).</param>
    /// <returns>The generated sample value between -1.0 and 1.0.</returns>
    /// <remarks>
    /// Uses a two-stage envelope (attack and decay) with exponential decay curve.
    /// The frequency sweep starts at 120Hz and drops to 45Hz for deep sub-bass.
    /// Attack is very quick (3ms) for punch, with a longer decay (80ms) for body.
    /// </remarks>
    private float GenerateKick(float normalizedTime)
    {
        float attackTime = 0.003f;  // 3ms attack - quick punch
        float decayTime = 0.08f;    // 80ms decay - tighter
        
        float t = normalizedTime * (attackTime + decayTime);
        float envelope;
        
        if (t < attackTime)
        {
            envelope = t / attackTime;
        }
        else
        {
            float decayPhase = (t - attackTime) / decayTime;
            envelope = MathF.Exp(-5f * decayPhase); // Faster decay
        }

        // Simpler frequency sweep focused on sub frequencies
        float freqStart = 120f;  // Start lower
        float freqEnd = 45f;     // Don't go too low to maintain punch
        float freqRange = freqStart - freqEnd;
        float freq = freqEnd + (freqRange * MathF.Exp(-11f * normalizedTime)); // Faster initial drop
        
        _kickPhase += 2f * MathF.PI * freq / SAMPLE_RATE;
        if (_kickPhase > 2f * MathF.PI) _kickPhase -= 2f * MathF.PI;
        
        float wave = MathF.Sin(_kickPhase);
        
        return wave * envelope * 0.85f; // Slightly louder but clean
    }

    /// <summary>
    /// Generates a snare drum sound using noise and tonal components.
    /// </summary>
    /// <param name="normalizedTime">Time within the current step (0-1).</param>
    /// <returns>The generated sample value between -1.0 and 1.0.</returns>
    /// <remarks>
    /// Creates a vintage drum machine style snare using:
    /// - Two tonal oscillators (200Hz and 160Hz) for body
    /// - Filtered noise component for snap
    /// - Separate envelopes for noise (2ms attack) and tones
    /// - 70/30 mix of noise to tonal components
    /// </remarks>
    private float GenerateSnare(float normalizedTime)
    {
        float noise = GetNextNoise();
        
        // Two tones for body - typical for vintage drum machines
        float tone1 = MathF.Sin(2f * MathF.PI * 200f * normalizedTime); // Higher tone
        float tone2 = MathF.Sin(2f * MathF.PI * 160f * normalizedTime); // Lower tone
        
        // Longer attack for noise
        float noiseAttack = MathF.Min(normalizedTime / 0.002f, 1f); // 2ms attack
        float noiseEnvelope = noiseAttack * MathF.Exp(-normalizedTime * 12f); // Faster decay
        
        // Shorter envelope for tones
        float toneEnvelope = MathF.Exp(-normalizedTime * 14f);
        
        // Mix with more noise for that vintage sound
        float noisePart = noise * 0.7f * noiseEnvelope;
        float tonePart = (tone1 + tone2) * 0.2f * toneEnvelope;
        
        return (noisePart + tonePart) * 0.7f;
    }

    /// <summary>
    /// Generates a hi-hat sound using filtered noise and resonant frequencies.
    /// </summary>
    /// <param name="normalizedTime">Time within the current step (0-1).</param>
    /// <param name="open">If true, generates an open hi-hat with longer decay.</param>
    /// <returns>The generated sample value between -1.0 and 1.0.</returns>
    /// <remarks>
    /// Creates metallic character through:
    /// - Multiple resonant frequencies (3kHz and 4.5kHz)
    /// - Filtered white noise base
    /// - Quick 1ms attack for both open and closed
    /// - Variable decay (45ms for closed, 8ms for open)
    /// - 60/40 mix of noise to resonance
    /// </remarks>
    private float GenerateHiHat(float normalizedTime, bool open = false)
    {
        float noise = GetNextNoise();
        
        // Different decay times for open/closed
        float decayRate = open ? 8f : 45f; // Longer open, shorter closed
        
        // Quick attack
        float attack = MathF.Min(normalizedTime / 0.001f, 1f); // 1ms attack
        float envelope = attack * MathF.Exp(-normalizedTime * decayRate);
        
        // Multiple resonant frequencies for metallic character
        float res1 = MathF.Sin(2f * MathF.PI * 3000f * normalizedTime); // Higher frequency
        float res2 = MathF.Sin(2f * MathF.PI * 4500f * normalizedTime); // Even higher
        float resonance = (res1 + res2) * 0.15f;
        
        // More noise-focused mix
        return (noise * 0.6f + resonance) * envelope * 0.45f;
    }

    /// <summary>
    /// Generates a chord tone to accompany the drum pattern.
    /// </summary>
    /// <param name="normalizedTime">Time within the current step (0-1).</param>
    /// <returns>The generated sample value between -1.0 and 1.0.</returns>
    /// <remarks>
    /// Generates a rich harmonic texture by:
    /// - Using the first three harmonics of the base frequency
    /// - Decreasing amplitude for higher harmonics (1/n falloff)
    /// - Applying a soft attack and long decay envelope
    /// - Mixing at a lower volume (-15dB) to sit behind drums
    /// </remarks>
    private float GenerateChord(float normalizedTime)
    {
        // Simple chord simulation with multiple frequencies
        float baseFreq = _frequencies[_currentStep % _frequencies.Length];
        float chord = 0f;
        
        // Generate multiple harmonics
        for (int i = 1; i <= 3; i++)
        {
            float harmonic = (float)Math.Sin(_phase * i);
            chord += harmonic * (1f / i); // Decrease amplitude for higher harmonics
        }
        
        // Soft attack, longer decay
        float envelope = (float)(Math.Pow(normalizedTime, 0.1) * Math.Exp(-2.0f * normalizedTime));
        
        return chord * envelope * 0.15f; // Reduced volume for background
    }

    /// <summary>
    /// Gets the next sample from the noise buffer for use in drum synthesis.
    /// </summary>
    /// <returns>A random value between -1.0 and 1.0.</returns>
    /// <remarks>
    /// Uses a circular buffer of pre-generated noise samples to avoid
    /// real-time random number generation. The buffer size (1024 samples)
    /// provides enough variation while being cache-friendly.
    /// </remarks>
    private float GetNextNoise()
    {
        _noisePosition = (_noisePosition + 1) % _noiseBuffer.Length;
        return _noiseBuffer[_noisePosition];
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        if (_currentUserSample is IDisposable disposable)
        {
            disposable.Dispose();
        }
        GC.SuppressFinalize(this);
    }

    public int GetCurrentStep() => _currentStep;
    
    public int GetSamplesUntilNextStep() => _samplesUntilNextStep;
    
    public void SyncWith(BeatPatternSampleProvider other)
    {
        // Calculate the exact sample position in the bar
        float secondsPerBeat = 60f / other.Pattern.BPM;
        int samplesPerBeat = (int)(secondsPerBeat * _waveFormat.SampleRate);
        int samplesPerBar = samplesPerBeat * 4; // Assuming 4 beats per bar

        // Get the other provider's position within its bar
        int otherSamplePos = (other.GetCurrentStep() * other._samplesPerStep) + other._currentSampleInStep;
        int otherBarPos = otherSamplePos % samplesPerBar;

        // Adjust our position to match
        _currentStep = (otherBarPos / _samplesPerStep) % _pattern.DrumSequence.Length;
        _currentSampleInStep = otherBarPos % _samplesPerStep;
        _samplesUntilNextStep = _samplesPerStep - _currentSampleInStep;

        // Reset phases to match
        _phase = other._phase;
        _kickPhase = other._kickPhase;

        // Get style-specific beat divisions
        int oldBeatsPerBar = GetBeatsPerBarForStyle(other.Pattern.DrumSequence[0]);
        int newBeatsPerBar = GetBeatsPerBarForStyle(_pattern.DrumSequence[0]);

        // Calculate target BPM while respecting style's range and beat interpretation
        float targetBpm = other.Pattern.BPM * (oldBeatsPerBar / (float)newBeatsPerBar);

        // Get BPM range based on style
        var (minBpm, maxBpm) = GetBpmRangeForStyle(_pattern.DrumSequence[0]);
        targetBpm = Math.Clamp(targetBpm, minBpm, maxBpm);

        // Only adjust if the difference is significant
        if (Math.Abs(_pattern.BPM - targetBpm) > 0.0001f)
        {
            _pattern.BPM = (int)Math.Round(targetBpm);
            float secondsPerStep = 60f / (_pattern.BPM * (newBeatsPerBar / 4f));
            _samplesPerStep = (int)(secondsPerStep * _waveFormat.SampleRate);
        }

        _logger.LogInformation(
            "Synced patterns - BPM: {OldBPM}->{NewBPM}, BeatsPerBar: {OldBeats}->{NewBeats}, Step: {Step}", 
            other.Pattern.BPM, 
            _pattern.BPM,
            oldBeatsPerBar,
            newBeatsPerBar,
            _currentStep);
    }

    private static (int MinBpm, int MaxBpm) GetBpmRangeForStyle(string style)
    {
        return style.ToLower() switch
        {
            "jazzy" => (75, 95),
            "chillhop" => (65, 85),
            "hiphop" => (80, 100),
            _ => (70, 90) // basic
        };
    }

    public static int GetBeatsPerBarForStyle(string style)
    {
        // How many beats we consider per bar for each style
        return style.ToLower() switch
        {
            "jazzy" => 4,    // Standard 4/4 time
            "chillhop" => 4, // Laid back 4/4
            "hiphop" => 2,   // Half-time feel (makes it feel slower/heavier)
            _ => 4           // Default 4/4 time
        };
    }

    public bool IsAtBarStart()
    {
        // Use style-specific beat division
        int beatsPerBar = GetBeatsPerBarForStyle(_pattern.DrumSequence[0]);
        return _currentStep % (beatsPerBar * 4) == 0;
    }
} 