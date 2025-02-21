using DiscussionServiceApplication.RabbitMQ.Options;
using DiscussionServiceApplication.RabbitMQ.Producer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DiscussionServiceApplication.RabbitMQ.Service
{
    public class RabbitMQService(IOptions<RabbitMQConnectionOptions> options, 
                                 IMessageProducer messageProducer,
                                 ILogger<RabbitMQService> logger) : IRabbitMQService
    {
        private readonly RabbitMQConnectionOptions _options = options.Value;
        private readonly IMessageProducer _messageProducer = messageProducer;
        private readonly ILogger<RabbitMQService> _logger = logger;

        public async Task SendSubscriptionMessage<T>(T message, CancellationToken cancellationToken)
        {
            var isSubscriptionNameEmpty = string.IsNullOrWhiteSpace(_options.SubscriptionQueueName);

            if (isSubscriptionNameEmpty)
            {
                _logger.LogWarning("No subscription queue name provided");

                return;
            }

            await _messageProducer.SendMessageAsync(message, _options.SubscriptionQueueName, cancellationToken);
        }
    }
}
