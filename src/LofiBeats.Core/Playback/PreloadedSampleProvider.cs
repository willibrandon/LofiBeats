using NAudio.Wave;

namespace LofiBeats.Core.Playback;

/// <summary>
/// Provides audio samples from a preloaded memory buffer.
/// </summary>
public class PreloadedSampleProvider : ISampleProvider
{
    private readonly byte[] _audioData;
    private readonly WaveFormat _waveFormat;
    private int _position;
    private readonly int _bytesPerSample;

    public WaveFormat WaveFormat => _waveFormat;

    public PreloadedSampleProvider(byte[] audioData, WaveFormat waveFormat)
    {
        _audioData = audioData;
        _waveFormat = waveFormat;
        _bytesPerSample = waveFormat.BitsPerSample / 8;
    }

    public int Read(float[] buffer, int offset, int count)
    {
        int bytesRead = 0;
        int samplesRead = 0;

        while (samplesRead < count && _position < _audioData.Length)
        {
            if (_position + _bytesPerSample > _audioData.Length)
                break;

            // Convert bytes to float sample
            float sample = 0;
            if (_waveFormat.BitsPerSample == 16)
            {
                short value = BitConverter.ToInt16(_audioData, _position);
                sample = value / 32768f;
            }
            else if (_waveFormat.BitsPerSample == 32)
            {
                int value = BitConverter.ToInt32(_audioData, _position);
                sample = value / 2147483648f;
            }
            else if (_waveFormat.BitsPerSample == 8)
            {
                sample = (_audioData[_position] - 128) / 128f;
            }

            buffer[offset + samplesRead] = sample;
            _position += _bytesPerSample;
            bytesRead += _bytesPerSample;
            samplesRead++;
        }

        return samplesRead;
    }

    public void Reset()
    {
        _position = 0;
    }
} 