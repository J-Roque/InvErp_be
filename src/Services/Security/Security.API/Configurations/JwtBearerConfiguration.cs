using System.Security.Claims;
using System.Text.Json;
using Common.Constants.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Security.Application.RemoteServices.Context;

namespace Security.API.Configurations;

public static class JwtBearerConfiguration
{
    public static void ConfigureJwtBearer(this IServiceCollection services)
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = async context =>
                    {
                        
                        var jwt = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
                        if (string.IsNullOrEmpty(jwt))
                        {
                            context.NoResult();
                            return;
                        }
                        
                        var remote = context.HttpContext.RequestServices.GetRequiredService<IRemoteServiceContext>();
                        var validationResult = await remote.Auth.JwtVerifierAsync(jwt);
                        
                        if (!validationResult.IsValid)
                        {
                            context.Fail(validationResult.Message);
                            return;
                        }

                        if (validationResult.UserId is null or <= 0)
                        {
                            context.Fail("UserId no es válido");
                            return;
                        }

                        if (validationResult.Profile is null)
                        {
                            context.Fail("El usuario no tiene un perfil asignado");
                            return;
                        }

                        if (validationResult.Roles is null || validationResult.Roles.Count == 0)
                        {
                            context.Fail("El usuario no tiene roles ni permisos asignados");
                            return;
                        }

                        var claims = new List<Claim>
                        {
                            new Claim(ClaimConstants.UserId, validationResult.UserId.ToString() ?? string.Empty),
                            new Claim(ClaimConstants.ProfileId, validationResult.Profile.Id.ToString() ?? string.Empty),
                            new Claim(ClaimConstants.Email, validationResult.Email ?? ""),
                            new Claim(ClaimConstants.Ip, validationResult.Ip ?? ""),
                            new Claim(ClaimConstants.TokenId, validationResult.TokenId.ToString() ?? 0.ToString())
                        };

                        if (validationResult.Roles != null)
                            claims.AddRange(validationResult.Roles.Select(role =>
                                new Claim(ClaimConstants.RoleId, role.Id.ToString())));

                        var identity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);
                        context.Principal = principal;
                        context.Success();
                    },

                    OnAuthenticationFailed = async context =>
                    {
                        await WriteJsonErrorResponse(context.Response,
                            "Error de autenticación: " + context.Exception.Message, 401);
                        context.Fail(context.Exception.Message);
                    },

                    OnChallenge = async context =>
                    {
                        if (!context.Response.HasStarted)
                        {
                            var message = context.AuthenticateFailure?.Message
                                          ?? context.ErrorDescription
                                          ?? "Acceso no autorizado";

                            await WriteJsonErrorResponse(context.Response, message, 401);
                        }

                        context.HandleResponse();
                    }
                };

                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
            });
    }

    private static async Task WriteJsonErrorResponse(HttpResponse response, string message, int statusCode)
    {
        response.StatusCode = statusCode;
        response.ContentType = "application/json";

        var errorResponse = new
        {
            title = "UnauthorizedException",
            status = statusCode,
            detail = message,
            instance = response.HttpContext.Request.Path,
            traceId = response.HttpContext.TraceIdentifier,
            message
        };

        await response.WriteAsync(JsonSerializer.Serialize(errorResponse));
    }
}