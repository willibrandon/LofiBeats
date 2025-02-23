# Delay Effect Plugin Example

This is an example plugin for LofiBeats that demonstrates how to create a custom audio effect. The delay effect adds an echo to the input audio by creating a delayed copy of the signal mixed with the original.

## Features

- 500ms delay time
- Configurable feedback (echo decay)
- Adjustable wet/dry mix
- Stereo support
- Real-time processing

## Implementation Details

The delay effect works by:
1. Maintaining a circular buffer for delayed samples
2. Reading from the buffer at a fixed delay time
3. Writing back to the buffer with feedback
4. Mixing the delayed signal with the original

Key parameters:
- Delay Time: 500ms (22050 samples at 44.1kHz)
- Feedback: 50% (controls echo decay)
- Wet Mix: 50% (delayed signal level)
- Dry Mix: 70% (original signal level)

## Building

```bash
dotnet build
```

The built plugin will be in the `bin/Debug/net9.0` directory.

## Installation

Copy the built `DelayEffect.dll` to your LofiBeats plugins directory:
- Windows: `%LOCALAPPDATA%\LofiBeats\Plugins`
- macOS: `~/Library/Application Support/LofiBeats/Plugins`
- Linux: `~/.local/share/LofiBeats/Plugins`

## Usage

Once installed, the effect can be used with the `lofi` command:

```bash
# Add the delay effect
lofi effect --name delay

# Remove the delay effect
lofi effect --name delay --remove
```

## Testing

Run the tests with:

```bash
dotnet test
```

The tests verify:
- Basic initialization
- Wave format handling
- Delay signal generation
- Null source handling

## Learning from this Example

This example demonstrates:
1. Implementing the `IAudioEffect` interface
2. Using the `PluginEffectNameAttribute`
3. Handling audio buffers efficiently
4. Writing unit tests for audio effects
5. Proper resource management

Use this as a template for creating your own LofiBeats plugins! 