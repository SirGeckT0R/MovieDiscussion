using MovieServiceDomain.Models;

namespace MovieServiceDataAccess.Specifications.ReviewSpecifications
{
    public class ReviewByMovieAndProfileIdSpecification : Specification<Review>
    {
        public ReviewByMovieAndProfileIdSpecification(Guid profileId, Guid movieId) : base(x => x.MovieId.Equals(movieId) && x.ProfileId.Equals(profileId))
        {
        }
    }
}
