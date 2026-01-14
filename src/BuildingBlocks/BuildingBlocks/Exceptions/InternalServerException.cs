namespace BuildingBlocks.Exceptions;

public class InternalServerException : Exception
{
    public string? CustomMessage { get; }

    public InternalServerException(string message) : base(message)
    {
        CustomMessage = message;
    }

    public InternalServerException(string message, string customMessage) : base(message)
    {
        CustomMessage = customMessage;
    }
}
