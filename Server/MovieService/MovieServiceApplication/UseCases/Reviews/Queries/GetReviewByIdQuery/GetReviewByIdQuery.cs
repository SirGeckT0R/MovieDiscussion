using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;

namespace MovieServiceApplication.UseCases.Reviews.Queries.GetReviewByIdQuery
{
    public record GetReviewByIdQuery(Guid Id) : IQuery<ReviewDto>;
}
