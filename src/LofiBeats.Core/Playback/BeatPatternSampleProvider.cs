using LofiBeats.Core.Models;
using Microsoft.Extensions.Logging;
using NAudio.Wave;

namespace LofiBeats.Core.Playback;

public class BeatPatternSampleProvider : ISampleProvider
{
    private readonly BeatPattern _pattern;
    private readonly ILogger _logger;
    private readonly WaveFormat _waveFormat;
    private float _phase;
    private int _currentStep;
    private readonly float[] _frequencies = [440f, 587.33f, 659.25f, 783.99f]; // A4, D5, E5, G5
    private int _samplesPerStep;
    private int _currentSampleInStep;
    private readonly Random _rand = new();
    private float _kickPhase;
    private float _kickFreq = 150f; // Starting frequency for kick
    private readonly float[] _noiseBuffer = new float[1024]; // For noise-based sounds
    private int _noisePosition;

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

    public WaveFormat WaveFormat => _waveFormat;

    public BeatPatternSampleProvider(BeatPattern pattern, ILogger logger)
    {
        _pattern = pattern;
        _logger = logger;
        _waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);

        // Calculate samples per step based on tempo
        float secondsPerBeat = 60f / pattern.Tempo;
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

        _logger.LogInformation("BeatPatternSampleProvider initialized with tempo {Tempo} BPM", pattern.Tempo);
    }

    private void RandomizeOffsets()
    {
        for (int i = 0; i < _timeOffsets.Length; i++)
        {
            // For certain drums only (like snare/hat), add random offset
            string drum = _pattern.DrumSequence[i];
            if (drum == "snare" || drum == "hat")
            {
                float shiftMs = (float)(_rand.NextDouble() * 2 * _maxTimeShiftMs - _maxTimeShiftMs);
                _timeOffsets[i] = (int)(shiftMs * _samplesPerMs);
            }
            else
            {
                _timeOffsets[i] = 0; // no offset for kick
            }

            // Also randomize velocity for this step
            _velocities[i] = drum.ToLower() switch
            {
                "kick" => (float)(_kickMinVelocity + _rand.NextDouble() * (_kickMaxVelocity - _kickMinVelocity)),
                "snare" => (float)(_snareMinVelocity + _rand.NextDouble() * (_snareMaxVelocity - _snareMinVelocity)),
                "hat" => (float)(_hatMinVelocity + _rand.NextDouble() * (_hatMaxVelocity - _hatMinVelocity)),
                _ => 1.0f
            };
        }
    }

    public int Read(float[] buffer, int offset, int count)
    {
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
                _kickFreq = 150f; // Reset kick frequency
                RandomizeOffsets(); // Randomize offsets for next step
            }

            // Update phase for oscillator
            _phase += (float)(2 * Math.PI * _frequencies[_currentStep % _frequencies.Length] / _waveFormat.SampleRate);
            if (_phase > 2 * Math.PI) _phase -= (float)(2 * Math.PI);
        }

        return count;
    }

    private float GenerateSample()
    {
        string currentDrum = _pattern.DrumSequence[_currentStep];
        if (currentDrum == "_") return 0f; // Silent for rest

        float sample = 0f;
        float normalizedTime = (float)_currentSampleInStep / _samplesPerStep;
        float velocity = _velocities[_currentStep];

        switch (currentDrum.ToLower())
        {
            case "kick":
                sample += GenerateKick(normalizedTime) * velocity;
                break;
            case "snare":
                sample += GenerateSnare(normalizedTime) * velocity;
                break;
            case "hat":
                sample += GenerateHiHat(normalizedTime) * velocity;
                break;
        }

        // Add a simple chord tone based on the current step
        if (_pattern.ChordProgression?.Length > 0)
        {
            sample += GenerateChord(normalizedTime);
        }

        return sample * 0.5f; // Reduce overall volume
    }

    private float GenerateKick(float normalizedTime)
    {
        // Frequency sweep from 150Hz down to 50Hz
        _kickFreq = Math.Max(50f, _kickFreq * 0.9995f);
        _kickPhase += (float)(2 * Math.PI * _kickFreq / _waveFormat.SampleRate);
        if (_kickPhase > 2 * Math.PI) _kickPhase -= (float)(2 * Math.PI);

        // Amplitude envelope
        float envelope = (float)Math.Exp(-8.0f * normalizedTime);
        
        // Add some distortion for punch
        float kick = (float)Math.Sin(_kickPhase);
        kick = Math.Sign(kick) * (float)Math.Pow(Math.Abs(kick), 0.9);
        
        return kick * envelope * 0.8f;
    }

    private float GenerateSnare(float normalizedTime)
    {
        // Mix of noise and two sine tones
        float noise = GetNextNoise();
        
        // Two sine tones for body
        float tone1 = (float)Math.Sin(2 * Math.PI * 200 * normalizedTime);
        float tone2 = (float)Math.Sin(2 * Math.PI * 180 * normalizedTime);
        
        // Different envelopes for noise and tones
        float noiseEnv = (float)Math.Exp(-4.0f * normalizedTime);
        float toneEnv = (float)Math.Exp(-8.0f * normalizedTime);
        
        return (noise * 0.6f * noiseEnv + (tone1 + tone2) * 0.2f * toneEnv) * 0.7f;
    }

    private float GenerateHiHat(float normalizedTime)
    {
        float noise = GetNextNoise();
        
        // Bandpass filter simulation (very basic)
        noise = (float)Math.Sin(2 * Math.PI * 8000 * normalizedTime + noise);
        
        // Sharp envelope
        float envelope = (float)Math.Exp(-30.0f * normalizedTime);
        
        return noise * envelope * 0.3f;
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
} 