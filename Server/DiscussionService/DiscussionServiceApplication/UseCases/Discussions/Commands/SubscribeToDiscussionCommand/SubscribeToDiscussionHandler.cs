using DiscussionServiceApplication.Interfaces.UseCases;
using DiscussionServiceDataAccess.Interfaces.UnitOfWork;
using DiscussionServiceDomain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DiscussionServiceApplication.UseCases.Discussions.Commands.SubscribeToDiscussionCommand
{
    public class SubscribeToDiscussionHandler(IUnitOfWork unitOfWork, ILogger<SubscribeToDiscussionHandler> logger) : ICommandHandler<SubscribeToDiscussionCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<SubscribeToDiscussionHandler> _logger = logger;

        public async Task<Unit> Handle(SubscribeToDiscussionCommand request, CancellationToken cancellationToken)
        {
            var discussion = await _unitOfWork.Discussions.GetByIdAsync(request.DiscussionId, cancellationToken);

            if (discussion == null)
            {
                _logger.LogError("Subscibe to discussion command failed for {DiscussionId}: discussion not found", request.DiscussionId);

                throw new NotFoundException("Discussion not found");
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
