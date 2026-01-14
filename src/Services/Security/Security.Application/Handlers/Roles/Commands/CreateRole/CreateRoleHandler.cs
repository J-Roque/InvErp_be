using BuildingBlocks.CQRS;
using Security.Application.Data;
using Security.Application.Dtos.General;
using Security.Domain.Models;

namespace Security.Application.Handlers.Roles.Commands.CreateRole;

public class CreateRoleHandler(IApplicationDbContext dbContext)
    : ICommandHandler<CreateRoleCommand, CreateRoleResult>
{
    public async Task<CreateRoleResult> Handle(CreateRoleCommand command, CancellationToken cancellationToken)
    {
        var role = CreateNewRole(command.Role);
        dbContext.Roles.Add(role);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateRoleResult(role.Id);
    }

    private Role CreateNewRole(RoleDto roleDto)
    {
        var newRole = Role.Create(
            id: 0,
            name: roleDto.Name,
            description: roleDto.Description,
            isActive: roleDto.IsActive,
            imageAttachmentId: null,
            code: null,
            isProtected: false,
            identityStatusId: roleDto.IdentityStatusId
        );

        return newRole;
    }
}
