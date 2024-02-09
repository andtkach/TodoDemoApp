using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Threading.Channels;
using Tasks.Api.Database;
using Tasks.Api.Endpoints;
using Tasks.Api.Extensions;
using Tasks.Api.Mapper;
using Tasks.Api.Processors;
using Tasks.Api.Services;
using Tasks.Common.Message;

var allowAllOrigins = "AllowAllOrigins";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowAllOrigins,
        builder =>
        {
            builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin();
        });
});

builder.Services.AddAutoMapper(typeof(MapperProfile));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(Channel.CreateBounded<MessageTask>(100));
builder.Services.AddHostedService<TaskProcessor>();

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();

    busConfigurator.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(new Uri(builder.Configuration["MessageBroker:Host"]!), h =>
        {
            h.Username(builder.Configuration["MessageBroker:Username"]);
            h.Password(builder.Configuration["MessageBroker:Password"]);
        });

        configurator.ConfigureEndpoints(context);
    });
});


builder.Services.AddScoped<ILoggedInUserService, LoggedInUserService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthorization();

builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddStackExchangeRedisCache(options =>
    options.Configuration = builder.Configuration.GetConnectionString("Cache"));

builder.Services.AddJwtAuthentication(builder);

var app = builder.Build();
app.UseAuthentication();
app.UseCors(allowAllOrigins);
app.UseSwagger();
app.UseSwaggerUI();
app.ApplyMigrations();
app.UseAuthorization();
app.MapInfoEndpoints();
app.MapTaskEndpoints();
app.Run();
