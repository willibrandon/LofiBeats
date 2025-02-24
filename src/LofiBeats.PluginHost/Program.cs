using System.CommandLine;
using System.Text.Json;
using System.Reflection;
using LofiBeats.Core.PluginApi;
using LofiBeats.PluginHost.Models;

namespace LofiBeats.PluginHost;

public class Program
{
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
                var directory = Path.GetDirectoryName(pluginAssemblyPath);
                if (!string.IsNullOrEmpty(directory) && Directory.Exists(directory))
                {
                    Console.WriteLine($"Searched in directory: {directory}");
                    Console.WriteLine($"Directory contents: {string.Join(", ", Directory.GetFiles(directory))}");
                }
                else
                {
                    Console.WriteLine($"Directory does not exist: {directory}");
                }
                await Console.Out.FlushAsync();
            }
            else
            {
                // Load the plugin assembly
                try
                {
                    var assembly = Assembly.LoadFrom(pluginAssemblyPath);
                    var effectTypes = assembly.GetTypes()
                        .Where(t => !t.IsAbstract && typeof(IAudioEffect).IsAssignableFrom(t));

                    foreach (var type in effectTypes)
                    {
                        try
                        {
                            if (Activator.CreateInstance(type) is IAudioEffect instance)
                            {
                                _effectTypes[instance.Name.ToLowerInvariant()] = type;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[ERROR] Failed to instantiate effect type {type.FullName}: {ex.Message}");
                        }
                    }

                    Console.WriteLine($"[DEBUG] Loaded {_effectTypes.Count} effect type(s)");
                    Console.WriteLine($"[DEBUG] Available effects: {string.Join(", ", _effectTypes.Keys)}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Failed to load plugin assembly: {ex.Message}");
                    Console.WriteLine($"[ERROR] Stack trace: {ex.StackTrace}");
                }
            }

            // Send startup success message - we continue even if plugin not found or failed to load
            Console.WriteLine("[DEBUG] PluginHost started");
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
            Console.WriteLine($"[ERROR] {ex.Message}");
            Console.WriteLine($"[ERROR] Stack trace: {ex.StackTrace}");
            await Console.Out.FlushAsync();
            Environment.Exit(1);
        }
    }

    private static PluginResponse HandleInit(JsonElement? payload)
    {
        // For initial handshake, return the list of available effects
        var effects = _effectTypes.Values.Select(type =>
        {
            var instance = Activator.CreateInstance(type) as IAudioEffect;
            return new
            {
                name = instance?.Name.ToLowerInvariant(),
                description = instance?.Description,
                version = instance?.Version,
                author = instance?.Author
            };
        }).ToList();

        return new PluginResponse
        {
            Status = "ok",
            Message = "Plugin host initialized",
            Payload = JsonDocument.Parse(JsonSerializer.Serialize(new { effects })).RootElement
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
        await Console.Out.WriteLineAsync($"[RESPONSE] {json}");
        await Console.Out.FlushAsync();
    }
}
