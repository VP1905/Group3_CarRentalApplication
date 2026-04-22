using G3VehicleInventory.Application.Interfaces;
using G3VehicleInventory.Application.Services;
using G3VehicleInventory.Domain.AggregatesModel.VehicleAggregate;
using G3VehicleInventory.Infrastructure.Data;
using G3VehicleInventory.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using G3SharedKernel.Extensions;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

var otlpEndpoint = builder.Configuration["OTLP:Endpoint"] ?? "http://localhost:4317";

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddOpenTelemetry(logging =>
{
    logging.IncludeFormattedMessage = true;
    logging.IncludeScopes = true;
    logging.AddOtlpExporter(o =>
    {
        o.Endpoint = new Uri(otlpEndpoint);
    });
});

// OpenTelemetry
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing =>
    {
        tracing.AddAspNetCoreInstrumentation();
        tracing.AddHttpClientInstrumentation();
        tracing.AddOtlpExporter(o =>
        {
            o.Endpoint = new Uri(otlpEndpoint);
        });
    })
    .WithMetrics(metrics =>
    {
        metrics.AddAspNetCoreInstrumentation();
        metrics.AddHttpClientInstrumentation();
        metrics.AddOtlpExporter(o =>
        {
            o.Endpoint = new Uri(otlpEndpoint);
        });
    });

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

app.UseSwagger();
app.UseSwaggerUI();

app.UseGR3GlobalExceptionMiddleware();
app.UseGR3ApiKeyMiddleware();

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