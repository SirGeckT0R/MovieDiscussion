using AutoMapper;
using DiscussionServiceApplication.Interfaces.UseCases;
using DiscussionServiceDataAccess.Interfaces.UnitOfWork;
using DiscussionServiceDomain.Exceptions;
using MediatR;

namespace DiscussionServiceApplication.UseCases.Discussions.Commands.UpdateDiscussionCommand
{
    public class UpdateDiscussionHandler(IUnitOfWork unitOfWork, IMapper mapper) : ICommandHandler<UpdateDiscussionCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Unit> Handle(UpdateDiscussionCommand request, CancellationToken cancellationToken)
        {
            var candidate = await _unitOfWork.Discussions.GetByIdAsync(request.Id, cancellationToken);

            if (candidate == null)
            {
                throw new NotFoundException("Discussion not found");
            }

            var isAuthorized = candidate.CreatedBy.Equals(request.UpdatedBy);
            if (!isAuthorized)
            {
                throw new UnauthorizedException("Only creator of discussion can update it");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var newDiscussion = _mapper.Map(request, candidate);
            _unitOfWork.Discussions.Update(newDiscussion, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
