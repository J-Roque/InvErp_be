using Security.Application.Dtos.General;
using Security.Application.Handlers.Profiles.Commands.CreateProfile;
using Security.Application.Interfaces.Context;

namespace Security.API.Endpoints.Profiles;

public record CreateProfileRequest(ProfileDto Profile);
public record CreateProfileData(ProfileDto Profile, IUserContext UserContext);
public record CreateProfileResponse(long Id);

public class CreateProfile : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/profiles", async (CreateProfileRequest request, IUserContext userContext, ISender sender) =>
            {
                var data = new CreateProfileData(request.Profile, userContext);
                var command = data.Adapt<CreateProfileCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<CreateProfileResponse>();
                return Results.Created($"/profiles/{response.Id}", response);
            })
            .WithName("CreateProfile")
            .Produces<CreateProfileResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Crear Perfil")
            .WithDescription("Crear Perfil")
            .RequireAuthorization();
    }
}
