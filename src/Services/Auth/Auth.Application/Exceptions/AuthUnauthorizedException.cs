using BuildingBlocks.Exceptions;

namespace Auth.Application.Exceptions;

public class AuthUnauthorizedException : UnauthorizedException
{
    public AuthUnauthorizedException(string message) : base(message)
    {
    }
}
