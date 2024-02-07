using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Tasks.Api.Contracts;

namespace Tasks.Api.Extensions
{
    public static class StartupExtensions
    {
        public static void AddJwtAuthentication(this IServiceCollection services, WebApplicationBuilder builder)
        {
            //IdentityModelEventSource.ShowPII = true;

            var key = builder.Configuration["JwtSettings:Key"];
            if (string.IsNullOrEmpty(key))
            {
                throw new InvalidOperationException("JwtSettings:Key is missing");
            }

            var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                            .AddJwtBearer(o =>
                            {
                                o.RequireHttpsMetadata = false;
                                o.SaveToken = false;
                                o.TokenValidationParameters = new TokenValidationParameters
                                {
                                    ValidateIssuerSigningKey = true,
                                    ValidateIssuer = true,
                                    ValidateAudience = true,
                                    ValidateLifetime = true,
                                    ClockSkew = TimeSpan.Zero,
                                    ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                                    ValidAudience = builder.Configuration["JwtSettings:Audience"],
                                    IssuerSigningKey = new SymmetricSecurityKey(hmac.Key)
                                };

                                o.Events = new JwtBearerEvents()
                                {
                                    OnAuthenticationFailed = c =>
                                    {
                                        c.NoResult();
                                        c.Response.StatusCode = 500;
                                        c.Response.ContentType = "text/plain";
                                        var result = JsonSerializer.Serialize(new ErrorResponse("500", c.Exception.ToString()));
                                        return c.Response.WriteAsync(result);
                                    },
                                    OnChallenge = context =>
                                    {
                                        context.HandleResponse();
                                        context.Response.StatusCode = 401;
                                        context.Response.ContentType = "application/json";
                                        var result = JsonSerializer.Serialize(new ErrorResponse("401", "Not authorized"));
                                        return context.Response.WriteAsync(result);
                                    },
                                    OnForbidden = context =>
                                    {
                                        context.Response.StatusCode = 403;
                                        context.Response.ContentType = "application/json";
                                        var result = JsonSerializer.Serialize(new ErrorResponse("40", "Not authorized"));
                                        return context.Response.WriteAsync(result);
                                    }
                                };
                            });

        }
    }
}
