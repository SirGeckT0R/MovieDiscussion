namespace DiscussionServiceApplication.RabbitMQ.Service
{
    public interface IRabbitMQService
    {
        Task SendSubscriptionMessage<T>(T message, CancellationToken cancellationToken);
    }
}
