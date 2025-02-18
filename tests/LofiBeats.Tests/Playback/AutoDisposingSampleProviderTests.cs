using Moq;
using NAudio.Wave;
using LofiBeats.Core.Playback;

namespace LofiBeats.Tests.Playback;

[Collection("AI Generated Tests")]
public class AutoDisposingSampleProviderTests
{
    [Fact]
    [Trait("Category", "AI_Generated")]
    public void WaveFormat_ShouldMatchUnderlyingProvider()
    {
        // Arrange
        var mockFormat = new WaveFormat(44100, 2);
        var mockProvider = new Mock<ISampleProvider>();
        mockProvider.Setup(x => x.WaveFormat).Returns(mockFormat);

        // Act
        var provider = new AutoDisposingSampleProvider(mockProvider.Object);

        // Assert
        Assert.Same(mockFormat, provider.WaveFormat);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void Read_ShouldReturnDataFromUnderlyingProvider()
    {
        // Arrange
        var mockProvider = new Mock<ISampleProvider>();
        mockProvider.Setup(x => x.Read(It.IsAny<float[]>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns<float[], int, int>((buffer, offset, count) => count);

        var provider = new AutoDisposingSampleProvider(mockProvider.Object);
        var buffer = new float[1024];

        // Act
        int samplesRead = provider.Read(buffer, 0, buffer.Length);

        // Assert
        Assert.Equal(buffer.Length, samplesRead);
        mockProvider.Verify(x => x.Read(buffer, 0, buffer.Length), Times.Once);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void Read_WhenNoMoreData_ShouldDisposeUnderlyingProvider()
    {
        // Arrange
        var mockProvider = new Mock<ISampleProvider>();
        mockProvider.As<IDisposable>();
        mockProvider.Setup(x => x.Read(It.IsAny<float[]>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns(0);

        var provider = new AutoDisposingSampleProvider(mockProvider.Object);
        var buffer = new float[1024];

        // Act
        provider.Read(buffer, 0, buffer.Length);

        // Assert
        mockProvider.As<IDisposable>().Verify(x => x.Dispose(), Times.Once);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void Read_AfterDisposal_ShouldReturnZero()
    {
        // Arrange
        var mockProvider = new Mock<ISampleProvider>();
        mockProvider.As<IDisposable>();
        mockProvider.Setup(x => x.Read(It.IsAny<float[]>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns(0);

        var provider = new AutoDisposingSampleProvider(mockProvider.Object);
        var buffer = new float[1024];

        // Act
        int firstRead = provider.Read(buffer, 0, buffer.Length);
        int secondRead = provider.Read(buffer, 0, buffer.Length);

        // Assert
        Assert.Equal(0, firstRead);
        Assert.Equal(0, secondRead);
        mockProvider.As<IDisposable>().Verify(x => x.Dispose(), Times.Once);
    }

    [Fact]
    [Trait("Category", "AI_Generated")]
    public void Read_WithNonDisposableProvider_ShouldHandleEndOfStreamGracefully()
    {
        // Arrange
        var mockProvider = new Mock<ISampleProvider>();
        mockProvider.Setup(x => x.Read(It.IsAny<float[]>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns(0);

        var provider = new AutoDisposingSampleProvider(mockProvider.Object);
        var buffer = new float[1024];

        // Act
        int samplesRead = provider.Read(buffer, 0, buffer.Length);

        // Assert
        Assert.Equal(0, samplesRead);
        // No exception should be thrown even though the provider is not disposable
    }
} 