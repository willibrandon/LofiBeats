namespace LofiBeats.Core.Telemetry;

/// <summary>
/// Contains constant values for telemetry events and metrics.
/// </summary>
public static class TelemetryConstants
{
    public static class Events
    {
        public const string BeatGenerated = "BeatGenerated";
        public const string PlaybackStarted = "PlaybackStarted";
        public const string PlaybackStopped = "PlaybackStopped";
        public const string PlaybackPaused = "PlaybackPaused";
        public const string PlaybackResumed = "PlaybackResumed";
        public const string EffectAdded = "EffectAdded";
        public const string EffectRemoved = "EffectRemoved";
        public const string ApplicationStarted = "ApplicationStarted";
        public const string ApplicationStopped = "ApplicationStopped";
    }

    public static class Metrics
    {
        public const string BeatGenerationTime = "BeatGeneration.Duration";
        public const string AudioBufferUsage = "Audio.BufferUsage";
        public const string EffectProcessingTime = "Effect.ProcessingTime";
        public const string MemoryUsage = "System.MemoryUsage";
        public const string CpuUsage = "System.CpuUsage";
        public const string SessionDuration = "Session.Duration";
    }

    public static class Properties
    {
        public const string BeatStyle = "BeatStyle";
        public const string EffectName = "EffectName";
        public const string EffectParameters = "EffectParameters";
        public const string AudioFormat = "AudioFormat";
        public const string DeviceName = "DeviceName";
        public const string OsVersion = "OsVersion";
        public const string AppVersion = "AppVersion";
    }
} 