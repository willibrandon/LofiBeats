using LofiBeats.Core.Models;
using Microsoft.Extensions.Logging;
using NAudio.Wave;

namespace LofiBeats.Core.Playback;

public class BeatPatternSampleProvider : ISampleProvider, IDisposable
{
    private readonly BeatPattern _pattern;
    private readonly ILogger _logger;
    private readonly WaveFormat _waveFormat;
    private readonly UserSampleRepository _userSamples;
    private float _phase;
    private int _currentStep;
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

    public WaveFormat WaveFormat => _waveFormat;

    public BeatPatternSampleProvider(BeatPattern pattern, ILogger logger, UserSampleRepository userSamples)
    {
        _pattern = pattern;
        _logger = logger;
        _userSamples = userSamples;
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

        // Check if this is a user sample
        if (_pattern.HasUserSample(_currentStep) && _userSamples.HasSample(currentDrum))
        {
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
} 