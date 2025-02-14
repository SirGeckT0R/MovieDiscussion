using DiscussionServiceApplication.RabbitMQ.Options;
using RabbitMQ.Client;

namespace DiscussionServiceApplication.RabbitMQ.Connection
{
    public class RabbitMQConnection : IRabbitMQConnection, IDisposable
    {
        private IConnection? _connection;

        private readonly RabbitMQConnectionOptions _options;

        private readonly SemaphoreSlim _connectionLock = new SemaphoreSlim(1, 1);
        private Task<IConnection> _taskConnection;

        private bool disposed = false;

        public RabbitMQConnection(RabbitMQConnectionOptions options)
        {
            _options = options;
            InitializeConnection();
        }

        private void InitializeConnection()
        {
            var factory = new ConnectionFactory
            {
                HostName = _options.HostName,
                Port = _options.Port,
                UserName = _options.UserName,
                Password = _options.Password,
            };

            _taskConnection = factory.CreateConnectionAsync();
        }

        public async ValueTask<IConnection> GetConnectionAsync(CancellationToken cancellationToken)
        {
            if (_connection != null)
            {
                return _connection;
            }

            await _connectionLock.WaitAsync(cancellationToken);
            try
            {
                if (_connection != null)
                {
                    return _connection;
                }

                _connection = await _taskConnection;

                return _connection;
            }
            finally
            {
                _connectionLock.Release();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                _connection?.Dispose();
            }

            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
