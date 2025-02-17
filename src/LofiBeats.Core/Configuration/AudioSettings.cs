namespace LofiBeats.Core.Configuration;

public class AudioSettings
{
    public int DefaultBPM { get; set; } = 80;
    public int SampleRate { get; set; } = 44100;
    public int Channels { get; set; } = 2;
    public EffectSettings Effects { get; set; } = new();
}

public class EffectSettings
{
    public VinylCrackleSettings VinylCrackle { get; set; } = new();
    public LowPassSettings LowPass { get; set; } = new();
}

public class VinylCrackleSettings
{
    public double Frequency { get; set; } = 0.0005;
    public double Amplitude { get; set; } = 0.2;
}

public class LowPassSettings
{
    public float CutoffFrequency { get; set; } = 2000;
    public float Resonance { get; set; } = 1.0f;
} 