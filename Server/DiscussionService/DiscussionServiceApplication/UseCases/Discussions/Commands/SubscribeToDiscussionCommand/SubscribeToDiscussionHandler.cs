using DiscussionServiceApplication.Interfaces.UseCases;
using DiscussionServiceDataAccess.Interfaces.UnitOfWork;
using DiscussionServiceDomain.Exceptions;
using MediatR;

namespace DiscussionServiceApplication.UseCases.Discussions.Commands.SubscribeToDiscussionCommand
{
    public class SubscribeToDiscussionHandler(IUnitOfWork unitOfWork) : ICommandHandler<SubscribeToDiscussionCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Unit> Handle(SubscribeToDiscussionCommand request, CancellationToken cancellationToken)
        {
            var discussion = await _unitOfWork.Discussions.GetByIdAsync(request.DiscussionId, cancellationToken);

            if (discussion == null)
            {
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
