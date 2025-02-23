Below is a **step-by-step, chunked implementation guide** for **Secure Plugin Execution** in LofiBeats using **out-of-process plugin hosting**. Each chunk is intentionally small to keep an AI collaborator or a human developer focused. At the end of each chunk, you will **verify or test** the changes and then **commit** before moving on.

---
## Chunk 1: Outline Goals & Create a Dedicated `PluginHost` Project

### Purpose
1. Establish a clear vision of out-of-process plugin execution.
2. Create a dedicated host application (a console project) to run plugins in an isolated process.

### Steps
1. **Create or plan the `PluginHost`**:
   - **Directory Structure**: Under `src/`, create a folder `LofiBeats.PluginHost`.  
   - **Project Creation**: Initialize a new .NET console app (target .NET 9) named `LofiBeats.PluginHost.csproj`.
   - **Basic Main Method**: The new `Program.cs` can read arguments like `--plugin-assembly <Path>` to specify which plugin assembly to load.  

2. **Define PluginHost Responsibilities**:
   - Parse command-line arguments to get the plugin assembly path.
   - Load the plugin assembly (still in-process for the host, but separate from the main LofiBeats process).
   - Implement a loop or a minimal IPC mechanism (can be STDIN/STDOUT, named pipe, or socket).
   - Respond to host commands like “Initialize plugin,” “Execute effect,” “Unload plugin,” etc.

3. **Decide on IPC** (for example, **STDIN/STDOUT** first for simplicity):
   - Start with a basic approach: read lines from STDIN, parse JSON, handle commands, send JSON back over STDOUT.

4. **Prepare a minimal message schema**:
   - For example, define a `PluginMessage` with fields like `Action`, `Payload`.
   - In `PluginHost`, implement simple JSON `Console.ReadLine()` / `Console.WriteLine()` to exchange messages.

5. **Verification / Testing**:
   - Ensure `LofiBeats.PluginHost.csproj` builds and runs with a simple `Console.WriteLine("PluginHost started");`.
   - **No actual plugin logic** yet—just confirm the new project compiles.
   - Test by running `dotnet run --project .\src\LofiBeats.PluginHost\LofiBeats.PluginHost.csproj` to see console output.

6. **Git Commit**:
   - Commit with a message: “Add basic LofiBeats.PluginHost project for out-of-process plugin loading.”

---

## Chunk 2: Adapt `PluginManager` to Launch the `PluginHost` Externally

### Purpose
1. Modify how plugins are “loaded” so that instead of loading the assembly in-process, we launch `LofiBeats.PluginHost` as a separate process.

### Steps
1. **Introduce a new mode in `PluginManager`**:
   - In `PluginManager.cs`, add a property like `OutOfProcessEnabled = true`.  
   - If `OutOfProcessEnabled` is set, do not directly call `Activator.CreateInstance` on the plugin assembly.

2. **Spawning the Process**:
   - In `PluginLoader` or `PluginManager`, replace the typical assembly load with a call to start the new `PluginHost`:
     ```csharp
     var startInfo = new ProcessStartInfo
     {
         FileName = "dotnet",
         Arguments = $"run --project \"{pathToPluginHostCsproj}\" --plugin-assembly \"{dllPath}\"",
         RedirectStandardInput = true,
         RedirectStandardOutput = true,
         UseShellExecute = false,
         CreateNoWindow = true
     };
     var process = Process.Start(startInfo);
     ```
   - Store references to `process.StandardInput` and `process.StandardOutput` in a new structure, e.g. `PluginHostConnection`.

3. **Plugin Proxy Object**:
   - In `PluginManager`, instead of returning a real plugin instance, return a **proxy** that wraps the `PluginHostConnection`. Any effect calls will eventually be translated into IPC calls to the plugin host process.

4. **Verification / Testing**:
   - Confirm that launching `PluginHost` from `PluginManager` works (the process starts, no crash).
   - Not fully functional yet, but you should see the new console logs from `PluginHost`.
   - Possibly debug log to confirm a child process is started.  

5. **Git Commit**:
   - Commit with message: “PluginManager spawns PluginHost process instead of direct assembly load.”

---

## Chunk 3: Establish a Minimal IPC Protocol (JSON Over STDIO)

### Purpose
1. Let the main LofiBeats process send commands to the plugin host and receive responses.
2. Provide enough scaffolding to demonstrate basic command/response.

### Steps
1. **Define a ‘hello’ or ‘init’ command**:
   - In the main app (`PluginManager` or `PluginLoader`), after spawning the process, send a JSON message: `{"action":"init","payload":{"pluginName":"MyPlugin"}}`.
2. **PluginHost listening**:
   - In `PluginHost`’s `Program.cs`, read lines from `Console.In`. For each line:
     - Deserialize to a `PluginMessage { string Action; object Payload; }`.
     - If `Action == "init"`, store plugin info, and respond with `{"status":"ok","message":"Plugin initialized"}`.
3. **Send response**:
   - After processing “init”, the plugin host does:
     ```csharp
     var response = new { status = "ok", message = "Plugin initialized" };
     Console.WriteLine(JsonSerializer.Serialize(response));
     ```
   - Flush stdout if necessary.

4. **Round-trip**:
   - In `PluginManager`, read `process.StandardOutput.ReadLine()` to parse the plugin’s response.  
   - Log or store that the plugin is “initialized.”

5. **Verification / Testing**:
   - Write a quick test or debug call:
     1. Start the plugin host process.
     2. Send `init` message.
     3. Read the response.
     4. Print out “Received: <JSON> from plugin host.”  
   - Confirm the JSON matches expectations.

6. **Git Commit**:
   - Commit with message: “Establish minimal JSON-based IPC protocol for plugin init.”

---

## Chunk 4: Load the Plugin Assembly in `PluginHost` and Confirm Metadata

### Purpose
1. Actually load the plugin assembly in `PluginHost` instead of the main app.
2. Retrieve plugin metadata (like effect names) to prove out-of-process reflection.

### Steps
1. **Load Plugin Assembly**:
   - In `PluginHost`, when receiving the `init` or a specialized “load” command, do something like:
     ```csharp
     var assembly = Assembly.LoadFrom(pluginAssemblyPath);
     // Reflect to find classes implementing IAudioEffect
     var effectTypes = assembly.GetTypes()
         .Where(t => typeof(IAudioEffect).IsAssignableFrom(t) && !t.IsAbstract)
         .ToList();
     ```
2. **Return Basic Metadata**:
   - For each effect type found, gather the `Name`, `Version`, `Description` from the `IAudioEffect` instance.  
   - Return that data in a JSON response, e.g.:  
     ```json
     { "status": "ok", "effects": [ { "name": "xxx", "description": "...", ...} ] }
     ```
3. **PluginManager**: On receiving that metadata, store it in the dictionary. This is the same dictionary you used to store effect metadata previously, but now populated from remote.

4. **Verification / Testing**:
   - Attempt to “load” a known plugin. See if it returns correct effect names.
   - If the plugin assembly is missing or has an error, ensure the plugin host handles the exception gracefully (and returns an error response).

5. **Git Commit**:
   - Commit with message: “Implement reflection-based metadata loading in PluginHost.”

---

## Chunk 5: Implement a Basic “CreateEffect” RPC Over IPC

### Purpose
1. Let the main app request the creation of an effect instance in the plugin host and hold a reference ID.

### Steps
1. **In PluginHost**:
   - Define a new command, e.g. `"createEffect"`. The `payload` might contain `effectName`.
   - On receiving it, find the type for that effect, `Activator.CreateInstance(...)`, store the instance in a dictionary, e.g. `_activeEffects[guid] = createdEffect`.
   - Return a reference ID, for example `{"status":"ok","effectId":"<someGuid>"}`.

2. **In PluginManager**:
   - Create a new method `CreateEffect(string effectName)`. This:
     1. Sends `{"action":"createEffect","payload":{"effectName": "myEffect"}}`.
     2. Receives the response, extracts `effectId`.
     3. Returns a local “proxy” object that knows how to talk to that effect ID in the plugin host.

3. **Verification / Testing**:
   - Hard-code a known effect name in a test. Confirm the host returns an effect ID.
   - Watch logs or debug prints to confirm the instance is created in the plugin host.

4. **Git Commit**:
   - Commit with message: “Add createEffect RPC in PluginHost and proxy in PluginManager.”

---

## Chunk 6: Implement “ApplyEffect” or “ProcessAudio” Call (Proof-of-Concept)

### Purpose
1. Show how real effect calls (like processing audio buffers) might occur out-of-process.
2. This chunk is mostly conceptual for LofiBeats, because real-time audio typically calls `Read(...)` in-process.

### Steps
1. **Proxy Method**: In your `proxyEffect`, define `ApplyEffect(buffer, offset, count)` or a simplified call like `TestEffectParameter(parameterValue)`.
2. **Send IPC Command**: 
   - Something like `{"action":"applyEffect","payload":{"effectId":"xxx","data":...}}`.
   - In real usage, streaming entire audio buffers over IPC might be too big. This is often not practical for real-time. However, to prove the concept, do a small test to see how you'd pass parameters for effect control.  
   - Alternatively, store effect parameters in the plugin process and do actual audio processing in the plugin process if you can feed it audio. For real-time, you might store user effect parameters only out-of-process, and the main app still does the PCM rendering. The simplest demonstration is “set reverb intensity,” “turn vinyl crackle on,” etc.

3. **Handle in PluginHost**:
   - On `applyEffect`, find the effect instance in `_activeEffects`. Possibly call a method on it. Return success.

4. **Verification / Testing**:
   - Write a small test that sets some effect parameters or calls a method on the effect. Check the plugin host logs.
   - Confirm the out-of-process call is working.  

5. **Git Commit**:
   - Commit with message: “Implement rudimentary effect method invocation over IPC.”

---

## Chunk 7: Sandbox the Plugin Host Process (Windows / Linux / macOS Variation)

### Purpose
1. Enhance security by restricting the plugin host process privileges using OS-level means.
2. Each OS has different approaches, but you’ll unify them at a conceptual level.

### Steps
1. **Windows**:
   - Investigate launching the process with a restricted token or an AppContainer. A minimal approach:  
     ```csharp
     // Harder in pure C#: might use P/Invoke to create a restricted token, then pass to StartInfo
     // Or rely on packaging with MSIX for AppContainer. 
     // Alternatively, at least separate user account or something like 
     // "runas /user:lofiSandboxUser" might be used in dev environments.
     ```
   - Decide which is viable for your environment.

2. **Linux**:
   - You can run the plugin host as a separate user with fewer privileges. E.g., `setuid` to `lofiSandbox` user.  
   - Possibly use cgroups or a container approach to further isolate the plugin’s CPU/memory usage.

3. **macOS**:
   - Similar approach to Linux. Possibly run as another user or use macOS sandboxing entitlements if distributing in an app container.

4. **Integration**:
   - Add code in `PluginManager` (or a small utility class) that spawns the plugin host with the restricted environment. 
   - For Windows, you might end up with special instructions for advanced setups; you can log a warning if not possible.

5. **Verification / Testing**:
   - Confirm you can still spawn the process. 
   - On Windows, test that plugin host has limited filesystem access. On Linux/macOS, test that the user permissions are indeed restricted.

6. **Git Commit**:
   - Commit with message: “Add OS-level sandbox approach for plugin host processes.”

---

## Chunk 8: Final Integration & Cleanup

### Purpose
1. Make the entire user workflow consistent. 
2. Ensure the plugin system “just works” for enabling/disabling out-of-process mode.

### Steps
1. **Configuration**:
   - Add a config switch `PluginSettings:RunOutOfProcess = true/false` in `appsettings.json` or `LofiBeats.Core` config. 
   - If `RunOutOfProcess == false`, revert to the old in-process approach for dev / debugging. If `true`, do the ephemeral process approach.

2. **Exception Handling**:
   - Ensure that if the plugin host fails or crashes, `PluginManager` can handle the error gracefully (log error, optionally try restarting, or show an error in CLI).

3. **Clean Up**:
   - On LofiBeats shutdown, kill all plugin host processes (or request them to shut down). 
   - Make sure no orphaned processes remain. Possibly track each spawned process in a dictionary and call `.Kill()` or send a “shutdown” command at the end.

4. **Verification / Testing**:
   - Full test run:  
     1. Start LofiBeats with `RunOutOfProcess = true`.  
     2. Load a known plugin. Confirm initialization.  
     3. Perform some effect calls or metadata fetch.  
     4. Stop LofiBeats. Confirm that no plugin processes remain.  
   - Run on all 3 OS targets, verify no major differences.

5. **Git Commit**:
   - Commit final “Out-of-process plugin execution integrated” changes.

---

## Final Check

When you’ve completed all chunks, you’ll have:

1. A **`PluginHost`** project that loads the plugin assemblies in an isolated process.
2. An **IPC-based** approach for commands and data.
3. **`PluginManager`** (in LofiBeats) that spawns and manages the plugin host processes as proxies for effect creation and usage.
4. Optional **sandboxing** per OS for stronger security.
5. A **toggle** between out-of-process (secure) vs. in-process (debug) plugin execution.

By verifying each chunk individually and committing, you ensure a stable, incremental path to fully secure plugin execution in LofiBeats.