using Microsoft.Extensions.Logging;

namespace LofiBeats.Core.BeatGeneration;

public class BeatGeneratorFactory : IBeatGeneratorFactory
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly Dictionary<string, IBeatGenerator> _generators;

    public string[] GetAvailableStyles() => [.. _generators.Keys];

    public BeatGeneratorFactory(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
        _generators = [];
        InitializeGenerators();
    }

    private void InitializeGenerators()
    {
        // Create instances of each generator
        var generators = new IBeatGenerator[]
        {
            new BasicLofiBeatGenerator(_loggerFactory.CreateLogger<BasicLofiBeatGenerator>()),
            new JazzyBeatGenerator(_loggerFactory.CreateLogger<JazzyBeatGenerator>()),
            new ChillhopBeatGenerator(_loggerFactory.CreateLogger<ChillhopBeatGenerator>()),
            new HipHopBeatGenerator(_loggerFactory.CreateLogger<HipHopBeatGenerator>())
        };

        // Add each generator to the dictionary
        foreach (var generator in generators)
        {
            _generators[generator.Style] = generator;
        }
    }

    public IBeatGenerator GetGenerator(string style)
    {
        style = style.ToLower();
        if (_generators.TryGetValue(style, out var generator))
        {
            return generator;
        }

        // Default to basic if style not found
        return _generators["basic"];
    }
} 