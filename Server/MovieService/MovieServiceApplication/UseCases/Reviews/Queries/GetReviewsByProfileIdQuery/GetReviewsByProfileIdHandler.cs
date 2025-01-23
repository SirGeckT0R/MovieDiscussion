using AutoMapper;
using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDataAccess.Specifications.ReviewSpecifications;

namespace MovieServiceApplication.UseCases.Reviews.Queries.GetReviewsByProfileIdQuery
{
    public class GetReviewsByProfileIdHandler(IUnitOfWork unitOfWork, IMapper mapper) : IQueryHandler<GetReviewsByProfileIdQuery, ICollection<ReviewDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<ICollection<ReviewDto>> Handle(GetReviewsByProfileIdQuery request, CancellationToken cancellationToken)
        {
            var reviews = await _unitOfWork.Reviews.GetWithSpecificationAsync(new ReviewsByProfileIdSpecification(request.ProfileId), cancellationToken);

            return _mapper.Map<ICollection<ReviewDto>>(reviews);
        }
    }
}
