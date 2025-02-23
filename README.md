# LofiBeats

A cross-platform command-line tool and service for generating and playing lofi beats with real-time effects.

## Features

- Generate lofi beats with different styles (basic, jazzy, chillhop, hiphop)
  - Configurable musical key (C, F#, Bb, etc.)
  - Automatic chord transposition
  - Enharmonic equivalent support
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
- WebSocket connections for real-time control
- Cross-platform support (Windows, macOS, Linux)
- Telemetry support with local file and Seq logging
- Plugin system for custom effects

## Quick Start

```bash
# Install the tool
dotnet tool install --global LofiBeats.Cli

# Start playing some beats
lofi play --style jazzy

# Add some effects
lofi effect --name reverb
lofi effect --name tapeflutter
lofi effect --name vinyl

# Control volume
lofi volume 0.8

# Stop playback
lofi stop

# Stop with tapestop effect
lofi stop --tapestop

# Play with custom BPM
lofi play --style chillhop --bpm 82

# Play in a specific key
lofi play --style jazzy --key F#
lofi play --style chillhop --key Bb

# Crossfade between patterns
lofi play --style jazzy --transition crossfade # Default 2s crossfade
lofi play --style chillhop --transition crossfade --xfade-duration 3 # Custom duration
lofi play --style hiphop --transition crossfade --after 30s # Scheduled crossfade

# Schedule playback to start later
lofi play --style jazzy --after 5m     # Start playing in 5 minutes
lofi stop --tapestop --after 30s       # Stop with effect in 30 seconds

# Schedule playback to stop later
lofi stop --after 10m
lofi stop --tapestop --after 30s

# Manage scheduled actions
lofi schedule list                      # View all scheduled actions
lofi schedule cancel <action-id>        # Cancel a specific action

# Save current configuration as a preset
lofi preset save mypreset.json

# Load a saved preset
lofi preset load mypreset.json

# Save presets in a custom directory
lofi preset save presets/chillvibes.json

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

## Plugins

LofiBeats supports custom audio effect plugins that extend the application's capabilities. The plugin system allows you to create and use your own audio effects.

For detailed information about creating plugins, installation, requirements, and best practices, see [Plugins Guide](docs/PLUGINS.md).

## Architecture

LofiBeats consists of three main components:

1. **LofiBeats.Cli**: Command-line interface tool
2. **LofiBeats.Service**: Background service with REST API and WebSockets support
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

- `POST /api/lofi/generate` - Generate a new beat pattern (supports `style`, `bpm`, and `key` parameters)
- `POST /api/lofi/play` - Start playback (supports `style`, `bpm`, `key`, `transition`, and `xfadeDuration` parameters)
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

## WebSockets Support

The service also provides real-time control and event notifications through WebSocket connections at `ws://localhost:5001/ws`.

### Authentication

When authentication is enabled, connect with a token:
```
ws://localhost:5001/ws?token=your_auth_token
```

### Commands

Send commands as JSON messages with the following format:
```json
{
  "type": "command",
  "action": "play",
  "payload": {
    "style": "jazzy",
    "bpm": 85,
    "key": "C#",
    "transition": "crossfade",
    "xfadeDuration": 2.0
  }
}
```

Available commands:
- `play` - Start playback with options
  ```json
  {
    "style": "basic|jazzy|chillhop|hiphop",
    "bpm": 80,
    "key": "C|C#|D|Eb|E|F|F#|G|Ab|A|Bb|B",
    "transition": "immediate|crossfade",
    "xfadeDuration": 2.0
  }
  ```
- `stop` - Stop playback
  ```json
  {
    "tapestop": false
  }
  ```
- `volume` - Adjust volume
  ```json
  {
    "level": 0.8
  }
  ```
- `add-effect` - Add an audio effect
  ```json
  {
    "name": "reverb|vinyl|tapestop|tapeflutter",
    "parameters": {
      "key": "value"
    }
  }
  ```
- `remove-effect` - Remove an audio effect
  ```json
  {
    "name": "reverb"
  }
  ```
- `sync-state` - Request current playback state

### Events

The server broadcasts events in the following format:
```json
{
  "type": "event",
  "action": "playback-started",
  "payload": {
    "style": "jazzy",
    "bpm": 85
  }
}
```

Available events:
- `volume-changed` - Volume level changed
- `playback-started` - Playback has started
- `playback-stopped` - Playback has stopped
- `beat-generated` - New beat pattern generated
- `effect-added` - Audio effect was added
- `effect-removed` - Audio effect was removed
- `metrics-updated` - Performance metrics update

### Error Messages

Error responses follow this format:
```json
{
  "type": "error",
  "action": "unknown-command",
  "payload": {
    "message": "Unknown command: invalid-action"
  }
}
```

Error types:
- `unknown-command` - Command not recognized
- `invalid-payload` - Invalid command parameters
- `auth-failed` - Authentication failure

### Rate Limiting

- Maximum message size: 4KB
- Rate limit: 60 messages per minute per client
- Maximum concurrent connections: 100

## Development

### Running Tests

```bash
# Run all tests
dotnet test

# Run specific test categories
dotnet test --filter "Category!=Platform_Specific"
dotnet test --filter "Category=AI_Generated"
```

### Test Container

For running tests in CI or containerized environments, we provide a Docker container with a pre-configured audio testing environment. See [CONTAINER.md](docs/CONTAINER.md) for details.

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
