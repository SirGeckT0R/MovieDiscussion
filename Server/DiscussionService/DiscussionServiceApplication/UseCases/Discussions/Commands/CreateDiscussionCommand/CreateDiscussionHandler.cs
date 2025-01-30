using AutoMapper;
using DiscussionServiceApplication.Interfaces.UseCases;
using DiscussionServiceDataAccess.Interfaces.UnitOfWork;
using DiscussionServiceDomain.Models;
using MediatR;

namespace DiscussionServiceApplication.UseCases.Discussions.Commands.CreateDiscussionCommand
{
    public class CreateDiscussionHandler(IUnitOfWork unitOfWork, IMapper mapper) : ICommandHandler<CreateDiscussionCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Unit> Handle(CreateDiscussionCommand request, CancellationToken cancellationToken)
        {
            var discussion = _mapper.Map<Discussion>(request);
            await _unitOfWork.Discussions.AddAsync(discussion, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
