using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions;
using Microsoft.EntityFrameworkCore;
using Security.Application.Data;
using Security.Domain.Enums;

namespace Security.Application.Handlers.Roles.Commands.DeleteRole;

public class DeleteRoleHandler(IApplicationDbContext dbContext)
    : ICommandHandler<DeleteRoleCommand, DeleteRoleResult>
{
    public async Task<DeleteRoleResult> Handle(DeleteRoleCommand command, CancellationToken cancellationToken)
    {
        var role = await dbContext.Roles
            .FirstOrDefaultAsync(r => r.Id == command.Id, cancellationToken);

        if (role == null)
        {
            throw new NotFoundException("Rol", command.Id);
        }

        if (role.IsProtected)
        {
            throw new BadRequestException("No se puede eliminar un rol protegido");
        }

        // Verificar si hay usuarios con este rol
        var usersWithRole = await dbContext.Users
            .Include(u => u.UserRoles)
            .AnyAsync(u => u.UserRoles.Any(ur => ur.RoleId == command.Id), cancellationToken);

        if (usersWithRole)
        {
            throw new BadRequestException("No se puede eliminar el rol porque está asignado a uno o más usuarios");
        }

        // Soft delete - cambiar estado a Inactive
        role.Update(
            name: role.Name,
            description: role.Description,
            identityStatusId: IdentityStatus.Inactive
        );

        await dbContext.SaveChangesAsync(cancellationToken);

        return new DeleteRoleResult(true);
    }
}
