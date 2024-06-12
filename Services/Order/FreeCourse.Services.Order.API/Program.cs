using FreeCourse.Services.Order.Application.Consumers;
using FreeCourse.Services.Order.Application.Handlers;
using FreeCourse.Services.Order.Infrastructure;
using FreeCourse.Shared.Services;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x =>
{
	x.AddConsumer<CreateOrderMessageCommandConsumer>();

	// Default Port : 5672
	x.UsingRabbitMq((context, cfg) =>
	{
		cfg.Host(builder.Configuration["RabbitMQUrl"], "/", host =>
		{
			host.Username("guest");
			host.Password("guest");
		});

		cfg.ReceiveEndpoint("create-order-service", e =>
		{
			e.ConfigureConsumer<CreateOrderMessageCommandConsumer>(context);
		});
	});
});

// Add services to the container.

var requireAuthorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
JsonWebTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
	options.Authority = builder.Configuration["IdentityServerURL"];
	options.Audience = "resource_order";
	options.RequireHttpsMetadata = false;
});

builder.Services.AddDbContext<OrderDbContext>(opt => 
{
	opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), configure => 
	{
		configure.MigrationsAssembly("FreeCourse.Services.Order.Infrastructure");
	});
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetOrdersByUserIdQueryHandler>());

builder.Services.AddScoped<ISharedIdentityService, SharedIdentityService>();

builder.Services.AddControllers(opt => 
{
	opt.Filters.Add(new AuthorizeFilter(requireAuthorizePolicy));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
