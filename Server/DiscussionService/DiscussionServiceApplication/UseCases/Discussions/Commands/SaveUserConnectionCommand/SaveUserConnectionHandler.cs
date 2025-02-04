using DiscussionServiceApplication.Dto;
using DiscussionServiceApplication.Interfaces.UseCases;
using DiscussionServiceDataAccess.Interfaces.UnitOfWork;
using DiscussionServiceDomain.Exceptions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace DiscussionServiceApplication.UseCases.Discussions.Commands.SaveUserConnectionCommand
{
    public class SaveUserConnectionHandler(IUnitOfWork unitOfWork, IDistributedCache cache, ILogger<SaveUserConnectionHandler> logger) : ICommandHandler<SaveUserConnectionCommand, UserConnection>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IDistributedCache _cache = cache;
        private readonly ILogger<SaveUserConnectionHandler> _logger = logger;

        public async Task<UserConnection> Handle(SaveUserConnectionCommand request, CancellationToken cancellationToken)
        {
            Guid accountId;
            var isAccountIdCorrect = Guid.TryParse(request.AccountIdClaimValue, out accountId);

            if (!isAccountIdCorrect)
            {
                _logger.LogError("Save user connection command failed for {AccountId}: account Id not found", request.AccountIdClaimValue);

                throw new UnauthorizedException("Account Id not found");
            }

            Guid discussionId;
            var isDiscussionIdCorrect = Guid.TryParse(request.DiscussionId, out discussionId);

            if (!isDiscussionIdCorrect)
            {
                _logger.LogError("Save user connection command failed for {DiscussionId}: incorrect discussion id", request.DiscussionId);

                throw new BadRequestException("Incorrect discussion id");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var discussion = await _unitOfWork.Discussions.GetByIdAsync(discussionId, cancellationToken);

            if (discussion == null)
            {
                _logger.LogError("Save user connection command failed for {DiscussionId}: discussion not found", discussionId);

                throw new NotFoundException("Discussion not found");
            }

            if (!discussion.IsActive)
            {
                _logger.LogError("Save user connection command failed for {DiscussionId}: discussion is not active", discussionId);

                throw new UnauthorizedException("Discussion is not active");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var userConnection = new UserConnection(discussion.Id, accountId);
            var connectionString = JsonSerializer.Serialize(userConnection);

            cancellationToken.ThrowIfCancellationRequested();
            await _cache.SetStringAsync(request.ConnectionId, connectionString);

            return userConnection;
        }
    }
}
