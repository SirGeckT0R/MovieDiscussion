using DiscussionServiceApplication.Dto;
using DiscussionServiceApplication.Interfaces.UseCases;
using DiscussionServiceDataAccess.Interfaces.UnitOfWork;
using DiscussionServiceDomain.Exceptions;
using DiscussionServiceDomain.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace DiscussionServiceApplication.UseCases.Discussions.Commands.AddMessageToDiscussionCommand
{
    public class AddMessageToDiscussionHandler(IUnitOfWork unitOfWork, IDistributedCache cache) : ICommandHandler<AddMessageToDiscussionCommand, UserConnection>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IDistributedCache _cache = cache;

        public async Task<UserConnection> Handle(AddMessageToDiscussionCommand request, CancellationToken cancellationToken)
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

            cancellationToken.ThrowIfCancellationRequested();
            var discussion = await _unitOfWork.Discussions.GetByIdAsync(userConnection.DiscussionId, cancellationToken);

            if (discussion == null)
            {
                throw new NotFoundException("Discussion not found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var message = new Message(request.Text, userConnection.AccountId);
            discussion.Messages.Add(message);
            _unitOfWork.Discussions.Update(discussion, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return userConnection;
        }
    }
}
