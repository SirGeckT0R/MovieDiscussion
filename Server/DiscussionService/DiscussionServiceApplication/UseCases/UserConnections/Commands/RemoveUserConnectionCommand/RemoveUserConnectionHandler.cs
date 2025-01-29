﻿using DiscussionServiceApplication.Dto;
using DiscussionServiceApplication.Interfaces.UseCases;
using DiscussionServiceDomain.Exceptions;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace DiscussionServiceApplication.UseCases.UserConnections.Commands.RemoveUserConnectionCommand
{
    public class RemoveUserConnectionHandler(IDistributedCache cache) : ICommandHandler<RemoveUserConnectionCommand, UserConnection>
    {
        private readonly IDistributedCache _cache = cache;

        public async Task<UserConnection> Handle(RemoveUserConnectionCommand request, CancellationToken cancellationToken)
        {
            var stringConnection = await _cache.GetStringAsync(request.ConnectionId);

            if (string.IsNullOrWhiteSpace(stringConnection))
            {
                throw new NotFoundException("No connection was found in cache");
            }

            var connection = JsonSerializer.Deserialize<UserConnection>(stringConnection);

            if (connection == null)
            {
                throw new NotFoundException("No user connection was found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            await _cache.RemoveAsync(request.ConnectionId);

            return connection;
        }
    }
}
