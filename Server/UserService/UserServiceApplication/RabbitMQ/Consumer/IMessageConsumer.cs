using RabbitMQ.Client.Events;

namespace UserServiceApplication.RabbitMQ.Consumer
{
    public interface IMessageConsumer
    {
        Task ConsumeAsync(string queueName, AsyncEventHandler<BasicDeliverEventArgs> eventHandler, CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);
    }
}
