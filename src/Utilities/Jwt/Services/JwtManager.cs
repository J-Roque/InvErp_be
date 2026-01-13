using Jwt.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Jwt.Services;

public class JwtManager : IJwtManager
{
    public string GenerateJwt(string secret, object payload)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var payloadDictionary = ConvertToDictionary(payload);

        var claims = payloadDictionary.Select(kvp => new Claim(kvp.Key, kvp.Value?.ToString() ?? string.Empty));

        var header = new JwtHeader(credentials);
        var jwtPayload = new JwtPayload(claims);

        var token = new JwtSecurityToken(header, jwtPayload);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }


    public T? GetContent<T>(string secret, string token) where T : class, new()
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
        };

        try
        {
            tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            if (validatedToken is not JwtSecurityToken jwtToken) throw new SecurityTokenException("Invalid token");

            var payload = jwtToken.Claims.ToDictionary(c => c.Type, c => c.Value);
            return MapToClass<T>(payload);
        }
        catch
        {
            return null;
        }
    }

    private static Dictionary<string, object?> ConvertToDictionary(object obj)
    {
        return obj.GetType()
            .GetProperties()
            .ToDictionary(prop => prop.Name, prop => prop.GetValue(obj));
    }

    private static T MapToClass<T>(Dictionary<string, string> payload) where T : class, new()
    {
        var obj = new T();
        var objType = typeof(T);

        foreach (var kvp in payload)
        {
            var property = objType.GetProperty(kvp.Key);
            if (property == null || !property.CanWrite) continue;
            var value = Convert.ChangeType(kvp.Value, property.PropertyType);
            property.SetValue(obj, value);
        }

        return obj;
    }
}
