using AutoMapper;
using Microsoft.Extensions.Logging;
using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDomain.Exceptions;

namespace MovieServiceApplication.UseCases.Reviews.Queries.GetReviewByIdQuery
{
    public class GetReviewByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetReviewByIdHandler> logger) : IQueryHandler<GetReviewByIdQuery, ReviewDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetReviewByIdHandler> _logger = logger;

        public async Task<ReviewDto> Handle(GetReviewByIdQuery request, CancellationToken cancellationToken)
        {
            var review = await _unitOfWork.Reviews.GetByIdAsync(request.Id, cancellationToken);

            if (review == null)
            {
                _logger.LogError("Get review by id command failed: review not found");

                throw new NotFoundException("Review not found");
            }

            cancellationToken.ThrowIfCancellationRequested();

            return _mapper.Map<ReviewDto>(review);
        }
    }
}
