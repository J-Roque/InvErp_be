using Carter;
using MediatR;
using Security.Application.Dtos.Results;
using Security.Application.Handlers.Users.Queries.GetUsers;

namespace Security.API.Endpoints.Users;

public record GetUsersRequest(int Page = 1, int PageSize = 10, int IgnorePagination = 0, string? SearchTerm = null);

public record GetUsersResponse(IEnumerable<UserPaginationItem> Data, long TotalCount, int Page, int PageSize);

public class GetUsers : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/users", async ([AsParameters] GetUsersRequest request, ISender sender) =>
            {
                var paginationRequest = new UserPaginationRequest(
                    request.Page,
                    request.PageSize,
                    request.IgnorePagination,
                    request.SearchTerm);

                var query = new GetUsersQuery(paginationRequest);
                var result = await sender.Send(query);
                var response = result.Adapt<GetUsersResponse>();
                return Results.Ok(response);
            })
            .WithName("GetUsers")
            .Produces<GetUsersResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Obtener Usuarios")
            .WithDescription("Obtener lista de usuarios con paginación y filtros")
            .RequireAuthorization();
    }
}
