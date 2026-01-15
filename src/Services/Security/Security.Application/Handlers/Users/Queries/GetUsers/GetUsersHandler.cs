using BuildingBlocks.CQRS;
using Security.Application.Interfaces.Persistance;
using Security.Domain.Parameters.User;
using System.Text.Json.Nodes;

namespace Security.Application.Handlers.Users.Queries.GetUsers;

public class GetUsersHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetUsersQuery, GetUsersResult>
{
    public async Task<GetUsersResult> Handle(GetUsersQuery query, CancellationToken cancellationToken)
    {
        var page = query.PaginationRequest.Page;
        var pageSize = query.PaginationRequest.PageSize;
        var ignorePagination = query.PaginationRequest.IgnorePagination == 1;

        var searchTerm = query.PaginationRequest.SearchTerm;

        JsonObject filters = new()
        {
            ["SearchTerm"] = !string.IsNullOrWhiteSpace(searchTerm) ? searchTerm : null,
        };

        var filtersJson = searchTerm != null ? filters.ToJsonString() : null;

        var paginationParameters = new UserPaginationParameter
        {
            Page = page,
            PageSize = pageSize,
            IgnorePagination = ignorePagination,
            Filters = filtersJson
        };

        var paginatedResult = await unitOfWork.Users.GetUsersWithFiltersAndPagination(paginationParameters);

        return new GetUsersResult
        (
            Data: paginatedResult.Data,
            TotalCount: paginatedResult.TotalCount,
            Page: paginatedResult.Page,
            PageSize: paginatedResult.PageSize
        );
    }
}
