using NAudio.Wave;

namespace LofiBeats.Core.Playback;

public class TestTone : ISampleProvider
{
    private readonly WaveFormat _wf = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);
    private float _phase;
    private readonly float _phaseIncrement = (float)(2 * Math.PI * 440.0 / 44100);

    public WaveFormat WaveFormat => _wf;

    public int Read(float[] buffer, int offset, int count)
    {
        for (int n = 0; n < count; n += 2)
        {
            float sample = (float)Math.Sin(_phase);
            buffer[offset + n] = sample;     // left
            buffer[offset + n + 1] = sample; // right
            _phase += _phaseIncrement;
            if (_phase > 2 * Math.PI) _phase -= (float)(2 * Math.PI);
        }
        return count;
    }
} 