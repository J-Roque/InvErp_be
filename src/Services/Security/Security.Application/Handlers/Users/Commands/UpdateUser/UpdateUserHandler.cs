using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions;
using Crypto.Services;
using Microsoft.EntityFrameworkCore;
using Security.Application.Data;
using Security.Domain.ValueObjects;

namespace Security.Application.Handlers.Users.Commands.UpdateUser;

public class UpdateUserHandler(IApplicationDbContext dbContext)
    : ICommandHandler<UpdateUserCommand, UpdateUserResult>
{
    public async Task<UpdateUserResult> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var user = await dbContext.Users
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Id == command.Id, cancellationToken);

            if (user == null)
            {
                throw new NotFoundException("Usuario", command.Id);
            }

            // Verificar si el username ya existe en otro usuario
            var existingUser = await dbContext.Users
                .FirstOrDefaultAsync(u => u.Username == command.User.Username && u.Id != command.Id, cancellationToken);

            if (existingUser != null)
            {
                throw new BadRequestException("El valor del campo usuario ya está en uso por otro usuario");
            }

            // Buscar la persona asociada
            var person = await dbContext.Persons
                .FirstOrDefaultAsync(p => p.Id == user.PersonId, cancellationToken);

            if (person == null)
            {
                throw new NotFoundException("Persona asociada al usuario", user.PersonId);
            }

            // Validar roles
            var roles = await dbContext.Roles
                .Where(r => command.User.RoleIds.Contains(r.Id))
                .ToListAsync(cancellationToken);

            var missingRoleIds = command.User.RoleIds.Except(roles.Select(r => r.Id)).ToList();
            if (missingRoleIds.Count != 0)
            {
                throw new NotFoundException($"No se encontraron roles con los Ids: {string.Join(", ", missingRoleIds)}");
            }

            // Actualizar Person
            person.Update(
                firstName: command.User.FirstName,
                lastName: command.User.LastName,
                businessName: "",
                documentTypeId: command.User.DocumentTypeId,
                documentNumber: command.User.DocumentNumber,
                email: command.User.Email,
                isActive: true,
                personType: PersonType.Of("N"),
                updatedBy: command.UserContext.UserId
            );

            // Actualizar User
            user.Update(
                username: command.User.Username,
                identityStatusId: command.User.IdentityStatusId,
                profileId: command.User.ProfileId
            );

            // Actualizar password si se proporciona
            if (!string.IsNullOrWhiteSpace(command.User.Password))
            {
                var hasher = new PasswordHasher();
                var (hash, salt) = hasher.Hash(command.User.Password);
                user.UpdateMyPassword(hash, salt);
            }

            // Actualizar imagen si se proporciona
            if (command.User.ImageAttachmentId.HasValue)
            {
                user.HandleImageAttachment(command.User.ImageAttachmentId.Value);
            }

            // Actualizar roles - remover los que ya no están y agregar los nuevos
            var currentRoleIds = user.UserRoles.Select(ur => ur.RoleId).ToList();
            var rolesToRemove = currentRoleIds.Except(command.User.RoleIds).ToList();
            var rolesToAdd = command.User.RoleIds.Except(currentRoleIds).ToList();

            foreach (var roleId in rolesToRemove)
            {
                user.RemoveRoles(roleId);
            }

            foreach (var roleId in rolesToAdd)
            {
                user.SetRoles(roleId);
            }

            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return new UpdateUserResult(user.Id);
        }
        catch (BadRequestException)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
        catch (NotFoundException)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw new InternalServerException(e.Message, "Error interno al actualizar usuario");
        }
    }
}
