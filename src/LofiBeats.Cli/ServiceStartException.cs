namespace LofiBeats.Cli;

public class ServiceStartException(string message, Exception? innerException = null)
    : Exception(message, innerException)
{
}