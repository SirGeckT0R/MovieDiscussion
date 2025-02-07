using DiscussionServiceApplication.Interfaces.UseCases;
using DiscussionServiceDataAccess.Interfaces.UnitOfWork;
using DiscussionServiceDomain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DiscussionServiceApplication.UseCases.Discussions.Commands.DeleteDiscussionCommand
{
    public class DeleteDiscussionHandler(IUnitOfWork unitOfWork, ILogger<DeleteDiscussionHandler> logger) : ICommandHandler<DeleteDiscussionCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<DeleteDiscussionHandler> _logger = logger;

        public async Task<Unit> Handle(DeleteDiscussionCommand request, CancellationToken cancellationToken)
        {
            var candidate = await _unitOfWork.Discussions.GetByIdTrackingAsync(request.Id, cancellationToken);

            if (candidate == null)
            {
                _logger.LogError("Delete discussion command failed for {DiscussionId}: discussion not found", request.Id);

                throw new NotFoundException("Discussion not found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            _unitOfWork.Discussions.Delete(candidate, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
