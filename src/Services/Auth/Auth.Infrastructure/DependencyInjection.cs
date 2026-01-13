using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Auth.Application.Data;
using Auth.Application.Interfaces.Persistence;
using Auth.Infrastructure.Data.Repositories;

namespace Auth.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

        services.AddScoped<DapperDataContext>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
