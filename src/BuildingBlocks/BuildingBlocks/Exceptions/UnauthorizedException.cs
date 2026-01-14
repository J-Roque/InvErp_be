namespace BuildingBlocks.Exceptions;

public class UnauthorizedException : Exception
{
    public string? CustomMessage { get; }

    public UnauthorizedException(string message) : base(message)
    {
        CustomMessage = message;
    }

    public UnauthorizedException(string message, string customMessage) : base(message)
    {
        CustomMessage = customMessage;
    }
}
