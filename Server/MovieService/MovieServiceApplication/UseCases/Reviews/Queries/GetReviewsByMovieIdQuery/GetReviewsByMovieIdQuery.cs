using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;

namespace MovieServiceApplication.UseCases.Reviews.Queries.GetReviewsByMovieIdQuery
{
    public record GetReviewsByMovieIdQuery(Guid MovieId) : IQuery<ICollection<ReviewDto>>;
}
