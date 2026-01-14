namespace BuildingBlocks.Exceptions;

public class BadRequestException : Exception
{
    public string? CustomMessage { get; }

    public BadRequestException(string message) : base(message)
    {
        CustomMessage = message;
    }

    public BadRequestException(string message, string customMessage) : base(message)
    {
        CustomMessage = customMessage;
    }
}
