using G3ReservationAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<G3ReservationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        x => x.MigrationsHistoryTable("__EFMigrationsHistory", "reservation")));

builder.Services.AddHttpClient();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
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