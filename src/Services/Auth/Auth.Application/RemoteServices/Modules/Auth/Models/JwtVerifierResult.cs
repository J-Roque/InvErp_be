namespace Auth.Application.RemoteServices.Modules.Auth.Models;

public class ProfileDtoResult
{
    public long Id { get; set; }
    public string Name { get; set; } = "";
}
public class RoleDtoResult
{
    public long Id { get; set; }
    public string Name { get; set; } = "";
}
public class JwtVerifierResult
{
    public bool IsValid { get; set; }
    public string Message { get; set; } = "";
    public long? UserId { get; set; }
    public string? Email { get; set; }
    public List<RoleDtoResult>? Roles { get; set; }
    public ProfileDtoResult? Profile { get; set; }
    public string? Ip { get; set; }
    public long? TokenId { get; set; }

}
