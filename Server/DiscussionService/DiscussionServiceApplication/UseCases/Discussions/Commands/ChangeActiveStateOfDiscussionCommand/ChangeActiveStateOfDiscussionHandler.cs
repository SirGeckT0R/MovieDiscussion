using DiscussionServiceApplication.Interfaces.UseCases;
using DiscussionServiceDataAccess.Interfaces.UnitOfWork;
using DiscussionServiceDomain.Exceptions;
using MediatR;

namespace DiscussionServiceApplication.UseCases.Discussions.Commands.ChangeActiveStateOfDiscussionCommand
{
    public class ChangeActiveStateOfDiscussionHandler(IUnitOfWork unitOfWork) : ICommandHandler<ChangeActiveStateOfDiscussionCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Unit> Handle(ChangeActiveStateOfDiscussionCommand request, CancellationToken cancellationToken)
        {
            var discussion = await _unitOfWork.Discussions.GetByIdTrackingAsync(request.Id, cancellationToken);

            if (discussion == null)
            {
                throw new NotFoundException("Discussion not found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            discussion.IsActive = request.NewState; 
            _unitOfWork.Discussions.Update(discussion, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return  Unit.Value;
        }
    }
}
