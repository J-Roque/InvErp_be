using BuildingBlocks.Exceptions;

namespace Security.Application.Exceptions;

public class ProfileNotFoundException : NotFoundException
{
    public ProfileNotFoundException(long Id, string? customMessage) : base("Id", Id, customMessage)
    {
    }
}