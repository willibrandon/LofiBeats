using NAudio.Wave;
using Xunit;

namespace LofiBeats.Plugins.DelayEffect.Tests;

public class DelayAudioEffectTests
{
    private sealed class TestSampleProvider(float[] samples, int sampleRate = 44100, int channels = 2) : ISampleProvider
    {
        public WaveFormat WaveFormat { get; } = WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channels);

        private int _position;

        public int Read(float[] buffer, int offset, int count)
        {
            int samplesAvailable = Math.Min(count, samples.Length - _position);
            Array.Copy(samples, _position, buffer, offset, samplesAvailable);
            _position += samplesAvailable;
            return samplesAvailable;
        }
    }

    [Fact]
    public void Constructor_InitializesWithDefaultValues()
    {
        // Arrange & Act
        var effect = new DelayAudioEffect();

        // Assert
        Assert.NotNull(effect.WaveFormat);
        Assert.Equal(44100, effect.WaveFormat.SampleRate);
        Assert.Equal(2, effect.WaveFormat.Channels);
    }

    [Fact]
    public void SetSource_UpdatesWaveFormat()
    {
        // Arrange
        var effect = new DelayAudioEffect();
        var testSamples = new float[1000];
        var source = new TestSampleProvider(testSamples, 48000, 1);

        // Act
        effect.SetSource(source);

        // Assert
        Assert.Equal(48000, effect.WaveFormat.SampleRate);
        Assert.Equal(1, effect.WaveFormat.Channels);
    }

    [Fact]
    public void ApplyEffect_CreatesDelayedSignal()
    {
        // Arrange
        var effect = new DelayAudioEffect();
        // Create a buffer large enough to hold the delay (500ms = 22050 samples at 44.1kHz)
        var inputSamples = new float[44100]; // 1 second of audio
        // Create an impulse at the start
        inputSamples[0] = 1.0f;
        var source = new TestSampleProvider(inputSamples);
        effect.SetSource(source);

        var outputBuffer = new float[44100];

        // Act
        int samplesRead = effect.Read(outputBuffer, 0, outputBuffer.Length);

        // Assert
        Assert.Equal(44100, samplesRead);
        // Check for delayed signal (should appear around 22050 samples - 500ms at 44.1kHz)
        Assert.True(outputBuffer[0] > 0.5f); // Original signal with dry mix
        // Find a delayed peak after the initial signal
        bool foundDelayedPeak = false;
        for (int i = 21000; i < 23000; i++)
        {
            if (outputBuffer[i] > 0.2f)
            {
                foundDelayedPeak = true;
                break;
            }
        }
        Assert.True(foundDelayedPeak, "Should find a delayed signal peak");
    }

    [Fact]
    public void Read_WithNullSource_ReturnsZero()
    {
        // Arrange
        var effect = new DelayAudioEffect();
        var buffer = new float[1000];

        // Act
        int samplesRead = effect.Read(buffer, 0, buffer.Length);

        // Assert
        Assert.Equal(0, samplesRead);
    }
} 