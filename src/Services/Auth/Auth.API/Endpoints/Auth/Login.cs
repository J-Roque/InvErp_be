using Auth.Application.Dtos.Input;
using Auth.Application.Dtos.Result;
using Auth.Application.Handler.Auth.Login;
using Carter;
using Mapster;
using MediatR;
using UAParser;

namespace Auth.API.Endpoints.Auth;

public record LoginRequest(LoginInput Login);

public record LoginInformation(LoginInput Login, string Ip, string Device, string UserAgent);

public record LoginResponse(LoginAuth LoginAuth);

public class Login : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/login", async (HttpContext httpContext, LoginRequest request, ISender sender) =>
            {
                var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "Desconocido";

                var userAgent = httpContext.Request.Headers.UserAgent.ToString() ?? "Desconocido";

                var deviceInfo = ParseUserAgent(userAgent);

                var info = new LoginInformation(
                    Login: request.Login,
                    Ip: ip,
                    Device: deviceInfo,
                    UserAgent: userAgent
                );
                var command = info.Adapt<LoginCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<LoginResponse>();
                return Results.Ok(response.LoginAuth);
            })
            .WithName("Login")
            .Produces<LoginAuth>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .WithSummary("Login")
            .WithDescription("Login");
    }

    private static string ParseUserAgent(string? userAgent)
    {
        if (string.IsNullOrEmpty(userAgent))
            return "Desconocido";

        var parser = Parser.GetDefault();
        var clientInfo = parser.Parse(userAgent);

        var os = clientInfo.OS.Family;
        var browser = clientInfo.UA.Family;

        return $"{os} - {browser}";
    }
}
