using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;

namespace MovieServiceApplication.UseCases.Reviews.Queries.GetReviewsByAccountIdQuery
{
    public record GetReviewsByAccountIdQuery(Guid AccountId) : IQuery<ICollection<ReviewDto>>;
}
