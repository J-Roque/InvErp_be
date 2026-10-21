using Auth.Application.Configuration;
using Auth.Application.Data;
using Auth.Application.Interfaces.Persistence;
using Auth.Domain.Enums;
using Jwt.Model;
using Jwt.Services;
using Microsoft.Extensions.Logging;

namespace Auth.Application.Handler.Auth.ValidatePrivateJwt;

/// <summary>
/// Valida el JWT de sesión que otros microservicios reciben y devuelve los datos del usuario.
/// Solo acepta tokens de login: los de restablecimiento de contraseña se firman con la misma
/// clave y tienen el mismo formato, así que se distinguen por el tipo guardado en la BD.
/// </summary>
public class ValidatePrivateJwtHandler(
    IApplicationDbContext dbContext,
    IUnitOfWork unitOfWork,
    AppConstants appConstants,
    ILogger<ValidatePrivateJwtHandler> logger) :
    ICommandHandler<ValidatePrivateJwtCommand, ValidatePrivateJwtResult>
{
    // El motivo concreto solo va al log: este mensaje puede llegar al cliente final
    private const string InvalidMessage = "La sesión no es válida o ha expirado. Inicie sesión nuevamente.";

    public async Task<ValidatePrivateJwtResult> Handle(ValidatePrivateJwtCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var manager = new JwtManager();
            var payload = manager.GetContent<JwtPayloadModel>(appConstants.Jwt.Secret, request.Jwt);

            // Se verifica la firma del token
            if (payload == null || string.IsNullOrEmpty(payload.Code))
            {
                return Invalid("firma o payload inválido");
            }

            var token = await unitOfWork.Auth.GetJwtDataInfo(payload.Code);

            if (token == null)
            {
                return Invalid("token inexistente");
            }

            if (token.Type != TokenTypes.Login)
            {
                return Invalid($"tipo de token no permitido ({token.Type})", token.Id);
            }

            if (!token.IsActive)
            {
                return Invalid("token inactivo", token.Id);
            }

            var now = DateTime.UtcNow;

            if (token.ExpiresAt < now)
            {
                await dbContext.Tokens
                    .Where(t => t.Id == token.Id)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(t => t.IsActive, false)
                        .SetProperty(t => t.LastModified, now)
                        .SetProperty(t => t.LastModifiedBy, token.UserId), cancellationToken);

                return Invalid("token expirado", token.Id);
            }

            await dbContext.Tokens
                .Where(t => t.Id == token.Id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(t => t.LastUsedAt, now), cancellationToken);

            return new ValidatePrivateJwtResult(
                IsValid: true,
                Message: "Token válido",
                UserId: token.UserId,
                Email: token.Email,
                Roles: token.Roles,
                Profile: token.Profile,
                Ip: token.Ip,
                TokenId: token.Id
            );
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error inesperado al validar el JWT de sesión");
            return Invalid("error inesperado");
        }
    }

    private ValidatePrivateJwtResult Invalid(string reason, long? tokenId = null)
    {
        logger.LogInformation("JWT de sesión rechazado: {Reason} (TokenId {TokenId})", reason, tokenId);

        return new ValidatePrivateJwtResult(
            IsValid: false,
            Message: InvalidMessage,
            UserId: -1,
            Email: "",
            Roles: [],
            Profile: null,
            Ip: "",
            TokenId: 0
        );
    }
}
