using Carter;
using MediatR;
using Security.API.Context;
using Security.Application.Dtos.Input;
using Security.Application.Handlers.Users.Commands.CreateUser;
using Security.Application.Interfaces.Context;

namespace Security.API.Endpoints.Users;

public record CreateUserRequest(CreateUserInput User);

public record CreateUserData(CreateUserInput User, IUserContext UserContext);

public record CreateUserResponse(long Id);

public class CreateUser : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/users", async (CreateUserRequest request, IUserContext userContext, ISender sender) =>
            {
                var data = new CreateUserData(request.User, userContext);
                var command = data.Adapt<CreateUserCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<CreateUserResponse>();
                return Results.Created($"/users/{response.Id}", response);
            })
            .WithName("CreateUser")
            .Produces<CreateUserResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Crear Usuario")
            .WithDescription("Crear Usuario");
            // .RequireAuthorization(); // TODO: Habilitar despues de crear el primer usuario
    }
}
