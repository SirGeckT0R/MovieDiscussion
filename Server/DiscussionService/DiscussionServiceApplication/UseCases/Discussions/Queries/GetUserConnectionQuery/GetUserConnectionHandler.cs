using DiscussionServiceApplication.Dto;
using DiscussionServiceApplication.Interfaces.UseCases;
using DiscussionServiceDomain.Exceptions;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace DiscussionServiceApplication.UseCases.Discussions.Queries.GetUserConnectionQuery
{
    public class GetUserConnectionHandler(IDistributedCache cache) : IQueryHandler<GetUserConnectionQuery, UserConnection>
    {
        private readonly IDistributedCache _cache = cache;

        public async Task<UserConnection> Handle(GetUserConnectionQuery request, CancellationToken cancellationToken)
        {
            var stringConnection = await _cache.GetStringAsync(request.ConnectionId, cancellationToken);

            if (string.IsNullOrWhiteSpace(stringConnection))
            {
                throw new NotFoundException("User connection not found in cache");
            }

            var userConnection = JsonSerializer.Deserialize<UserConnection>(stringConnection);

            if (userConnection == null)
            {
                throw new CacheException("User connection can't be parsed");
            }

            return userConnection;
        }
    }
}
