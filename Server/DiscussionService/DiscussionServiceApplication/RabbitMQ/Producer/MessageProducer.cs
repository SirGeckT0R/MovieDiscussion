using DiscussionServiceApplication.RabbitMQ.Connection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace DiscussionServiceApplication.RabbitMQ.Producer
{
    public class MessageProducer(IRabbitMQConnection connection,
                                 ILogger<MessageProducer> logger) : IMessageProducer
    {
        private readonly IRabbitMQConnection _connection = connection;
        private readonly ILogger<MessageProducer> _logger = logger;

        public async Task SendMessageAsync<T>(T message, string queueName, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Sending message to RabbitMQ queue");

            var establishedConnection = await _connection.GetConnectionAsync(cancellationToken);

            using var channel = await establishedConnection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queueName, exclusive: false);

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            await channel.BasicPublishAsync(exchange: "", routingKey: queueName, body: body);

            _logger.LogInformation("Message was sent to RabbitMQ queue");
        }
    }
}
