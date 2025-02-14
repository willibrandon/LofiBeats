# Next Phase Implementation Guide

## Chunk 1: Reverb Effect Implementation

**Goal**  
Implement a **Reverb** effect (simple feedback delay line or a more sophisticated approach) in the audio processing chain.

**Steps**  
1. **Create `ReverbEffect.cs`** in `LofiBeats.Core/Effects`, if not already present:
   ```csharp
   using NAudio.Wave;
   using System;

   namespace LofiBeats.Core.Effects
   {
       public class ReverbEffect : IAudioEffect
       {
           private readonly ISampleProvider _source;
           public string Name => "reverb";
           public WaveFormat WaveFormat => _source.WaveFormat;

           private float[] _delayBuffer;
           private int _writePos;
           private readonly int _delaySamples;
           private float _feedback;
           private float _mix;

           public ReverbEffect(ISampleProvider source, float delayMs = 250f, float feedback = 0.3f, float mix = 0.5f)
           {
               _source = source;
               _feedback = feedback;
               _mix = mix;

               int sampleRate = source.WaveFormat.SampleRate;
               _delaySamples = (int)(delayMs / 1000f * sampleRate) * source.WaveFormat.Channels;
               _delayBuffer = new float[_delaySamples];
           }

           public int Read(float[] buffer, int offset, int count)
           {
               int samplesRead = _source.Read(buffer, offset, count);
               ApplyEffect(buffer, offset, samplesRead);
               return samplesRead;
           }

           public void ApplyEffect(float[] buffer, int offset, int count)
           {
               for (int i = 0; i < count; i++)
               {
                   float inputSample = buffer[offset + i];
                   float delayedSample = _delayBuffer[_writePos];

                   // Mixed output
                   float outputSample = (inputSample * (1 - _mix)) + (delayedSample * _mix);
                   buffer[offset + i] = outputSample;

                   // Write to the delay buffer
                   _delayBuffer[_writePos] = inputSample + delayedSample * _feedback;

                   // Increment write position
                   _writePos++;
                   if (_writePos >= _delayBuffer.Length)
                       _writePos = 0;
               }
           }
       }
   }
   ```
2. **Extend the `EffectFactory`** to support reverb creation:
   ```csharp
   // Inside EffectFactory.cs
   case "reverb":
       return new ReverbEffect(source);
   ```
3. **Test** by adding the effect via a CLI command:
   ```bash
   lofi-beats effect --name=reverb --enable
   ```
   (This assumes you have a test source—like a test tone—to hear the effect.)

**Verification / Testing**  
- **Build**: Confirm no compilation errors.  
- **At runtime**: Start playback, enable reverb, and listen for a subtle echo.  
- If you have automated tests for effects, add a small buffer test verifying the effect changes sample data.

**Git Commit**  
```bash
git add .
git commit -m "Added Reverb effect implementation and integrated into EffectFactory"
```

---

## Chunk 2: Volume Control Implementation

**Goal**  
Allow the user to adjust **master volume** in real time.

**Steps**  
1. **Add a `VolumeSampleProvider`** or use `WaveOutEvent`’s volume property.
   - **Option A** (NAudio’s `WaveOutEvent.Volume`):
     ```csharp
     public void SetVolume(float volume)
     {
         // Volume range is 0.0f to 1.0f for WaveOutEvent
         _waveOut.Volume = Math.Clamp(volume, 0f, 1f);
     }
     ```
   - **Option B** (Wrap the final mixer in a `VolumeSampleProvider`).
2. **Extend `IAudioPlaybackService`**:
   ```csharp
   public interface IAudioPlaybackService
   {
       // existing methods...
       void SetVolume(float volume);
   }
   ```
3. **Implement in `AudioPlaybackService`**:
   ```csharp
   public void SetVolume(float volume)
   {
       // If using WaveOutEvent
       _waveOut.Volume = volume;
       _logger.LogInformation($"Volume set to: {volume}");
   }
   ```
4. **Add CLI command** (e.g., `volume`):
   ```csharp
   var volumeCommand = new Command("volume", "Adjusts master volume");
   var volumeOption = new Option<float>("--level", "Volume level between 0.0 and 1.0");
   volumeCommand.AddOption(volumeOption);

   volumeCommand.SetHandler((InvocationContext ctx) =>
   {
       var playback = host.Services.GetRequiredService<IAudioPlaybackService>();
       float vol = ctx.ParseResult.GetValueForOption(volumeOption);
       playback.SetVolume(vol);
       Console.WriteLine($"Volume set to {vol}");
   });
   rootCommand.Add(volumeCommand);
   ```

**Verification / Testing**  
- **Build** and run: `lofi-beats volume --level=0.2`
- Listen for volume changes if you have test tone or real audio.

**Git Commit**  
```bash
git add .
git commit -m "Implemented master volume control and CLI command"
```

---

## Chunk 3: Pause/Resume Functionality

**Goal**  
Add **pause** (and optional **resume**) in audio playback. Currently, only start/stop is present.

**Steps**  
1. **Enhance `IAudioPlaybackService`** with:
   ```csharp
   void PausePlayback();
   void ResumePlayback();
   PlaybackState GetPlaybackState();
   ```
2. **Implement** in `AudioPlaybackService`:
   ```csharp
   public void PausePlayback()
   {
       if (_waveOut.PlaybackState == PlaybackState.Playing)
       {
           _waveOut.Pause();
           _logger.LogInformation("Playback paused.");
       }
   }

   public void ResumePlayback()
   {
       if (_waveOut.PlaybackState == PlaybackState.Paused)
       {
           _waveOut.Play();
           _logger.LogInformation("Playback resumed.");
       }
   }

   public PlaybackState GetPlaybackState() => _waveOut.PlaybackState;
   ```
3. **Add CLI commands**:
   ```csharp
   var pauseCommand = new Command("pause", "Pauses audio playback");
   pauseCommand.SetHandler(() =>
   {
       var playback = host.Services.GetRequiredService<IAudioPlaybackService>();
       playback.PausePlayback();
       Console.WriteLine("Playback paused.");
   });
   rootCommand.Add(pauseCommand);

   var resumeCommand = new Command("resume", "Resumes audio playback");
   resumeCommand.SetHandler(() =>
   {
       var playback = host.Services.GetRequiredService<IAudioPlaybackService>();
       playback.ResumePlayback();
       Console.WriteLine("Playback resumed.");
   });
   rootCommand.Add(resumeCommand);
   ```
4. **Test** by playing audio, pausing, verifying pause, and resuming.

**Verification / Testing**  
- **Build**: No errors.  
- **Run**: `lofi-beats play`, then `lofi-beats pause`, then `lofi-beats resume`.
- Confirm the audio halts and then continues.

**Git Commit**  
```bash
git add .
git commit -m "Added pause/resume functionality to playback service and CLI"
```

---

## Chunk 4: Enhanced Beat Generation

**Goal**  
Provide **multiple patterns** and **variations** for the beat generator to produce more interesting, random (or user-specified) lofi sequences.

**Steps**  
1. **Extend `IBeatGenerator`** or create multiple generator strategies:
   ```csharp
   public interface IBeatGenerator
   {
       BeatPattern GeneratePattern(string style = "basic");
   }
   ```
2. **In `BasicLofiBeatGenerator.cs`**, allow multiple style branches:
   ```csharp
   public BeatPattern GeneratePattern(string style = "basic")
   {
       switch (style)
       {
           case "jazzy":
               // Different chord progressions, more frequent hi-hats, etc.
               break;
           case "chillhop":
               // ...
               break;
           default:
               // existing logic
               break;
       }
       // ...
   }
   ```
3. **Add CLI** to specify style: `lofi-beats generate --style=jazzy`
   ```csharp
   var styleOption = new Option<string>("--style", () => "basic", "Beat style");
   generateCommand.AddOption(styleOption);

   generateCommand.SetHandler((InvocationContext ctx) =>
   {
       var generator = host.Services.GetRequiredService<IBeatGenerator>();
       string style = ctx.ParseResult.GetValueForOption(styleOption);
       var pattern = generator.GeneratePattern(style);
       Console.WriteLine($"Generated {style} pattern at {pattern.Tempo} BPM");
   });
   ```
4. **Optional**: Create multiple classes (`JazzyBeatGenerator`, `AmbientBeatGenerator`) and use a Factory/Strategy pattern.

**Verification / Testing**  
- **Run** different styles: `lofi-beats generate --style=jazzy`, etc.
- Confirm distinct results in chords/drum sequences.

**Git Commit**  
```bash
git add .
git commit -m "Enhanced beat generation with multiple styles and CLI support"
```

---

## Chunk 5: Interactive Mode for Real-Time Control

**Goal**  
Add an **interactive mode** allowing users to issue commands in a **REPL-like** environment without re-running the application each time.

**Steps**  
1. **In `Program.cs`** (or `CommandLineInterface.cs`), define an `interactive` command:
   ```csharp
   var interactiveCommand = new Command("interactive", "Enters an interactive mode");
   interactiveCommand.SetHandler(() =>
   {
       Console.WriteLine("Entering interactive mode. Type 'help' for commands, 'exit' to quit.");
       while (true)
       {
           Console.Write("> ");
           var line = Console.ReadLine();
           if (string.IsNullOrWhiteSpace(line)) continue;
           if (line.Equals("exit", StringComparison.OrdinalIgnoreCase)) break;

           // Parse line as if it were CLI arguments
           rootCommand.Invoke(line);
       }
   });
   rootCommand.Add(interactiveCommand);
   ```
2. **Launch** with `lofi-beats interactive`. The user can type `play`, `pause`, `effect --name=vinyl`, etc.
3. **Enhance** with a small command summary in the loop (like a help screen).

**Verification / Testing**  
- **Run**: `lofi-beats interactive`.  
- Type commands: `generate`, `play`, `pause`, `volume --level=0.5`, etc.  
- Confirm each command works without exiting.

**Git Commit**  
```bash
git add .
git commit -m "Added interactive mode for real-time control"
```

---

## Chunk 6: User Feedback Enhancements

**Goal**  
Improve **user experience** with progress indicators, better error handling/messages, and help documentation.

**Steps**  
1. **Better Error Handling**:
   - Wrap major operations in try/catch and display user-friendly messages.  
   - E.g., if an effect name is invalid, display `Effect 'xyz' not recognized. Type 'effect --list' to see valid effects.`
2. **Help/Documentation**:
   - Provide a `--help` or `help` subcommand that lists all available commands, usage, and examples.
   - Use `System.CommandLine`’s built-in help generator or manually outline them.
3. **Progress Indicators** (Optional):
   - For generating a pattern or loading resources, display a simple progress bar or status text. This might just be a textual spinner in the console.
4. **Update Command Definitions** to ensure each has a clear description, e.g.:
   ```csharp
   var generateCommand = new Command("generate", "Generates a new lofi beat pattern")
   {
       new Option<string>("--style", () => "basic", "Specifies the style of the beat (basic, jazzy, chillhop, etc.)")
   };
   ```

**Verification / Testing**  
- **Check** that `--help` or `help` displays a complete list of commands and their descriptions.  
- **Trigger** errors deliberately (e.g., `lofi-beats effect --name=unknownEffect`) to confirm user-friendly messages.

**Git Commit**  
```bash
git add .
git commit -m "Improved user feedback with better errors, help text, and optional progress indicators"
```

---

## Chunk 7: Telemetry and Performance Monitoring (Infrastructure)

**Goal**  
Add **telemetry** (e.g., using Application Insights or a custom analytics solution) to measure usage and performance.

**Steps**  
1. **Decide** on a telemetry provider:
   - [Application Insights](https://learn.microsoft.com/en-us/azure/azure-monitor/app/console) (requires Azure subscription)  
   - Third-party or custom solution
2. **Add NuGet package** if needed:
   ```bash
   dotnet add package Microsoft.ApplicationInsights
   ```
3. **Initialize** telemetry in `Startup.cs` or an extension method:
   ```csharp
   services.AddApplicationInsightsTelemetryWorkerService();
   ```
4. **Instrument** key events (e.g., `generate`, `play`, etc.) by logging metrics:
   ```csharp
   using Microsoft.ApplicationInsights;

   public class TelemetryReporter
   {
       private readonly TelemetryClient _telemetry;

       public TelemetryReporter(TelemetryClient telemetry)
       {
           _telemetry = telemetry;
       }

       public void TrackBeatGenerated(string style)
       {
           _telemetry.TrackEvent("BeatGenerated", new Dictionary<string, string>
           {
               { "Style", style }
           });
       }
   }
   ```
5. **Verify** locally or with a test telemetry environment.

**Verification / Testing**  
- **Build**: No errors.  
- **Run**: Confirm that telemetry events are being sent (check logs or Azure Portal if configured).

**Git Commit**  
```bash
git add .
git commit -m "Added basic telemetry and performance monitoring"
```

---

## Chunk 8: Crash Reporting

**Goal**  
Catch **unhandled exceptions** or crashes and report them to a logging system or telemetry service.

**Steps**  
1. **Global exception handling** in `Program.cs` or via `AppDomain.CurrentDomain.UnhandledException`:
   ```csharp
   AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
   {
       // Log or report the exception
       var ex = args.ExceptionObject as Exception;
       // e.g., TelemetryReporter.TrackException(ex);
       Console.Error.WriteLine($"Unhandled exception: {ex}");
   };
   ```
2. **Ensure** your logging or telemetry includes an exception call, e.g.:
   ```csharp
   // In TelemetryReporter
   public void TrackException(Exception ex)
   {
       _telemetry.TrackException(ex);
   }
   ```
3. **Test** by forcing an unhandled exception in a development build.

**Verification / Testing**  
- **Induce** an error scenario (e.g., throw an exception in a test command) to see if it’s captured.  
- Ensure the user sees a friendly message and the error is logged or reported.

**Git Commit**  
```bash
git add .
git commit -m "Implemented crash reporting and global exception handling"
```

---

## Chunk 9: Update Mechanism

**Goal**  
Provide a means for users to **update** the CLI tool to the latest version automatically or via a simple command.

**Steps**  
1. **Decide** on an approach:
   - If distributing via `dotnet tool`, users can run `dotnet tool update`.
   - If using a custom installer or script, provide a command to pull the latest from GitHub or a package feed.
2. **Example**: If you publish as a .NET Global Tool:
   - In your `.csproj`, set `<PackAsTool>true</PackAsTool>`.
   - Publish to NuGet.  
   - Users install with `dotnet tool install --global LofiBeats.Cli --version x.x.x`.
   - Updating: `dotnet tool update --global LofiBeats.Cli`.
3. **Add an `update` command** to notify the user to run the `dotnet tool update` or your chosen procedure:
   ```csharp
   var updateCommand = new Command("update", "Guides the user to update the CLI tool");
   updateCommand.SetHandler(() =>
   {
       Console.WriteLine("To update: dotnet tool update --global LofiBeats.Cli");
   });
   rootCommand.Add(updateCommand);
   ```

**Verification / Testing**  
- **Package** your CLI as a global tool and install it locally.  
- **Run** `dotnet tool update --global LofiBeats.Cli` to confirm the process works.

**Git Commit**  
```bash
git add .
git commit -m "Added basic update mechanism and instructions for global tool updates"
```

---

## Chunk 10: Installation & Setup Documentation

**Goal**  
Complete a thorough **INSTALL.md** or similar documentation describing how to install, configure, and run the application.

**Steps**  
1. **Create** an `INSTALL.md` (or `docs/INSTALL.md`) with:
   - **Prerequisites** (e.g., .NET 9 runtime, audio drivers).
   - **Steps** to build from source:
     ```bash
     git clone ...
     dotnet build
     dotnet run --project LofiBeats.Cli
     ```
   - **Steps** to install as a global tool (if applicable).
   - **Configuration** tips (editing `appsettings.json`, environment variables).
   - **Basic usage** examples.
2. **Link** from `README.md` to `INSTALL.md`.

**Verification / Testing**  
- **Have a colleague or friend** follow the instructions to confirm they can install and run without missing steps.

**Git Commit**  
```bash
git add .
git commit -m "Added detailed INSTALL.md for setup and usage instructions"
```

---

# Final Review

Once all chunks are complete:

1. **Review** the entire commit history and codebase to ensure consistency.  
2. **Test** end-to-end on all target platforms (Windows, macOS, Linux).  
3. **Tag/Release** your new version if using Git or GitHub releases.  

By following these **ten chunks** sequentially, you will have systematically implemented and tested all the **missing or incomplete areas** of your Lofi Beats Generator & Player. This incremental approach helps ensure high code quality, stable feature additions, and minimal confusion along the way.