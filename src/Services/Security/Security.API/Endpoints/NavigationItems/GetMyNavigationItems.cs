using Security.Application.Dtos.General;
using Security.Application.Handlers.NavigationItems.Queries.GetMyNavigationItems;
namespace Security.API.Endpoints.NavigationItems;

public class GetMyNavigationItems: ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/menu-items/me", async (ISender sender, IUserContext userContext) =>
        {
            var userId = userContext.UserId;
            var profileId = userContext.ProfileId;
            var result = await sender.Send(new GetMyNavigationItemsQuery(userId, profileId));
            return Results.Ok(result.NavigationItems);
        })
            .WithName("GetMyNavigationItems")
            .Produces<List<NavigationItemDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Obtener Mis Items de Navegación")
            .WithDescription("Obtener Mis Items de Navegación")
            .RequireAuthorization();
    }

}
