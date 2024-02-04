using Microsoft.EntityFrameworkCore;
using Tasks.Api.Database;
using Tasks.Api.Endpoints;
using Tasks.Api.Extensions;
using Tasks.Api.Mapper;

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

builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddStackExchangeRedisCache(options =>
    options.Configuration = builder.Configuration.GetConnectionString("Cache"));

var app = builder.Build();
app.UseCors(allowAllOrigins);

app.UseSwagger();
app.UseSwaggerUI();
app.ApplyMigrations();

app.MapTaskEndpoints();

app.Run();
