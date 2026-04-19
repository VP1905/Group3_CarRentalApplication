var builder = WebApplication.CreateBuilder(args);

// Added YARP Reverse Proxy
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseHttpsRedirection();

// Enable Gateway Routing
app.MapReverseProxy();

app.Run();