namespace BuildingBlocks.Exceptions;

public class ForbiddenException : Exception
{
    public string? CustomMessage { get; }

    public ForbiddenException(string message) : base(message)
    {
        CustomMessage = message;
    }

    public ForbiddenException(string message, string customMessage) : base(message)
    {
        CustomMessage = customMessage;
    }
}
