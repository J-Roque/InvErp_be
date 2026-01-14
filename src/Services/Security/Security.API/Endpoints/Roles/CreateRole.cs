using Security.Application.Dtos.General;
using Security.Application.Handlers.Roles.Commands.CreateRole;
using Security.Application.Interfaces.Context;

namespace Security.API.Endpoints.Roles;

public record CreateRoleRequest(RoleDto Role);
public record CreateRoleData(RoleDto Role, IUserContext UserContext);
public record CreateRoleResponse(long Id);

public class CreateRole : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/roles", async (CreateRoleRequest request, IUserContext userContext, ISender sender) =>
            {
                var data = new CreateRoleData(request.Role, userContext);
                var command = data.Adapt<CreateRoleCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<CreateRoleResponse>();
                return Results.Created($"/roles/{response.Id}", response);
            })
            .WithName("CreateRole")
            .Produces<CreateRoleResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Crear Rol")
            .WithDescription("Crear Rol")
            .RequireAuthorization();
    }
}
