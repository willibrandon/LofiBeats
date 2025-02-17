using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace LofiBeats.Core.Scheduling;

/// <summary>
/// Manages scheduled playback actions like delayed stops or fades.
/// </summary>
public class PlaybackScheduler : IDisposable
{
    private readonly ILogger<PlaybackScheduler> _logger;
    private readonly ConcurrentDictionary<Guid, Timer> _timers = new();
    private bool _disposed;

    private static readonly Action<ILogger, Guid, int, Exception?> _logSchedulingAction =
        LoggerMessage.Define<Guid, int>(
            LogLevel.Information,
            new EventId(1, nameof(ScheduleAction)),
            "Scheduling action {ActionId} to run in {Delay}ms");

    private static readonly Action<ILogger, Guid, Exception?> _logExecutingAction =
        LoggerMessage.Define<Guid>(
            LogLevel.Information,
            new EventId(2, "ExecutingScheduledAction"),
            "Executing scheduled action {ActionId}");

    private static readonly Action<ILogger, Guid, Exception?> _logCancellingAction =
        LoggerMessage.Define<Guid>(
            LogLevel.Information,
            new EventId(3, nameof(CancelAction)),
            "Cancelling scheduled action {ActionId}");

    public PlaybackScheduler(ILogger<PlaybackScheduler> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Schedules an action to be executed after a given delay.
    /// </summary>
    /// <param name="delay">Delay in milliseconds before executing</param>
    /// <param name="callback">Action to invoke when the timer fires</param>
    /// <returns>A Guid that can be used to cancel or reference this schedule later</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when delay is negative</exception>
    /// <exception cref="ArgumentNullException">Thrown when callback is null</exception>
    /// <exception cref="ObjectDisposedException">Thrown when the scheduler has been disposed</exception>
    public Guid ScheduleAction(int delay, Action callback)
    {
        if (delay < 0) throw new ArgumentOutOfRangeException(nameof(delay), "Delay must be non-negative");
        if (callback == null) throw new ArgumentNullException(nameof(callback));
        if (_disposed) throw new ObjectDisposedException(nameof(PlaybackScheduler));

        var id = Guid.NewGuid();
        _logSchedulingAction(_logger, id, delay, null);

        var timer = new Timer(_ =>
        {
            try
            {
                _logExecutingAction(_logger, id, null);
                callback();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing scheduled action {ActionId}", id);
            }
            finally
            {
                // Clean up the timer after execution
                CancelAction(id);
            }
        }, null, delay, Timeout.Infinite);

        _timers[id] = timer;
        return id;
    }

    /// <summary>
    /// Cancels a scheduled action if it hasn't already fired.
    /// </summary>
    /// <param name="id">The ID of the scheduled action to cancel</param>
    /// <returns>True if the action was found and cancelled, false otherwise</returns>
    /// <exception cref="ObjectDisposedException">Thrown when the scheduler has been disposed</exception>
    public bool CancelAction(Guid id)
    {
        if (_disposed) throw new ObjectDisposedException(nameof(PlaybackScheduler));

        if (_timers.TryRemove(id, out var timer))
        {
            _logCancellingAction(_logger, id, null);
            timer.Dispose();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Gets the number of currently scheduled actions.
    /// </summary>
    public int ScheduledActionCount => _timers.Count;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Cancel and dispose all timers
                foreach (var timer in _timers.Values)
                {
                    timer.Dispose();
                }
                _timers.Clear();
            }
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
} 