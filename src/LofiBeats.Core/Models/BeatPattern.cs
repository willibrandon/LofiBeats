namespace LofiBeats.Core.Models;

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

    public override string ToString()
    {
        var userSamples = string.Join(", ", UserSampleSteps.Select(kvp => $"Step {kvp.Key}: {kvp.Value}"));
        return $"Tempo: {BPM} BPM, Drums: [{string.Join(", ", DrumSequence)}], " +
               $"Chords: [{string.Join(", ", ChordProgression)}], " +
               $"User Samples: [{userSamples}]";
    }
} 