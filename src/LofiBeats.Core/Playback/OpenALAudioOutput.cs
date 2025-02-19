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
    private readonly object _lock = new object();

    /// <summary>
    /// Gets the OpenAL source ID. Used for testing.
    /// </summary>
    public int SourceId => _source;

    public OpenALAudioOutput(ILogger<OpenALAudioOutput> logger)
    {
        _logger = logger;
        _buffers = new List<int>();
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
                    int processed = AL.GetSource(_source, ALGetSourcei.BuffersProcessed);
                    AL.GetError(); // Clear any error
                    
                    for (int i = 0; i < processed; i++)
                    {
                        int buffer = AL.SourceUnqueueBuffer(_source);
                        AL.GetError(); // Clear any error
                        
                        if (QueueBuffer(buffer))
                        {
                            AL.SourceQueueBuffer(_source, buffer);
                            AL.GetError(); // Clear any error
                        }
                    }

                    var state = AL.GetSource(_source, ALGetSourcei.SourceState);
                    AL.GetError(); // Clear any error
                    
                    if (state == (int)ALSourceState.Stopped)
                    {
                        AL.SourcePlay(_source);
                        AL.GetError(); // Clear any error
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

        if (_isPaused)
        {
            AL.SourcePlay(_source);
            _isPaused = false;
        }
        else
        {
            // Initialize buffers
            const int BUFFER_COUNT = 3;
            int[] buffers = AL.GenBuffers(BUFFER_COUNT);
            _buffers.AddRange(buffers);

            foreach (var buffer in _buffers)
            {
                if (QueueBuffer(buffer))
                {
                    AL.SourceQueueBuffer(_source, buffer);
                }
            }

            AL.SourcePlay(_source);
        }

        _isPlaying = true;
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
            
            // Final rewind to ensure the source is in a clean state
            AL.SourceRewind(_source);
            AL.GetError(); // Clear any error from rewinding
            
            _isPlaying = false;
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
                        AL.DeleteBuffers(_buffers.ToArray());
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