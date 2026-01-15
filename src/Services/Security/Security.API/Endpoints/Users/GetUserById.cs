using Carter;
using MediatR;
using Security.Application.Dtos.Results;
using Security.Application.Handlers.Users.Queries.GetUserById;

namespace Security.API.Endpoints.Users;

public record GetUserByIdResponse(UserInfo User);

public class GetUserById : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/users/{id}", async (long id, ISender sender) =>
            {
                var query = new GetUserByIdQuery(id);
                var result = await sender.Send(query);
                var response = result.Adapt<GetUserByIdResponse>();
                return Results.Ok(response);
            })
            .WithName("GetUserById")
            .Produces<GetUserByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Obtener Usuario por Id")
            .WithDescription("Obtener información detallada de un usuario por su Id")
            .RequireAuthorization();
    }
}
