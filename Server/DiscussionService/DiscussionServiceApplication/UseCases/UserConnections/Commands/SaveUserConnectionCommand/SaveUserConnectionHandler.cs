using DiscussionServiceApplication.Dto;
using DiscussionServiceApplication.Interfaces.UseCases;
using DiscussionServiceDataAccess.Interfaces.UnitOfWork;
using DiscussionServiceDomain.Exceptions;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace DiscussionServiceApplication.UseCases.UserConnections.Commands.SaveUserConnectionCommand
{
    public class SaveUserConnectionHandler(IUnitOfWork unitOfWork, IDistributedCache cache) : ICommandHandler<SaveUserConnectionCommand, UserConnection>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IDistributedCache _cache = cache;

        public async Task<UserConnection> Handle(SaveUserConnectionCommand request, CancellationToken cancellationToken)
        {
            Guid accountId;
            var isAccountIdCorrect = Guid.TryParse(request.AccountIdClaimValue, out accountId);

            if (!isAccountIdCorrect)
            {
                throw new UnauthorizedException("No account Id was found");
            }

            Guid discussionId;
            var isDiscussionIdCorrect = Guid.TryParse(request.DiscussionId, out discussionId);

            if (!isDiscussionIdCorrect)
            {
                throw new BadRequestException("Incorrect discussion id");
            }

            //check if user is in database
            var username = "";

            cancellationToken.ThrowIfCancellationRequested();
            var discussion = await _unitOfWork.Discussions.GetByIdAsync(discussionId, cancellationToken);

            if (discussion == null)
            {
                throw new NotFoundException("Discussion not found");
            }

            if (!discussion.IsActive)
            {
                throw new UnauthorizedException("Discussion is not active");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var userConnection = new UserConnection(discussion.Id, accountId, username);
            var connectionString = JsonSerializer.Serialize(userConnection);

            cancellationToken.ThrowIfCancellationRequested();
            await _cache.SetStringAsync(request.ConnectionId, connectionString);

            return userConnection;
        }
    }
}
