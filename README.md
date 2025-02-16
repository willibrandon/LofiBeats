# LofiBeats

A cross-platform command-line tool and service for generating and playing lofi beats with real-time effects.

## Features

- Generate lofi beats with different styles (basic, jazzy, chillhop, hiphop)
- Real-time audio effects:
  - Tape Stop
  - Vinyl Simulation (Flutter and Hiss)
  - Reverb
  - Low Pass Filter
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

# Add some effects
lofi effect --name reverb
lofi effect --name tapeflutter
lofi effect --name vinyl

# Control volume
lofi volume 0.8

# Stop playback with tapestop effect
lofi stop --tapestop
```

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

1. `appsettings.json` - CLI configuration
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
- `POST /api/lofi/play` - Start playback
- `POST /api/lofi/stop` - Stop playback
- `POST /api/lofi/pause` - Pause playback
- `POST /api/lofi/resume` - Resume playback
- `POST /api/lofi/volume` - Set volume level
- `POST /api/lofi/effect` - Add/remove effects
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
