# LofiBeats Audio Test Container

This container provides a configured environment for running audio tests in CI/CD pipelines, specifically designed for .NET applications using OpenAL and PulseAudio.

## Features

- OpenAL with PulseAudio backend
- Virtual audio device support
- Configured for GitHub Actions
- Zero-hardware audio testing

## Usage

### GitHub Actions

```yaml
jobs:
  linux:
    runs-on: ubuntu-latest
    container:
      image: ghcr.io/willibrandon/lofibeats/audiotest:latest
      options: --init  # Important for proper audio daemon initialization
    
    steps:
      - name: Setup Audio
        run: /entrypoint.sh echo "Audio setup complete"
      
      # Your build and test steps here
```

### Local Testing

```bash
# Build the container
docker build -t lofibeats-test -f Dockerfile.test .

# Run tests
docker run --rm --init lofibeats-test dotnet test
```

## Technical Details

### Audio Configuration

- **PulseAudio**: Configured in system mode with a null sink
- **OpenAL**: Uses PulseAudio backend with optimal settings
- **Sample Rate**: 44.1kHz
- **Format**: 32-bit float stereo

### Key Components

1. **PulseAudio Configuration**
   - System mode for containerized environment
   - Null sink for virtual audio output
   - TCP/Unix socket support for flexible connectivity

2. **OpenAL Setup**
   - Configured via `alsoft.conf`
   - Debug logging enabled
   - Optimized buffer settings

3. **Container Entrypoint**
   - Initializes audio subsystem
   - Provides debug information
   - Ensures proper daemon startup

## Debugging

The container includes several diagnostic tools:

```bash
# View PulseAudio status
pactl info

# Check OpenAL devices
openal-info

# View audio debug logs
cat /tmp/openal.log
```

## Known Issues

1. Warning during cleanup: `AL lib: (EE) alc_cleanup: 1 device not closed`
   - This is expected and doesn't affect test execution
   - Occurs during container shutdown

## Contributing

Improvements to the audio configuration or container setup are welcome! Please test any changes thoroughly in a CI environment before submitting PRs.

## License

Same as the main project - MIT License
