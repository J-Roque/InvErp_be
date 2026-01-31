using BuildingBlocks.Exceptions;

namespace Messaging.Application.Exceptions;

public class EmailToSendNotFoundException : NotFoundException
{
    public EmailToSendNotFoundException(long Id, string? customMessage) : base("Id", Id, customMessage)
    {
    }
}
