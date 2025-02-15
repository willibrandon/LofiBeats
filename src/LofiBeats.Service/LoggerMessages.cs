namespace LofiBeats.Service;

public static class LoggerMessages
{
    public static readonly Action<ILogger, bool, string, Exception?> LogTelemetryConfig = LoggerMessage.Define<bool, string>(
        LogLevel.Information,
        new EventId(1, nameof(LogTelemetryConfig)),
        "Telemetry Configuration: EnableSeq={EnableSeq}, SeqServerUrl={SeqServerUrl}");

    public static readonly Action<ILogger, Exception?> LogNoTelemetryConfig = LoggerMessage.Define(
        LogLevel.Warning,
        new EventId(2, nameof(LogNoTelemetryConfig)),
        "No telemetry configuration found in appsettings.json, using defaults");

    public static readonly Action<ILogger, string?, Exception?> LogUnhandledException = LoggerMessage.Define<string?>(
        LogLevel.Critical,
        new EventId(3, nameof(LogUnhandledException)),
        "Unhandled exception occurred: {Message}");
}
