using MovieServiceDomain.Models;

namespace MovieServiceDataAccess.Specifications.ReviewSpecifications
{
    public class ReviewsByMovieIdSpecification : Specification<Review>
    {
        public ReviewsByMovieIdSpecification(Guid movieId) : base(x => x.MovieId.Equals(movieId))
        {
        }
    }
}
