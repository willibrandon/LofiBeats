# Chunk-Based Implementation Guide

## Chunk 1: Project Setup

**Goal**  
Initialize the solution and set up the basic folder structure and .NET projects.

**Steps**  
1. **Create a new .NET 9 solution** (adjust the .NET version if needed).
   - From the command line:  
     ```bash
     dotnet new sln --name LofiBeats
     ```
2. **Create three class library projects** and one console (or `.exe`) project:
   - `dotnet new console --name LofiBeats.Cli`
   - `dotnet new classlib --name LofiBeats.Core`
   - `dotnet new classlib --name LofiBeats.Infrastructure`
   - `dotnet new xunit --name LofiBeats.Tests`
3. **Add the projects to the solution**:
   ```bash
   dotnet sln add LofiBeats.Cli/LofiBeats.Cli.csproj
   dotnet sln add LofiBeats.Core/LofiBeats.Core.csproj
   dotnet sln add LofiBeats.Infrastructure/LofiBeats.Infrastructure.csproj
   dotnet sln add LofiBeats.Tests/LofiBeats.Tests.csproj
   ```
4. **Reference the relevant projects**:
   - In `LofiBeats.Cli`: `dotnet add reference ../LofiBeats.Core/LofiBeats.Core.csproj ../LofiBeats.Infrastructure/LofiBeats.Infrastructure.csproj`
   - In `LofiBeats.Tests`: `dotnet add reference ../LofiBeats.Core/LofiBeats.Core.csproj ../LofiBeats.Infrastructure/LofiBeats.Infrastructure.csproj`
5. **Create the basic directory structure** (you can do this with manual folder creation or via your IDE):
   ```
   LofiBeats
   ├─ src
   │  ├─ LofiBeats.Cli
   │  ├─ LofiBeats.Core
   │  └─ LofiBeats.Infrastructure
   └─ tests
      └─ LofiBeats.Tests
   ```

**Verification / Testing**  
- Run `dotnet build` at the solution root. There should be **no errors**.
- Confirm the folder structure and `.csproj` references look correct in your IDE.

**Git Commit**  
- Commit these initial changes:
  ```bash
  git add .
  git commit -m "Initial solution setup with projects: CLI, Core, Infrastructure, and Tests"
  ```

---

## Chunk 2: Basic CLI Scaffolding

**Goal**  
Set up a minimal **CLI skeleton** using `System.CommandLine` (or any chosen CLI library) in the `LofiBeats.Cli` project.

**Steps**  
1. **Add NuGet package** for command line parsing. For example:
   ```bash
   cd LofiBeats.Cli
   dotnet add package System.CommandLine --prerelease
   ```
   > *Note*: At the time of writing, `System.CommandLine` might still be in preview or pre-release. Adjust as needed.
2. **Modify `Program.cs`** in `LofiBeats.Cli` to define a minimal `Main` and wire up the root command:
   ```csharp
   using System;
   using System.CommandLine;
   using System.CommandLine.Invocation;

   namespace LofiBeats.Cli
   {
       public class Program
       {
           public static int Main(string[] args)
           {
               var rootCommand = new RootCommand
               {
                   new Command("hello", "Says hello")
                   {
                       Handler = CommandHandler.Create(() =>
                       {
                           Console.WriteLine("Hello from LofiBeats CLI!");
                       })
                   }
               };

               return rootCommand.InvokeAsync(args).Result;
           }
       }
   }
   ```
3. **Build and run**:
   ```bash
   dotnet run --project LofiBeats.Cli -- hello
   ```
   You should see output: `Hello from LofiBeats CLI!`

**Verification / Testing**  
- Ensure the command line command (`hello`) works.
- Confirm that no errors occur on build.

**Git Commit**  
```bash
git add .
git commit -m "Added basic CLI scaffolding with System.CommandLine"
```

---

## Chunk 3: Dependency Injection Setup

**Goal**  
Set up **Dependency Injection (DI)** and basic logging configuration in the CLI project.

**Steps**  
1. **Add NuGet packages** for DI and logging:
   ```bash
   dotnet add package Microsoft.Extensions.DependencyInjection
   dotnet add package Microsoft.Extensions.Hosting
   dotnet add package Microsoft.Extensions.Logging
   ```
2. **Create `Startup.cs`** (or similar) in `LofiBeats.Cli` to register services. For now, just place minimal placeholders:
   ```csharp
   using Microsoft.Extensions.Configuration;
   using Microsoft.Extensions.DependencyInjection;
   using Microsoft.Extensions.Hosting;
   using Microsoft.Extensions.Logging;

   namespace LofiBeats.Cli
   {
       public static class Startup
       {
           public static IHostBuilder CreateHostBuilder(string[] args)
           {
               return Host.CreateDefaultBuilder(args)
                   .ConfigureServices((context, services) =>
                   {
                       // Add your services here. Example:
                       // services.AddSingleton<ISomeService, SomeService>();
                   })
                   .ConfigureLogging(logging =>
                   {
                       logging.ClearProviders();
                       logging.AddConsole();
                   });
           }
       }
   }
   ```
3. **Refactor `Program.cs`** to use `Startup.CreateHostBuilder`:
   ```csharp
   using System.CommandLine;
   using System.CommandLine.Invocation;
   using Microsoft.Extensions.Hosting;

   namespace LofiBeats.Cli
   {
       public class Program
       {
           public static int Main(string[] args)
           {
               var builder = Startup.CreateHostBuilder(args);
               var host = builder.Build();

               // You can now resolve services from DI if needed.

               var rootCommand = new RootCommand("LofiBeats CLI")
               {
                   new Command("hello", "Says hello")
                   {
                       Handler = CommandHandler.Create(() =>
                       {
                           System.Console.WriteLine("Hello from DI-enabled LofiBeats CLI!");
                       })
                   }
               };

               return rootCommand.InvokeAsync(args).Result;
           }
       }
   }
   ```
4. **Build and test**. The CLI should still respond to the `hello` command, but now you have a DI-enabled host to work with.

**Verification / Testing**  
- Build: `dotnet build`  
- Run: `dotnet run --project LofiBeats.Cli -- hello`  
- Expect the same output. There should be no runtime or compile errors.

**Git Commit**  
```bash
git add .
git commit -m "Set up DI (HostBuilder) and logging in the CLI"
```

---

## Chunk 4: Beat Generation Basics (Core)

**Goal**  
Implement a **beat generation engine** in `LofiBeats.Core` to create a rudimentary lofi drum pattern and chord progression.

**Steps**  
1. **Create `IBeatGenerator.cs`** interface in `LofiBeats.Core/BeatGeneration`:
   ```csharp
   namespace LofiBeats.Core.BeatGeneration
   {
       public interface IBeatGenerator
       {
           BeatPattern GeneratePattern();
       }
   }
   ```
2. **Create a simple model** `BeatPattern` in `LofiBeats.Core/Models`:
   ```csharp
   namespace LofiBeats.Core.Models
   {
       public class BeatPattern
       {
           public int Tempo { get; set; }
           public string[] DrumSequence { get; set; }
           public string[] ChordProgression { get; set; }
       }
   }
   ```
3. **Implement `BasicLofiBeatGenerator.cs`** in `LofiBeats.Core/BeatGeneration`:
   ```csharp
   using System;
   using LofiBeats.Core.Models;

   namespace LofiBeats.Core.BeatGeneration
   {
       public class BasicLofiBeatGenerator : IBeatGenerator
       {
           private static Random _rnd = new Random();

           public BeatPattern GeneratePattern()
           {
               int tempo = _rnd.Next(70, 91);
               var drums = new[] { "kick", "hat", "snare", "hat" };
               var chords = new[] { "Fmaj7", "Am7", "Dm7", "G7" };

               return new BeatPattern
               {
                   Tempo = tempo,
                   DrumSequence = drums,
                   ChordProgression = chords
               };
           }
       }
   }
   ```
4. **Register the generator** in `Startup.cs`:
   ```csharp
   // inside ConfigureServices
   services.AddSingleton<IBeatGenerator, BasicLofiBeatGenerator>();
   ```
5. **Use it** (optional test) in the CLI:
   ```csharp
   // In Program.cs, after building host
   var generator = host.Services.GetRequiredService<IBeatGenerator>();

   var genCommand = new Command("generate", "Generates a new beat pattern")
   {
       Handler = CommandHandler.Create(() =>
       {
           var pattern = generator.GeneratePattern();
           Console.WriteLine($"Generated pattern at {pattern.Tempo} BPM");
       })
   };
   rootCommand.Add(genCommand);
   ```

**Verification / Testing**  
- Build and run: `dotnet run --project LofiBeats.Cli -- generate`  
- Confirm it prints a random tempo and the placeholder sequence.

**Git Commit**  
```bash
git add .
git commit -m "Implemented basic beat generation (IBeatGenerator, BeatPattern)"
```

---

## Chunk 5: Audio Effects (Core)

**Goal**  
Create an **effects** subsystem to apply audio filters like vinyl crackle, low-pass, and reverb, using the `NAudio` library.

**Steps**  
1. **Add NuGet package** for NAudio in the main solution or specifically in Core:
   ```bash
   dotnet add ../LofiBeats.Core/LofiBeats.Core.csproj package NAudio
   ```
2. **Create an `IAudioEffect` interface** in `LofiBeats.Core/Effects`:
   ```csharp
   using NAudio.Wave;

   namespace LofiBeats.Core.Effects
   {
       public interface IAudioEffect : ISampleProvider
       {
           string Name { get; }
           void ApplyEffect(float[] buffer, int offset, int count);
       }
   }
   ```
3. **Implement a vinyl crackle effect** (`VinylCrackleEffect.cs`):
   ```csharp
   using NAudio.Wave;
   using System;

   namespace LofiBeats.Core.Effects
   {
       public class VinylCrackleEffect : IAudioEffect
       {
           private readonly ISampleProvider _source;
           private readonly Random _rand = new Random();
           public string Name => "vinyl";
           public WaveFormat WaveFormat => _source.WaveFormat;

           public VinylCrackleEffect(ISampleProvider source)
           {
               _source = source;
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
                   if (_rand.NextDouble() < 0.0005)
                   {
                       buffer[offset + i] += (float)(_rand.NextDouble() * 2.0 - 1.0) * 0.2f;
                   }
               }
           }
       }
   }
   ```
4. **Add at least one more effect** (e.g., `LowPassFilterEffect` or `ReverbEffect`) in a similar way.
5. **Create an effect factory** (`EffectFactory.cs`) to instantiate effects by name:
   ```csharp
   using System;
   using NAudio.Wave;

   namespace LofiBeats.Core.Effects
   {
       public interface IEffectFactory
       {
           IAudioEffect CreateEffect(string effectName, ISampleProvider source);
       }

       public class EffectFactory : IEffectFactory
       {
           public IAudioEffect CreateEffect(string effectName, ISampleProvider source)
           {
               switch (effectName.ToLower())
               {
                   case "vinyl":
                       return new VinylCrackleEffect(source);
                   // case "lowpass": ...
                   // case "reverb": ...
                   default:
                       return null;
               }
           }
       }
   }
   ```
6. **Register the factory** in `Startup.cs`:
   ```csharp
   services.AddSingleton<IEffectFactory, EffectFactory>();
   ```

**Verification / Testing**  
- **Compilation**: Ensure everything builds without errors.  
- **No immediate audio test** yet. We'll integrate in the next chunk.

**Git Commit**  
```bash
git add .
git commit -m "Added audio effects subsystem (IAudioEffect, VinylCrackleEffect, EffectFactory)"
```

---

## Chunk 6: Audio Playback Service (Core)

**Goal**  
Create a service to **mix** audio, apply effects, and output to a device using NAudio. We’ll add a minimal playback approach here.

**Steps**  
1. **In `LofiBeats.Core/Playback`**, create an interface `IAudioPlaybackService.cs`:
   ```csharp
   namespace LofiBeats.Core.Playback
   {
       public interface IAudioPlaybackService
       {
           void StartPlayback();
           void StopPlayback();
           void AddEffect(IAudioEffect effect);
           void RemoveEffect(string effectName);
       }
   }
   ```
2. **Create `AudioPlaybackService.cs`** to implement `IAudioPlaybackService`:
   ```csharp
   using System;
   using System.Collections.Generic;
   using Microsoft.Extensions.Logging;
   using NAudio.Wave;
   using LofiBeats.Core.Effects;

   namespace LofiBeats.Core.Playback
   {
       public class AudioPlaybackService : IAudioPlaybackService, IDisposable
       {
           private readonly ILogger<AudioPlaybackService> _logger;
           private WaveOutEvent _waveOut;
           private MixingSampleProvider _mixer;
           private List<IAudioEffect> _effects = new List<IAudioEffect>();

           public AudioPlaybackService(ILogger<AudioPlaybackService> logger)
           {
               _logger = logger;
               _waveOut = new WaveOutEvent();

               _mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(44100, 2))
               {
                   ReadFully = true
               };
               _waveOut.Init(_mixer);
           }

           public void StartPlayback()
           {
               if (_waveOut.PlaybackState != PlaybackState.Playing)
               {
                   _waveOut.Play();
                   _logger.LogInformation("Playback started.");
               }
           }

           public void StopPlayback()
           {
               _waveOut.Stop();
               _logger.LogInformation("Playback stopped.");
           }

           public void AddEffect(IAudioEffect effect)
           {
               _effects.Add(effect);
               _mixer.AddMixerInput(effect);
               _logger.LogInformation($"{effect.Name} effect added.");
           }

           public void RemoveEffect(string effectName)
           {
               var effectToRemove = _effects.Find(e => e.Name.Equals(effectName, StringComparison.OrdinalIgnoreCase));
               if (effectToRemove != null)
               {
                   _mixer.RemoveMixerInput(effectToRemove);
                   _effects.Remove(effectToRemove);
                   _logger.LogInformation($"{effectName} effect removed.");
               }
           }

           public void Dispose()
           {
               _waveOut?.Dispose();
           }
       }
   }
   ```
3. **Register `AudioPlaybackService`** in `Startup.cs`:
   ```csharp
   services.AddSingleton<IAudioPlaybackService, AudioPlaybackService>();
   ```
4. **(Optional)** If you need to test real-time audio now, you can add a simple sine-wave or noise generator as a separate `ISampleProvider` input. For instance:
   ```csharp
   public class TestTone : ISampleProvider
   {
       private WaveFormat _wf = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);
       private float _phase;
       private float _phaseIncrement = (float)(2 * Math.PI * 440.0 / 44100);

       public WaveFormat WaveFormat => _wf;

       public int Read(float[] buffer, int offset, int count)
       {
           for (int n = 0; n < count; n += 2)
           {
               float sample = (float)Math.Sin(_phase);
               buffer[offset + n] = sample;     // left
               buffer[offset + n + 1] = sample; // right
               _phase += _phaseIncrement;
               if (_phase > 2 * Math.PI) _phase -= (float)(2 * Math.PI);
           }
           return count;
       }
   }
   ```
   Then do something like:
   ```csharp
   _mixer.AddMixerInput(new TestTone());
   ```

**Verification / Testing**  
- **Build** the solution. No errors.  
- If you added a test tone, run the CLI and call a hypothetical `play` command. You should hear the tone.

**Git Commit**  
```bash
git add .
git commit -m "Implemented AudioPlaybackService with wave out and mixing"
```

---

## Chunk 7: Integrate CLI Commands (Generate & Play)

**Goal**  
Wire up your **generate** and **play** commands to use the beat generator and playback service. (Full “scheduling” of beats might be simplified here—this chunk focuses on hooking up commands.)

**Steps**  
1. **Add subcommands** in `Program.cs` or a dedicated `CommandLineInterface.cs` file in `LofiBeats.Cli`.
   ```csharp
   var generateCommand = new Command("generate", "Generates a new lofi beat pattern");
   generateCommand.SetHandler(() =>
   {
       var generator = host.Services.GetRequiredService<IBeatGenerator>();
       var pattern = generator.GeneratePattern();
       Console.WriteLine($"Pattern: Tempo={pattern.Tempo}, Drums={string.Join(",",pattern.DrumSequence)}");
   });

   var playCommand = new Command("play", "Starts audio playback");
   playCommand.SetHandler(() =>
   {
       var playback = host.Services.GetRequiredService<IAudioPlaybackService>();
       playback.StartPlayback();
       Console.WriteLine("Playing...");
   });

   rootCommand.Add(generateCommand);
   rootCommand.Add(playCommand);
   ```
2. **Add an `effect` subcommand** to enable or remove effects:
   ```csharp
   var effectCommand = new Command("effect", "Manage effects");
   var effectNameOpt = new Option<string>("--name");
   var enableOpt = new Option<bool>("--enable", () => true);

   effectCommand.AddOption(effectNameOpt);
   effectCommand.AddOption(enableOpt);

   effectCommand.SetHandler((InvocationContext ctx) =>
   {
       var playback = host.Services.GetRequiredService<IAudioPlaybackService>();
       var effectFactory = host.Services.GetRequiredService<IEffectFactory>();

       string name = ctx.ParseResult.GetValueForOption(effectNameOpt);
       bool enable = ctx.ParseResult.GetValueForOption(enableOpt);

       if (enable)
       {
           // We need a source for the effect. In a more advanced system,
           // you'd chain the effect on your existing audio pipeline.
           // For demonstration, let's add a test tone with the effect:
           var testTone = new TestTone(); // or some wave provider
           var newEffect = effectFactory.CreateEffect(name, testTone);

           if (newEffect != null)
           {
               playback.AddEffect(newEffect);
               Console.WriteLine($"{name} enabled.");
           }
           else
           {
               Console.WriteLine($"Effect '{name}' not recognized.");
           }
       }
       else
       {
           playback.RemoveEffect(name);
           Console.WriteLine($"{name} disabled.");
       }
   });

   rootCommand.Add(effectCommand);
   ```
3. **Run**:
   - `dotnet run --project LofiBeats.Cli -- generate`
   - `dotnet run --project LofiBeats.Cli -- play`
   - `dotnet run --project LofiBeats.Cli -- effect --name=vinyl`
   - etc.

**Verification / Testing**  
- Confirm your new commands appear (`generate`, `play`, `effect`).  
- Validate that running them results in correct console messages. If you added a test tone or real input, you should hear audio playback.

**Git Commit**  
```bash
git add .
git commit -m "Integrated CLI commands for generate, play, and effect management"
```

---

## Chunk 8: Testing & Logging Enhancements

**Goal**  
Add **unit tests** for core modules (beat generation, effect logic) and confirm logging is working as intended.

**Steps**  
1. **Testing Beat Generation** in `LofiBeats.Tests/BeatGenerationTests.cs`:
   ```csharp
   using LofiBeats.Core.BeatGeneration;
   using Xunit;

   public class BeatGenerationTests
   {
       [Fact]
       public void BasicLofiBeatGenerator_ReturnsPattern()
       {
           // Arrange
           var generator = new BasicLofiBeatGenerator();

           // Act
           var pattern = generator.GeneratePattern();

           // Assert
           Assert.NotNull(pattern);
           Assert.True(pattern.Tempo >= 70 && pattern.Tempo <= 90);
           Assert.NotNull(pattern.DrumSequence);
           Assert.NotNull(pattern.ChordProgression);
       }
   }
   ```
2. **Testing Effects**:  
   - Since real-time audio testing is tricky, you can create small **buffer tests** to confirm that an effect modifies the audio in the intended manner (e.g., `VinylCrackleEffect` occasionally changes sample data).
3. **Logging**:  
   - Confirm `appsettings.json` or your logging configuration is set to log at `Information` level or above.
   - Validate logs appear in the console when playing or adding effects.

**Verification / Testing**  
- Run `dotnet test` in the solution root.  
- Ensure **all tests** pass.

**Git Commit**  
```bash
git add .
git commit -m "Added unit tests for beat generation and effect logic"
```

---

## Chunk 9: (Optional) Configuration & Deployment

**Goal**  
Add an `appsettings.json` or environment-based configuration. Show how to **publish** the application cross-platform.

**Steps**  
1. **Add `appsettings.json`** at the solution or CLI project root:
   ```json
   {
     "Logging": {
       "LogLevel": {
         "Default": "Information",
         "Microsoft": "Warning"
       }
     },
     "AudioSettings": {
       "DefaultTempo": 80
     }
   }
   ```
2. **Read in `Startup.cs`**:
   ```csharp
   public static IHostBuilder CreateHostBuilder(string[] args)
   {
       return Host.CreateDefaultBuilder(args)
           .ConfigureAppConfiguration((hostingContext, config) =>
           {
               config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
           })
           .ConfigureServices((context, services) =>
           {
               // ...
           })
           .ConfigureLogging(logging =>
           {
               logging.ClearProviders();
               logging.AddConsole();
           });
   }
   ```
3. **Publish** for the desired runtime:
   ```bash
   dotnet publish LofiBeats.Cli -c Release -r win10-x64 --self-contained false
   ```
   or for Linux:
   ```bash
   dotnet publish LofiBeats.Cli -c Release -r linux-x64 --self-contained false
   ```
4. **Distribution**: 
   - Zip the `publish` folder or distribute the single-file executable (if single-file publish is desired).
   - On Linux/macOS: `chmod +x LofiBeats.Cli`.

**Verification / Testing**  
- Confirm the published output runs on the target machine (`./LofiBeats.Cli -- generate` etc.).
- Check logs at runtime to validate your logging settings.

**Git Commit**  
```bash
git add .
git commit -m "Added configuration (appsettings.json) and deployment instructions"
```

---

## Final Verification & Review

1. **Review** all commits to ensure logical progression.
2. **Test** the entire app end-to-end:
   - `generate` a beat.
   - `play` audio (with or without test tone).
   - `effect --name=vinyl --enable` and check logs and sound changes if possible.
3. **Perform** any final **cleanup** or **refactoring** as needed.

**Git Commit**  
```bash
git add .
git commit -m "Final review and cleanup"
```

**At this point, your Lofi Beats CLI** is operational and ready for further enhancements!
