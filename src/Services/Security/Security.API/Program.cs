using Carter;
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

var app = builder.Build();

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