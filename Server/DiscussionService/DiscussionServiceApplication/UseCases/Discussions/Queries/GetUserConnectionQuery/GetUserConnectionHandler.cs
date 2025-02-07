using DiscussionServiceApplication.Dto;
using DiscussionServiceApplication.Interfaces.UseCases;
using DiscussionServiceDomain.Exceptions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace DiscussionServiceApplication.UseCases.Discussions.Queries.GetUserConnectionQuery
{
    public class GetUserConnectionHandler(IDistributedCache cache, ILogger<GetUserConnectionHandler> logger) : IQueryHandler<GetUserConnectionQuery, UserConnection>
    {
        private readonly IDistributedCache _cache = cache;
        private readonly ILogger<GetUserConnectionHandler> _logger = logger;

        public async Task<UserConnection> Handle(GetUserConnectionQuery request, CancellationToken cancellationToken)
        {
            var stringConnection = await _cache.GetStringAsync(request.ConnectionId, cancellationToken);

            if (string.IsNullOrWhiteSpace(stringConnection))
            {
                _logger.LogError("Get user connection query failed for {ConnectionId}: user connection not found in cache", request.ConnectionId);

                throw new NotFoundException("User connection not found in cache");
            }

            var userConnection = JsonSerializer.Deserialize<UserConnection>(stringConnection);

            if (userConnection == null)
            {
                _logger.LogError("Get user connection query failed for {ConnectionId}: user connection can't be parsed", request.ConnectionId);

                throw new CacheException("User connection can't be parsed");
            }

            return userConnection;
        }
    }
}
