using AutoMapper;
using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDataAccess.Specifications.ReviewSpecifications;

namespace MovieServiceApplication.UseCases.Reviews.Queries.GetReviewsByMovieIdQuery
{
    public class GetReviewsByMovieIdHandler(IUnitOfWork unitOfWork, IMapper mapper) : IQueryHandler<GetReviewsByMovieIdQuery, PaginatedCollection<ReviewDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<PaginatedCollection<ReviewDto>> Handle(GetReviewsByMovieIdQuery request, CancellationToken cancellationToken)
        {
            var reviewSpecification = new ReviewsByMovieIdSpecification(request.MovieId);
            var (reviews, totalPages) = await _unitOfWork.Reviews.GetPaginatedWithSpecificationAsync(
                                                                                                    reviewSpecification, 
                                                                                                    request.PageIndex,
                                                                                                    request.PageSize,
                                                                                                    cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            var mappedCollection = _mapper.Map<ICollection<ReviewDto>>(reviews);

            var paginatedCollection = new PaginatedCollection<ReviewDto>(mappedCollection, request.PageIndex, totalPages);

            return paginatedCollection;
        }
    }
}
