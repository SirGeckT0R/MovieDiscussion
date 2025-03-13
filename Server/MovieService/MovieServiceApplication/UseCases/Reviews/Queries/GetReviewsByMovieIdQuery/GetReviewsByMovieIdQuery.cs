using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;

namespace MovieServiceApplication.UseCases.Reviews.Queries.GetReviewsByMovieIdQuery
{
    public record GetReviewsByMovieIdQuery(Guid MovieId, int PageIndex = 1, int PageSize = 10) : IQuery<PaginatedCollection<ReviewDto>>;
}
