Fire and Forget Audio Playback with NAudio
February 1. 2014
Posted in:

    NAudio audio

Every so often I get a request for help from someone wanting to create a simple one-liner function that can play an audio file with NAudio. Often they will create something like this:

// warning: this won't work
public void PlaySound(string fileName)
{
    using (var output = new WaveOut())
    using (var player = new AudioFilePlayer(fileName))
    {
        output.Init(player);
        output.Play();
    }
}

Unfortunately this won’t actually work, since the Play method doesn’t block until playback is finished – it simply begins playback. So you end up disposing the playback device almost instantaneously after beginning playback. A slightly improved option simply waits for playback to stop, creating a blocking call. It uses WaveOutEvent, as the standard WaveOut only works on GUI threads.

// better, but still not ideal 
public void PlaySound(string fileName)
{
    using (var output = new WaveOutEvent())
    using (var player = new AudioFilePlayer(fileName))
    {
        output.Init(player);
        output.Play();
        while (output.PlaybackState == PlaybackState.Playing)
        {
            Thread.Sleep(500);
        }
    }
}

This approach is better, but is still not ideal, since it now blocks until audio playback is complete. It is also not particularly suitable for scenarios in which you are playing lots of short sounds, such as sound effects in a computer game. The problem is, you don’t really want to be continually opening and closing the soundcard, or having multiple instances of an output device active at once. So in this post, I explain the approach I would typically take for an application that needs to regularly play sounds in a “Fire and Forget” manner.
Use a Single Output Device

First, I’d recommend just opening the output soundcard once. Choose the output model you want (e.g. WasapiOut, WaveOutEvent), and play all the sounds through it.
Use a MixingSampleProvider

This means that to play multiple sounds simultaneously, you’ll need a mixer. I always recommend mixing with 32 bit IEEE floating point samples. and in NAudio the best way to do this is through using the MixingSampleProvider class.
Play Continuously

Obviously there are times when your application won’t be playing any sounds, so you could start and stop the output device whenever playback is idle. But it tends to be more straightforward to simply leave the soundcard running playing silence, and then just add inputs to the mixer. If you set the ReadFully property on MixingSampleProvider to true, it’s Read method will return buffers full of silence even when there are no mixer inputs. This means that the output device will keep playing continuously.
Use a Single WaveFormat

The one down-side of this approach is that you can’t mix together audio that doesn’t share the same WaveFormat. The bit depth won’t be a problem, since we are automatically converting everything to IEEE floating point. But if you are working with a stereo mixer, any mono inputs need to be made stereo before playing them. More annoying is the issue of sample rate conversion. If the files you need to play contain a mixture of sample rates, you’ll need to convert them all to a common value. 44.1kHz would be a typical choice, since this is likely to be the sample rate your soundcard is operating at.
Dispose Readers

The MixingSampleProvider has a nice feature where it will automatically remove an input whose Read method returns 0. However, it won’t attempt to Dispose that input for you, leaving you with a resource leak. The easiest way round this is to create a derived ISampleProvider class that encapsulates the AudioFileReader, and auto-disposes it when it reaches the end.
Cache Sounds

In a computer game scenario, you’ll likely be playing the same sounds again and again. You don’t really want to keep reading them from disk (and decoding them if they compressed). So it would be best to load the whole thing into memory, allowing us to replay many copies of it directly from the byte array of PCM data, using a RawSourceWaveStream. This approach has the advantage of allowing you to dispose the AudioFileReader immediately after caching its contents.
Source Code

That’s enough waffling, let’s have a look at some code that implements the features mentioned above. Let’s start with what I’ve called AudioPlaybackEngine. This is responsible for playing our sounds. You can either call PlaySound with a path to a file, for use with longer pieces of audio, or passing in a CachedSound for use with sound effects you want to play many times. I’ve included automatic conversion from mono to stereo, but no resampling is included here, so if you pass in a file of the wrong sample rate it won’t play:

class AudioPlaybackEngine : IDisposable
{
    private readonly IWavePlayer outputDevice;
    private readonly MixingSampleProvider mixer;

    public AudioPlaybackEngine(int sampleRate = 44100, int channelCount = 2)
    {
        outputDevice = new WaveOutEvent();
        mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channelCount));
        mixer.ReadFully = true;
        outputDevice.Init(mixer);
        outputDevice.Play();
    }

    public void PlaySound(string fileName)
    {
        var input = new AudioFileReader(fileName);
        AddMixerInput(new AutoDisposeFileReader(input));
    }

    private ISampleProvider ConvertToRightChannelCount(ISampleProvider input)
    {
        if (input.WaveFormat.Channels == mixer.WaveFormat.Channels)
        {
            return input;
        }
        if (input.WaveFormat.Channels == 1 && mixer.WaveFormat.Channels == 2)
        {
            return new MonoToStereoSampleProvider(input);
        }
        throw new NotImplementedException("Not yet implemented this channel count conversion");
    }

    public void PlaySound(CachedSound sound)
    {
        AddMixerInput(new CachedSoundSampleProvider(sound));
    }

    private void AddMixerInput(ISampleProvider input)
    {
        mixer.AddMixerInput(ConvertToRightChannelCount(input));
    }

    public void Dispose()
    {
        outputDevice.Dispose();
    }

    public static readonly AudioPlaybackEngine Instance = new AudioPlaybackEngine(44100, 2);
}

The CachedSound class is responsible for reading an audio file into memory. Sample rate conversion would be best done in here as part of the caching process, so it minimises the performance hit of resampling during playback.

class CachedSound
{
    public float[] AudioData { get; private set; }
    public WaveFormat WaveFormat { get; private set; }
    public CachedSound(string audioFileName)
    {
        using (var audioFileReader = new AudioFileReader(audioFileName))
        {
            // TODO: could add resampling in here if required
            WaveFormat = audioFileReader.WaveFormat;
            var wholeFile = new List<float>((int)(audioFileReader.Length / 4));
            var readBuffer= new float[audioFileReader.WaveFormat.SampleRate * audioFileReader.WaveFormat.Channels];
            int samplesRead;
            while((samplesRead = audioFileReader.Read(readBuffer,0,readBuffer.Length)) > 0)
            {
                wholeFile.AddRange(readBuffer.Take(samplesRead));
            }
            AudioData = wholeFile.ToArray();
        }
    }
}

There’s also a simple helper class to turn a CachedSound into an ISampleProvider that can be easily added to the mixer:

class CachedSoundSampleProvider : ISampleProvider
{
    private readonly CachedSound cachedSound;
    private long position;

    public CachedSoundSampleProvider(CachedSound cachedSound)
    {
        this.cachedSound = cachedSound;
    }

    public int Read(float[] buffer, int offset, int count)
    {
        var availableSamples = cachedSound.AudioData.Length - position;
        var samplesToCopy = Math.Min(availableSamples, count);
        Array.Copy(cachedSound.AudioData, position, buffer, offset, samplesToCopy);
        position += samplesToCopy;
        return (int)samplesToCopy;
    }

    public WaveFormat WaveFormat { get { return cachedSound.WaveFormat; } }
}

And here’s the auto disposing helper for when you are playing from an AudioFileReader directly:

class AutoDisposeFileReader : ISampleProvider
{
    private readonly AudioFileReader reader;
    private bool isDisposed;
    public AutoDisposeFileReader(AudioFileReader reader)
    {
        this.reader = reader;
        this.WaveFormat = reader.WaveFormat;
    }

    public int Read(float[] buffer, int offset, int count)
    {
        if (isDisposed)
            return 0;
        int read = reader.Read(buffer, offset, count);
        if (read == 0)
        {
            reader.Dispose();
            isDisposed = true;
        }
        return read;
    }

    public WaveFormat WaveFormat { get; private set; }
}

With all this set up, now we can have our goal of using a very simple fire and forget syntax for playback:

// on startup:
var zap = new CachedSound("zap.wav");
var boom = new CachedSound("boom.wav");

// later in the app...
AudioPlaybackEngine.Instance.PlaySound(zap);
AudioPlaybackEngine.Instance.PlaySound(boom);
AudioPlaybackEngine.Instance.PlaySound("crash.wav");

// on shutdown
AudioPlaybackEngine.Instance.Dispose();

Further Enhancements

This is far from complete. Obviously I’ve not added in the resampler stage here, and it would be nice to add a master volume level for the audio playback engine, as well as allowing you to set individual sound volume and panning positions. You could even have a maximum limit of concurrent sounds. But none of those enhancements are too hard to add.

I’ll try to get something like this added into the NAudio WPF Demo application, maybe with a few of these enhancements thrown in. For now, you can get at the code from this gist.
Want to get up to speed with the the fundamentals principles of digital audio and how to got about writing audio applications with NAudio? Be sure to check out my Pluralsight courses, Digital Audio Fundamentals, and Audio Programming with NAudio.
