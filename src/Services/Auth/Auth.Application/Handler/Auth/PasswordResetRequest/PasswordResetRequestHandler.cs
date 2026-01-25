
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
using System.Text.Json;

namespace Auth.Application.Handler.Auth.PasswordResetRequest;

public class PasswordResetRequestHandler(IApplicationDbContext dbContext, IUnitOfWork unitOfWork, IRemoteServiceContext remoteServiceContext, AppConstants appConstants)
    : ICommandHandler<PasswordResetRequestCommand, PasswordResetRequestResult>
{
    public async Task<PasswordResetRequestResult> Handle(PasswordResetRequestCommand command,
        CancellationToken cancellationToken)
    {
        var type = command.Reset.Type;
        var user = await unitOfWork.Auth.SearchUserForLogin(command.Reset.Value);

        switch(user)
        {
            case null when type == "email":
                throw new AuthUnauthorizedException("User is null", "Email incorrecto o no existe");
            case null when type == "username":
                throw new AuthUnauthorizedException("User is null", "Usuario incorrecto o no existe");
            case null:
                throw new AuthUnauthorizedException("User is null", "Usuario o email incorrecto o no existe");
        }

        if(user.Email == null)
        {
            throw new BadRequestException("User.Email is null", "El usuario no tiene un correo electrónico asociado");

        }

        var existingToken = await dbContext.Tokens
            .Where(t => t.UserId == user.UserId && t.Type == TokenTypes.PasswordReset && t.IsActive == true)
            .OrderByDescending(t => t.GeneratedAt)
            .FirstOrDefaultAsync(cancellationToken);

        if (existingToken != null)
        {
            if (existingToken.ExpiresAt >= DateTime.UtcNow)
            {
                var errorMessage =
                    "Ya se ha solicitado un restablecimiento de contraseña. Por favor, revise su correo electrónico.";
                return new PasswordResetRequestResult(new ResetRequestMessage(errorMessage, null));
            }

            if (existingToken.LastUsedAt < DateTime.UtcNow)
            {
                existingToken.IsActive = false;
                dbContext.Tokens.Update(existingToken);
                await dbContext.SaveChangesAsync(cancellationToken);

                var errorMessage =
                    "El enlace de restablecimiento de contraseña ha expirado. Por favor, solicite uno nuevo.";
                return new PasswordResetRequestResult(new ResetRequestMessage(errorMessage, null));
            }
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
            generatedAt: DateTime.UtcNow,
            expiresAt: DateTime.UtcNow.AddMinutes(60),
            lastUsedAt: DateTime.UtcNow,
            ip: null,
            device: null,
            userAgent: null
        );

        await dbContext.Tokens.AddAsync(newToken, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        await SaveEmail(user.Username, jwt, user.Email ?? "");

        var maskedEmail = MaskEmail(user.Email ?? "");
        var message =
            $"Hemos enviado un enlace de restablecimiento de contraseña a su correo electrónico {maskedEmail}";
        var res = new ResetRequestMessage(message, null);
        return new PasswordResetRequestResult(res);
    }
    private static string MaskEmail(string email)
    {
        if (string.IsNullOrEmpty(email) || !email.Contains('@'))
            throw new ArgumentException("El correo electrónico no es válido.");

        var parts = email.Split('@');
        var username = parts[0];
        var domain = parts[1];

        if (username.Length <= 3)
        {
            return $"{username[0]}{new string('*', username.Length - 1)}@{domain}";
        }

        var halfLength = username.Length / 2;
        var start = Math.Max(halfLength - 1, 1);
        var end = Math.Min(halfLength + 2, username.Length - 1);

        var firstPart = username[..start];
        var lastPart = username[end..];
        return $"{firstPart}***{lastPart}@{domain}";
    }

    private async Task SaveEmail(string username, string jwt, string email)
    {
        try
        {
            var model = new ChangePasswordRequestModel()
            {
                Username = username,
                RequestDate = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss"),
                Jwt = jwt,
                ResetUrl = $"{appConstants.Frontend.BaseUrl}/auth/config-password?token={jwt}"
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
                Console.WriteLine("Email de Solicitud de Cambio de Contraseña Preparado");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error al guardar email de Solicitud de Cambio de Contraseña: ", e.Message);
        }
    }

}
