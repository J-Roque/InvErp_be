using Carter;
using MediatR;
using Security.Application.Dtos.General;
using Security.Application.Handlers.Roles.Commands.UpdateRole;
using Security.Application.Interfaces.Context;

namespace Security.API.Endpoints.Roles;

public record UpdateRoleRequest(RoleDto Role);

public record UpdateRoleData(long Id, RoleDto Role, IUserContext UserContext);

public record UpdateRoleResponse(long Id);

public class UpdateRole : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/roles/{id}", async (long id, UpdateRoleRequest request, IUserContext userContext, ISender sender) =>
            {
                var data = new UpdateRoleData(id, request.Role, userContext);
                var command = data.Adapt<UpdateRoleCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<UpdateRoleResponse>();
                return Results.Ok(response);
            })
            .WithName("UpdateRole")
            .Produces<UpdateRoleResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Actualizar Rol")
            .WithDescription("Actualizar información de un rol existente")
            .RequireAuthorization();
    }
}
