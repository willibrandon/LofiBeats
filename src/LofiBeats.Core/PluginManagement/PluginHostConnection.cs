using System.Diagnostics;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace LofiBeats.Core.PluginManagement;

/// <summary>
/// Manages communication with a plugin host process.
/// </summary>
public sealed class PluginHostConnection : IPluginHostConnection, IDisposable
{
    private readonly Process _process;
    private readonly ILogger<PluginHostConnection> _logger;
    private readonly SemaphoreSlim _sendLock = new(1, 1);
    private readonly AutoResetEvent _responseEvent = new(false);
    private readonly Queue<string> _responseQueue = new();
    private bool _isDisposed;

    // High-performance structured logging
    private static readonly Action<ILogger, string, Exception?> _logProcessStarted =
        LoggerMessage.Define<string>(LogLevel.Information, new EventId(1, "PluginHostStarted"),
            "Plugin host process started with ID {ProcessId}");

    private static readonly Action<ILogger, string, Exception?> _logProcessStopped =
        LoggerMessage.Define<string>(LogLevel.Information, new EventId(2, "ProcessStopped"),
            "Plugin host process stopped: {ProcessId}");

    private static readonly Action<ILogger, string, Exception> _logProcessError =
        LoggerMessage.Define<string>(LogLevel.Error, new EventId(3, "ProcessError"),
            "Error in plugin host process: {Message}");

    public PluginHostConnection(Process process, ILogger<PluginHostConnection> logger)
    {
        _process = process ?? throw new ArgumentNullException(nameof(process));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        // Set up output handling
        process.OutputDataReceived += (sender, e) =>
        {
            if (e.Data != null)
            {
                lock (_responseQueue)
                {
                    _responseQueue.Enqueue(e.Data);
                    _responseEvent.Set();
                }
            }
        };

        process.ErrorDataReceived += (sender, e) =>
        {
            if (e.Data != null)
            {
                _logger.LogError("Plugin host error: {Error}", e.Data);
            }
        };

        try
        {
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("already been started"))
        {
            // If the process output is already being read, that's okay - we'll still get the events
            _logger.LogDebug("Process output already being read: {Message}", ex.Message);
        }

        _logProcessStarted(_logger, process.Id.ToString(), null);
    }

    /// <summary>
    /// Sends a message to the plugin host process and waits for a response.
    /// </summary>
    public async Task<T?> SendMessageAsync<T>(object message, CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);

        await _sendLock.WaitAsync(cancellationToken);
        try
        {
            if (_process.HasExited)
            {
                throw new InvalidOperationException("Plugin host process has exited");
            }

            var json = JsonSerializer.Serialize(message);
            _logger.LogDebug("Sending message to plugin host: {Message}", json);
            await _process.StandardInput.WriteLineAsync(json.AsMemory(), cancellationToken);
            await _process.StandardInput.FlushAsync();

            // Wait for response with timeout
            var timeout = TimeSpan.FromSeconds(5);
            var startTime = DateTime.UtcNow;

            while (DateTime.UtcNow - startTime < timeout)
            {
                if (_responseEvent.WaitOne(100)) // Wait up to 100ms for each check
                {
                    string? response;
                    lock (_responseQueue)
                    {
                        while (_responseQueue.Count > 0)
                        {
                            response = _responseQueue.Dequeue();
                            if (response == null) continue;

                            // Skip any lines that start with [STATUS], [DEBUG], etc.
                            if (response.StartsWith('['))
                            {
                                _logger.LogDebug("Skipping status message: {Message}", response);
                                continue;
                            }

                            try
                            {
                                _logger.LogDebug("Attempting to parse response: {Response}", response);
                                return JsonSerializer.Deserialize<T>(response);
                            }
                            catch (JsonException ex)
                            {
                                // If this response isn't valid JSON or the right type,
                                // log it and keep waiting for the real response
                                _logger.LogDebug(ex, "Failed to parse response as JSON: {Message}", response);
                            }
                        }
                    }
                }

                if (cancellationToken.IsCancellationRequested)
                {
                    throw new OperationCanceledException("Operation cancelled while waiting for response");
                }

                if (_process.HasExited)
                {
                    throw new InvalidOperationException("Plugin host process has exited while waiting for response");
                }
            }

            // If we get here, we timed out. Log the queue contents for debugging
            lock (_responseQueue)
            {
                if (_responseQueue.Count > 0)
                {
                    _logger.LogError("Timeout with {Count} messages in queue: {Messages}",
                        _responseQueue.Count,
                        string.Join(Environment.NewLine, _responseQueue));
                }
            }

            throw new TimeoutException("Timeout waiting for response from plugin host");
        }
        catch (Exception ex)
        {
            _logProcessError(_logger, ex.Message, ex);
            throw;
        }
        finally
        {
            _sendLock.Release();
        }
    }

    public void Dispose()
    {
        if (_isDisposed) return;
        _isDisposed = true;

        try
        {
            _sendLock.Dispose();
            _responseEvent.Dispose();

            if (!_process.HasExited)
            {
                // Try to gracefully stop the process first
                try
                {
                    _process.StandardInput.Close();
                    if (!_process.WaitForExit(1000)) // Wait up to 1 second
                    {
                        _process.Kill();
                    }
                }
                catch
                {
                    // If graceful shutdown fails, force kill
                    _process.Kill();
                }
                _process.WaitForExit();
            }
            _logProcessStopped(_logger, _process.Id.ToString(), null);
            _process.Dispose();
        }
        catch (Exception ex)
        {
            _logProcessError(_logger, ex.Message, ex);
        }
    }
} 