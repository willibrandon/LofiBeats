# User Samples Guide

## Sample Management

The system supports four basic drum types that can be replaced with custom samples:
- `kick` - Bass drum
- `snare` - Snare drum
- `hat` - Closed hi-hat
- `ohat` - Open hi-hat

```bash
# Register a basic sample
lofi sample register <type> <path>

# Register a sample with velocity layer
lofi sample register <type> <path> --velocity <0-127>

# List all registered samples
lofi sample list
```

## Velocity Layers

You can register multiple versions of the same drum type with different velocity layers for more expressive playback:

```bash
# Register soft, medium, and hard versions of a kick drum
lofi sample register kick /path/to/kick_soft.wav --velocity 32
lofi sample register kick /path/to/kick_med.wav --velocity 64
lofi sample register kick /path/to/kick_hard.wav --velocity 96
```

The system will automatically select the most appropriate sample based on the playback velocity. For example, softer hits in the pattern will trigger samples registered with lower velocity values.

Note: When registering samples without a velocity layer, they will be used as the default sound for that drum type regardless of velocity.

## Sample Storage

Samples are stored in:
- Windows: `%LOCALAPPDATA%\LofiBeats\Samples`
- macOS: `~/Library/Application Support/LofiBeats/Samples`
- Linux: `~/.local/share/LofiBeats/Samples`

## Supported Formats

- WAV files (recommended)
- MP3 files
- AIFF files
- Other formats supported by NAudio

## Best Practices

1. Use high-quality, uncompressed WAV files for best performance
2. Keep samples short and focused for optimal memory usage
3. Use consistent sample rates (44.1kHz recommended)
4. Normalize samples before registering
5. Use descriptive names for easy organization 