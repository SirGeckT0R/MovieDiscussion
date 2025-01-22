using AutoMapper;
using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDataAccess.Specifications.ReviewSpecifications;

namespace MovieServiceApplication.UseCases.Reviews.Queries.GetReviewsByUserIdQuery
{
    public class GetReviewsByUserIdHandler(IUnitOfWork unitOfWork, IMapper mapper) : IQueryHandler<GetReviewsByUserIdQuery, ICollection<ReviewDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<ICollection<ReviewDto>> Handle(GetReviewsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var reviews = await _unitOfWork.Reviews.GetWithSpecificationAsync(new ReviewsByUserIdSpecification(request.UserId), cancellationToken);

            return _mapper.Map<ICollection<ReviewDto>>(reviews);
        }
    }
}
