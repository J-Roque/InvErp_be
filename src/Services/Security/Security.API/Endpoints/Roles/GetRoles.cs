using Carter;
using MediatR;
using Security.Application.Dtos.Results;
using Security.Application.Handlers.Roles.Queries.GetRoles;

namespace Security.API.Endpoints.Roles;

public record GetRolesRequest(int Page = 1, int PageSize = 10, int IgnorePagination = 0, string? SearchTerm = null);

public record GetRolesResponse(IEnumerable<RolePaginationItem> Data, long TotalCount, int Page, int PageSize);

public class GetRoles : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/roles", async ([AsParameters] GetRolesRequest request, ISender sender) =>
            {
                var paginationRequest = new RolePaginationRequest(
                    request.Page,
                    request.PageSize,
                    request.IgnorePagination,
                    request.SearchTerm);

                var query = new GetRolesQuery(paginationRequest);
                var result = await sender.Send(query);
                var response = result.Adapt<GetRolesResponse>();
                return Results.Ok(response);
            })
            .WithName("GetRoles")
            .Produces<GetRolesResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Obtener Roles")
            .WithDescription("Obtener lista de roles con paginación y filtros")
            .RequireAuthorization();
    }
}
