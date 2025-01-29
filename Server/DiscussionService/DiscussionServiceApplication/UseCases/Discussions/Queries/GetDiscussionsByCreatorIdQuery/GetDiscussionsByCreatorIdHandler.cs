using AutoMapper;
using DiscussionServiceApplication.Dto;
using DiscussionServiceApplication.Interfaces.UseCases;
using DiscussionServiceDataAccess.Interfaces.UnitOfWork;
using DiscussionServiceDataAccess.Specifications.Discussions;

namespace DiscussionServiceApplication.UseCases.Discussions.Queries.GetDiscussionsByCreatorIdQuery
{
    public class GetDiscussionsByCreatorIdHandler(IUnitOfWork unitOfWork, IMapper mapper) : IQueryHandler<GetDiscussionsByCreatorIdQuery, ICollection<DiscussionDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork; 
        private readonly IMapper _mapper = mapper;

        public async Task<ICollection<DiscussionDto>> Handle(GetDiscussionsByCreatorIdQuery request, CancellationToken cancellationToken)
        {
            var specification = new DiscussionsByCreatorIdSpecification(request.CreatorProfileId);
            var discussions = await _unitOfWork.Discussions.GetWithSpecificationAsync(specification, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            return _mapper.Map<ICollection<DiscussionDto>>(discussions);
        }
    }
}
