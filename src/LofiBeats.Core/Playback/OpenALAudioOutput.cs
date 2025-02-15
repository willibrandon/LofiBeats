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
        var device = ALC.OpenDevice(string.Empty);
        if (device == IntPtr.Zero)
        {
            throw new Exception("Failed to open OpenAL device");
        }

        _context = ALC.CreateContext(device, Array.Empty<int>());
        ALC.MakeContextCurrent(_context.Value);

        AL.GenSource(out _source);
        AL.Source(_source, ALSourcef.Gain, _volume);
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
                int processed = AL.GetSource(_source, ALGetSourcei.BuffersProcessed);
                
                for (int i = 0; i < processed; i++)
                {
                    int buffer = AL.SourceUnqueueBuffer(_source);
                    if (QueueBuffer(buffer))
                    {
                        AL.SourceQueueBuffer(_source, buffer);
                    }
                }

                var state = AL.GetSource(_source, ALGetSourcei.SourceState);
                if (state == (int)ALSourceState.Stopped)
                {
                    AL.SourcePlay(_source);
                }
            }
            Thread.Sleep(10);
        }
    }

    private bool QueueBuffer(int buffer)
    {
        if (_waveProvider == null) return false;

        byte[] data = new byte[BUFFER_SIZE];
        int bytesRead = _waveProvider.Read(data, 0, data.Length);
        
        if (bytesRead == 0) return false;

        var format = _waveProvider.WaveFormat.Channels == 2 ? ALFormat.Stereo16 : ALFormat.Mono16;
        
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

        AL.SourcePause(_source);
        _isPaused = true;
    }

    public void Stop()
    {
        if (!_isPlaying) return;

        AL.SourceStop(_source);
        AL.SourceRewind(_source); // Rewind the source
        
        // Clear all queued buffers
        int queued = AL.GetSource(_source, ALGetSourcei.BuffersQueued);
        for (int i = 0; i < queued; i++)
        {
            AL.SourceUnqueueBuffer(_source);
        }

        _isPlaying = false;
        _isPaused = false;
    }

    public void SetVolume(float volume)
    {
        _volume = Math.Clamp(volume, 0.0f, 1.0f);
        AL.Source(_source, ALSourcef.Gain, _volume);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (disposing)
            {
                _cancellationTokenSource.Cancel();
                _updateThread?.Join();

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
            _isDisposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
} 