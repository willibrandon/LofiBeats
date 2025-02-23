# LofiBeats Plugin API

A .NET library for creating audio effect plugins for the LofiBeats application. This package contains the core interfaces and attributes needed to create custom audio effects.

## Quick Start

1. Install the NuGet package:
```shell
dotnet add package LofiBeats.Core.PluginApi
```

2. Create an effect class:
```csharp
using LofiBeats.Core.PluginApi;
using NAudio.Wave;

[PluginEffectName("myeffect", Description = "My custom audio effect")]
public class MyAudioEffect : IAudioEffect
{
    private WaveFormat _waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);
    private ISampleProvider? _source;

    public string Name => "My Effect";
    public WaveFormat WaveFormat => _waveFormat;

    public void SetSource(ISampleProvider source)
    {
        _source = source;
        _waveFormat = source.WaveFormat;
    }

    public void ApplyEffect(float[] buffer, int offset, int count)
    {
        // Implement your audio processing here
    }

    public int Read(float[] buffer, int offset, int count)
    {
        return _source?.Read(buffer, offset, count) ?? 0;
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