namespace MovieServiceDomain.Models
{
    public class Watchlist : IdModel
    {
        public Guid ProfileId { get; set; }
        public ICollection<Guid> MovieIds { get; private set; } = [];

        public Watchlist() { }

        public Watchlist(Guid profileId, ICollection<Guid> movieIds)
        {
            ProfileId = profileId;
            MovieIds = movieIds;
        }
    }
}
