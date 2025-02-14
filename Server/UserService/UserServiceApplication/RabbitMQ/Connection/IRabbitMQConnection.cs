using RabbitMQ.Client;

namespace UserServiceApplication.RabbitMQ.Connection
{
    public interface IRabbitMQConnection
    {
        ValueTask<IConnection> GetConnectionAsync(CancellationToken cancellationToken);
    }
}
