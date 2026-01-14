namespace BuildingBlocks.Exceptions;

public class NotFoundException : Exception
{
    public string? CustomMessage { get; }

    public NotFoundException(string message) : base(message)
    {
        CustomMessage = message;
    }

    public NotFoundException(string message, string customMessage) : base(message)
    {
        CustomMessage = customMessage;
    }

    public NotFoundException(string name, object key) : base($"Entity \"{name}\" ({key}) was not found.")
    {
        CustomMessage = $"Entity \"{name}\" ({key}) was not found.";
    }
}
