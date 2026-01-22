using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions;
using Microsoft.EntityFrameworkCore;
using Security.Application.Data;
using Security.Application.Dtos.Results;
using Security.Domain.Enums;

namespace Security.Application.Handlers.Roles.Queries.GetRoleById;

public class GetRoleByIdHandler(IApplicationDbContext dbContext) : IQueryHandler<GetRoleByIdQuery, GetRoleByIdResult>
{
    public async Task<GetRoleByIdResult> Handle(GetRoleByIdQuery query, CancellationToken cancellationToken)
    {
        var role = await dbContext.Roles
            .FirstOrDefaultAsync(r => r.Id == query.Id, cancellationToken);

        if (role == null)
        {
            throw new NotFoundException("Rol", query.Id);
        }

        // Obtener nombres de creador y modificador
        string? createdByName = null;
        string? lastModifiedByName = null;

        if (role.CreatedBy.HasValue)
        {
            var creator = await dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == role.CreatedBy.Value, cancellationToken);
            createdByName = creator?.Username;
        }

        if (role.LastModifiedBy.HasValue)
        {
            var modifier = await dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == role.LastModifiedBy.Value, cancellationToken);
            lastModifiedByName = modifier?.Username;
        }

        var roleInfo = new RoleInfo(
            Id: role.Id,
            Name: role.Name,
            Code: role.Code,
            Description: role.Description,
            IsActive: role.IsActive,
            IsProtected: role.IsProtected,
            ImageAttachmentId: role.ImageAttachmentId,
            ImageAttachmentUrl: null,
            IdentityStatusId: role.IdentityStatusId,
            IdentityStatus: GetIdentityStatusName(role.IdentityStatusId),
            CreatedAt: role.CreatedAt,
            CreatedBy: role.CreatedBy,
            CreatedByName: createdByName,
            LastModified: role.LastModified,
            LastModifiedBy: role.LastModifiedBy,
            LastModifiedByName: lastModifiedByName
        );

        return new GetRoleByIdResult(roleInfo);
    }

    private static string GetIdentityStatusName(IdentityStatus status) => status switch
    {
        IdentityStatus.Active => "Activo",
        IdentityStatus.Inactive => "Inactivo",
        IdentityStatus.Blocked => "Bloqueado",
        _ => "Desconocido"
    };
}
