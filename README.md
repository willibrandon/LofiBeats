# LofiBeats

A cross-platform command-line tool and service for generating and playing lofi beats with real-time effects.

## Features

- Generate lofi beats with different styles (basic, jazzy, chillhop, hiphop)
- Real-time audio effects:
  - Tape Stop (gradually slow down the audio like a tape machine powering off)
  - Tape Flutter (adds wow/flutter pitch drift and tape hiss for vintage vibes)
  - Vinyl (adds vinyl record crackle and noise for that authentic feel)
  - Reverb (adds space and atmosphere to create depth)
  - Low Pass Filter (reduces high frequencies for that warm, mellow sound)
- Advanced Drum Synthesis:
  - Multi-stage envelopes for realistic drum sounds
  - Velocity and timing humanization
  - Customizable synthesis parameters
- Smooth transitions between patterns:
  - Crossfade between styles (0.1s to 10s duration)
  - Bar-synchronized transitions
  - Schedule future transitions
- User Sample Support:
  - Register your own drum samples
  - Support for velocity layers
  - Automatic sample management
  - Preloading option for optimal performance
- Preset Management:
  - Save current style, volume, and effects to a preset file
  - Load presets to quickly restore your favorite configurations
  - JSON-based preset format for easy sharing and editing
- Schedule Management:
  - Schedule playback and stop actions with delays
  - List all pending scheduled actions
  - Cancel specific scheduled actions by ID
  - Real-time progress tracking for scheduled actions
- Interactive mode for live control
- RESTful API service for remote control
- Cross-platform support (Windows, macOS, Linux)
- Telemetry support with local file and Seq logging
- Plugin system for custom effects

## Quick Start

```bash
# Install the tool
dotnet tool install --global LofiBeats.Cli

# Start playing some beats
lofi play --style jazzy

# Play with custom BPM
lofi play --style chillhop --bpm 82

# Crossfade between patterns
lofi play --style jazzy --transition crossfade              # Default 2s crossfade
lofi play --style chillhop --transition crossfade --xfade-duration 3  # Custom duration
lofi play --style hiphop --transition crossfade --after 30s # Scheduled crossfade

# Schedule playback to start later
lofi play --style hiphop --after 5m
lofi play --style chillhop --bpm 85 --after 30s

# Generate a pattern with specific BPM
lofi generate --style hiphop --bpm 95

# Add some effects
lofi effect --name reverb
lofi effect --name tapeflutter
lofi effect --name vinyl

# Control volume
lofi volume 0.8

# Schedule commands
lofi play --style jazzy --after 5m     # Start playing in 5 minutes
lofi stop --tapestop --after 30s       # Stop with effect in 30 seconds

# Manage scheduled actions
lofi schedule list                      # View all scheduled actions
lofi schedule cancel <action-id>        # Cancel a specific action

# Save current configuration as a preset
lofi preset save mypreset.json

# Load a saved preset
lofi preset load mypreset.json

# Save presets in a custom directory
lofi preset save presets/chillvibes.json

# Stop playback
lofi stop

# Stop with tapestop effect
lofi stop --tapestop

# Schedule playback to stop later
lofi stop --after 10m
lofi stop --tapestop --after 30s

# Register your own samples
lofi sample register kick /path/to/kick.wav
lofi sample register snare /path/to/snare.wav
lofi sample register hat /path/to/hihat.wav

# Register samples with velocity layers (0-127)
lofi sample register kick /path/to/soft_kick.wav --velocity 64
lofi sample register kick /path/to/hard_kick.wav --velocity 127

# List registered samples
lofi sample list

# Unregister samples
lofi sample unregister kick        # Unregister a basic sample
lofi sample unregister kick-soft   # Unregister a sample with all its velocity layers
```

## User Samples

LofiBeats supports using your own audio samples for drum sounds. Samples are automatically managed and stored in your local application data directory.

For detailed information about sample management, velocity layers, supported formats, and best practices, see [User Samples Guide](docs/USER_SAMPLES.md).

## Architecture

LofiBeats consists of three main components:

1. **LofiBeats.Cli**: Command-line interface tool
2. **LofiBeats.Service**: Background service with REST API
3. **LofiBeats.Core**: Core audio processing and effect implementation

## Build Requirements

- .NET 9.0 SDK
- PowerShell 7+ (Windows) or Terminal (macOS/Linux)
- Audio output device with working drivers

### Platform-Specific Requirements

#### Windows
- NAudio native dependencies (included)
- System.Management package for process management

#### macOS
- CoreAudio drivers
- `ps` command available for process management

#### Linux
- ALSA audio system
- `pkill` command available for process management

## Building from Source

```bash
# Clone the repository
git clone https://github.com/willibrandon/LofiBeats.git
cd LofiBeats

# Build the solution
dotnet build

# Run tests (platform-specific tests will be skipped on non-Windows platforms)
dotnet test

# Create platform-specific releases
./scripts/publish.sh --runtime linux-x64 --version 1.0.0
./scripts/publish.sh --runtime osx-x64 --version 1.0.0
./scripts/publish.sh --runtime win-x64 --version 1.0.0
```

## Configuration

The application uses two configuration files:

1. `cli.appsettings.json` - CLI configuration
2. `service.appsettings.json` - Service configuration

Key configuration options:

```json
{
  "Telemetry": {
    "IsEnabled": true,
    "EnableSeq": false,
    "SeqServerUrl": "http://localhost:5341",
    "EnableLocalFile": true
  }
}
```

## API Endpoints

The service exposes the following REST API endpoints:

- `POST /api/lofi/generate` - Generate a new beat pattern
- `POST /api/lofi/play` - Start playback (supports crossfade with `transition` and `xfadeDuration` parameters)
- `POST /api/lofi/stop` - Stop playback
- `POST /api/lofi/pause` - Pause playback
- `POST /api/lofi/resume` - Resume playback
- `POST /api/lofi/volume` - Set volume level
- `POST /api/lofi/effect` - Add/remove effects
- `POST /api/lofi/schedule-play` - Schedule a future playback
- `POST /api/lofi/schedule-stop` - Schedule a future stop
- `GET /api/lofi/preset/current` - Get current preset state
- `POST /api/lofi/preset/apply` - Apply a preset configuration
- `GET /healthz` - Health check endpoint

## Development

### Running Tests

```bash
# Run all tests
dotnet test

# Run specific test categories
dotnet test --filter "Category!=Platform_Specific"
dotnet test --filter "Category=AI_Generated"
```

### Adding New Effects

1. Create a new effect class in `src/LofiBeats.Core/Effects`
2. Implement the `IAudioEffect` interface
3. Register the effect in `EffectFactory.cs`

### Test Container

For running tests in CI or containerized environments, we provide a Docker container with a pre-configured audio testing environment. See [CONTAINER.md](CONTAINER.md) for details.

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## Telemetry

The application collects anonymous usage telemetry to improve the user experience. Data is stored in:

- Windows: `%LOCALAPPDATA%\LofiBeats\Telemetry`
- macOS: `~/Library/Application Support/LofiBeats/Telemetry`
- Linux: `~/.local/share/LofiBeats/Telemetry`

Telemetry can be disabled in the configuration file.

## License

This project is licensed under the MIT License - see the LICENSE file for details.
