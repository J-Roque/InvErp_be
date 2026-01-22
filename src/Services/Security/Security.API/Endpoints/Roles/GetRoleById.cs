using Carter;
using MediatR;
using Security.Application.Dtos.Results;
using Security.Application.Handlers.Roles.Queries.GetRoleById;

namespace Security.API.Endpoints.Roles;

public record GetRoleByIdResponse(RoleInfo Role);

public class GetRoleById : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/roles/{id}", async (long id, ISender sender) =>
            {
                var query = new GetRoleByIdQuery(id);
                var result = await sender.Send(query);
                var response = result.Adapt<GetRoleByIdResponse>();
                return Results.Ok(response);
            })
            .WithName("GetRoleById")
            .Produces<GetRoleByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Obtener Rol por Id")
            .WithDescription("Obtener información detallada de un rol por su Id")
            .RequireAuthorization();
    }
}
