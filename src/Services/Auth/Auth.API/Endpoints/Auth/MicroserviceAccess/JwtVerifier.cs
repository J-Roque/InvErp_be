using Auth.Application.Handler.Auth.ValidatePrivateJwt;
using Carter;
using Mapster;
using MediatR;

namespace Auth.API.Endpoints.Auth.MicroserviceAccess;

public record JwtVerifierRequest(string Jwt);

/// <summary>
/// Endpoint interno: lo usan los demás microservicios (Security) para validar la sesión
/// de cada petición. En producción no debería exponerse fuera de la red interna.
/// </summary>
public class JwtVerifier : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/ms/auth/verify-jwt", async (JwtVerifierRequest request, ISender sender) =>
            {
                var command = request.Adapt<ValidatePrivateJwtCommand>();
                var result = await sender.Send(command);
                return Results.Ok(result);
            })
            .WithName("PrivateJwtVerifier")
            .Produces<ValidatePrivateJwtResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Private Jwt Verifier")
            .WithDescription("Valida el JWT de sesión para otros microservicios");
    }
}
