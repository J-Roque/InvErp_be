using Security.Application.Dtos.General;
using Security.Application.Interfaces.Context;

namespace Security.API.Context;

public class UserContext : IUserContext
{
    public long UserId { get; }
    public long ProfileId { get; }
    public List<long> Roles { get; }
    public string? Email { get; }
    public string? Ip { get; }
    public long? TokenId { get; }

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        var context = httpContextAccessor.HttpContext;

        if (context == null)
        {
            UserId = 0;
            ProfileId = 0;
            Roles = [];
            Email = null;
            Ip = null;
            TokenId = 0;
            return;
        }

        // Por ahora, valores por defecto para el seed
        // TODO: Implementar extracción de claims del JWT cuando se habilite autenticación
        UserId = 0;
        ProfileId = 1;
        Roles = [1];
        Email = "system@system.com";
        Ip = context.Connection.RemoteIpAddress?.ToString();
        TokenId = 0;
    }

    public UserContextDto ToDto()
    {
        return new UserContextDto(UserId, ProfileId, Roles, Email, Ip, TokenId);
    }
}
