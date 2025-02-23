using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace LofiBeats.Core.Scheduling;

/// <summary>
/// Manages scheduled playback actions like delayed stops or fades.
/// </summary>
public class PlaybackScheduler(ILogger<PlaybackScheduler> logger) : IDisposable
{
    private readonly ILogger<PlaybackScheduler> _logger = logger;
    private readonly ConcurrentDictionary<Guid, (Timer Timer, string Description)> _timers = new();
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

    /// <summary>
    /// Gets the number of currently scheduled actions.
    /// </summary>
    public int ScheduledActionCount => _timers.Count;

    /// <summary>
    /// Schedules an action to be executed after a given delay.
    /// Returns a Guid you can use to reference this schedule later.
    /// </summary>
    /// <param name="delay">Delay in milliseconds before executing</param>
    /// <param name="callback">Action to invoke</param>
    /// <param name="description">Optional description of the scheduled action</param>
    /// <returns>A Guid that can be used to cancel or reference this schedule later</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when delay is negative</exception>
    /// <exception cref="ArgumentNullException">Thrown when callback is null</exception>
    /// <exception cref="ObjectDisposedException">Thrown when the scheduler has been disposed</exception>
    public Guid ScheduleAction(int delay, Action callback, string? description = null)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(delay);
        ArgumentNullException.ThrowIfNull(callback);
        ObjectDisposedException.ThrowIf(_disposed, this);

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

        _timers[id] = (timer, description ?? string.Empty);
        return id;
    }

    /// <summary>
    /// Schedules a stop action to be executed after a given delay.
    /// If there are any existing stop actions, they will be cancelled.
    /// Returns a Guid you can use to reference this schedule later.
    /// </summary>
    public Guid ScheduleStopAction(int delay, Action callback, string? description = null)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(delay);
        ArgumentNullException.ThrowIfNull(callback);
        ObjectDisposedException.ThrowIf(_disposed, this);

        // Cancel any existing stop actions
        var existingStopActions = _timers
            .Where(t => t.Value.Description.Contains("stop", StringComparison.OrdinalIgnoreCase))
            .Select(t => t.Key)
            .ToList();

        foreach (var existingId in existingStopActions)
        {
            CancelAction(existingId);
        }

        return ScheduleAction(delay, callback, description);
    }

    /// <summary>
    /// Gets a list of all currently scheduled actions.
    /// </summary>
    /// <returns>A list of tuples containing the action ID and its description</returns>
    public IReadOnlyList<(Guid Id, string Description)> GetScheduledActions()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        return _timers.Select(kvp => (kvp.Key, kvp.Value.Description)).ToList();
    }

    /// <summary>
    /// Cancels a scheduled action if it hasn't already fired.
    /// </summary>
    /// <param name="id">The ID of the scheduled action to cancel</param>
    /// <returns>True if the action was found and cancelled, false otherwise</returns>
    /// <exception cref="ObjectDisposedException">Thrown when the scheduler has been disposed</exception>
    public bool CancelAction(Guid id)
    {
        if (_disposed) return false;

        if (_timers.TryRemove(id, out var timerInfo))
        {
            _logCancellingAction(_logger, id, null);
            timerInfo.Timer.Dispose();
            return true;
        }
        return false;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Cancel and dispose all timers
                foreach (var (timer, _) in _timers.Values)
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