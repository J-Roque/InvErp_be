

using BuildingBlocks.Exceptions;
using Crypto.Services;
using Security.Application.Exceptions;

namespace Security.Application.Handlers.Users.Commands.ChangeOwnPassword;

public class ChangeOwnPasswordHandler(IApplicationDbContext dbContext)
    :ICommandHandler<ChangeOwnPasswordCommand, ChangeOwnPasswordResult>
{
    public async Task<ChangeOwnPasswordResult> Handle(ChangeOwnPasswordCommand command,
        CancellationToken cancellationToken)
    {
        var Id = command.UserContext.UserId;
        var user = await dbContext.Users.FindAsync([Id], cancellationToken) ??
                   throw new UserNotFoundException(Id, $"Usuario con Id {Id} no encontrado");

        var oldPassword = command.User.OldPassword;
        var newPassword = command.User.NewPassword;

        // Se verifica si la contraseña actual es correcta
        var hasher = new PasswordHasher();
        var isOldPasswordValid = hasher.Verify(user.Password, user.Salt, oldPassword);
        if (!isOldPasswordValid)
        {
            throw new BadRequestException("La contraseña actual es incorrecta");
        }

        // Se actualiza la contraseña
        var (newHash, newSalt) = hasher.Hash(newPassword);
        user.UpdateMyPassword(newHash, newSalt);
        dbContext.Users.Update(user);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new ChangeOwnPasswordResult(true);
    }

}
