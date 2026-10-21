using Auth.Application.Configuration;
using Auth.Application.Data;
using Auth.Application.Exceptions;
using Auth.Application.Interfaces.Persistence;
using Auth.Domain.Enums;
using Crypto.Services;
using Jwt.Model;
using Jwt.Services;

namespace Auth.Application.Handler.Auth.ChangeUserPassword;

public class ChangeUserPasswordHandler(IApplicationDbContext dbContext, IUnitOfWork unitOfWork, AppConstants appConstants) :
    ICommandHandler<ChangeUserPasswordCommand, ChangeUserPasswordResult>
{
    public async Task<ChangeUserPasswordResult> Handle(ChangeUserPasswordCommand command,
        CancellationToken cancellationToken)
    {
        var jwt = command.Reset.ResetJwt;
        var jwtManager = new JwtManager();
        var superPasswordJwt = appConstants.Jwt.Secret;
        var payload = jwtManager.GetContent<JwtPayloadModel>(superPasswordJwt, jwt);

        // Se verifica si el código es válido
        if (payload == null)
        {
            throw new AuthUnauthorizedException("payload is null", "El token de restablecimiento de contraseña no es válido");
        }

        var code = payload.Code;

        // Se verifica si el token existe
        var existingToken = await dbContext.Tokens
            .Where(t => t.Code == code && t.Type == TokenTypes.PasswordReset)
            .OrderByDescending(t => t.GeneratedAt)
            .FirstOrDefaultAsync(cancellationToken);

        if (existingToken == null)
        {
            throw new AuthUnauthorizedException(
                "existingToken is null",
                "El token de restablecimiento de contraseña no es válido o ha expirado");
        }

        // Se verifica si el token aún es válido
        if (!existingToken.IsActive)
        {
            throw new AuthUnauthorizedException(
                "existingToken is not active",
                "El token de restablecimiento de contraseña no está activo");
        }

        // Se verifica si el token ya expiró
        if (existingToken.ExpiresAt < DateTime.UtcNow)
        {
            existingToken.IsActive = false;
            dbContext.Tokens.Update(existingToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            throw new AuthUnauthorizedException(
                "existingToken has expired",
                "El token de restablecimiento de contraseña ha expirado");
        }

        // Se encripta la nueva contraseña
        var password = command.Reset.NewPassword;
        var hasher = new PasswordHasher();
        var (hash, salt) = hasher.Hash(password);

        var isSuccess = await unitOfWork.Auth.ChangePassword(
            userId: existingToken.UserId,
            newPassword: hash,
            salt: salt,
            tokenCode: code);

        return new ChangeUserPasswordResult(isSuccess);
    }
}
