using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions;
using Microsoft.EntityFrameworkCore;
using Security.Application.Data;

namespace Security.Application.Handlers.Roles.Commands.UpdateRole;

public class UpdateRoleHandler(IApplicationDbContext dbContext)
    : ICommandHandler<UpdateRoleCommand, UpdateRoleResult>
{
    public async Task<UpdateRoleResult> Handle(UpdateRoleCommand command, CancellationToken cancellationToken)
    {
        var role = await dbContext.Roles
            .FirstOrDefaultAsync(r => r.Id == command.Id, cancellationToken);

        if (role == null)
        {
            throw new NotFoundException("Rol", command.Id);
        }

        if (role.IsProtected)
        {
            throw new BadRequestException("No se puede modificar un rol protegido");
        }

        // Verificar si el nombre ya existe en otro rol
        var existingRole = await dbContext.Roles
            .FirstOrDefaultAsync(r => r.Name == command.Role.Name && r.Id != command.Id, cancellationToken);

        if (existingRole != null)
        {
            throw new BadRequestException("Ya existe un rol con ese nombre");
        }

        role.Update(
            name: command.Role.Name,
            description: command.Role.Description,
            identityStatusId: command.Role.IdentityStatusId
        );

        if (command.Role.ImageAttachmentId.HasValue)
        {
            role.HandleImageAttachment(command.Role.ImageAttachmentId.Value);
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateRoleResult(role.Id);
    }
}
