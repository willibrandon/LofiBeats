namespace LofiBeats.Cli.Commands;

public record ApiResponse
{
    public string? Message { get; init; }
    public string? Error { get; init; }
}

public record PlayResponse : ApiResponse
{
    public object? Pattern { get; init; }
} 