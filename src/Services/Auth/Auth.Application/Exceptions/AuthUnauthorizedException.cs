namespace Auth.Application.Exceptions;

public class AuthUnauthorizedException : Exception
{
    public AuthUnauthorizedException(string message) : base(message)
    {
    }
}
