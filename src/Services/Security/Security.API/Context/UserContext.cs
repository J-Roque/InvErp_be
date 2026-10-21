using System.Security.Claims;
using Common.Constants.Claims;
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

        // Datos del usuario autenticado: los claims los crea JwtBearerConfiguration
        // con la respuesta del verificador de Auth. Sin sesión, todo queda en 0/vacío.
        var user = context.User;

        UserId = GetLong(user, ClaimConstants.UserId);
        ProfileId = GetLong(user, ClaimConstants.ProfileId);
        Roles = user.FindAll(ClaimConstants.RoleId)
            .Select(c => long.TryParse(c.Value, out var roleId) ? roleId : 0)
            .Where(roleId => roleId > 0)
            .ToList();
        Email = NullIfEmpty(user.FindFirst(ClaimConstants.Email)?.Value);
        Ip = context.Connection.RemoteIpAddress?.ToString();
        TokenId = GetLong(user, ClaimConstants.TokenId);
    }

    private static long GetLong(ClaimsPrincipal user, string claimType) =>
        long.TryParse(user.FindFirst(claimType)?.Value, out var value) ? value : 0;

    private static string? NullIfEmpty(string? value) => string.IsNullOrWhiteSpace(value) ? null : value;

    public UserContextDto ToDto()
    {
        return new UserContextDto(UserId, ProfileId, Roles, Email, Ip, TokenId);
    }
}
