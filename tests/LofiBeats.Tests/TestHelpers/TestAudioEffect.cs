using LofiBeats.Core.PluginApi;
using NAudio.Wave;

namespace LofiBeats.Tests.TestHelpers;

public sealed class TestAudioEffect : IAudioEffect
{
    private WaveFormat _waveFormat;
    private ISampleProvider? _source;

    public string Author => "LofiBeats Team";

    public string Description => "An effect used for testing";

    public string Name => "testeffect";

    public string Version => "1.0.0";

    public WaveFormat WaveFormat => _waveFormat;

    public TestAudioEffect()
    {
        _waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);
    }

    public void SetSource(ISampleProvider source)
    {
        _source = source;
        _waveFormat = source.WaveFormat;
    }

    public void ApplyEffect(float[] buffer, int offset, int count)
    {
        // No-op for test
    }

    public int Read(float[] buffer, int offset, int count)
    {
        return _source?.Read(buffer, offset, count) ?? count;
    }
} 