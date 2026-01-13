using System.Text.Json;
using Auth.Application.Dtos.General;

namespace Auth.Application.Dtos.Result;

public class JwtDataInfoSp
{
    // Datos del Jwt
    public long Id { get; set; }
    public string? Type { get; set; } = "";
    public DateTime ExpiresAt { get; set; }
    public string? Ip { get; set; }
    public bool IsActive { get; set; }

    // Datos del usuario
    public long? UserId { get; set; }
    public string? Email { get; set; }
    public string? Roles { get; set; }
    public string? Profile { get; set; }
}

public class JwtDataInfo : JwtDataInfoSp
{
    public new List<RoleDto>? Roles { get; set; }
    public new ProfileDto? Profile { get; set; }

    public JwtDataInfo(JwtDataInfoSp jwtDataInfo)
    {
        Id = jwtDataInfo.Id;
        Type = jwtDataInfo.Type;
        ExpiresAt = jwtDataInfo.ExpiresAt;
        Ip = jwtDataInfo.Ip;
        IsActive = jwtDataInfo.IsActive;
        UserId = jwtDataInfo.UserId;
        Email = jwtDataInfo.Email;
        Roles = jwtDataInfo.Roles != null
            ? JsonSerializer.Deserialize<List<RoleDto>>(jwtDataInfo.Roles)
            : [];
        Profile = jwtDataInfo.Profile != null
            ? JsonSerializer.Deserialize<ProfileDto>(jwtDataInfo.Profile)
            : null;
    }
}
