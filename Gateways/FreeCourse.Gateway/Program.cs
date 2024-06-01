using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication().AddJwtBearer("GatewayAuthenticationScheme", options => 
{
	options.Authority = builder.Configuration["IdentityServerURL"];
	options.Audience = "resource_gateway";
	options.RequireHttpsMetadata = false;
});

builder.Configuration
	.SetBasePath(builder.Environment.ContentRootPath)
	.AddJsonFile("appsettings.json", true, true)
	.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
	.AddJsonFile($"configuration.{builder.Environment.EnvironmentName}.json")
	.AddEnvironmentVariables();

builder.Services.AddOcelot();

var app = builder.Build();

await app.UseOcelot();

app.MapGet("/", () => "Hello World!");

app.Run();
