using BuildingBlocks.CQRS;
using BuildingBlocks.Pagination;
using Security.Application.Dtos.Results;

namespace Security.Application.Handlers.Users.Queries.GetUsers;

public record UserPaginationRequest(int Page = 1, int PageSize = 10, int IgnorePagination = 0, string? SearchTerm = null)
    : PaginationRequest(Page, PageSize, IgnorePagination, SearchTerm);

public record GetUsersQuery(UserPaginationRequest PaginationRequest) : IQuery<GetUsersResult>;

public record GetUsersResult(IEnumerable<UserPaginationItem> Data, long TotalCount, int Page, int PageSize)
    : UserPaginationResult(Data, TotalCount, Page, PageSize);
