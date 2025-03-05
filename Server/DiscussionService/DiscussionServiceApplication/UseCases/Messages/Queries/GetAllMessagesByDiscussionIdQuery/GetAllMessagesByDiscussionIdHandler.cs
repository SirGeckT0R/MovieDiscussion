using AutoMapper;
using DiscussionServiceApplication.Dto;
using DiscussionServiceApplication.Interfaces.UseCases;
using DiscussionServiceDataAccess.Interfaces.UnitOfWork;
using DiscussionServiceDataAccess.Specifications.Messages;

namespace DiscussionServiceApplication.UseCases.Messages.Queries.GetAllMessagesByDiscussionIdQuery
{
    public class GetAllMessagesByDiscussionIdHandler(IUnitOfWork unitOfWork, 
                                                     IMapper mapper) 
                                                    : IQueryHandler<GetAllMessagesByDiscussionIdQuery, ICollection<MessageDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<ICollection<MessageDto>> Handle(GetAllMessagesByDiscussionIdQuery request, CancellationToken cancellationToken)
        {
            var specification = new MessagesByDiscussionIdSpecification(request.DiscussionId);
            var messages = await _unitOfWork.Messages.GetWithSpecificationAsync(specification, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            return _mapper.Map<ICollection<MessageDto>>(messages);
        }
    }
}
