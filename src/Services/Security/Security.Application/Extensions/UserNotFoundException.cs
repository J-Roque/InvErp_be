using BuildingBlocks.Exceptions;

namespace Security.Application.Exceptions
{
    public class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException(long Id, string? customMessage) : base("Id", Id, customMessage)
        {
        }
    }
}
