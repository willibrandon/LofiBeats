using LofiBeats.Core.Playback;
using Microsoft.Extensions.Logging;
using Moq;
using NAudio.Wave;
using OpenTK.Audio.OpenAL;
using System.Runtime.InteropServices;

namespace LofiBeats.Tests.Playback
{
    public class OpenALAudioOutputTests : IDisposable
    {
        private readonly Mock<ILogger<OpenALAudioOutput>> _loggerMock;
        private readonly TestWaveProvider _waveProvider;
        private readonly OpenALAudioOutput? _openAL;
        private readonly bool _isLinux;

        public OpenALAudioOutputTests()
        {
            _loggerMock = new Mock<ILogger<OpenALAudioOutput>>();
            _waveProvider = new TestWaveProvider();
            _isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            
            // Only create OpenAL instance if we're on Linux
            if (_isLinux)
            {
                _openAL = new OpenALAudioOutput(_loggerMock.Object);
                _openAL.Init(_waveProvider);
            }
        }

        public void Dispose()
        {
            if (_isLinux)
            {
                _openAL?.Dispose();
            }
        }

        [SkippableFact]
        public void Play_ProcessesAudioData()
        {
            Skip.IfNot(_isLinux, "Test only runs on Linux");
            Assert.NotNull(_openAL);

            // Arrange
            _waveProvider.Reset();

            // Act
            _openAL.Play();
            Thread.Sleep(100); // Give some time for audio processing

            // Assert
            Assert.Equal(PlaybackState.Playing, _openAL.PlaybackState);
            Assert.True(_waveProvider.ReadCount > 0, "No audio data was read from the provider");
            
            // Verify OpenAL state
            var state = AL.GetSource(_openAL.SourceId, ALGetSourcei.SourceState);
            Assert.Equal((int)ALSourceState.Playing, state);
            
            // Check for any OpenAL errors
            var error = AL.GetError();
            Assert.Equal(ALError.NoError, error);
        }

        [SkippableFact]
        public void Pause_StopsProcessingAudio()
        {
            Skip.IfNot(_isLinux, "Test only runs on Linux");
            Assert.NotNull(_openAL);

            // Arrange
            _waveProvider.Reset();
            _openAL.Play();
            Thread.Sleep(50);
            var readCountBeforePause = _waveProvider.ReadCount;

            // Act
            _openAL.Pause();
            Thread.Sleep(50);

            // Assert
            Assert.Equal(PlaybackState.Paused, _openAL.PlaybackState);
            Assert.Equal(readCountBeforePause, _waveProvider.ReadCount);
            
            // Verify OpenAL state
            var state = AL.GetSource(_openAL.SourceId, ALGetSourcei.SourceState);
            Assert.Equal((int)ALSourceState.Paused, state);
            
            var error = AL.GetError();
            Assert.Equal(ALError.NoError, error);
        }

        [SkippableFact]
        public void Stop_ClearsBuffers()
        {
            Skip.IfNot(_isLinux, "Test only runs on Linux");
            Assert.NotNull(_openAL);

            // Arrange
            _waveProvider.Reset();
            _openAL.Play();
            Thread.Sleep(50);

            // Act
            _openAL.Stop();

            // Assert
            Assert.Equal(PlaybackState.Stopped, _openAL.PlaybackState);
            
            // Verify OpenAL state
            var state = AL.GetSource(_openAL.SourceId, ALGetSourcei.SourceState);
            Assert.Equal((int)ALSourceState.Stopped, state);
            
            // Verify buffers were unqueued
            var queued = AL.GetSource(_openAL.SourceId, ALGetSourcei.BuffersQueued);
            Assert.Equal(0, queued);
            
            var error = AL.GetError();
            Assert.Equal(ALError.NoError, error);
        }

        [SkippableFact]
        public void SetVolume_AffectsGain()
        {
            Skip.IfNot(_isLinux, "Test only runs on Linux");
            Assert.NotNull(_openAL);

            // Act
            _openAL.SetVolume(0.5f);

            // Assert
            float gain = AL.GetSource(_openAL.SourceId, ALSourcef.Gain);
            Assert.Equal(0.5f, gain, 0.01f);
            
            var error = AL.GetError();
            Assert.Equal(ALError.NoError, error);
        }

        [Fact]
        public void Play_WithNullDevice_ThrowsException()
        {
            // Skip if OpenAL is not initialized
            if (_openAL == null) return;

            // Arrange
            var provider = new TestWaveProvider();
            _openAL.Init(provider);

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => _openAL.Play());
            Assert.Contains("No valid audio output device", ex.Message);
        }

        private sealed class TestWaveProvider : IWaveProvider
        {
            public WaveFormat WaveFormat { get; }
            public int ReadCount { get; private set; }
            
            public TestWaveProvider()
            {
                WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);
            }
            
            public void Reset()
            {
                ReadCount = 0;
            }

            public int Read(byte[] buffer, int offset, int count)
            {
                ReadCount++;
                
                // Generate a simple sine wave
                for (int i = 0; i < count / 4; i++)
                {
                    float sample = (float)Math.Sin(2 * Math.PI * 440 * i / 44100);
                    byte[] bytes = BitConverter.GetBytes(sample);
                    Buffer.BlockCopy(bytes, 0, buffer, offset + i * 4, 4);
                }
                
                return count;
            }
        }
    }
}
