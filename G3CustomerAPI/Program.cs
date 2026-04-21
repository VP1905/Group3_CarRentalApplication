using Azure.Monitor.OpenTelemetry.AspNetCore;
using G3CustomerAPI.Data;
using G3SharedKernel.Extensions;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var appInsightsConnection = builder.Configuration["ApplicationInsights:ConnectionString"];

var otelBuilder = builder.Services.AddOpenTelemetry()
    .WithTracing(tracing =>
    {
        tracing.AddAspNetCoreInstrumentation();
        tracing.AddHttpClientInstrumentation();
    })
    .WithMetrics(metrics =>
    {
        metrics.AddAspNetCoreInstrumentation();
        metrics.AddHttpClientInstrumentation();
    });

if (!string.IsNullOrWhiteSpace(appInsightsConnection) &&
    appInsightsConnection != "PLACEHOLDER_FILL_AFTER_AZURE_SETUP")
{
    otelBuilder.UseAzureMonitor();
}

builder.Services.AddControllers();

builder.Services.AddDbContext<G3CustomerDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        x => x.MigrationsHistoryTable("__EFMigrationsHistory", "customer")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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