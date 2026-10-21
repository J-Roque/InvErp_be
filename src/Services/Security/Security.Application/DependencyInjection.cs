using System.Reflection;
using BuildingBlocks.Behaviors;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Security.Application.Configuration;
using Security.Application.RemoteServices.Modules.Attachments.Interfaces;
using Security.Application.RemoteServices.Modules.Attachments.Services;
using Security.Application.RemoteServices.Modules.Auth.Interfaces;
using Security.Application.RemoteServices.Modules.Auth.Services;
using Security.Application.RemoteServices.Modules.Event.Interfaces;
using Security.Application.RemoteServices.Modules.Event.Services;
using Security.Application.RemoteServices.Modules.Messaging.Interfaces;
using Security.Application.RemoteServices.Modules.Messaging.Services;

namespace Security.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddSingleton<AppConstants>();

        // Servicios remotos (llamadas HTTP a otros microservicios)
        services.AddScoped<IAuthRemoteService, AuthRemoteService>();
        services.AddScoped<IEventRemoteService, EventRemoteService>();
        services.AddScoped<IMessagingRemoteService, MessagingRemoteService>();
        services.AddScoped<IAttachmentRemoteService, AttachmentRemoteService>();
        services.AddScoped<IRemoteServiceContext, RemoteServiceContext>();

        return services;
    }
}
