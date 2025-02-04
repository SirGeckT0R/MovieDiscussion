using AutoMapper;
using DiscussionServiceApplication.Dto;
using DiscussionServiceApplication.Interfaces.UseCases;
using DiscussionServiceDataAccess.Interfaces.UnitOfWork;
using DiscussionServiceDomain.Exceptions;
using Microsoft.Extensions.Logging;

namespace DiscussionServiceApplication.UseCases.Discussions.Queries.GetDiscussionByIdQuery
{
    public class GetDiscussionByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetDiscussionByIdHandler> logger) : IQueryHandler<GetDiscussionByIdQuery, DiscussionDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetDiscussionByIdHandler> _logger = logger;

        public async Task<DiscussionDto> Handle(GetDiscussionByIdQuery request, CancellationToken cancellationToken)
        {
            var candidate = await _unitOfWork.Discussions.GetByIdAsync(request.Id, cancellationToken);

            if (candidate == null)
            {
                _logger.LogError("Get discussion by id query failed for {DiscussionId}: discussion not found", request.Id);

                throw new NotFoundException("Discussion not found");
            }

            cancellationToken.ThrowIfCancellationRequested();

            return _mapper.Map<DiscussionDto>(candidate);
        }
    }
}
