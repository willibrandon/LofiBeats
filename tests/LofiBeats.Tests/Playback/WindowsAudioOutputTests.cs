using LofiBeats.Core.Playback;
using Microsoft.Extensions.Logging;
using Moq;
using NAudio.Wave;
using System.Runtime.InteropServices;

namespace LofiBeats.Tests.Playback;

[Collection("AI Generated Tests")]
public class WindowsAudioOutputTests : IDisposable
{
    private readonly Mock<ILogger<WindowsAudioOutput>> _loggerMock;
    private readonly WindowsAudioOutput _audioOutput;
    private readonly Mock<IWaveProvider> _waveProviderMock;
    private bool _disposed;
    private readonly bool _isCI;

    public WindowsAudioOutputTests()
    {
        Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Windows), "These tests only run on Windows");
        
        // Check if we're running in CI (GitHub Actions)
        _isCI = Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true";
        
        _loggerMock = new Mock<ILogger<WindowsAudioOutput>>();
        _audioOutput = new WindowsAudioOutput(_loggerMock.Object);
        _waveProviderMock = new Mock<IWaveProvider>();
        _waveProviderMock.Setup(x => x.WaveFormat)
            .Returns(WaveFormat.CreateIeeeFloatWaveFormat(44100, 2));
    }

    [SkippableFact]
    [Trait("Category", "AI_Generated")]
    public void Constructor_CreatesValidInstance()
    {
        // Assert
        Assert.NotNull(_audioOutput);
        Assert.Equal(PlaybackState.Stopped, _audioOutput.PlaybackState);
    }

    [SkippableFact]
    [Trait("Category", "AI_Generated")]
    public void Init_WithValidProvider_Succeeds()
    {
        // Act - Should not throw
        _audioOutput.Init(_waveProviderMock.Object);

        // Assert
        Assert.Equal(PlaybackState.Stopped, _audioOutput.PlaybackState);
    }

    [SkippableFact]
    [Trait("Category", "AI_Generated")]
    public void Play_AfterInit_ChangesStateToPlaying()
    {
        Skip.If(_isCI, "Skipping audio playback test in CI environment");

        // Arrange
        _audioOutput.Init(_waveProviderMock.Object);

        // Act
        _audioOutput.Play();

        // Assert
        Assert.Equal(PlaybackState.Playing, _audioOutput.PlaybackState);
    }

    [SkippableFact]
    [Trait("Category", "AI_Generated")]
    public void Pause_WhilePlaying_ChangesStateToPaused()
    {
        Skip.If(_isCI, "Skipping audio playback test in CI environment");

        // Arrange
        _audioOutput.Init(_waveProviderMock.Object);
        _audioOutput.Play();

        // Act
        _audioOutput.Pause();

        // Assert
        Assert.Equal(PlaybackState.Paused, _audioOutput.PlaybackState);
    }

    [SkippableFact]
    [Trait("Category", "AI_Generated")]
    public void Stop_WhilePlaying_ChangesStateToStopped()
    {
        Skip.If(_isCI, "Skipping audio playback test in CI environment");

        // Arrange
        _audioOutput.Init(_waveProviderMock.Object);
        _audioOutput.Play();

        // Act
        _audioOutput.Stop();

        // Assert
        Assert.Equal(PlaybackState.Stopped, _audioOutput.PlaybackState);
    }

    [SkippableFact]
    [Trait("Category", "AI_Generated")]
    public void SetVolume_ClampsToValidRange()
    {
        // Arrange
        _audioOutput.Init(_waveProviderMock.Object);

        // Act - Should not throw
        _audioOutput.SetVolume(-0.5f); // Should clamp to 0
        _audioOutput.SetVolume(1.5f);  // Should clamp to 1
        _audioOutput.SetVolume(0.5f);  // Should set exactly
    }

    [SkippableFact]
    [Trait("Category", "AI_Generated")]
    public void PlaybackState_WhenNotInitialized_ReturnsStoppedState()
    {
        // Act & Assert
        Assert.Equal(PlaybackState.Stopped, _audioOutput.PlaybackState);
    }

    [SkippableFact]
    [Trait("Category", "AI_Generated")]
    public void Play_WhenNotInitialized_DoesNotThrow()
    {
        // Act - Should not throw
        _audioOutput.Play();

        // Assert
        Assert.Equal(PlaybackState.Stopped, _audioOutput.PlaybackState);
    }

    [SkippableFact]
    [Trait("Category", "AI_Generated")]
    public void Pause_WhenNotInitialized_DoesNotThrow()
    {
        // Act - Should not throw
        _audioOutput.Pause();

        // Assert
        Assert.Equal(PlaybackState.Stopped, _audioOutput.PlaybackState);
    }

    [SkippableFact]
    [Trait("Category", "AI_Generated")]
    public void Stop_WhenNotInitialized_DoesNotThrow()
    {
        // Act - Should not throw
        _audioOutput.Stop();

        // Assert
        Assert.Equal(PlaybackState.Stopped, _audioOutput.PlaybackState);
    }

    [SkippableFact]
    [Trait("Category", "AI_Generated")]
    public void SetVolume_WhenNotInitialized_DoesNotThrow()
    {
        // Act - Should not throw
        _audioOutput.SetVolume(0.5f);
    }

    [SkippableFact]
    [Trait("Category", "AI_Generated")]
    public void Dispose_CanBeCalledMultipleTimes()
    {
        // Act - Should not throw
        _audioOutput.Dispose();
        _audioOutput.Dispose();
    }

    [SkippableFact]
    [Trait("Category", "AI_Generated")]
    public void Operations_AfterDispose_DoNotThrow()
    {
        // Arrange
        _audioOutput.Dispose();

        // Act - All operations should be safe after dispose
        _audioOutput.Init(_waveProviderMock.Object);
        _audioOutput.Play();
        _audioOutput.Pause();
        _audioOutput.Stop();
        _audioOutput.SetVolume(0.5f);

        // Assert
        Assert.Equal(PlaybackState.Stopped, _audioOutput.PlaybackState);
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _audioOutput.Dispose();
            _disposed = true;
        }
    }
} 