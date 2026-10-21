
using Auth.Application.Configuration;
using Auth.Application.Data;
using Auth.Application.Dtos.Result;
using Auth.Application.Exceptions;
using Auth.Application.Interfaces.Persistence;
using Auth.Application.RemoteServices.Context;
using Auth.Application.RemoteServices.Modules.Messaging.Models;
using Auth.Domain.Enums;
using BuildingBlocks.Exceptions;
using Jwt.Model;
using Jwt.Services;
using Mail.Constants;
using Mail.Models;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Auth.Application.Handler.Auth.PasswordResetRequest;

public class PasswordResetRequestHandler(IApplicationDbContext dbContext, IUnitOfWork unitOfWork, IRemoteServiceContext remoteServiceContext, AppConstants appConstants, ILogger<PasswordResetRequestHandler> logger)
    : ICommandHandler<PasswordResetRequestCommand, PasswordResetRequestResult>
{
    // Misma respuesta exista o no la cuenta, para no revelar qué usuarios o correos están registrados
    private const string GenericMessage =
        "Si los datos corresponden a una cuenta registrada, recibirás en tu correo electrónico un enlace para restablecer tu contraseña.";

    private const int ResetTokenExpirationMinutes = 60;

    // Tiempo mínimo entre envíos a un mismo usuario, para evitar llenarle la bandeja de correos
    private static readonly TimeSpan ResendCooldown = TimeSpan.FromMinutes(2);

    public async Task<PasswordResetRequestResult> Handle(PasswordResetRequestCommand command,
        CancellationToken cancellationToken)
    {
        var user = await unitOfWork.Auth.SearchUserForLogin(command.Reset.Value);

        if (user == null || string.IsNullOrWhiteSpace(user.Email))
        {
            logger.LogWarning("Solicitud de restablecimiento para una cuenta inexistente o sin email (tipo {Type})",
                command.Reset.Type);
            return GenericResult();
        }

        var now = DateTime.UtcNow;
        var activeTokens = await dbContext.Tokens
            .Where(t => t.UserId == user.UserId && t.Type == TokenTypes.PasswordReset && t.IsActive)
            .ToListAsync(cancellationToken);

        if (activeTokens.Any(t => t.ExpiresAt >= now && t.GeneratedAt > now - ResendCooldown))
        {
            logger.LogInformation("Restablecimiento solicitado de nuevo antes del tiempo mínimo para el usuario {UserId}",
                user.UserId);
            return GenericResult();
        }

        // Solo el último enlace enviado queda válido
        foreach (var token in activeTokens)
        {
            token.IsActive = false;
        }

        var code = Guid.NewGuid().ToString();
        var payload = new JwtPayloadModel()
        {
            Code = code
        };

        var jwtManager = new JwtManager();
        var superPasswordJwt = appConstants.Jwt.Secret;
        var jwt = jwtManager.GenerateJwt(superPasswordJwt, payload);

        var newToken = Token.Create(
            userId: user.UserId,
            code: code,
            type: TokenTypes.PasswordReset,
            generatedAt: now,
            expiresAt: now.AddMinutes(ResetTokenExpirationMinutes),
            lastUsedAt: now,
            ip: null,
            device: null,
            userAgent: null
        );

        await dbContext.Tokens.AddAsync(newToken, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var emailSaved = await SaveEmail(user.Username, jwt, user.Email);

        // Si el correo no se pudo registrar, se desactiva el token para que el usuario pueda reintentar
        if (!emailSaved)
        {
            newToken.IsActive = false;
            dbContext.Tokens.Update(newToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            throw new InternalServerException(
                "SaveEmailToSend failed",
                "No se pudo enviar el enlace de restablecimiento de contraseña. Por favor, intente nuevamente.");
        }

        return GenericResult();
    }

    private static PasswordResetRequestResult GenericResult() =>
        new(new ResetRequestMessage(GenericMessage, null));

    private async Task<bool> SaveEmail(string username, string jwt, string email)
    {
        try
        {
            var model = new ChangePasswordRequestModel()
            {
                Username = username,
                RequestDate = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss"),
                Jwt = jwt,
                ResetUrl = $"{appConstants.Frontend.BaseUrl}/login/reset-password?token={jwt}"
            };

            var modelStr = JsonSerializer.Serialize(model);

            var body = new EmailToSendBody()
            {
                Subject = Subjects.ChangePasswordRequestSubject,
                Template = TemplateTypes.ChangePasswordRequest,
                Data = modelStr,
                Recipients = email,
                Priority = 1,
                Bcc = "",
                Cc = ""
            };

            var newEmail = await remoteServiceContext.Messaging.SaveEmailToSend(body);

            if (newEmail is { Id: > 0 })
            {
                logger.LogInformation("Email de solicitud de cambio de contraseña preparado (Id {EmailId})", newEmail.Id);
                return true;
            }

            logger.LogError("Messaging no registró el email de solicitud de cambio de contraseña para {Username}", username);
            return false;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error al guardar email de solicitud de cambio de contraseña para {Username}", username);
            return false;
        }
    }

}
