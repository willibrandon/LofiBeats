namespace LofiBeats.Core.Models;

/// <summary>
/// Represents a drum pattern with configurable synthesis parameters.
/// </summary>
public class BeatPattern
{
    public int BPM { get; set; }
    public string[] DrumSequence { get; set; } = Array.Empty<string>();
    public string[] ChordProgression { get; set; } = Array.Empty<string>();

    /// <summary>
    /// Maps step indices to user sample names. If a step has a user sample,
    /// it overrides the corresponding DrumSequence entry.
    /// </summary>
    public Dictionary<int, string> UserSampleSteps { get; set; } = new();

    /// <summary>
    /// Synthesis parameters for each drum type.
    /// </summary>
    public Dictionary<string, DrumSynthParams> SynthParams { get; set; } = new()
    {
        ["kick"] = DrumSynthParams.DefaultKick,
        ["snare"] = DrumSynthParams.DefaultSnare,
        ["hat"] = DrumSynthParams.DefaultHiHat,
        ["ohat"] = DrumSynthParams.DefaultOpenHiHat
    };

    /// <summary>
    /// Determines if a step should use a user-supplied sample.
    /// </summary>
    /// <param name="stepIndex">The index of the step to check.</param>
    /// <returns>True if the step has a user sample, false otherwise.</returns>
    public bool HasUserSample(int stepIndex) => UserSampleSteps.ContainsKey(stepIndex);

    /// <summary>
    /// Gets the sample name for a specific step, considering both built-in and user samples.
    /// </summary>
    /// <param name="stepIndex">The index of the step.</param>
    /// <returns>The sample name to use for this step.</returns>
    public string GetSampleForStep(int stepIndex)
    {
        if (UserSampleSteps.TryGetValue(stepIndex, out var userSample))
        {
            return userSample;
        }

        return stepIndex < DrumSequence.Length ? DrumSequence[stepIndex] : "_";
    }

    /// <summary>
    /// Gets the synthesis parameters for a specific drum type.
    /// </summary>
    /// <param name="drumType">The type of drum (e.g., "kick", "snare").</param>
    /// <returns>The synthesis parameters, or default parameters if not configured.</returns>
    public DrumSynthParams GetSynthParams(string drumType)
    {
        return SynthParams.TryGetValue(drumType.ToLower(), out var parameters) 
            ? parameters 
            : DrumSynthParams.Default;
    }

    public override string ToString()
    {
        var userSamples = string.Join(", ", UserSampleSteps.Select(kvp => $"Step {kvp.Key}: {kvp.Value}"));
        return $"Tempo: {BPM} BPM, Drums: [{string.Join(", ", DrumSequence)}], " +
               $"Chords: [{string.Join(", ", ChordProgression)}], " +
               $"User Samples: [{userSamples}]";
    }
}

/// <summary>
/// Parameters for synthesizing drum sounds.
/// </summary>
public record DrumSynthParams
{
    /// <summary>
    /// Attack time in seconds.
    /// </summary>
    public float AttackTime { get; init; }

    /// <summary>
    /// Hold time in seconds.
    /// </summary>
    public float HoldTime { get; init; }

    /// <summary>
    /// Decay time in seconds.
    /// </summary>
    public float DecayTime { get; init; }

    /// <summary>
    /// Starting frequency for frequency sweeps.
    /// </summary>
    public float StartFrequency { get; init; }

    /// <summary>
    /// Ending frequency for frequency sweeps.
    /// </summary>
    public float EndFrequency { get; init; }

    /// <summary>
    /// Noise mix level (0-1).
    /// </summary>
    public float NoiseMix { get; init; }

    /// <summary>
    /// Base amplitude (0-1).
    /// </summary>
    public float Amplitude { get; init; }

    /// <summary>
    /// Default synthesis parameters.
    /// </summary>
    public static DrumSynthParams Default => new()
    {
        AttackTime = 0.005f,
        HoldTime = 0.01f,
        DecayTime = 0.1f,
        StartFrequency = 440f,
        EndFrequency = 220f,
        NoiseMix = 0f,
        Amplitude = 0.8f
    };

    /// <summary>
    /// Default kick drum parameters.
    /// </summary>
    public static DrumSynthParams DefaultKick => new()
    {
        AttackTime = 0.003f,
        HoldTime = 0.01f,
        DecayTime = 0.08f,
        StartFrequency = 120f,
        EndFrequency = 45f,
        NoiseMix = 0f,
        Amplitude = 0.85f
    };

    /// <summary>
    /// Default snare drum parameters.
    /// </summary>
    public static DrumSynthParams DefaultSnare => new()
    {
        AttackTime = 0.002f,
        HoldTime = 0.005f,
        DecayTime = 0.1f,
        StartFrequency = 200f,
        EndFrequency = 160f,
        NoiseMix = 0.7f,
        Amplitude = 0.7f
    };

    /// <summary>
    /// Default closed hi-hat parameters.
    /// </summary>
    public static DrumSynthParams DefaultHiHat => new()
    {
        AttackTime = 0.001f,
        HoldTime = 0.001f,
        DecayTime = 0.05f,
        StartFrequency = 3000f,
        EndFrequency = 4500f,
        NoiseMix = 0.8f,
        Amplitude = 0.45f
    };

    /// <summary>
    /// Default open hi-hat parameters.
    /// </summary>
    public static DrumSynthParams DefaultOpenHiHat => new()
    {
        AttackTime = 0.001f,
        HoldTime = 0.01f,
        DecayTime = 0.3f,
        StartFrequency = 3000f,
        EndFrequency = 4500f,
        NoiseMix = 0.8f,
        Amplitude = 0.45f
    };
} 