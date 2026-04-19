using G3VehicleInventory.Application.Interfaces;
using G3VehicleInventory.Application.Services;
using G3VehicleInventory.Domain.AggregatesModel.VehicleAggregate;
using G3VehicleInventory.Infrastructure.Data;
using G3VehicleInventory.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// SQL Server DbContext
builder.Services.AddDbContext<InventoryDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Services
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Allow only requests coming through API Gateway
app.Use(async (context, next) =>
{
    var expectedSecret = builder.Configuration["GatewayAccess:InternalSecret"];

    if (!context.Request.Headers.TryGetValue("X-Internal-Gateway", out var providedSecret) ||
        string.IsNullOrWhiteSpace(expectedSecret) ||
        providedSecret != expectedSecret)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        await context.Response.WriteAsync("Unauthorized: Direct API access is not allowed.");
        return;
    }

    await next();
});

app.UseAuthorization();

app.MapControllers();

app.Run();
public partial class Program { }