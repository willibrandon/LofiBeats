# LofiBeats Plugin API

A .NET library for creating audio effect plugins for the LofiBeats application. This package contains the core interfaces needed to create custom audio effects.

## Quick Start

1. Install the NuGet package:
```shell
dotnet add package LofiBeats.Core.PluginApi
```

2. Create an effect class:
```csharp
using LofiBeats.Core.PluginApi;
using NAudio.Wave;

public class MyCustomEffect : IAudioEffect
{
    private ISampleProvider? _source;
    private WaveFormat _waveFormat;

    public string Author => "Example Author";

    public string Description => "An example custom audio effect";

    public string Name => "mycustomeffect";

    public string Version => "1.0.0";

    public WaveFormat WaveFormat => _waveFormat;

    public MyCustomEffect()
    {
        _waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);
    }

    public void SetSource(ISampleProvider source)
    {
        _source = source;
        _waveFormat = source.WaveFormat;
    }

    public void ApplyEffect(float[] buffer, int offset, int count)
    {
        // Implement your audio processing here
        // This method is called for each block of audio samples
    }

    public int Read(float[] buffer, int offset, int count)
    {
        if (_source == null) return 0;
        return _source.Read(buffer, offset, count);
    }
}
```

## Documentation

For comprehensive documentation including:
- Detailed plugin development guide
- Best practices and security considerations
- Installation and deployment instructions
- Troubleshooting guide

See the [Plugin System Documentation](https://github.com/willibrandon/LofiBeats/blob/main/docs/PLUGINS.md).

## Example Projects

Find complete plugin examples in the [examples directory](https://github.com/willibrandon/LofiBeats/tree/main/examples/Plugins):
- BasicEffect: Simple gain effect
- DelayEffect: Time-based delay effect
- MultiParameterEffect: Effect with multiple configurable parameters 