using System;

namespace LofiBeats.Core.Playback;

/// <summary>
/// Manages the timing and volume calculations for crossfading between two audio sources.
/// This class is thread-safe and handles the volume ramping calculations for both the old
/// and new audio sources during a crossfade transition.
/// </summary>
public class CrossfadeManager
{
    private readonly float _crossfadeDurationSeconds;
    private readonly object _lockObj = new();
    private bool _isCrossfading;
    private DateTime _startTime;

    /// <summary>
    /// Initializes a new instance of the CrossfadeManager class.
    /// </summary>
    /// <param name="crossfadeDurationSeconds">The duration of the crossfade in seconds.</param>
    public CrossfadeManager(float crossfadeDurationSeconds)
    {
        _crossfadeDurationSeconds = crossfadeDurationSeconds;
    }

    /// <summary>
    /// Begins a new crossfade transition. This resets the internal timer and
    /// sets the crossfading state to active.
    /// </summary>
    public void BeginCrossfade()
    {
        lock (_lockObj)
        {
            _isCrossfading = true;
            _startTime = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Gets the current volume scale factor for the old (fading out) audio source.
    /// </summary>
    /// <returns>
    /// A value between 0.0 and 1.0, where:
    /// - 1.0 means full volume (when not crossfading or at start)
    /// - 0.0 means silent (when crossfade is complete)
    /// - Values in between represent the linear fade out
    /// </returns>
    public float GetOldVolumeScale()
    {
        lock (_lockObj)
        {
            if (!_isCrossfading) return 1.0f;
            var elapsed = (float)(DateTime.UtcNow - _startTime).TotalSeconds;
            if (elapsed >= _crossfadeDurationSeconds) return 0.0f;
            return 1.0f - (elapsed / _crossfadeDurationSeconds);
        }
    }

    /// <summary>
    /// Gets the current volume scale factor for the new (fading in) audio source.
    /// </summary>
    /// <returns>
    /// A value between 0.0 and 1.0, where:
    /// - 0.0 means silent (when not crossfading or at start)
    /// - 1.0 means full volume (when crossfade is complete)
    /// - Values in between represent the linear fade in
    /// </returns>
    public float GetNewVolumeScale()
    {
        lock (_lockObj)
        {
            if (!_isCrossfading) return 0.0f;
            var elapsed = (float)(DateTime.UtcNow - _startTime).TotalSeconds;
            if (elapsed >= _crossfadeDurationSeconds) return 1.0f;
            return elapsed / _crossfadeDurationSeconds;
        }
    }

    /// <summary>
    /// Checks if the crossfade transition is complete.
    /// </summary>
    /// <returns>
    /// True if either:
    /// - No crossfade is in progress
    /// - The crossfade duration has elapsed
    /// False if a crossfade is still in progress.
    /// </returns>
    public bool IsCrossfadeComplete()
    {
        lock (_lockObj)
        {
            if (!_isCrossfading) return true;
            return (DateTime.UtcNow - _startTime).TotalSeconds >= _crossfadeDurationSeconds;
        }
    }

    /// <summary>
    /// Cancels the current crossfade operation, if one is in progress.
    /// This immediately stops the transition without completing the fade.
    /// </summary>
    public void CancelCrossfade()
    {
        lock (_lockObj)
        {
            _isCrossfading = false;
        }
    }
} 