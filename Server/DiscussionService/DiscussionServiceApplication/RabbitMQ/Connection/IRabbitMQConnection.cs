using RabbitMQ.Client;

namespace DiscussionServiceApplication.RabbitMQ.Connection
{
    public interface IRabbitMQConnection
    {
        ValueTask<IConnection> GetConnectionAsync(CancellationToken cancellationToken);
    }
}
