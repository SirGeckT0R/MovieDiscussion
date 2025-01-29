using AutoMapper;
using DiscussionServiceApplication.Dto;
using DiscussionServiceApplication.Interfaces.UseCases;
using DiscussionServiceDataAccess.Interfaces.UnitOfWork;
using DiscussionServiceDomain.Exceptions;

namespace DiscussionServiceApplication.UseCases.Discussions.Queries.GetDiscussionByIdQuery
{
    public class GetDiscussionByIdHandler(IUnitOfWork unitOfWork, IMapper mapper) : IQueryHandler<GetDiscussionByIdQuery, DiscussionDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<DiscussionDto> Handle(GetDiscussionByIdQuery request, CancellationToken cancellationToken)
        {
            var candidate = await _unitOfWork.Discussions.GetByIdAsync(request.Id, cancellationToken);

            if (candidate == null)
            {
                throw new NotFoundException("Discussion not found");
            }

            cancellationToken.ThrowIfCancellationRequested();

            return _mapper.Map<DiscussionDto>(candidate);
        }
    }
}
