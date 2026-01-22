using BuildingBlocks.CQRS;
using BuildingBlocks.Pagination;
using Security.Application.Dtos.Results;

namespace Security.Application.Handlers.Roles.Queries.GetRoles;

public record RolePaginationRequest(int Page = 1, int PageSize = 10, int IgnorePagination = 0, string? SearchTerm = null)
    : PaginationRequest(Page, PageSize, IgnorePagination, SearchTerm);

public record GetRolesQuery(RolePaginationRequest PaginationRequest) : IQuery<GetRolesResult>;

public record GetRolesResult(IEnumerable<RolePaginationItem> Data, long TotalCount, int Page, int PageSize)
    : RolePaginationResult(Data, TotalCount, Page, PageSize);
