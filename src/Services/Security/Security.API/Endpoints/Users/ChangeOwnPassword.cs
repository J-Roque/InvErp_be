using Security.API.Context;
using Security.Application.Dtos.Input;
using Security.Application.Handlers.Users.Commands.ChangeOwnPassword;

namespace Security.API.Endpoints.Users;
public record ChangeOwnPasswordRequest(ChangeOwnPasswordInput User);
public record changeOwnPasswordDAta(ChangeOwnPasswordInput user, IUserContext UserContext);
public record ChangeOwnPasswordResponse(bool IsSuccess);
public class ChangeOwnPassword : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/users/me/password",
            async (ChangeOwnPasswordRequest request, IUserContext UserContext, ISender sender) =>
        {
            var data = new changeOwnPasswordDAta(request.User, UserContext);
            var command = data.Adapt<ChangeOwnPasswordCommand>();
            var result = await sender.Send(command);
            var response =  result.Adapt<ChangeOwnPasswordResponse>();
            return Results.Ok();
        })
        .WithName("ChangeOwnPassword")
        .WithTags("Users")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized);
    }
}
