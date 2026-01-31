using Messaging.API;
using Messaging.API.Interceptors;
using Messaging.Application;
using Messaging.Infrastructure;
using Quartz;
using Messaging.Application.Jobs;
using Messaging.Application.Configuration;
using Messaging.Application.Utilities;
using Messaging.Application.Configuraction;

var builder = WebApplication.CreateBuilder(args);

// Configurar Quartz para procesar emails cada minuto
builder.Services.AddQuartz(q =>
{
    var jobKey = new JobKey("SendEmailJob");
    q.AddJob<SendEmailJob>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("SendEmailJob-trigger")
        .WithCronSchedule("0 0/1 * * * ?", x => x.WithMisfireHandlingInstructionFireAndProceed())
    );
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

// Add services to the container.
builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices()
    .AddSingleton<ErrorHandlingInterceptor>();

builder.Services.AddSingleton<AppConstants>();
builder.Services.AddSingleton<MailSenderUtility>();

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin",
        policyBuilder =>
        {
            policyBuilder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowOrigin");

app.UseApiServices();

app.MapControllers();

app.Run();
