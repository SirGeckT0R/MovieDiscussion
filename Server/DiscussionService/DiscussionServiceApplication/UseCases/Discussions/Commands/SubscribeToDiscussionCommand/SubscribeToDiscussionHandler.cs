using DiscussionServiceApplication.Interfaces.UseCases;
using DiscussionServiceDataAccess.Interfaces.UnitOfWork;
using DiscussionServiceDomain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using UserGrpcClient;

namespace DiscussionServiceApplication.UseCases.Discussions.Commands.SubscribeToDiscussionCommand
{
    public class SubscribeToDiscussionHandler(IUnitOfWork unitOfWork, ILogger<SubscribeToDiscussionHandler> logger, UserService.UserServiceClient client) : ICommandHandler<SubscribeToDiscussionCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<SubscribeToDiscussionHandler> _logger = logger;
        private readonly UserService.UserServiceClient _userServiceClient = client;

        public async Task<Unit> Handle(SubscribeToDiscussionCommand request, CancellationToken cancellationToken)
        {
            var discussion = await _unitOfWork.Discussions.GetByIdAsync(request.DiscussionId, cancellationToken);

            if (discussion == null)
            {
                _logger.LogError("Subscribe to discussion command failed for {DiscussionId}: discussion not found", request.DiscussionId);

                throw new NotFoundException("Discussion not found");
            }

            var getUserInfoRequest = new GetUserInfoRequest { UserId = request.AccountId.ToString() };

            var reply = await _userServiceClient.GetUserInfoAsync(getUserInfoRequest);

            if (!reply.IsEmailConfirmed)
            {
                _logger.LogError("Subscribe to discussion command failed for {DiscussionId}: Email is not confirmed", request.DiscussionId);

                throw new BadRequestException("Email is not confirmed");
            }

            cancellationToken.ThrowIfCancellationRequested();
            discussion.Subscribers.Add(request.AccountId);
            _unitOfWork.Discussions.Update(discussion, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}
