using Carter;
using MediatR;
using Security.Application.Dtos.Input;
using Security.Application.Handlers.Users.Commands.UpdateUser;
using Security.Application.Interfaces.Context;

namespace Security.API.Endpoints.Users;

public record UpdateUserRequest(UpdateUserInput User);

public record UpdateUserData(long Id, UpdateUserInput User, IUserContext UserContext);

public record UpdateUserResponse(long Id);

public class UpdateUser : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/users/{id}", async (long id, UpdateUserRequest request, IUserContext userContext, ISender sender) =>
            {
                var data = new UpdateUserData(id, request.User, userContext);
                var command = data.Adapt<UpdateUserCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<UpdateUserResponse>();
                return Results.Ok(response);
            })
            .WithName("UpdateUser")
            .Produces<UpdateUserResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Actualizar Usuario")
            .WithDescription("Actualizar información de un usuario existente")
            .RequireAuthorization();
    }
}
