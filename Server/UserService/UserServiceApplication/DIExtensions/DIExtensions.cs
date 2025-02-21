using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using UserServiceApplication.RabbitMQ.Connection;
using UserServiceApplication.RabbitMQ.Consumer;
using UserServiceApplication.RabbitMQ.Options;

namespace UserServiceApplication.DIExtensions
{
    public static class DIExtensions
    {
        public static void AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMQConnectionOptions>(configuration.GetSection("RabbitMQConnectionOptions"));

            services.AddSingleton<IRabbitMQConnection>(provider =>
                {
                    var rabbitMQConnectionOptions = provider.GetRequiredService<IOptions<RabbitMQConnectionOptions>>().Value;

                    return new RabbitMQConnection(rabbitMQConnectionOptions);
                }
            );

            services.AddTransient<IMessageConsumer, MessageConsumer>();
            services.AddHostedService<SubscriptionConsumer>();
        }
    }
}
