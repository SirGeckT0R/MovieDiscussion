using MovieServiceDomain.Models;

namespace MovieServiceDataAccess.Specifications.WatchlistSpecifications
{
    public class WatchlistByUserIdSpecification : Specification<Watchlist>
    {
        public WatchlistByUserIdSpecification(Guid userId) : base(x => x.UserId.Equals(userId))
        {
        }
    }
}
