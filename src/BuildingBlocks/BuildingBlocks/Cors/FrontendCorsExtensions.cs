using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Cors;

public static class FrontendCorsExtensions
{
    public const string PolicyName = "AllowOrigin";

    /// <summary>
    /// Permite peticiones de navegador solo desde el frontend configurado (Frontend:BaseUrl)
    /// y desde los orígenes extra de Cors:AllowedOrigins. Sin orígenes configurados,
    /// ningún navegador puede llamar al servicio; las llamadas entre microservicios no usan CORS.
    /// </summary>
    public static IServiceCollection AddFrontendCors(this IServiceCollection services, IConfiguration configuration)
    {
        var origins = (configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [])
            .Append(configuration["Frontend:BaseUrl"])
            .Where(origin => !string.IsNullOrWhiteSpace(origin))
            .Select(origin => origin!.Trim().TrimEnd('/'))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        services.AddCors(options =>
        {
            options.AddPolicy(PolicyName, policy =>
            {
                if (origins.Length == 0) return;

                policy.WithOrigins(origins)
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        return services;
    }
}
