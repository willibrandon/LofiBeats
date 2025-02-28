using Microsoft.Extensions.Logging;
using NAudio.Wave;
using OpenTK.Audio.OpenAL;

namespace LofiBeats.Core.Playback;

/// <summary>
/// Linux/macOS implementation of audio output using OpenAL.
/// </summary>
public class OpenALAudioOutput : IAudioOutput
{
    private readonly ILogger<OpenALAudioOutput> _logger;
    private ALContext? _context;
    private int _source;
    private readonly List<int> _buffers;
    private IWaveProvider? _waveProvider;
    private bool _isPlaying;
    private bool _isPaused;
    private float _volume = 1.0f;
    private bool _isDisposed;
    private const int BUFFER_SIZE = 8192;
    private Thread? _updateThread;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly Lock _lock = new();

    /// <summary>
    /// Gets the OpenAL source ID. Used for testing.
    /// </summary>
    public int SourceId => _source;

    public OpenALAudioOutput(ILogger<OpenALAudioOutput> logger)
    {
        _logger = logger;
        _buffers = [];
        _cancellationTokenSource = new CancellationTokenSource();
        InitializeOpenAL();
    }

    public PlaybackState PlaybackState
    {
        get
        {
            if (_isPaused) return PlaybackState.Paused;
            return _isPlaying ? PlaybackState.Playing : PlaybackState.Stopped;
        }
    }

    private void InitializeOpenAL()
    {
        lock (_lock)
        {
            var device = ALC.OpenDevice(string.Empty);
            if (device == IntPtr.Zero)
            {
                throw new Exception("Failed to open OpenAL device");
            }

            _context = ALC.CreateContext(device, Array.Empty<int>());
            ALC.MakeContextCurrent(_context.Value);

            AL.GenSource(out _source);
            AL.Source(_source, ALSourcef.Gain, _volume);
            AL.GetError(); // Clear any error from initialization
        }
    }

    public void Init(IWaveProvider waveProvider)
    {
        _waveProvider = waveProvider;
        StartUpdateThread();
    }

    private void StartUpdateThread()
    {
        _updateThread = new Thread(() => UpdateBuffers(_cancellationTokenSource.Token));
        _updateThread.Start();
    }

    private void UpdateBuffers(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            if (_isPlaying && !_isPaused && _waveProvider != null)
            {
                lock (_lock)
                {
                    // Get number of processed buffers
                    int processed = AL.GetSource(_source, ALGetSourcei.BuffersProcessed);
                    AL.GetError(); // Clear any error
                    
                    // If we have processed buffers, unqueue and requeue them
                    if (processed > 0)
                    {
                        for (int i = 0; i < processed; i++)
                        {
                            // Unqueue one buffer at a time to maintain steady flow
                            int buffer = AL.SourceUnqueueBuffer(_source);
                            if (AL.GetError() != ALError.NoError) continue;
                            
                            // Try to refill and requeue the buffer
                            if (QueueBuffer(buffer))
                            {
                                AL.SourceQueueBuffer(_source, buffer);
                                AL.GetError(); // Clear any error
                            }
                        }
                    }

                    // Check if we need to restart playback
                    var state = AL.GetSource(_source, ALGetSourcei.SourceState);
                    AL.GetError(); // Clear any error
                    
                    // If source stopped but we're supposed to be playing, restart it
                    if (state == (int)ALSourceState.Stopped && _isPlaying && !_isPaused)
                    {
                        // Check if we have any queued buffers before playing
                        int queued = AL.GetSource(_source, ALGetSourcei.BuffersQueued);
                        AL.GetError(); // Clear any error
                        
                        if (queued > 0)
                        {
                            AL.SourcePlay(_source);
                            if (AL.GetError() != ALError.NoError)
                            {
                                _logger.LogWarning("Failed to restart playback after buffer processing");
                            }
                        }
                        else
                        {
                            // No buffers queued - try to queue initial set
                            bool anyQueued = false;
                            foreach (var buffer in _buffers)
                            {
                                if (QueueBuffer(buffer))
                                {
                                    AL.SourceQueueBuffer(_source, buffer);
                                    if (AL.GetError() == ALError.NoError)
                                    {
                                        anyQueued = true;
                                    }
                                }
                            }
                            
                            if (anyQueued)
                            {
                                AL.SourcePlay(_source);
                                if (AL.GetError() != ALError.NoError)
                                {
                                    _logger.LogWarning("Failed to start playback after re-queuing buffers");
                                }
                            }
                        }
                    }
                }
            }
            Thread.Sleep(10);
        }
    }

    private bool QueueBuffer(int buffer)
    {
        if (_waveProvider == null) return false;

        // Calculate buffer size based on wave format
        int bytesPerSample = _waveProvider.WaveFormat.BitsPerSample / 8;
        int channels = _waveProvider.WaveFormat.Channels;
        int frameSize = bytesPerSample * channels;
        
        // Ensure buffer size is a multiple of the frame size
        int bufferSize = (BUFFER_SIZE / frameSize) * frameSize;
        
        byte[] data = new byte[bufferSize];
        int bytesRead = _waveProvider.Read(data, 0, data.Length);
        
        if (bytesRead == 0) return false;

        ALFormat format;
        if (_waveProvider.WaveFormat.Encoding == WaveFormatEncoding.IeeeFloat)
        {
            format = channels == 2 ? ALFormat.StereoFloat32Ext : ALFormat.MonoFloat32Ext;
        }
        else
        {
            format = channels == 2 ? ALFormat.Stereo16 : ALFormat.Mono16;
        }
        
        unsafe
        {
            fixed (byte* ptr = data)
            {
                AL.BufferData(buffer, format, (IntPtr)ptr, bytesRead, _waveProvider.WaveFormat.SampleRate);
            }
        }
        
        return true;
    }

    public void Play()
    {
        if (_waveProvider == null)
        {
            throw new InvalidOperationException("No valid audio output device initialized. Call Init() with a valid wave provider first.");
        }

        if (_isPlaying && !_isPaused) return;

        lock (_lock)
        {
            if (_isPaused)
            {
                AL.SourcePlay(_source);
                AL.GetError(); // Clear any error
                _isPaused = false;
            }
            else
            {
                // Ensure we're in a clean state
                if (_buffers.Count > 0)
                {
                    Stop();
                    // Give a short time for the stop to complete
                    Thread.Sleep(10);
                }

                // Verify source is in a clean state
                var initialState = AL.GetSource(_source, ALGetSourcei.SourceState);
                if (initialState != (int)ALSourceState.Initial && initialState != (int)ALSourceState.Stopped)
                {
                    _logger.LogWarning("Source in unexpected state before play: {State}", initialState);
                    AL.SourceRewind(_source);
                    AL.GetError();
                }

                // Initialize buffers
                const int BUFFER_COUNT = 3;
                int[] buffers = AL.GenBuffers(BUFFER_COUNT);
                _buffers.AddRange(buffers);

                // Queue initial buffers
                int successfulQueues = 0;
                foreach (var buffer in _buffers)
                {
                    if (QueueBuffer(buffer))
                    {
                        AL.SourceQueueBuffer(_source, buffer);
                        var error = AL.GetError();
                        if (error == ALError.NoError)
                        {
                            successfulQueues++;
                        }
                    }
                }

                // Only start playing if we successfully queued at least one buffer
                if (successfulQueues > 0)
                {
                    // Start playback
                    AL.SourcePlay(_source);
                    var error = AL.GetError();
                    if (error != ALError.NoError)
                    {
                        _logger.LogWarning("Failed to start playback: {Error}", error);
                        return;
                    }
                    
                    // Verify the source is actually playing
                    Thread.Sleep(10); // Give a short time for the state to update
                    var state = AL.GetSource(_source, ALGetSourcei.SourceState);
                    if (state != (int)ALSourceState.Playing)
                    {
                        _logger.LogWarning("Source failed to enter playing state. Current state: {State}", state);
                        return;
                    }
                    
                    // Verify buffer processing has started
                    Thread.Sleep(50); // Give time for initial buffer processing
                    int processed = AL.GetSource(_source, ALGetSourcei.BuffersProcessed);
                    if (processed == 0)
                    {
                        _logger.LogWarning("No buffers processed after initial play");
                    }
                }
                else
                {
                    _logger.LogWarning("Failed to queue any buffers during play");
                    return;
                }
            }

            _isPlaying = true;
        }
    }

    public void Pause()
    {
        if (!_isPlaying || _isPaused) return;

        lock (_lock)
        {
            AL.SourcePause(_source);
            AL.GetError(); // Clear any error
            _isPaused = true;
            
            // Ensure any in-progress buffer processing is complete
            int processed = AL.GetSource(_source, ALGetSourcei.BuffersProcessed);
            if (processed > 0)
            {
                // Unqueue any processed buffers without re-queuing them
                int[] unqueuedBuffers = new int[processed];
                AL.SourceUnqueueBuffers(_source, processed, unqueuedBuffers);
                AL.GetError(); // Clear any error
            }
        }
    }

    public void Stop()
    {
        if (!_isPlaying) return;

        lock (_lock)
        {
            // Set flags first to prevent update thread from queueing more
            _isPlaying = false;
            _isPaused = false;

            // Stop playback first
            AL.SourceStop(_source);
            AL.GetError(); // Clear any error from stopping
            
            // Get the number of queued buffers
            int queued = AL.GetSource(_source, ALGetSourcei.BuffersQueued);
            AL.GetError(); // Clear any error from getting queued count
            
            if (queued > 0)
            {
                // Unqueue all buffers at once
                int[] unqueuedBuffers = new int[queued];
                AL.SourceUnqueueBuffers(_source, queued, unqueuedBuffers);
                AL.GetError(); // Clear any error from unqueuing
                
                // Delete the unqueued buffers and clear our buffer list
                AL.DeleteBuffers(unqueuedBuffers);
                AL.GetError(); // Clear any error from deleting
                _buffers.Clear();
            }
            
            // Reset source state
            AL.Source(_source, ALSourcef.Gain, _volume);
            AL.GetError(); // Clear any errors
            
            // Final rewind to ensure the source is in a clean state
            AL.SourceRewind(_source);
            AL.GetError(); // Clear any error from rewinding

            // Wait a short time to ensure OpenAL has processed the stop
            Thread.Sleep(10);
            
            // Double check that we're actually stopped and in a clean state
            var state = AL.GetSource(_source, ALGetSourcei.SourceState);
            if (state != (int)ALSourceState.Stopped && state != (int)ALSourceState.Initial)
            {
                // Force stop again if needed
                AL.SourceStop(_source);
                AL.GetError();
                
                // Wait again and verify
                Thread.Sleep(10);
                state = AL.GetSource(_source, ALGetSourcei.SourceState);
                if (state != (int)ALSourceState.Stopped && state != (int)ALSourceState.Initial)
                {
                    _logger.LogWarning("Failed to stop source. Current state: {State}", state);
                }
            }
            
            // Ensure no buffers are queued
            queued = AL.GetSource(_source, ALGetSourcei.BuffersQueued);
            if (queued > 0)
            {
                _logger.LogWarning("Source still has {Count} buffers queued after stop", queued);
                // Try one more time to unqueue them
                int[] remainingBuffers = new int[queued];
                AL.SourceUnqueueBuffers(_source, queued, remainingBuffers);
                AL.DeleteBuffers(remainingBuffers);
                AL.GetError();
            }
        }
    }

    public void SetVolume(float volume)
    {
        lock (_lock)
        {
            _volume = Math.Clamp(volume, 0.0f, 1.0f);
            AL.Source(_source, ALSourcef.Gain, _volume);
            AL.GetError(); // Clear any error
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (disposing)
            {
                _cancellationTokenSource.Cancel();
                _updateThread?.Join(); // Wait for update thread to finish

                lock (_lock)
                {
                    if (_buffers.Count > 0)
                    {
                        AL.DeleteBuffers([.. _buffers]);
                        _buffers.Clear();
                    }

                    AL.DeleteSource(_source);

                    if (_context.HasValue)
                    {
                        var device = ALC.GetContextsDevice(_context.Value);
                        ALC.MakeContextCurrent(ALContext.Null);
                        ALC.DestroyContext(_context.Value);
                        ALC.CloseDevice(device);
                    }
                }
            }
            _isDisposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
} 