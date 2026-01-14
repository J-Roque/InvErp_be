using Auth.Infrastructure;
using Auth.Application;
using BuildingBlocks.Exceptions.Handler;
using Carter;

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

// Registrar el manejador de excepciones
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Usar el manejador de excepciones
app.UseExceptionHandler(options => { });

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