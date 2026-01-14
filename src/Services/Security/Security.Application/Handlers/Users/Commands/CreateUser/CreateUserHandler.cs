using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions;
using Crypto.Services;
using Microsoft.EntityFrameworkCore;
using Security.Application.Data;
using Security.Application.Dtos.Input;
using Security.Domain.Models;
using Security.Domain.ValueObjects;

namespace Security.Application.Handlers.Users.Commands.CreateUser;

public class CreateUserHandler(IApplicationDbContext dbContext)
    : ICommandHandler<CreateUserCommand, CreateUserResult>
{
    public async Task<CreateUserResult> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            // Se busca si hay un usuario con el mismo username
            var existingUser = await dbContext.Users
                .FirstOrDefaultAsync(u => u.Username == command.User.Username, cancellationToken);

            if (existingUser != null)
            {
                throw new BadRequestException("El valor del campo usuario ya está en uso");
            }

            // Se buscan los roles
            var roles = await dbContext.Roles
                .Where(r => command.User.RoleIds.Contains(r.Id))
                .ToListAsync(cancellationToken);

            var roleIds = command.User.RoleIds;

            // Se buscan los roles que no se encontraron
            var missingRoleIds = roleIds.Except(roles.Select(r => r.Id)).ToList();
            if (missingRoleIds.Count != 0)
            {
                throw new NotFoundException($"No se encontraron roles con los Ids: {string.Join(", ", missingRoleIds)}");
            }

            var person = CreateNewPerson(command.User);
            await dbContext.Persons.AddAsync(person, cancellationToken);

            await dbContext.SaveChangesAsync(cancellationToken);

            var user = CreateNewUser(command.User, person.Id);
            await dbContext.Users.AddAsync(user, cancellationToken);

            await dbContext.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return new CreateUserResult(user.Id);
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
            throw new InternalServerException(e.Message, "Error interno al crear usuario");
        }
    }

    private User CreateNewUser(CreateUserInput userDto, long personId)
    {
        var password = userDto.Password;
        var hasher = new PasswordHasher();
        var (hash, salt) = hasher.Hash(password);

        var newUser = User.Create(
            personId: personId,
            username: userDto.Username,
            password: hash,
            salt: salt,
            identityStatusId: userDto.IdentityStatusId,
            imageAttachmentId: userDto.ImageAttachmentId,
            profileId: userDto.ProfileId
        );

        foreach (var roleId in userDto.RoleIds)
        {
            newUser.SetRoles(roleId);
        }

        // TODO: Implement providers and clients when available
        // foreach (var providerId in userDto.ProviderIds)
        // {
        //     newUser.AddProviders(providerId: providerId, providerPersonId: 0);
        // }
        //
        // foreach (var clientId in userDto.ClientIdS)
        // {
        //     newUser.AddClients(clientId: clientId, clientPersonId: 0);
        // }

        return newUser;
    }

    private Person CreateNewPerson(CreateUserInput userDto)
    {
        var newPerson = Person.Create(
            firstName: userDto.FirstName,
            lastName: userDto.LastName,
            businessName: "",
            documentTypeId: userDto.DocumentTypeId,
            documentNumber: userDto.DocumentNumber,
            email: userDto.Email,
            isActive: true,
            personType: PersonType.Of("N")
        );

        return newPerson;
    }
}
