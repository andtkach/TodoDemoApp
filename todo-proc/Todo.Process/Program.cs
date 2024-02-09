using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Todo.Process.Consumers;
using Todo.Process.Database;
using Todo.Process.Mapper;

namespace Todo.Process
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAutoMapper(typeof(MapperProfile));

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddMassTransit(busConfigurator =>
            {
                busConfigurator.SetKebabCaseEndpointNameFormatter();

                busConfigurator.AddConsumer<MessageConsumer>();
                
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

            builder.Services.AddDbContext<ApplicationDbContext>(
                options => options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));

            builder.Services.AddHttpClient();

            var oaiKey = builder.Configuration["OpenAI:Key"];
            Console.WriteLine($"OpenAI Key: {oaiKey}");
            
            builder.Services.AddHttpClient("OpenAI", httpClient =>
            {
                httpClient.BaseAddress = new Uri(builder.Configuration["OpenAI:Url"] ?? throw new ConfigurationException("OpenAI url is not defined"));

                httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
                httpClient.DefaultRequestHeaders.Add(HeaderNames.Authorization, $"Bearer {oaiKey}");
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
