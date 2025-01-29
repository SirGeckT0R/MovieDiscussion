using DiscussionServiceApplication.Dto;
using DiscussionServiceApplication.Interfaces.UseCases;
using DiscussionServiceDomain.Exceptions;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace DiscussionServiceApplication.UseCases.UserConnections.Queries.GetUserConnectionQuery
{
    public class GetUserConnectionHandler(IDistributedCache cache) : IQueryHandler<GetUserConnectionQuery, UserConnection>
    {
        private readonly IDistributedCache _cache = cache;

        public async Task<UserConnection> Handle(GetUserConnectionQuery request, CancellationToken cancellationToken)
        {
            var stringConnection = await _cache.GetStringAsync(request.ConnectionId);

            if (string.IsNullOrWhiteSpace(stringConnection))
            {
                throw new NotFoundException("No user connection was found");
            }

            var userConnection = JsonSerializer.Deserialize<UserConnection>(stringConnection);

            if (userConnection == null)
            {
                throw new NotFoundException("No user connection was found");
            }

            return userConnection;
        }
    }
}
