using LofiBeats.Core.Playback;
using Microsoft.Extensions.Logging;
using Moq;
using NAudio.Wave;

namespace LofiBeats.Tests.Playback;

[Collection("AI Generated Tests")]
public class UserSampleRepositoryTests : IDisposable
{
    private readonly Mock<ILogger<UserSampleRepository>> _loggerMock;
    private readonly string _testFilePath;
    private readonly UserSampleRepository _repository;
    private bool _disposed;

    public UserSampleRepositoryTests()
    {
        _loggerMock = new Mock<ILogger<UserSampleRepository>>();
        _testFilePath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.wav");
        CreateTestWavFile(_testFilePath);
        _repository = new UserSampleRepository(_loggerMock.Object);
    }

    private static void CreateTestWavFile(string path)
    {
        var sampleRate = 44100;
        var duration = 0.1f; // 100ms
        var frequency = 440f; // A4 note
        var numSamples = (int)(sampleRate * duration);

        using var writer = new WaveFileWriter(path, new WaveFormat(sampleRate, 16, 1));
        for (int i = 0; i < numSamples; i++)
        {
            var t = (float)i / sampleRate;
            var sample = (float)Math.Sin(2 * Math.PI * frequency * t);
            writer.WriteSample((short)(sample * short.MaxValue));
        }
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void RegisterSample_WithEmptyName_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _repository.RegisterSample("", _testFilePath));
        Assert.Throws<ArgumentException>(() => _repository.RegisterSample(" ", _testFilePath));
        Assert.Throws<ArgumentException>(() => _repository.RegisterSample(null!, _testFilePath));
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void RegisterSample_WithEmptyPath_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _repository.RegisterSample("test", ""));
        Assert.Throws<ArgumentException>(() => _repository.RegisterSample("test", " "));
        Assert.Throws<ArgumentException>(() => _repository.RegisterSample("test", null!));
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void RegisterSample_WithNonExistentFile_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => 
            _repository.RegisterSample("test", "nonexistent.wav"));
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void RegisterSample_WithNonWavFile_ThrowsArgumentException()
    {
        // Arrange
        var mp3Path = Path.Combine(Path.GetTempPath(), "test.mp3");
        File.WriteAllText(mp3Path, "dummy content");

        try
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => 
                _repository.RegisterSample("test", mp3Path));
        }
        finally
        {
            File.Delete(mp3Path);
        }
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void RegisterSample_WithInvalidVelocity_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => 
            _repository.RegisterSample("test", _testFilePath, -1));
        Assert.Throws<ArgumentException>(() => 
            _repository.RegisterSample("test", _testFilePath, 128));
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void UnregisterSample_WithEmptyName_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _repository.UnregisterSample(""));
        Assert.Throws<ArgumentException>(() => _repository.UnregisterSample(" "));
        Assert.Throws<ArgumentException>(() => _repository.UnregisterSample(null!));
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void UnregisterSample_WithNonExistentSample_ReturnsFalse()
    {
        // Act
        bool result = _repository.UnregisterSample("nonexistent");

        // Assert
        Assert.False(result);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void CreateSampleProvider_WithNonExistentSample_ThrowsKeyNotFoundException()
    {
        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => 
            _repository.CreateSampleProvider("nonexistent"));
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void CreateSampleProvider_AfterDispose_ThrowsObjectDisposedException()
    {
        // Arrange
        _repository.Dispose();

        // Act & Assert
        Assert.Throws<ObjectDisposedException>(() => 
            _repository.CreateSampleProvider("test"));
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void RegisterSample_AfterDispose_ThrowsObjectDisposedException()
    {
        // Arrange
        _repository.Dispose();

        // Act & Assert
        Assert.Throws<ObjectDisposedException>(() => 
            _repository.RegisterSample("test", _testFilePath));
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void UnregisterSample_AfterDispose_ThrowsObjectDisposedException()
    {
        // Arrange
        _repository.Dispose();

        // Act & Assert
        Assert.Throws<ObjectDisposedException>(() => 
            _repository.UnregisterSample("test"));
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            if (File.Exists(_testFilePath))
            {
                try { File.Delete(_testFilePath); } catch { }
            }
            _repository.Dispose();
            _disposed = true;
        }

        GC.SuppressFinalize(this);
    }
} 