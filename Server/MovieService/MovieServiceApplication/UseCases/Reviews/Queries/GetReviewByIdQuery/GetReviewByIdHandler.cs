using AutoMapper;
using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDomain.Exceptions;

namespace MovieServiceApplication.UseCases.Reviews.Queries.GetReviewByIdQuery
{
    public class GetReviewByIdHandler(IUnitOfWork unitOfWork, IMapper mapper) : IQueryHandler<GetReviewByIdQuery, ReviewDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<ReviewDto> Handle(GetReviewByIdQuery request, CancellationToken cancellationToken)
        {
            var review = await _unitOfWork.Reviews.GetByIdAsync(request.Id, cancellationToken);
            if (review == null)
            {
                throw new NotFoundException("Review not found");
            }
            cancellationToken.ThrowIfCancellationRequested();

            return _mapper.Map<ReviewDto>(review);
        }
    }
}
