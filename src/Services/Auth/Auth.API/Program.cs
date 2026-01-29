using Auth.Infrastructure;
using Auth.Application;
using BuildingBlocks.Exceptions.Handler;
using Carter;
using Auth.Application.RemoteServices.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configurar Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registrar Carter para Minimal APIs
builder.Services.AddCarter();

// Registrar Application y Infrastructure
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);

// Habilita la inyección de dependencias de los servicios remotos usando Scrutor
builder.Services.Scan(scan => scan
    .FromAssemblyOf<IRemoteServiceContext>()
    .AddClasses(c => 
        c.Where(t => t.Namespace != null && 
                     t.Namespace.Contains(".Modules.") && 
                     t.Namespace.EndsWith(".Services")))
    .AsImplementedInterfaces()
    .WithScopedLifetime());

builder.Services.AddScoped<IRemoteServiceContext, RemoteServiceContext>();

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

// Registrar el manejador de excepciones
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Usar el manejador de excepciones
app.UseExceptionHandler(options => { });

app.UseCors("AllowOrigin");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Mapear endpoints de Carter
app.MapCarter();

app.MapControllers();

app.Run();