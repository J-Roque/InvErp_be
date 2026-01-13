namespace Jwt.Abstractions;

public interface IJwtManager
{
    string GenerateJwt(string secret, object payload);
    T? GetContent<T>(string secret, string token) where T : class, new();
}
