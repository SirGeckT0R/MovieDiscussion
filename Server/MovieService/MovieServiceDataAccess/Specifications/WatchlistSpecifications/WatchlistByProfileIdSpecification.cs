using MovieServiceDomain.Models;

namespace MovieServiceDataAccess.Specifications.WatchlistSpecifications
{
    public class WatchlistByProfileIdSpecification : Specification<Watchlist>
    {
        public WatchlistByProfileIdSpecification(Guid profileId) : base(x => x.ProfileId.Equals(profileId))
        {
        }
    }
}
