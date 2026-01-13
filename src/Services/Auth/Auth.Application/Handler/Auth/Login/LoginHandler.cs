using Auth.Application.Configuration;
using Auth.Application.Data;
using Auth.Application.Dtos.Result;
using Auth.Application.Exceptions;
using Auth.Application.Interfaces.Persistence;
using Auth.Domain.Enums;
using BuildingBlocks.CQRS;
using Crypto.Services;
using Jwt.Model;
using Jwt.Services;

namespace Auth.Application.Handler.Auth.Login;

public class LoginHandler(IApplicationDbContext dbContext, IUnitOfWork unitOfWork, AppConstants appConstants) :
    ICommandHandler<LoginCommand, LoginResult>
{
    public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var username = request.Login.Username;
        var password = request.Login.Password;
        var ip = request.Ip;
        var device = request.Device;
        var userAgent = request.UserAgent;

        var user = await unitOfWork.Auth.SearchUserForLogin(username);

        if (user == null)
        {
            throw new AuthUnauthorizedException("Usuario o contraseña incorrectos");
        }

        var hasher = new PasswordHasher();
        var isValid = hasher.Verify(
            storedHash: user.Password,
            storedSalt: user.Salt,
            inputPassword: password
        );

        if (!isValid)
        {
            throw new AuthUnauthorizedException("Usuario o contraseña incorrectos");
        }

        var code = Guid.NewGuid().ToString();
        var payload = new JwtPayloadModel()
        {
            Code = code,
            ProfileName = user.Profile?.Name ?? ""
        };

        var jwtManager = new JwtManager();
        var superPasswordJwt = appConstants.Jwt.Secret;
        var jwt = jwtManager.GenerateJwt(superPasswordJwt, payload);

        // Se crea el registro de token
        var newToken = Token.Create(
            userId: user.UserId,
            code: code,
            type: TokenTypes.Login,
            generatedAt: DateTime.Now,
            expiresAt: DateTime.Now.AddMinutes(appConstants.Jwt.LoginExpirationMinutes),
            lastUsedAt: DateTime.Now,
            ip: ip,
            device: device,
            userAgent: userAgent
        );

        await dbContext.Tokens.AddAsync(newToken, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var response = new LoginAuth(
            UserId: user.UserId,
            Username: username,
            FullName: user.FullName,
            Email: user.Email ?? "",
            Jwt: jwt);

        return new LoginResult(response);
    }
}
