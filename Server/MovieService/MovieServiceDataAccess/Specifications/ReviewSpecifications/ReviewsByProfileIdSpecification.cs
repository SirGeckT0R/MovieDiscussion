using MovieServiceDomain.Models;

namespace MovieServiceDataAccess.Specifications.ReviewSpecifications
{
    public class ReviewsByProfileIdSpecification : Specification<Review>
    {
        public ReviewsByProfileIdSpecification(Guid profileId) : base(x => x.ProfileId.Equals(profileId))
        {
        }
    }
}
