using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using DiscussionServiceApplication.Behaviors;
using DiscussionServiceApplication.RabbitMQ.Connection;
using DiscussionServiceApplication.RabbitMQ.Producer;
using DiscussionServiceApplication.RabbitMQ.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using DiscussionServiceApplication.RabbitMQ.Service;

namespace DiscussionServiceApplication.Extensions
{
    public static class DIExtensions
    {
        public static void AddMediatR(this IServiceCollection services)
        {
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public static void AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMQConnectionOptions>(configuration.GetSection("RabbitMQConnectionOptions"));
            
            services.AddSingleton<IRabbitMQConnection>(provider =>
                { 
                    var rabbitMQConnectionOptions = provider.GetRequiredService<IOptions<RabbitMQConnectionOptions>>().Value;

                    return new RabbitMQConnection(rabbitMQConnectionOptions);
                }
            );

            services.AddScoped<IMessageProducer, MessageProducer>();
            services.AddScoped<IRabbitMQService, RabbitMQService>();
        }
    }
}
