using AutoMapper;
using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDataAccess.Specifications.ReviewSpecifications;

namespace MovieServiceApplication.UseCases.Reviews.Queries.GetReviewsByMovieIdQuery
{
    public class GetReviewsByMovieIdHandler(IUnitOfWork unitOfWork, IMapper mapper) : IQueryHandler<GetReviewsByMovieIdQuery, ICollection<ReviewDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<ICollection<ReviewDto>> Handle(GetReviewsByMovieIdQuery request, CancellationToken cancellationToken)
        {
            var reviewSpecification = new ReviewsByMovieIdSpecification(request.MovieId);
            var reviews = await _unitOfWork.Reviews.GetWithSpecificationAsync(reviewSpecification, cancellationToken);

            return _mapper.Map<ICollection<ReviewDto>>(reviews);
        }
    }
}
