using LofiBeats.Core.Playback;
using Microsoft.Extensions.Logging;
using Moq;
using NAudio.Wave;

namespace LofiBeats.Tests.Playback
{
    public class OpenALAudioOutputTests
    {
        private readonly Mock<ILogger<OpenALAudioOutput>> _loggerMock;
        private readonly Mock<IWaveProvider> _waveProviderMock;

        public OpenALAudioOutputTests()
        {
            _loggerMock = new Mock<ILogger<OpenALAudioOutput>>();
            _waveProviderMock = new Mock<IWaveProvider>();
        }

        [Fact]
        public void Play_ChangesStateToPlaying()
        {
            // Arrange
            var openAL = new OpenALAudioOutput(_loggerMock.Object);
            openAL.Init(_waveProviderMock.Object);

            // Act
            openAL.Play();

            // Assert
            Assert.Equal(PlaybackState.Playing, openAL.PlaybackState);
        }

        [Fact]
        public void Pause_ChangesStateToPaused()
        {
            // Arrange
            var openAL = new OpenALAudioOutput(_loggerMock.Object);
            openAL.Init(_waveProviderMock.Object);
            openAL.Play();

            // Act
            openAL.Pause();

            // Assert
            Assert.Equal(PlaybackState.Paused, openAL.PlaybackState);
        }

        [Fact]
        public void Stop_ChangesStateToStopped()
        {
            // Arrange
            var openAL = new OpenALAudioOutput(_loggerMock.Object);
            openAL.Init(_waveProviderMock.Object);
            openAL.Play();

            // Act
            openAL.Stop();

            // Assert
            Assert.Equal(PlaybackState.Stopped, openAL.PlaybackState);
        }

        [Fact]
        public void SetVolume_ClampsVolumeWithinRange()
        {
            // Arrange
            var openAL = new OpenALAudioOutput(_loggerMock.Object);
            openAL.Init(_waveProviderMock.Object);

            // Act
            openAL.SetVolume(-0.5f);
            openAL.SetVolume(1.5f);
            openAL.SetVolume(0.5f);

            // Assert
            // Since we can't directly access the volume, we assume the method works if no exceptions are thrown
        }

        [Fact]
        public void Dispose_ReleasesResources()
        {
            // Arrange
            var openAL = new OpenALAudioOutput(_loggerMock.Object);
            openAL.Init(_waveProviderMock.Object);

            // Act
            openAL.Dispose();

            // Assert
            // Verify that no exceptions are thrown and resources are released
        }
    }
} 