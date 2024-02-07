using Microsoft.EntityFrameworkCore;
using Tasks.Api.Database;
using Tasks.Api.Endpoints;
using Tasks.Api.Extensions;
using Tasks.Api.Mapper;
using Tasks.Api.Services;

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
app.MapTaskEndpoints();
app.Run();
