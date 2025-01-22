using MovieServiceDomain.Models;

namespace MovieServiceDataAccess.Specifications.ReviewSpecifications
{
    public class ReviewByMovieAndUserIdSpecification : Specification<Review>
    {
        public ReviewByMovieAndUserIdSpecification(Guid userId, Guid movieId) : base(x => x.MovieId.Equals(movieId) && x.UserId.Equals(userId))
        {
        }
    }
}
