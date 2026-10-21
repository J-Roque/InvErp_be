using Auth.API.RateLimiting;
using Auth.Application.Dtos.Input;
using Auth.Application.Dtos.Result;
using Auth.Application.Handler.Auth.ValidatePasswordResetToken;
using Carter;
using Mapster;
using MediatR;

namespace Auth.API.Endpoints.Auth;

public record ValidatePasswordResetTokenRequest(ValidatePasswordResetTokenInput Reset);

public record ValidatePasswordResetTokenResponse(ValidatePasswordResetTokenMessage Result);

public class ValidatePasswordResetToken : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        // POST para que el token viaje en el body y no quede en URLs ni en logs de acceso
        app.MapPost("/auth/password-reset/validate-token",
                async (ValidatePasswordResetTokenRequest request, ISender sender) =>
                {
                    var command = request.Adapt<ValidatePasswordResetTokenCommand>();
                    var result = await sender.Send(command);
                    var response = result.Adapt<ValidatePasswordResetTokenResponse>();
                    return Results.Ok(response.Result);
                })
            .RequireRateLimiting(RateLimitPolicies.PasswordReset)
            .WithName("ValidatePasswordResetToken")
            .Produces<ValidatePasswordResetTokenMessage>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status429TooManyRequests)
            .WithSummary("Validar Token de Cambio de Contraseña")
            .WithDescription("Indica si el enlace de restablecimiento de contraseña todavía puede usarse.");
    }
}
