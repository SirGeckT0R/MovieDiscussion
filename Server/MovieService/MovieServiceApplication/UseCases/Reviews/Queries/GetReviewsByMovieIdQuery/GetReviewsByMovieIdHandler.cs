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
            var reviews = await _unitOfWork.Reviews.GetWithSpecificationAsync(new ReviewsByMovieIdSpecification(request.MovieId), cancellationToken);

            return _mapper.Map<ICollection<ReviewDto>>(reviews);
        }
    }
}
