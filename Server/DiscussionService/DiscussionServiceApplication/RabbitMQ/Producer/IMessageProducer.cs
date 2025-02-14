namespace DiscussionServiceApplication.RabbitMQ.Producer
{
    public interface IMessageProducer
    {
        Task SendMessageAsync<T>(T message, string queueName, CancellationToken cancellationToken);
    }
}
