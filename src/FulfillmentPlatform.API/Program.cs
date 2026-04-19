using FulfillmentPlatform.API.Extensions;
using FulfillmentPlatform.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddPlatformServices(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<TenantResolutionMiddleware>();

app.MapControllers();

app.Run();

public partial class Program;
