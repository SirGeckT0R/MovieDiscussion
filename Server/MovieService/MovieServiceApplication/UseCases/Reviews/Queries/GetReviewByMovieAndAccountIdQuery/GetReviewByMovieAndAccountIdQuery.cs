using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;

namespace MovieServiceApplication.UseCases.Reviews.Queries.GetReviewByMovieAndAccountIdQuery
{
    public record GetReviewByMovieAndAccountIdQuery(Guid MovieId, Guid? AccountId) : IQuery<ReviewDto?>;
}
