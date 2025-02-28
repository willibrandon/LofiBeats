using LofiBeats.Core.Playback;
using Microsoft.Extensions.Logging;
using Moq;
using NAudio.Wave;
using OpenTK.Audio.OpenAL;
using System.Runtime.InteropServices;

namespace LofiBeats.Tests.Playback
{
    [Collection("AI Generated Tests")]
    public class OpenALAudioOutputTests : IDisposable
    {
        private readonly Mock<ILogger<OpenALAudioOutput>> _loggerMock;
        private readonly TestWaveProvider _waveProvider;
        private readonly OpenALAudioOutput? _openAL;
        private readonly bool _isUnix;

        public OpenALAudioOutputTests()
        {
            _loggerMock = new Mock<ILogger<OpenALAudioOutput>>();
            _waveProvider = new TestWaveProvider();
            _isUnix = RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || 
                      RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
            
            // Only create OpenAL instance if we're on Linux or macOS
            if (_isUnix)
            {
                _openAL = new OpenALAudioOutput(_loggerMock.Object);
                _openAL.Init(_waveProvider);
            }
        }

        public void Dispose()
        {
            if (_isUnix)
            {
                _openAL?.Dispose();
            }

            GC.SuppressFinalize(this);
        }

        [SkippableFact]
        [Trait("Category", "AI_Generated")]
        public void Play_ProcessesAudioData()
        {
            Skip.IfNot(_isUnix, "Test only runs on Unix-like systems (Linux/macOS)");
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
        [Trait("Category", "AI_Generated")]
        public void Pause_StopsProcessingAudio()
        {
            Skip.IfNot(_isUnix, "Test only runs on Unix-like systems (Linux/macOS)");
            Assert.NotNull(_openAL);

            // Arrange
            _waveProvider.Reset();
            _openAL.Play();
            
            // Wait for initial buffer queue to stabilize
            var stableCount = 0;
            var previousReadCount = 0;
            var attempts = 0;
            while (stableCount < 3 && attempts < 20)
            {
                Thread.Sleep(50);
                if (_waveProvider.ReadCount == previousReadCount)
                {
                    stableCount++;
                }
                else
                {
                    stableCount = 0;
                }
                previousReadCount = _waveProvider.ReadCount;
                attempts++;
            }
            
            var readCountBeforePause = _waveProvider.ReadCount;
            Assert.True(readCountBeforePause > 0, "No audio data was read before pause");

            // Act
            _openAL.Pause();
            Thread.Sleep(100); // Give more time for any in-flight buffers to complete

            // Assert
            Assert.Equal(PlaybackState.Paused, _openAL.PlaybackState);
            
            // Get the read count and wait a bit to ensure it's stable
            var readCountAfterPause = _waveProvider.ReadCount;
            Thread.Sleep(100);
            var finalReadCount = _waveProvider.ReadCount;
            
            // Verify that read count stabilizes after pause
            var message = $"Audio processing continued after pause stabilization (after pause: {readCountAfterPause}, final: {finalReadCount})";
            Assert.True(readCountAfterPause == finalReadCount, message);
            
            // Verify OpenAL state
            var state = AL.GetSource(_openAL.SourceId, ALGetSourcei.SourceState);
            Assert.True(state == (int)ALSourceState.Paused, "OpenAL source state was not paused");
            
            var error = AL.GetError();
            Assert.True(error == ALError.NoError, "OpenAL reported an error");
        }

        [SkippableFact]
        [Trait("Category", "AI_Generated")]
        public void Stop_ClearsBuffers()
        {
            Skip.IfNot(_isUnix, "Test only runs on Unix-like systems (Linux/macOS)");
            Assert.NotNull(_openAL);

            // Arrange
            _waveProvider.Reset();
            _openAL.Play();
            
            // Wait for buffers to be queued and stable (up to 1000ms)
            var initialQueued = 0;
            var previousQueued = 0;
            var stableCount = 0;
            var attempts = 0;
            while (stableCount < 3 && attempts < 20)
            {
                Thread.Sleep(50);
                initialQueued = AL.GetSource(_openAL.SourceId, ALGetSourcei.BuffersQueued);
                if (initialQueued == previousQueued && initialQueued > 0)
                {
                    stableCount++;
                }
                else
                {
                    stableCount = 0;
                }
                previousQueued = initialQueued;
                attempts++;
            }
            
            Assert.True(initialQueued > 0, "No buffers were queued before stopping");
            Console.WriteLine($"Initial queued buffers: {initialQueued}");

            // Act
            _openAL.Stop();
            Thread.Sleep(50); // Give more time for the source to fully stop

            // Assert
            Assert.Equal(PlaybackState.Stopped, _openAL.PlaybackState);
            
            // Verify OpenAL state
            var state = AL.GetSource(_openAL.SourceId, ALGetSourcei.SourceState);
            Console.WriteLine($"Expected state: either {(int)ALSourceState.Stopped} (Stopped) or {(int)ALSourceState.Initial} (Initial), Actual state: {state}");
            Assert.True(
                state == (int)ALSourceState.Stopped || state == (int)ALSourceState.Initial,
                $"Expected source to be in Stopped (0x1014) or Initial (0x1011) state, but got: 0x{state:X4}"
            );
            
            // Verify all buffers were unqueued
            var finalQueued = AL.GetSource(_openAL.SourceId, ALGetSourcei.BuffersQueued);
            Console.WriteLine($"Final queued buffers: {finalQueued}");
            Assert.True(finalQueued == 0, "Expected all buffers to be unqueued");
            
            var error = AL.GetError();
            Assert.Equal(ALError.NoError, error);
        }

        [SkippableFact]
        [Trait("Category", "AI_Generated")]
        public void SetVolume_AffectsGain()
        {
            Skip.IfNot(_isUnix, "Test only runs on Unix-like systems (Linux/macOS)");
            Assert.NotNull(_openAL);

            // Act
            _openAL.SetVolume(0.5f);

            // Assert
            float gain = AL.GetSource(_openAL.SourceId, ALSourcef.Gain);
            Assert.Equal(0.5f, gain, 0.01f);
            
            var error = AL.GetError();
            Assert.Equal(ALError.NoError, error);
        }

        [SkippableFact]
        [Trait("Category", "AI_Generated")]
        public void Play_WithNullDevice_ThrowsException()
        {
            Skip.IfNot(_isUnix, "Test only runs on Unix-like systems (Linux/macOS)");

            // Arrange
            var logger = new Mock<ILogger<OpenALAudioOutput>>().Object;
            var openAL = new OpenALAudioOutput(logger);

            // Simulate null device by not initializing the wave provider
            // This should cause Play() to throw since there's no valid device

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => openAL.Play());
            Assert.Contains("No valid audio output device", ex.Message);
        }

        [SkippableFact]
        [Trait("Category", "AI_Generated")]
        public void Play_AfterTapeStop_DetectsDeadlockedState()
        {
            Skip.IfNot(_isUnix, "Test only runs on Unix-like systems (Linux/macOS)");
            Assert.NotNull(_openAL);

            // Arrange
            _waveProvider.Reset();
            
            // First play - should work normally
            _openAL.Play();
            Thread.Sleep(500); // Increased wait time for initial playback
            
            // Verify initial state is good and buffers are being processed
            var initialState = AL.GetSource(_openAL.SourceId, ALGetSourcei.SourceState);
            Console.WriteLine($"Initial state: {initialState}");
            Assert.Equal((int)ALSourceState.Playing, initialState);
            
            // Wait for some initial buffer processing
            var initialProcessed = 0;
            var attempts = 0;
            while (initialProcessed == 0 && attempts < 10)
            {
                Thread.Sleep(100);
                initialProcessed = AL.GetSource(_openAL.SourceId, ALGetSourcei.BuffersProcessed);
                attempts++;
            }
            
            var initialQueued = AL.GetSource(_openAL.SourceId, ALGetSourcei.BuffersQueued);
            Console.WriteLine($"Initial state - Queued: {initialQueued}, Processed: {initialProcessed}");
            Assert.True(initialQueued > 0, "No buffers were initially queued");
            Assert.True(initialProcessed > 0, "No buffers were processed during initial playback");
            
            // Simulate tape stop by stopping playback
            _openAL.Stop();
            Thread.Sleep(200);
            
            // Verify stopped state and buffer counts
            var stoppedState = AL.GetSource(_openAL.SourceId, ALGetSourcei.SourceState);
            var stoppedQueued = AL.GetSource(_openAL.SourceId, ALGetSourcei.BuffersQueued);
            Console.WriteLine($"After stop - State: {stoppedState}, Queued: {stoppedQueued}");
            
            Assert.True(
                stoppedState == (int)ALSourceState.Initial || stoppedState == (int)ALSourceState.Stopped,
                $"Expected source to be in Initial or Stopped state after stop, but got: {stoppedState}"
            );
            Assert.True(stoppedQueued == 0, "Buffers should be unqueued after stop");
            
            // Try to play again
            _openAL.Play();
            Thread.Sleep(500);
            
            // Now check the actual state
            var playingState = AL.GetSource(_openAL.SourceId, ALGetSourcei.SourceState);
            var queuedAfter = AL.GetSource(_openAL.SourceId, ALGetSourcei.BuffersQueued);
            Console.WriteLine($"After play - State: {playingState}, Queued: {queuedAfter}");
            
            // Verify we're actually playing and have buffers queued
            Assert.Equal((int)ALSourceState.Playing, playingState);
            Assert.True(queuedAfter > 0, "No buffers queued after play");
            
            // Wait for buffer processing to start
            var maxAttempts = 20;
            var processedAfter = 0;
            attempts = 0;
            
            while (processedAfter == 0 && attempts < maxAttempts)
            {
                Thread.Sleep(100);
                processedAfter = AL.GetSource(_openAL.SourceId, ALGetSourcei.BuffersProcessed);
                var currentState = AL.GetSource(_openAL.SourceId, ALGetSourcei.SourceState);
                var currentQueued = AL.GetSource(_openAL.SourceId, ALGetSourcei.BuffersQueued);
                Console.WriteLine($"Attempt {attempts + 1} - State: {currentState}, Queued: {currentQueued}, Processed: {processedAfter}");
                attempts++;
            }
            
            var message = $"Source claims to be playing but buffers aren't being processed.\n" +
                         $"Initial state - Queued: {initialQueued}, Processed: {initialProcessed}\n" +
                         $"After stop - State: {stoppedState}, Queued: {stoppedQueued}\n" +
                         $"After play - State: {playingState}, Queued: {queuedAfter}\n" +
                         $"Final - Processed: {processedAfter} after {attempts * 100}ms";
            
            Assert.True(processedAfter > 0, message);
            
            // Verify no OpenAL errors occurred
            var error = AL.GetError();
            Assert.Equal(ALError.NoError, error);
        }

        private sealed class TestWaveProvider : IWaveProvider
        {
            public WaveFormat WaveFormat { get; }
            public int ReadCount { get; private set; }
            private const int BUFFER_SIZE = 8192;
            private const int BYTES_PER_FRAME = 8; // 2 channels * 4 bytes per float
            
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
                
                // Calculate how many complete frames we can write
                int framesRequested = count / BYTES_PER_FRAME;
                int framesToWrite = Math.Min(framesRequested, BUFFER_SIZE / BYTES_PER_FRAME);
                int bytesToWrite = framesToWrite * BYTES_PER_FRAME;
                
                // Generate a simple sine wave for both channels
                for (int i = 0; i < framesToWrite; i++)
                {
                    float sample = (float)Math.Sin(2 * Math.PI * 440 * i / 44100);
                    byte[] leftBytes = BitConverter.GetBytes(sample);
                    byte[] rightBytes = BitConverter.GetBytes(sample);
                    
                    // Write left channel
                    Buffer.BlockCopy(leftBytes, 0, buffer, offset + i * BYTES_PER_FRAME, 4);
                    // Write right channel
                    Buffer.BlockCopy(rightBytes, 0, buffer, offset + i * BYTES_PER_FRAME + 4, 4);
                }
                
                return bytesToWrite;
            }
        }
    }
}
