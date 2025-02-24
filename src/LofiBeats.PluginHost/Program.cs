using System.CommandLine;
using System.Text.Json;
using System.Reflection;
using LofiBeats.Core.PluginApi;
using LofiBeats.PluginHost.Models;

namespace LofiBeats.PluginHost;

public class Program
{
    private static Assembly? _pluginAssembly;
    private static readonly Dictionary<string, Type> _effectTypes = [];
    private static readonly Dictionary<string, IAudioEffect> _activeEffects = [];

    public static async Task<int> Main(string[] args)
    {
        var pluginAssemblyOption = new Option<string>(
            name: "--plugin-assembly",
            description: "Path to the plugin assembly to load"
        );

        var rootCommand = new RootCommand("LofiBeats Plugin Host - Runs plugins in an isolated process")
        {
            pluginAssemblyOption
        };

        rootCommand.SetHandler(async (string pluginAssembly) =>
        {
            await RunPluginHost(pluginAssembly);
        }, pluginAssemblyOption);

        return await rootCommand.InvokeAsync(args);
    }

    private static async Task RunPluginHost(string pluginAssemblyPath)
    {
        try
        {
            Console.WriteLine($"Starting plugin host with assembly: {pluginAssemblyPath}");
            Console.WriteLine($"Current directory: {Environment.CurrentDirectory}");
            Console.WriteLine($"Base directory: {AppContext.BaseDirectory}");
            await Console.Out.FlushAsync();

            // Verify plugin assembly path exists
            if (!File.Exists(pluginAssemblyPath))
            {
                Console.WriteLine($"Error: Plugin assembly not found at {pluginAssemblyPath}");
                Console.WriteLine($"Searched in directory: {Path.GetDirectoryName(pluginAssemblyPath)}");
                Console.WriteLine($"Directory contents: {string.Join(", ", Directory.GetFiles(Path.GetDirectoryName(pluginAssemblyPath) ?? "."))}");
                await Console.Out.FlushAsync();
                Environment.Exit(1);
                return;
            }

            Console.WriteLine($"Found plugin assembly at: {pluginAssemblyPath}");
            Console.WriteLine($"Assembly file size: {new FileInfo(pluginAssemblyPath).Length} bytes");
            await Console.Out.FlushAsync();

            // First ensure we can load the plugin API assembly
            try
            {
                var apiAssembly = typeof(IAudioEffect).Assembly;
                Console.WriteLine($"[DEBUG] Successfully loaded plugin API assembly: {apiAssembly.FullName}");
                Console.WriteLine($"[DEBUG] Plugin API assembly location: {apiAssembly.Location}");
                await Console.Out.FlushAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to load plugin API assembly: {ex.Message}");
                Console.WriteLine($"[ERROR] Stack trace: {ex.StackTrace}");
                await Console.Out.FlushAsync();
                Environment.Exit(1);
                return;
            }

            try
            {
                // Load the plugin assembly first
                _pluginAssembly = Assembly.LoadFrom(pluginAssemblyPath);
                Console.WriteLine($"[DEBUG] Successfully loaded plugin assembly: {_pluginAssembly.FullName}");
                Console.WriteLine($"[DEBUG] Plugin assembly location: {_pluginAssembly.Location}");
                await Console.Out.FlushAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to load plugin assembly: {ex.Message}");
                Console.WriteLine($"[ERROR] Stack trace: {ex.StackTrace}");
                Console.WriteLine($"[ERROR] Assembly file exists: {File.Exists(pluginAssemblyPath)}");
                Console.WriteLine($"[ERROR] Assembly file size: {new FileInfo(pluginAssemblyPath).Length} bytes");
                Console.WriteLine($"[ERROR] Assembly file permissions: {File.GetAttributes(pluginAssemblyPath)}");
                await Console.Out.FlushAsync();
                Environment.Exit(1);
                return;
            }

            // Find effect types
            var effectTypes = new List<Type>();
            try
            {
                Console.WriteLine("[DEBUG] Scanning for effect types...");
                await Console.Out.FlushAsync();

                foreach (var type in _pluginAssembly.GetTypes())
                {
                    Console.WriteLine($"[DEBUG] Found type: {type.FullName}");
                    if (typeof(IAudioEffect).IsAssignableFrom(type) && !type.IsAbstract)
                    {
                        effectTypes.Add(type);
                        Console.WriteLine($"[DEBUG] Found effect type: {type.FullName}");
                        Console.WriteLine($"[DEBUG] Effect type implements IAudioEffect: {typeof(IAudioEffect).IsAssignableFrom(type)}");
                        Console.WriteLine($"[DEBUG] Effect type is abstract: {type.IsAbstract}");
                        await Console.Out.FlushAsync();
                    }
                }

                if (effectTypes.Count == 0)
                {
                    Console.WriteLine("[WARNING] No effect types found in assembly");
                    Console.WriteLine($"[DEBUG] Assembly types: {string.Join(", ", _pluginAssembly.GetTypes().Select(t => t.FullName))}");
                    await Console.Out.FlushAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to enumerate types: {ex.Message}");
                Console.WriteLine($"[ERROR] Stack trace: {ex.StackTrace}");
                await Console.Out.FlushAsync();
                Environment.Exit(1);
                return;
            }

            // Try to instantiate effects
            foreach (var type in effectTypes)
            {
                try
                {
                    Console.WriteLine($"[DEBUG] Attempting to instantiate effect: {type.FullName}");
                    await Console.Out.FlushAsync();

                    if (Activator.CreateInstance(type) is IAudioEffect instance)
                    {
                        _effectTypes[instance.Name.ToLowerInvariant()] = type;
                        Console.WriteLine($"[DEBUG] Successfully instantiated effect: {instance.Name}");
                        Console.WriteLine($"[DEBUG] Effect metadata - Description: {instance.Description}, Version: {instance.Version}, Author: {instance.Author}");
                        await Console.Out.FlushAsync();
                    }
                }
                catch (Exception ex)
                {
                    // Log but continue - we might have other valid effects
                    Console.WriteLine($"[WARNING] Could not instantiate effect type {type.Name}: {ex.Message}");
                    Console.WriteLine($"[WARNING] Stack trace: {ex.StackTrace}");
                    await Console.Out.FlushAsync();
                }
            }

            // Send startup success message
            Console.WriteLine("[STATUS] PluginHost started");
            Console.WriteLine($"[STATUS] Loaded {_effectTypes.Count} effect type(s)");
            Console.WriteLine($"[STATUS] Available effects: {string.Join(", ", _effectTypes.Keys)}");
            await Console.Out.FlushAsync();

            // Set up JSON serializer options
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = false
            };

            // Main message loop
            while (true)
            {
                try
                {
                    // Read a line from STDIN
                    var line = await Console.In.ReadLineAsync();
                    if (line == null)
                    {
                        // EOF - parent process closed the pipe
                        break;
                    }

                    // Parse the message
                    var message = JsonSerializer.Deserialize<PluginMessage>(line, jsonOptions);
                    if (message == null)
                    {
                        await WriteResponse(new PluginResponse
                        {
                            Status = "error",
                            Message = "Invalid message format"
                        }, jsonOptions);
                        continue;
                    }

                    // Handle the message based on action
                    var response = message.Action switch
                    {
                        "init" => HandleInit(message.Payload),
                        "createEffect" => HandleCreateEffect(message.Payload),
                        _ => new PluginResponse
                        {
                            Status = "error",
                            Message = $"Unknown action: {message.Action}"
                        }
                    };

                    await WriteResponse(response, jsonOptions);
                }
                catch (Exception ex)
                {
                    await WriteResponse(new PluginResponse
                    {
                        Status = "error",
                        Message = $"Error processing message: {ex.Message}"
                    }, jsonOptions);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            await Console.Out.FlushAsync();
            Environment.Exit(1);
        }
    }

    private static PluginResponse HandleInit(JsonElement? payload)
    {
        // For initial handshake, we don't need to validate plugin name yet
        return new PluginResponse
        {
            Status = "ok",
            Message = "Plugin initialized"
        };
    }

    private static PluginResponse HandleCreateEffect(JsonElement? payload)
    {
        if (payload == null)
        {
            return new PluginResponse
            {
                Status = "error",
                Message = "Missing payload"
            };
        }

        var effectName = payload.Value.GetProperty("EffectName").GetString();
        if (effectName == null || !_effectTypes.ContainsKey(effectName.ToLowerInvariant()))
        {
            return new PluginResponse
            {
                Status = "error",
                Message = $"Effect {effectName} not found"
            };
        }

        try
        {
            var effectType = _effectTypes[effectName.ToLowerInvariant()];
            var instance = Activator.CreateInstance(effectType) as IAudioEffect;
            if (instance == null)
            {
                return new PluginResponse
                {
                    Status = "error",
                    Message = $"Failed to create effect instance for {effectName}"
                };
            }

            var effectId = Guid.NewGuid().ToString();
            _activeEffects[effectId] = instance;

            return new PluginResponse
            {
                Status = "ok",
                Message = "Effect created",
                Payload = JsonDocument.Parse(JsonSerializer.Serialize(new { effectId })).RootElement
            };
        }
        catch (Exception ex)
        {
            return new PluginResponse
            {
                Status = "error",
                Message = $"Error creating effect: {ex.Message}"
            };
        }
    }

    private static async Task WriteResponse(PluginResponse response, JsonSerializerOptions options)
    {
        var json = JsonSerializer.Serialize(response, options);
        await Console.Out.WriteLineAsync(json);
        await Console.Out.FlushAsync();
    }
}
