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
    private readonly float[] _frequencies = new[] { 440f, 587.33f, 659.25f, 783.99f }; // A4, D5, E5, G5
    private int _samplesPerStep;
    private int _currentSampleInStep;

    public WaveFormat WaveFormat => _waveFormat;

    public BeatPatternSampleProvider(BeatPattern pattern, ILogger logger)
    {
        _pattern = pattern;
        _logger = logger;
        _waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);

        // Calculate samples per step based on tempo
        float secondsPerBeat = 60f / pattern.Tempo;
        _samplesPerStep = (int)(secondsPerBeat * _waveFormat.SampleRate);

        _logger.LogInformation("BeatPatternSampleProvider initialized with tempo {Tempo} BPM", pattern.Tempo);
    }

    public int Read(float[] buffer, int offset, int count)
    {
        for (int i = 0; i < count; i += 2) // Stereo, so increment by 2
        {
            // Calculate the current step in the pattern
            _currentSampleInStep++;
            if (_currentSampleInStep >= _samplesPerStep)
            {
                _currentSampleInStep = 0;
                _currentStep = (_currentStep + 1) % _pattern.DrumSequence.Length;
            }

            // Generate the sample based on the current step
            float sample = GenerateSample();

            // Write to both channels
            buffer[offset + i] = sample;
            buffer[offset + i + 1] = sample;

            // Update phase for oscillator
            _phase += (float)(2 * Math.PI * _frequencies[_currentStep] / _waveFormat.SampleRate);
            if (_phase > 2 * Math.PI) _phase -= (float)(2 * Math.PI);
        }

        return count;
    }

    private float GenerateSample()
    {
        // Basic drum synthesis - this is a very simple implementation
        // In a real application, you'd want to use proper drum samples or more sophisticated synthesis
        string currentDrum = _pattern.DrumSequence[_currentStep];
        float sample = 0f;

        switch (currentDrum.ToLower())
        {
            case "kick":
                // Simple kick drum - sine wave with exponential amplitude decay
                sample = (float)(Math.Sin(_phase) * Math.Exp(-5.0 * _currentSampleInStep / _samplesPerStep));
                break;
            case "snare":
                // Simple snare - noise with exponential amplitude decay
                sample = (float)((new Random().NextDouble() * 2 - 1) * Math.Exp(-8.0 * _currentSampleInStep / _samplesPerStep));
                break;
            case "hat":
                // Simple hi-hat - filtered noise with quick decay
                sample = (float)((new Random().NextDouble() * 2 - 1) * Math.Exp(-30.0 * _currentSampleInStep / _samplesPerStep));
                break;
        }

        // Add a simple chord tone based on the current step
        if (_pattern.ChordProgression?.Length > 0)
        {
            float chordTone = (float)(Math.Sin(_phase) * 0.3 * Math.Exp(-2.0 * _currentSampleInStep / _samplesPerStep));
            sample += chordTone;
        }

        return sample * 0.5f; // Reduce overall volume
    }
} 