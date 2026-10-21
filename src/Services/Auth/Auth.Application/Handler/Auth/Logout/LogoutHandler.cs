using Auth.Application.Configuration;
using Auth.Application.Data;
using Auth.Domain.Enums;
using Jwt.Model;
using Jwt.Services;
using Microsoft.Extensions.Logging;

namespace Auth.Application.Handler.Auth.Logout;

/// <summary>
/// Cierra la sesión en el servidor desactivando el token de login, para que el JWT
/// deje de ser aceptado aunque alguien lo haya copiado. Es idempotente: con un token
/// inválido, ya cerrado o ausente responde igual y no revela nada.
/// </summary>
public class LogoutHandler(
    IApplicationDbContext dbContext,
    AppConstants appConstants,
    ILogger<LogoutHandler> logger)
    : ICommandHandler<LogoutCommand, LogoutResult>
{
    public async Task<LogoutResult> Handle(LogoutCommand command, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.Jwt))
        {
            return new LogoutResult(true);
        }

        var payload = new JwtManager().GetContent<JwtPayloadModel>(appConstants.Jwt.Secret, command.Jwt);

        if (payload == null || string.IsNullOrEmpty(payload.Code))
        {
            return new LogoutResult(true);
        }

        var now = DateTime.UtcNow;
        var closed = await dbContext.Tokens
            .Where(t => t.Code == payload.Code && t.Type == TokenTypes.Login && t.IsActive)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(t => t.IsActive, false)
                .SetProperty(t => t.LastModified, now), cancellationToken);

        if (closed > 0)
        {
            logger.LogInformation("Sesión cerrada por el usuario");
        }

        return new LogoutResult(true);
    }
}
