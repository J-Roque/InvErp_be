using Carter;
using MediatR;
using Security.Application.Handlers.Users.Commands.DeleteUser;
using Security.Application.Interfaces.Context;

namespace Security.API.Endpoints.Users;

public record DeleteUserData(long Id, IUserContext UserContext);

public record DeleteUserResponse(bool Success);

public class DeleteUser : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/users/{id}", async (long id, IUserContext userContext, ISender sender) =>
            {
                var data = new DeleteUserData(id, userContext);
                var command = data.Adapt<DeleteUserCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<DeleteUserResponse>();
                return Results.Ok(response);
            })
            .WithName("DeleteUser")
            .Produces<DeleteUserResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Eliminar Usuario")
            .WithDescription("Eliminar (desactivar) un usuario existente")
            .RequireAuthorization();
    }
}
