namespace Auth.Application.Handler.Auth.Logout;

public record LogoutCommand(string? Jwt) : ICommand<LogoutResult>;

public record LogoutResult(bool IsSuccess);
