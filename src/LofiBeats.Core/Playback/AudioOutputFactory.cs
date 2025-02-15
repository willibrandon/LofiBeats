using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;

namespace LofiBeats.Core.Playback;

/// <summary>
/// Factory for creating platform-specific audio output implementations.
/// </summary>
public static class AudioOutputFactory
{
    /// <summary>
    /// Creates an appropriate audio output implementation for the current platform.
    /// </summary>
    /// <param name="loggerFactory">Logger factory for creating loggers.</param>
    /// <returns>An IAudioOutput implementation suitable for the current platform.</returns>
    public static IAudioOutput CreateForCurrentPlatform(ILoggerFactory loggerFactory)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return new WindowsAudioOutput(loggerFactory.CreateLogger<WindowsAudioOutput>());
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || 
                RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return new OpenALAudioOutput(loggerFactory.CreateLogger<OpenALAudioOutput>());
        }
        else
        {
            throw new PlatformNotSupportedException(
                "The current platform is not supported for audio playback. " +
                "Supported platforms are: Windows, Linux, and macOS.");
        }
    }
} 