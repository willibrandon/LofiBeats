namespace LofiBeats.Core.Telemetry;

/// <summary>
/// Contains constant values for telemetry events and metrics.
/// </summary>
public static class TelemetryConstants
{
    public static class Events
    {
        // Playback Events
        public const string BeatGenerated = "BeatGenerated";
        public const string PlaybackStarted = "PlaybackStarted";
        public const string PlaybackStopped = "PlaybackStopped";
        public const string PlaybackPaused = "PlaybackPaused";
        public const string PlaybackResumed = "PlaybackResumed";
        public const string PlaybackScheduled = "PlaybackScheduled";
        
        // Effect Events
        public const string EffectAdded = "EffectAdded";
        public const string EffectRemoved = "EffectRemoved";
        public const string EffectParameterChanged = "EffectParameterChanged";
        public const string EffectChainModified = "EffectChainModified";
        
        // Application Lifecycle
        public const string ApplicationStarted = "ApplicationStarted";
        public const string ApplicationStopped = "ApplicationStopped";
        public const string ApplicationCrashed = "ApplicationCrashed";
        public const string ApplicationUpdated = "ApplicationUpdated";
        
        // User Interaction
        public const string UserPreferenceChanged = "UserPreferenceChanged";
        public const string CustomBeatPatternCreated = "CustomBeatPatternCreated";
        public const string CustomBeatPatternLoaded = "CustomBeatPatternLoaded";
        public const string PresetLoaded = "PresetLoaded";
        public const string PresetSaved = "PresetSaved";
        
        // Audio Device Events
        public const string AudioDeviceChanged = "AudioDeviceChanged";
        public const string AudioDeviceError = "AudioDeviceError";
        public const string AudioBufferUnderrun = "AudioBufferUnderrun";
        public const string AudioLatencySpike = "AudioLatencySpike";
    }

    public static class Metrics
    {
        // Performance Metrics
        public const string BeatGenerationTime = "BeatGeneration.Duration";
        public const string AudioBufferUsage = "Audio.BufferUsage";
        public const string EffectProcessingTime = "Effect.ProcessingTime";
        public const string AudioLatency = "Audio.Latency";
        public const string AudioProcessingLoad = "Audio.ProcessingLoad";
        
        // System Resource Metrics
        public const string MemoryUsage = "System.MemoryUsage";
        public const string CpuUsage = "System.CpuUsage";
        public const string DiskUsage = "System.DiskUsage";
        public const string ThreadPoolUsage = "System.ThreadPoolUsage";
        
        // Session Metrics
        public const string SessionDuration = "Session.Duration";
        public const string ActivePlaybackTime = "Session.ActivePlaybackTime";
        public const string EffectUsageTime = "Session.EffectUsageTime";
        public const string UserInteractionFrequency = "Session.UserInteractionRate";
        
        // Audio Quality Metrics
        public const string AudioClippingCount = "Audio.ClippingCount";
        public const string SignalToNoiseRatio = "Audio.SignalToNoiseRatio";
        public const string PeakAmplitude = "Audio.PeakAmplitude";
        public const string RmsLevel = "Audio.RmsLevel";
        
        // Beat Generation Metrics
        public const string BeatComplexityScore = "Beat.ComplexityScore";
        public const string BeatVariationRate = "Beat.VariationRate";
        public const string BeatSyncAccuracy = "Beat.SyncAccuracy";
        public const string PatternRepetitionRate = "Beat.PatternRepetitionRate";
    }

    public static class Properties
    {
        // Beat Properties
        public const string BeatStyle = "BeatStyle";
        public const string BeatTempo = "BeatTempo";
        public const string BeatKey = "BeatKey";
        public const string BeatComplexity = "BeatComplexity";
        
        // Effect Properties
        public const string EffectName = "EffectName";
        public const string EffectParameters = "EffectParameters";
        public const string EffectChainPosition = "EffectChainPosition";
        public const string EffectPresetName = "EffectPresetName";
        
        // Audio Properties
        public const string AudioFormat = "AudioFormat";
        public const string SampleRate = "SampleRate";
        public const string BitDepth = "BitDepth";
        public const string ChannelCount = "ChannelCount";
        public const string BufferSize = "BufferSize";
        
        // Device Properties
        public const string DeviceName = "DeviceName";
        public const string DeviceType = "DeviceType";
        public const string DeviceCapabilities = "DeviceCapabilities";
        public const string DriverVersion = "DriverVersion";
        
        // System Properties
        public const string OsVersion = "OsVersion";
        public const string AppVersion = "AppVersion";
        public const string RuntimeVersion = "RuntimeVersion";
        public const string ProcessorArchitecture = "ProcessorArchitecture";
        
        // User Properties
        public const string UserLocale = "UserLocale";
        public const string UserTimeZone = "UserTimeZone";
        public const string PreferredBeatStyle = "PreferredBeatStyle";
        public const string PreferredEffects = "PreferredEffects";
        
        // Playback Properties
        public const string StopDelay = "StopDelay";
        public const string UseEffect = "UseEffect";
    }
} 