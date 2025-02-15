# Installing LofiBeats

This document provides instructions for installing and setting up LofiBeats, a command-line tool for generating and playing lofi beats.

## Prerequisites

- .NET 9.0 SDK or Runtime (download from [dotnet.microsoft.com](https://dotnet.microsoft.com/download))
- Audio output device with working drivers
- PowerShell 7+ (Windows) or Terminal (macOS/Linux)
- Git (optional, for building from source)

## Installation Options

### Option 1: Install as a Global Tool (Recommended)

```bash
dotnet tool install --global LofiBeats.Cli
```

This will install the latest version of LofiBeats from NuGet. After installation, you can run the tool using the `lofi` command.

### Option 2: Build from Source

1. **Clone the Repository**
   ```bash
   git clone https://github.com/willibrandon/LofiBeats.git
   cd LofiBeats
   ```

2. **Build the Solution**
   ```bash
   dotnet build
   ```

3. **Run the CLI Project**
   ```bash
   dotnet run --project src/LofiBeats.Cli/LofiBeats.Cli.csproj
   ```

   Or install locally as a tool:
   ```bash
   dotnet pack src/LofiBeats.Cli/LofiBeats.Cli.csproj -c Release
   dotnet tool install --global --add-source src/LofiBeats.Cli/nupkg LofiBeats.Cli
   ```

## Configuration

### Settings Files

The application uses two configuration files:

1. **CLI Settings** (`appsettings.json`):
   - Located in the tool's installation directory
   - Controls CLI behavior and logging

2. **Service Settings** (`service.appsettings.json`):
   - Controls audio service behavior
   - Manages telemetry and performance settings

To customize settings:

1. Locate the settings files:
   ```bash
   # Windows
   %USERPROFILE%\.dotnet\tools\.store\lofibeats.cli\1.0.0\lofibeats.cli\1.0.0\tools\net9.0\any

   # macOS/Linux
   ~/.dotnet/tools/.store/lofibeats.cli/1.0.0/lofibeats.cli/1.0.0/tools/net9.0/any
   ```

2. Edit the appropriate settings file:
   ```json
   {
     "Telemetry": {
       "IsEnabled": true,
       "EnableSeq": false
     }
   }
   ```

### Environment Variables

The following environment variables can be used to customize behavior:

- `LOFIBEATS_TELEMETRY_ENABLED`: Enable/disable telemetry (true/false)
- `LOFIBEATS_SERVICE_URL`: Override the service URL (default: http://localhost:5000)
- `DOTNET_ENVIRONMENT`: Set environment (Development/Production)

## Basic Usage

1. **Generate a Beat**
   ```bash
   lofi generate --style jazzy
   ```

2. **Play Music**
   ```bash
   lofi play --style chillhop
   ```

3. **Add Effects**
   ```bash
   lofi effect vinyl
   lofi effect reverb
   ```

4. **Control Playback**
   ```bash
   lofi pause
   lofi resume
   lofi stop
   ```

5. **Adjust Volume**
   ```bash
   lofi volume --level 0.8
   ```

6. **Interactive Mode**
   ```bash
   lofi interactive
   ```

## Troubleshooting

### Common Issues

1. **Command Not Found**
   - Ensure the .NET SDK is installed and in your PATH
   - Try running `dotnet tool list -g` to verify installation
   - Restart your terminal after installation

2. **Audio Issues**
   - Check your system's audio output device
   - Verify audio drivers are up to date
   - Try running `lofi stop` and then `lofi play` again

3. **Service Not Starting**
   - Check if port 5000 is available
   - Ensure you have necessary permissions
   - Look for error messages in the console output

### Getting Help

If you encounter issues:

1. Run `lofi help` for command documentation
2. Check the [GitHub Issues](https://github.com/willibrandon/LofiBeats/issues)
3. Contact the maintainers through GitHub

## Uninstallation

To remove LofiBeats:

```bash
dotnet tool uninstall --global LofiBeats.Cli
```

This will remove the tool and its settings. Your generated beats and configurations will remain in your user directory. 