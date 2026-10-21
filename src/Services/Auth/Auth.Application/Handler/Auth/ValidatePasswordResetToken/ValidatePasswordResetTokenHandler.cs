using Auth.Application.Configuration;
using Auth.Application.Data;
using Auth.Application.Dtos.Result;
using Auth.Domain.Enums;
using Jwt.Model;
using Jwt.Services;

namespace Auth.Application.Handler.Auth.ValidatePasswordResetToken;

/// <summary>
/// Indica si un enlace de restablecimiento de contraseña todavía puede usarse.
/// Es de solo lectura y responde lo mismo para cualquier token no válido
/// (firma incorrecta, inexistente, usado o expirado) para no revelar su estado.
/// </summary>
public class ValidatePasswordResetTokenHandler(IApplicationDbContext dbContext, AppConstants appConstants)
    : ICommandHandler<ValidatePasswordResetTokenCommand, ValidatePasswordResetTokenResult>
{
    private const string InvalidMessage =
        "El enlace de restablecimiento de contraseña no es válido o ha expirado. Solicite uno nuevo.";

    private const string ValidMessage = "El enlace de restablecimiento de contraseña es válido";

    public async Task<ValidatePasswordResetTokenResult> Handle(ValidatePasswordResetTokenCommand command,
        CancellationToken cancellationToken)
    {
        var jwtManager = new JwtManager();
        var payload = jwtManager.GetContent<JwtPayloadModel>(appConstants.Jwt.Secret, command.Reset.ResetJwt);

        // Se verifica la firma del token
        if (payload == null || string.IsNullOrEmpty(payload.Code))
        {
            return Invalid();
        }

        // Se verifica que el token exista, siga activo y no haya expirado
        var now = DateTime.UtcNow;
        var isValid = await dbContext.Tokens
            .AsNoTracking()
            .AnyAsync(t => t.Code == payload.Code
                           && t.Type == TokenTypes.PasswordReset
                           && t.IsActive
                           && t.ExpiresAt >= now, cancellationToken);

        return isValid
            ? new ValidatePasswordResetTokenResult(new ValidatePasswordResetTokenMessage(true, ValidMessage))
            : Invalid();
    }

    private static ValidatePasswordResetTokenResult Invalid() =>
        new(new ValidatePasswordResetTokenMessage(false, InvalidMessage));
}
