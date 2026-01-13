namespace Auth.Application.Dtos.Result;

public record LoginAuth(
    long UserId,
    string Username,
    string FullName,
    string Email,
    string Jwt
);
