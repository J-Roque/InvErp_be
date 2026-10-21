using BuildingBlocks.Cors;
using BuildingBlocks.Exceptions.Handler;
using Carter;
using Security.API.Configurations;
using Security.API.Context;
using Security.Application;
using Security.Application.Interfaces.Context;
using Security.Infrastructure;
using Security.Infrastructure.Data;
using Security.Infrastructure.Data.Extensions;

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

// Registrar HttpContextAccessor y UserContext para auditoría
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserContext, UserContext>();

builder.Services.ConfigureJwtBearer();

builder.Services.AddAuthorization();

// Configurar CORS: solo el frontend configurado
builder.Services.AddFrontendCors(builder.Configuration);

// Registrar el manejador de excepciones
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Usar el manejador de excepciones
app.UseExceptionHandler(options => { });

app.UseCors(FrontendCorsExtensions.PolicyName);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    await app.InitialiseDatabaseAsync();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Mapear endpoints de Carter
app.MapCarter();

app.MapControllers();

app.Run();