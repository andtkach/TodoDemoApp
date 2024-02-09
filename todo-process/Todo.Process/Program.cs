using MassTransit;
using Todo.Process.Consumers;
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
