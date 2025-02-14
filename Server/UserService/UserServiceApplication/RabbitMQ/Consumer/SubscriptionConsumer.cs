using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using UserServiceApplication.Interfaces.Services;
using UserServiceApplication.RabbitMQ.Dto;
using UserServiceApplication.RabbitMQ.Options;

namespace UserServiceApplication.RabbitMQ.Consumer
{
    public class SubscriptionConsumer(IMessageConsumer messageConsumer,
                                 IOptions<RabbitMQConnectionOptions> options,
                                 IServiceScopeFactory serviceScopeFactory,
                                 ILogger<SubscriptionConsumer> logger) : BackgroundService
    {
        private readonly IMessageConsumer _messageConsumer = messageConsumer;
        private readonly RabbitMQConnectionOptions _options = options.Value;
        private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
        private readonly ILogger<SubscriptionConsumer> _logger = logger;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var isSubscriptionNameEmpty = string.IsNullOrWhiteSpace(_options.SubscriptionQueueName);

            if (isSubscriptionNameEmpty)
            {
                _logger.LogWarning("No subscription queue name provided");

                return;
            }

            await _messageConsumer.ConsumeAsync(_options.SubscriptionQueueName, SubscriptionRecievedAsync, stoppingToken);
        }
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await _messageConsumer.StopAsync(cancellationToken);

            await base.StopAsync(cancellationToken);
        }

        private async Task SubscriptionRecievedAsync(object sender, BasicDeliverEventArgs eventArgs)
        {
            var body = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            _logger.LogInformation("Recieved message {Message}", message);

            DiscussionActivationDto? messageDto;
            var isValidMessage = TryExtractMessage(message, out messageDto);

            if (!isValidMessage)
            {
                return;
            }

            var subscribers = messageDto!.Subscribers;

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                var users = await userService.GetFromCollectionAsync(subscribers, eventArgs.CancellationToken);
                var emails = users.Select(x => x.Email).ToList();

                if (emails.Count != subscribers.Count)
                {
                    _logger.LogWarning("Not all subscribers were found");
                }

                foreach (var email in emails)
                {
                    _logger.LogInformation("Sending notification to {Email} about discussion {DiscussionName}", email, messageDto.DiscussionName);

                    await emailService.SendEmailAsync(email, $"{messageDto.DiscussionName} has started", "Don't miss it!", eventArgs.CancellationToken);
                }
            }
            
            _logger.LogInformation("Proccessing of a message is done");
        }

        private bool TryExtractMessage(string message, out DiscussionActivationDto? messageDto)
        {
            messageDto = null;

            var isMessageEmpty = string.IsNullOrWhiteSpace(message);

            if (isMessageEmpty)
            {
                _logger.LogWarning("Empty message");

                return false;
            }

            var dto = JsonSerializer.Deserialize<DiscussionActivationDto>(message);

            if (dto == null)
            {
                _logger.LogWarning("No correct dto was deserialized");

                return false;
            }

            var subscribers = dto.Subscribers;

            if (subscribers == null || subscribers.Count == 0)
            {
                _logger.LogWarning("No subscribers to notify");

                return false;
            }

            messageDto = dto;

            return true;
        }
    }
}
