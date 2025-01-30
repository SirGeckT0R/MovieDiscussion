using DiscussionServiceApplication.Interfaces.UseCases;
using DiscussionServiceDataAccess.Interfaces.UnitOfWork;
using DiscussionServiceDomain.Exceptions;
using MediatR;

namespace DiscussionServiceApplication.UseCases.Discussions.Commands.DeleteDiscussionCommand
{
    public class DeleteDiscussionHandler(IUnitOfWork unitOfWork) : ICommandHandler<DeleteDiscussionCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Unit> Handle(DeleteDiscussionCommand request, CancellationToken cancellationToken)
        {
            var candidate = await _unitOfWork.Discussions.GetByIdTrackingAsync(request.Id, cancellationToken);

            if (candidate == null)
            {
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
