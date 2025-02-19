using NAudio.Wave;
using System.Buffers;

namespace LofiBeats.Core.Playback;

/// <summary>
/// Provides real-time crossfading between two audio sources by mixing their outputs
/// with dynamically adjusted volume levels. This provider is thread-safe and properly
/// manages audio buffer resources.
/// </summary>
/// <remarks>
/// Features:
/// - Thread-safe audio mixing
/// - Buffer pooling for memory efficiency
/// - Proper resource cleanup
/// - Automatic format validation
/// - Linear volume crossfading
/// 
/// Performance considerations:
/// - Uses ArrayPool for temporary buffer allocation
/// - Minimizes allocations in the audio processing path
/// - Implements proper resource disposal
/// - Handles buffer underruns gracefully
/// </remarks>
public sealed class CrossfadeSampleProvider : ISampleProvider, IDisposable
{
    private readonly ISampleProvider _oldProvider;
    private readonly ISampleProvider _newProvider;
    private readonly CrossfadeManager _xfadeManager;
    private bool _disposed;

    /// <summary>
    /// Gets the WaveFormat of this provider. Both input providers must share the same format.
    /// </summary>
    public WaveFormat WaveFormat { get; }

    /// <summary>
    /// Initializes a new instance of the CrossfadeSampleProvider class.
    /// </summary>
    /// <param name="oldProvider">The audio provider that will fade out.</param>
    /// <param name="newProvider">The audio provider that will fade in.</param>
    /// <param name="xfadeManager">The manager controlling crossfade timing and volume.</param>
    /// <exception cref="ArgumentNullException">Thrown if any parameter is null.</exception>
    /// <exception cref="ArgumentException">Thrown if the providers have different wave formats.</exception>
    public CrossfadeSampleProvider(
        ISampleProvider oldProvider,
        ISampleProvider newProvider,
        CrossfadeManager xfadeManager)
    {
        _oldProvider = oldProvider ?? throw new ArgumentNullException(nameof(oldProvider));
        _newProvider = newProvider ?? throw new ArgumentNullException(nameof(newProvider));
        _xfadeManager = xfadeManager ?? throw new ArgumentNullException(nameof(xfadeManager));

        // Validate matching formats
        if (!oldProvider.WaveFormat.Equals(newProvider.WaveFormat))
        {
            throw new ArgumentException(
                "Old and new providers must have matching wave formats",
                nameof(newProvider));
        }

        WaveFormat = oldProvider.WaveFormat;
    }

    /// <summary>
    /// Reads from both providers, applies crossfade volumes, and mixes the results.
    /// </summary>
    /// <param name="buffer">The buffer to fill with mixed audio data.</param>
    /// <param name="offset">The offset in the buffer to start writing.</param>
    /// <param name="count">The number of samples to read.</param>
    /// <returns>The number of samples written to the buffer.</returns>
    public int Read(float[] buffer, int offset, int count)
    {
        if (_disposed) return 0;

        // Rent temporary buffers from the pool
        float[] oldBuff = ArrayPool<float>.Shared.Rent(count);
        float[] newBuff = ArrayPool<float>.Shared.Rent(count);

        try
        {
            int oldRead = _oldProvider.Read(oldBuff, 0, count);
            int newRead = _newProvider.Read(newBuff, 0, count);

            // Get volume scales once per buffer to minimize lock contention
            float oldVol = _xfadeManager.GetOldVolumeScale();
            float newVol = _xfadeManager.GetNewVolumeScale();

            // Mix the audio with volume scaling
            for (int i = 0; i < count; i++)
            {
                float oldSample = (i < oldRead) ? oldBuff[i] * oldVol : 0f;
                float newSample = (i < newRead) ? newBuff[i] * newVol : 0f;
                buffer[offset + i] = oldSample + newSample;
            }

            // Return the larger of the two read counts to ensure we keep reading
            // as long as at least one source has data
            return Math.Max(oldRead, newRead);
        }
        finally
        {
            // Return buffers to the pool
            ArrayPool<float>.Shared.Return(oldBuff);
            ArrayPool<float>.Shared.Return(newBuff);
        }
    }

    /// <summary>
    /// Disposes of resources used by this provider and its source providers.
    /// </summary>
    public void Dispose()
    {
        if (_disposed) return;
        
        if (_oldProvider is IDisposable d1)
        {
            d1.Dispose();
        }
        
        if (_newProvider is IDisposable d2)
        {
            d2.Dispose();
        }
        
        _disposed = true;
    }
} 