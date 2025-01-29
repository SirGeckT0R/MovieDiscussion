using MovieServiceDomain.Models;

namespace MovieServiceDataAccess.Specifications.ReviewSpecifications
{
    public class ReviewByProfileAndMovieIdSpecification : Specification<Review>
    {
        public ReviewByProfileAndMovieIdSpecification(Guid profileId, Guid movieId) : base(x => x.MovieId.Equals(movieId) && x.ProfileId.Equals(profileId))
        {
        }
    }
}
