using AutoMapper;
using DiscussionServiceApplication.Dto;
using DiscussionServiceApplication.Interfaces.UseCases;
using DiscussionServiceDataAccess.Interfaces.UnitOfWork;

namespace DiscussionServiceApplication.UseCases.Discussions.Queries.GetAllDiscussionsQuery
{
    public class GetAllDiscussionsHandler(IUnitOfWork unitOfWork, IMapper mapper) : IQueryHandler<GetAllDiscussionsQuery, ICollection<DiscussionDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<ICollection<DiscussionDto>> Handle(GetAllDiscussionsQuery request, CancellationToken cancellationToken)
        {
            var discussions = await _unitOfWork.Discussions.GetAllAsync(cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            return _mapper.Map<ICollection<DiscussionDto>>(discussions);
        }
    }
}
