using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace Auth.API.RateLimiting;

public static class RateLimitPolicies
{
    /// <summary>
    /// Endpoints públicos del flujo de restablecimiento de contraseña
    /// (solicitud, validación del token y cambio).
    /// </summary>
    public const string PasswordReset = "password-reset";

    // Peticiones permitidas por IP en cada ventana, sumando todos los endpoints de la política
    private const int PasswordResetPermitLimit = 10;
    private static readonly TimeSpan PasswordResetWindow = TimeSpan.FromMinutes(1);

    public static IServiceCollection AddAuthRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.AddPolicy(PasswordReset, httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = PasswordResetPermitLimit,
                        Window = PasswordResetWindow,
                        QueueLimit = 0
                    }));
        });

        return services;
    }
}
