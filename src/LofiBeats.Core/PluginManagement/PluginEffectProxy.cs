using LofiBeats.Core.PluginApi;
using LofiBeats.PluginHost.Models;
using Microsoft.Extensions.Logging;
using NAudio.Wave;
using System.Text.Json;

namespace LofiBeats.Core.PluginManagement;

/// <summary>
/// A proxy class that implements IAudioEffect and forwards calls to a plugin host process.
/// </summary>
public sealed class PluginEffectProxy : IAudioEffect, IDisposable
{
    private readonly IPluginHostConnection _connection;
    private readonly ILogger<PluginEffectProxy> _logger;
    private readonly string _effectId;
    private ISampleProvider _source;
    private bool _isDisposed;

    public string Name { get; }
    public string Description { get; }
    public string Version { get; }
    public string Author { get; }
    public WaveFormat WaveFormat => _source.WaveFormat;

    public PluginEffectProxy(
        string name,
        string description,
        string version,
        string author,
        string effectId,
        ISampleProvider source,
        IPluginHostConnection connection,
        ILogger<PluginEffectProxy> logger)
    {
        Name = name;
        Description = description;
        Version = version;
        Author = author;
        _effectId = effectId;
        _source = source ?? throw new ArgumentNullException(nameof(source));
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void SetSource(ISampleProvider source)
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);
        _source = source ?? throw new ArgumentNullException(nameof(source));

        // Notify plugin host about source change
        var payload = JsonDocument.Parse(JsonSerializer.Serialize(new
        {
            EffectId = _effectId,
            SampleRate = source.WaveFormat.SampleRate,
            Channels = source.WaveFormat.Channels
        })).RootElement;

        _ = _connection.SendMessageAsync<PluginResponse>(new PluginMessage
        {
            Action = "setSource",
            Payload = payload
        });
    }

    public int Read(float[] buffer, int offset, int count)
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);

        // First read from the source
        var samplesRead = _source.Read(buffer, offset, count);

        // Then apply the effect through the plugin host
        ApplyEffect(buffer, offset, samplesRead);

        return samplesRead;
    }

    public void ApplyEffect(float[] buffer, int offset, int count)
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);

        // Send the buffer to the plugin host for processing
        var payload = JsonDocument.Parse(JsonSerializer.Serialize(new
        {
            EffectId = _effectId,
            Buffer = buffer[offset..(offset + count)],
            Offset = 0, // Since we're sending a slice, offset is always 0
            Count = count
        })).RootElement;

        _ = _connection.SendMessageAsync<PluginResponse>(new PluginMessage
        {
            Action = "applyEffect",
            Payload = payload
        });
    }

    public void Dispose()
    {
        if (_isDisposed) return;
        _isDisposed = true;

        try
        {
            // Notify plugin host to dispose the effect
            var payload = JsonDocument.Parse(JsonSerializer.Serialize(new
            {
                EffectId = _effectId
            })).RootElement;

            _ = _connection.SendMessageAsync<PluginResponse>(new PluginMessage
            {
                Action = "disposeEffect",
                Payload = payload
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error disposing plugin effect proxy");
        }
    }
} 