using DiscussionServiceApplication.Interfaces.UseCases;
using DiscussionServiceDataAccess.Interfaces.UnitOfWork;
using DiscussionServiceDomain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DiscussionServiceApplication.UseCases.Discussions.Commands.ChangeActiveStateOfDiscussionCommand
{
    public class ChangeActiveStateOfDiscussionHandler(IUnitOfWork unitOfWork, ILogger<ChangeActiveStateOfDiscussionHandler> logger) : ICommandHandler<ChangeActiveStateOfDiscussionCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<ChangeActiveStateOfDiscussionHandler> _logger = logger;

        public async Task<Unit> Handle(ChangeActiveStateOfDiscussionCommand request, CancellationToken cancellationToken)
        {
            var discussion = await _unitOfWork.Discussions.GetByIdTrackingAsync(request.Id, cancellationToken);

            if (discussion == null)
            {
                _logger.LogError("Change active state of discussion command failed for {DiscussionId}: discussion not found", request.Id);

                throw new NotFoundException("Discussion not found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            discussion.IsActive = request.NewState; 
            _unitOfWork.Discussions.Update(discussion, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
