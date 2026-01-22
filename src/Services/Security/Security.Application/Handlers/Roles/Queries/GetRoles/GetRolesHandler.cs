using BuildingBlocks.CQRS;
using Microsoft.EntityFrameworkCore;
using Security.Application.Data;
using Security.Application.Dtos.Results;
using Security.Domain.Enums;

namespace Security.Application.Handlers.Roles.Queries.GetRoles;

public class GetRolesHandler(IApplicationDbContext dbContext) : IQueryHandler<GetRolesQuery, GetRolesResult>
{
    public async Task<GetRolesResult> Handle(GetRolesQuery query, CancellationToken cancellationToken)
    {
        var page = query.PaginationRequest.Page;
        var pageSize = query.PaginationRequest.PageSize;
        var ignorePagination = query.PaginationRequest.IgnorePagination == 1;
        var searchTerm = query.PaginationRequest.SearchTerm;

        var rolesQuery = dbContext.Roles.AsQueryable();

        // Aplicar filtro de búsqueda
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            rolesQuery = rolesQuery.Where(r =>
                r.Name.Contains(searchTerm) ||
                (r.Code != null && r.Code.Contains(searchTerm)) ||
                (r.Description != null && r.Description.Contains(searchTerm)));
        }

        // Obtener total
        var totalCount = await rolesQuery.LongCountAsync(cancellationToken);

        // Aplicar paginación
        if (!ignorePagination)
        {
            rolesQuery = rolesQuery
                .OrderByDescending(r => r.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }
        else
        {
            rolesQuery = rolesQuery.OrderByDescending(r => r.Id);
        }

        var roles = await rolesQuery.ToListAsync(cancellationToken);

        var data = roles.Select(r => new RolePaginationItem(
            Id: r.Id,
            Name: r.Name,
            Code: r.Code,
            Description: r.Description,
            IsActive: r.IsActive,
            IsProtected: r.IsProtected,
            ImageAttachmentId: r.ImageAttachmentId,
            ImageAttachmentUrl: null,
            IdentityStatusId: r.IdentityStatusId,
            IdentityStatus: GetIdentityStatusName(r.IdentityStatusId),
            CreatedAt: r.CreatedAt
        ));

        return new GetRolesResult(
            Data: data,
            TotalCount: totalCount,
            Page: page,
            PageSize: pageSize
        );
    }

    private static string GetIdentityStatusName(IdentityStatus status) => status switch
    {
        IdentityStatus.Active => "Activo",
        IdentityStatus.Inactive => "Inactivo",
        IdentityStatus.Blocked => "Bloqueado",
        _ => "Desconocido"
    };
}
