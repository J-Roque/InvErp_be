using Auth.Application.Dtos.Input;
using Auth.Application.Dtos.Result;
using Auth.Application.Handler.Auth.PasswordResetRequest;
using Carter;
using Mapster;
using MediatR;

namespace Auth.API.Endpoints.Auth;
public record ResetRequest(PasswordResetRequestInput Reset);

public record ResetResponse(ResetRequestMessage Result);

public class PasswordResetRequest : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/auth/password-reset-request", async (ResetRequest request, ISender sender) =>
            {
                var command = request.Adapt<PasswordResetRequestCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<ResetResponse>();
                return Results.Ok(response.Result);
            })
            .WithName("PasswordResetRequest")
            .Produces<ResetRequestMessage>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .WithSummary("Solicitar Cambio de Contraseña")
            .WithDescription("Solicitar Cambio de Contraseña");



    }
}
