# Configuration & Deployment Guide

## Configuration

### Overview
LofiBeats uses a hierarchical configuration system powered by Microsoft.Extensions.Configuration. The configuration can be sourced from:
- JSON files (appsettings.json)
- Environment variables
- Command-line arguments
- Environment-specific settings (e.g., appsettings.Development.json)

### Configuration Files

#### Base Configuration (appsettings.json)
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AudioSettings": {
    "DefaultTempo": 80,
    "SampleRate": 44100,
    "Channels": 2,
    "Effects": {
      "VinylCrackle": {
        "Frequency": 0.0005,
        "Amplitude": 0.2
      },
      "LowPass": {
        "CutoffFrequency": 2000,
        "Resonance": 1.0
      }
    }
  }
}
```

### Configuration Classes
The configuration is strongly typed using the following classes:

```csharp
public class AudioSettings
{
    public int DefaultTempo { get; set; } = 80;
    public int SampleRate { get; set; } = 44100;
    public int Channels { get; set; } = 2;
    public EffectSettings Effects { get; set; } = new();
}
```

### Environment Variables
You can override any setting using environment variables. Use double underscores (__) to represent hierarchy:
```bash
AudioSettings__DefaultTempo=85
AudioSettings__Effects__VinylCrackle__Amplitude=0.3
```

### Command-line Arguments
Settings can also be overridden via command-line:
```bash
./LofiBeats.Cli --AudioSettings:DefaultTempo 85
```

## Deployment

### Prerequisites
- .NET 9.0 SDK or Runtime (depending on deployment type)
- Target platform (Windows/Linux) with appropriate audio drivers

### Publishing Profiles

#### Windows x64 (win-x64.pubxml)
```xml
<PropertyGroup>
  <Configuration>Release</Configuration>
  <RuntimeIdentifier>win-x64</RuntimeIdentifier>
  <SelfContained>false</SelfContained>
  <PublishSingleFile>true</PublishSingleFile>
  <PublishReadyToRun>true</PublishReadyToRun>
</PropertyGroup>
```

#### Linux x64 (linux-x64.pubxml)
```xml
<PropertyGroup>
  <Configuration>Release</Configuration>
  <RuntimeIdentifier>linux-x64</RuntimeIdentifier>
  <SelfContained>false</SelfContained>
  <PublishSingleFile>true</PublishSingleFile>
  <PublishReadyToRun>false</PublishReadyToRun>
</PropertyGroup>
```

### Publishing Commands

#### Windows Build
```bash
dotnet publish src/LofiBeats.Cli -c Release /p:PublishProfile=win-x64
```

Output location: `src/LofiBeats.Cli/bin/Release/net9.0/publish/win-x64/`

#### Linux Build
```bash
dotnet publish src/LofiBeats.Cli -c Release /p:PublishProfile=linux-x64
```

Output location: `src/LofiBeats.Cli/bin/Release/net9.0/publish/linux-x64/`

### Deployment Options

#### Framework-dependent Deployment (Current)
- Requires .NET Runtime on target machine
- Smaller deployment size
- Uses current profiles
```bash
dotnet publish -c Release --no-self-contained
```

#### Self-contained Deployment
- No .NET Runtime required
- Larger deployment size
- Modify profiles: `<SelfContained>true</SelfContained>`
```bash
dotnet publish -c Release --self-contained true
```

### Running the Published Application

#### Windows
```bash
cd src/LofiBeats.Cli/bin/Release/net9.0/publish/win-x64
./LofiBeats.Cli.exe -- generate
```

#### Linux
```bash
cd src/LofiBeats.Cli/bin/Release/net9.0/publish/linux-x64
chmod +x LofiBeats.Cli
./LofiBeats.Cli -- generate
```

### Deployment Package Contents
- Single executable file (LofiBeats.Cli[.exe])
- appsettings.json
- Dependencies (if not using single-file publishing)

### Troubleshooting

#### Common Issues
1. **Missing .NET Runtime**
   - Error: "A fatal error occurred. The required library hostfxr.dll could not be found"
   - Solution: Install .NET 9.0 Runtime or use self-contained deployment

2. **Configuration File Not Found**
   - Error: "The configuration file 'appsettings.json' was not found"
   - Solution: Ensure appsettings.json is in the same directory as the executable

3. **Audio Device Issues**
   - Error: "No audio device found" or similar
   - Solution: Verify audio drivers are installed and working

#### Environment-specific Configuration
Create environment-specific settings files:
- appsettings.Development.json
- appsettings.Production.json

Set the environment:
```bash
# Windows
set DOTNET_ENVIRONMENT=Production

# Linux/macOS
export DOTNET_ENVIRONMENT=Production
```

### Security Considerations
1. Don't store sensitive information in appsettings.json
2. Use environment variables or secure vaults for sensitive data
3. Consider using configuration encryption for production deployments

### Monitoring
- Application logs are written to console by default
- Configure additional logging providers in Startup.cs if needed
- Use environment variables to adjust log levels in production 