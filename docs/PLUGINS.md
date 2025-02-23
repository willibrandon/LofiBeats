# LofiBeats Plugin System

LofiBeats supports custom audio effect plugins, allowing you to extend the application with your own effects. This guide explains how to create, install, and use plugins.

## Plugin Directory Location

Plugins are stored in a platform-specific directory:

- **Windows**: `%LOCALAPPDATA%\LofiBeats\Plugins`
- **macOS**: `~/Library/Application Support/LofiBeats/Plugins`
- **Linux**: `~/.local/share/LofiBeats/Plugins`

The directory is automatically created when LofiBeats starts.

## Installing Plugins

To install a plugin:

1. Build your plugin as a .NET assembly (`.dll` file)
2. Copy the `.dll` file to your platform's plugin directory
3. LofiBeats will automatically detect and load the plugin

Plugins are loaded dynamically at runtime, so you don't need to restart LofiBeats when adding or removing plugins.

## Creating Plugins

### Requirements

- Target .NET 9.0 or later
- Reference `LofiBeats.Core` assembly
- Implement the `IAudioEffect` interface
- Provide a public parameterless constructor

### Example Plugin

```csharp
using LofiBeats.Core.Effects;
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

### Plugin Metadata

Your effect class should provide meaningful information through its properties:

- `Name`: A descriptive name for your effect (used in CLI commands)

### Best Practices

1. **Unique Names**: Ensure your effect name is unique and descriptive
2. **Error Handling**: Implement robust error handling in your audio processing code
3. **Resource Management**: Properly dispose of any resources your effect uses
4. **Thread Safety**: Make your effect thread-safe as it may be used in multiple contexts
5. **Performance**: Optimize your audio processing code for real-time performance

### Testing Your Plugin

1. Build your plugin project
2. Copy the `.dll` to the plugins directory
3. Run LofiBeats and verify your effect appears in the available effects:
   ```bash
   lofi effect list
   ```
4. Test your effect:
   ```bash
   lofi effect --name myeffect
   ```

## Using Plugins

Plugins are treated like built-in effects and can be used with the same commands:

```bash
# Add a plugin effect
lofi effect --name myeffect

# Remove a plugin effect
lofi effect --name myeffect --remove

# List all available effects (including plugins)
lofi effect list
```

## Troubleshooting

### Common Issues

1. **Plugin Not Found**
   - Verify the `.dll` is in the correct plugins directory
   - Check file permissions
   - Ensure the assembly is built for the correct platform

2. **Plugin Load Error**
   - Check the logs for detailed error messages
   - Verify all dependencies are available
   - Ensure the plugin targets a compatible .NET version

3. **Effect Not Working**
   - Enable debug logging for detailed diagnostics
   - Verify the effect name matches the `PluginEffectNameAttribute`
   - Check for runtime errors in the logs

### Logging

Plugin-related issues are logged to:
- Windows: `%LOCALAPPDATA%\LofiBeats\Logs`
- macOS: `~/Library/Application Support/LofiBeats/Logs`
- Linux: `~/.local/share/LofiBeats/Logs`

## Security Considerations

- Plugins run with the same privileges as LofiBeats
- Only install plugins from trusted sources
- Plugins have access to the audio stream and system resources
- The plugin loader implements basic security checks:
  - Validates assembly format
  - Checks for required interfaces
  - Monitors for resource abuse

## API Reference

For detailed API documentation, see the XML documentation in the `LofiBeats.Core` assembly.

Key interfaces and classes:
- `IAudioEffect`: Main interface for audio effects
- `ISampleProvider`: NAudio interface for audio processing

## Example Projects

See the [examples](../examples/Plugins) directory for sample plugin projects:
- Basic Effect Example
- Delay Effect Example
- Multi-Parameter Effect Example 