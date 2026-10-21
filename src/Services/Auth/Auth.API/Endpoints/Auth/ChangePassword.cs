using Auth.API.RateLimiting;
using Auth.Application.Dtos.Input;
using Auth.Application.Handler.Auth.ChangeUserPassword;
using Carter;
using Mapster;
using MediatR;

namespace Auth.API.Endpoints.Auth;

public record ChangePasswordRequest(ChangePasswordInput Reset);

public record ChangePasswordResponse(bool IsSuccess);

public class ChangePassword : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/auth/password-reset", async (ChangePasswordRequest request, ISender sender) =>
            {
                var command = request.Adapt<ChangeUserPasswordCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<ChangePasswordResponse>();
                return Results.Ok(response);
            })
            .RequireRateLimiting(RateLimitPolicies.PasswordReset)
            .WithName("ChangePassword")
            .Produces<ChangePasswordResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .WithSummary("Cambiar Contraseña")
            .WithDescription("Cambiar Contraseña");
    }
}
