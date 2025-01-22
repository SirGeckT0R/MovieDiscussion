using MovieServiceDomain.Models;

namespace MovieServiceDataAccess.Specifications.ReviewSpecifications
{
    public class ReviewsByUserIdSpecification : Specification<Review>
    {
        public ReviewsByUserIdSpecification(Guid userId) : base(x => x.UserId.Equals(userId))
        {
        }
    }
}
