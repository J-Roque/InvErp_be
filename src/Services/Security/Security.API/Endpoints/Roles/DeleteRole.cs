using Carter;
using MediatR;
using Security.Application.Handlers.Roles.Commands.DeleteRole;
using Security.Application.Interfaces.Context;

namespace Security.API.Endpoints.Roles;

public record DeleteRoleData(long Id, IUserContext UserContext);

public record DeleteRoleResponse(bool Success);

public class DeleteRole : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/roles/{id}", async (long id, IUserContext userContext, ISender sender) =>
            {
                var data = new DeleteRoleData(id, userContext);
                var command = data.Adapt<DeleteRoleCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<DeleteRoleResponse>();
                return Results.Ok(response);
            })
            .WithName("DeleteRole")
            .Produces<DeleteRoleResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Eliminar Rol")
            .WithDescription("Eliminar (desactivar) un rol existente")
            .RequireAuthorization();
    }
}
