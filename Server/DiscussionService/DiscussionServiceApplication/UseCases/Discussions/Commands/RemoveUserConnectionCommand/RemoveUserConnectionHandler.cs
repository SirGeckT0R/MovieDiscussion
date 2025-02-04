using DiscussionServiceApplication.Dto;
using DiscussionServiceApplication.Interfaces.UseCases;
using DiscussionServiceDomain.Exceptions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace DiscussionServiceApplication.UseCases.Discussions.Commands.RemoveUserConnectionCommand
{
    public class RemoveUserConnectionHandler(IDistributedCache cache, ILogger<RemoveUserConnectionHandler> logger) : ICommandHandler<RemoveUserConnectionCommand, UserConnection>
    {
        private readonly IDistributedCache _cache = cache;
        private readonly ILogger<RemoveUserConnectionHandler> _logger = logger;

        public async Task<UserConnection> Handle(RemoveUserConnectionCommand request, CancellationToken cancellationToken)
        {
            var stringConnection = await _cache.GetStringAsync(request.ConnectionId, cancellationToken);

            if (string.IsNullOrWhiteSpace(stringConnection))
            {
                _logger.LogError("Remove user connection command failed for {ConnectionId}: user connection not found in cache", request.ConnectionId);

                throw new NotFoundException("User connection not found in cache");
            }

            var connection = JsonSerializer.Deserialize<UserConnection>(stringConnection);

            if (connection == null)
            {
                _logger.LogError("Remove user connection command failed for {ConnectionId}: user connection can't be parsed", request.ConnectionId);

                throw new CacheException("User connection can't be parsed");
            }

            cancellationToken.ThrowIfCancellationRequested();
            await _cache.RemoveAsync(request.ConnectionId, cancellationToken);

            return connection;
        }
    }
}
