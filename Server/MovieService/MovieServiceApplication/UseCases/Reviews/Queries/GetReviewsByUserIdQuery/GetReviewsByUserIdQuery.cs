using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;

namespace MovieServiceApplication.UseCases.Reviews.Queries.GetReviewsByUserIdQuery
{
    public record GetReviewsByUserIdQuery(Guid UserId) : IQuery<ICollection<ReviewDto>>;
}
