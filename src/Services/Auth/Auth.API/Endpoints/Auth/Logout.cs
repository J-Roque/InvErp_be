using Auth.Application.Handler.Auth.Logout;
using Carter;
using MediatR;

namespace Auth.API.Endpoints.Auth;

public class Logout : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        // El JWT de la sesión llega en la cabecera Authorization: Bearer <token>
        app.MapPost("/auth/logout", async (HttpContext httpContext, ISender sender) =>
            {
                var authorization = httpContext.Request.Headers.Authorization.ToString();
                var jwt = authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
                    ? authorization["Bearer ".Length..].Trim()
                    : null;

                await sender.Send(new LogoutCommand(jwt));
                return Results.NoContent();
            })
            .WithName("Logout")
            .Produces(StatusCodes.Status204NoContent)
            .WithSummary("Cerrar sesión")
            .WithDescription("Invalida en el servidor el JWT de la sesión actual.");
    }
}
