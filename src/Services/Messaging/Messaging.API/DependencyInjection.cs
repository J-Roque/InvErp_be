using BuildingBlocks.Exceptions.Handler;

namespace Messaging.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddCarter();
        services.AddExceptionHandler<CustomExceptionHandler>();
        services.AddHealthChecks();

        return services;
    }

    public static IApplicationBuilder UseApiServices(this IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseEndpoints(endpoints => endpoints.MapCarter());
        app.UseExceptionHandler(options => { });
        app.UseHealthChecks("/health");
        return app;
    }
}