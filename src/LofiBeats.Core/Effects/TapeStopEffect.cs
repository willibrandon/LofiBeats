using NAudio.Wave;
using Microsoft.Extensions.Logging;

namespace LofiBeats.Core.Effects;

public class TapeStopEffect : IAudioEffect
{
    private ISampleProvider _source;
    private readonly ILogger<TapeStopEffect> _logger;

    private readonly float _durationSeconds;
    private float _samplesProcessed;
    private readonly float _totalSamples;
    private bool _finished;
    private float _readPosition;
    private float[] _sampleBuffer;
    private int _writePosition;
    private readonly int _bufferSize;
    private bool _bufferFilled;
    private readonly float _initialPitch = 1.0f;

    public string Name => "tapestop";
    public WaveFormat WaveFormat => _source.WaveFormat;
    public bool IsFinished => _finished;

    public TapeStopEffect(
        ISampleProvider source,
        ILogger<TapeStopEffect> logger,
        float durationSeconds = 2.0f)
    {
        _source = source ?? throw new ArgumentNullException(nameof(source));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _durationSeconds = durationSeconds;
        _totalSamples = durationSeconds * WaveFormat.SampleRate * WaveFormat.Channels;
        _finished = false;

        // Buffer size for storing samples (2 seconds worth)
        _bufferSize = WaveFormat.SampleRate * WaveFormat.Channels * 2;
        _sampleBuffer = new float[_bufferSize];
        _bufferFilled = false;
        
        _logger.LogInformation("TapeStopEffect initialized with duration: {Duration}s", durationSeconds);
    }

    public void SetSource(ISampleProvider source)
    {
        _source = source ?? throw new ArgumentNullException(nameof(source));
        _logger.LogInformation("TapeStopEffect source updated");
    }

    public int Read(float[] buffer, int offset, int count)
    {
        if (_finished)
        {
            return 0;
        }

        // First, read new samples into our buffer
        int samplesRead = _source.Read(_sampleBuffer, _writePosition, Math.Min(count, _bufferSize - _writePosition));
        
        if (samplesRead == 0)
        {
            _finished = true;
            return 0;
        }

        _writePosition = (_writePosition + samplesRead) % _bufferSize;
        
        if (!_bufferFilled && _writePosition >= _bufferSize / 2)
        {
            _bufferFilled = true;
        }

        if (!_bufferFilled)
        {
            // Copy input directly to output until we have enough samples
            Array.Copy(_sampleBuffer, 0, buffer, offset, samplesRead);
            return samplesRead;
        }

        // Apply the effect
        ApplyEffect(buffer, offset, count);
        return count;
    }

    public void ApplyEffect(float[] buffer, int offset, int count)
    {
        if (!_bufferFilled)
        {
            return;
        }

        for (int i = 0; i < count; i++)
        {
            float progress = Math.Min(1.0f, _samplesProcessed / _totalSamples);
            if (progress >= 1.0f)
            {
                buffer[offset + i] = 0f;
                _finished = true;
                continue;
            }

            // Calculate pitch scaling factor using a curve that starts slow and accelerates
            float pitchScale = (float)Math.Pow(1.0f - progress, 2.0) * _initialPitch;

            // Read position moves through the buffer at varying speed
            _readPosition += pitchScale;
            while (_readPosition >= _bufferSize)
            {
                _readPosition -= _bufferSize;
            }

            // Get the two sample positions for interpolation
            int pos1 = (int)_readPosition;
            int pos2 = (pos1 + 1) % _bufferSize;

            // Calculate interpolation fraction
            float fraction = _readPosition - pos1;

            // Get samples and interpolate
            float sample1 = _sampleBuffer[pos1];
            float sample2 = _sampleBuffer[pos2];
            float interpolatedSample = sample1 + (sample2 - sample1) * fraction;

            // Apply volume curve - use exponential fade for more natural sound
            float volume = (float)Math.Pow(1.0f - progress, 1.5);
            buffer[offset + i] = interpolatedSample * volume;

            _samplesProcessed++;
        }

        _logger.LogTrace("Applied tape stop effect to {SampleCount} samples, progress: {Progress:P2}", 
            count, _samplesProcessed / _totalSamples);
    }
} 