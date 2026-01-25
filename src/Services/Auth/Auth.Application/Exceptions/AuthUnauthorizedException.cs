using BuildingBlocks.Exceptions;

namespace Auth.Application.Exceptions;

public class AuthUnauthorizedException : UnauthorizedException
{
    public string? CustomMessage { get; }
    public AuthUnauthorizedException(string message) : base(message)
    {
    }
    public AuthUnauthorizedException(string details, string? customMessage = null) : base(details, customMessage)
    {
        CustomMessage = customMessage;
    }
}
