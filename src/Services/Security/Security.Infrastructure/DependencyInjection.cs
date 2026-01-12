using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Security.Application.Data;
using Security.Application.Interfaces.Persistance;
using Security.Infrastructure.Data.Repositories;
namespace Security.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Database");

            // Comentado temporalmente para aplicar migraciones
            //services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
            //services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

            services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                //options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                options.UseSqlServer(connectionString);
            });

            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

            services.AddScoped<DapperDataContext>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
