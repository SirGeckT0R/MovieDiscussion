namespace MovieServiceDomain.Models
{
    public class Watchlist : IdModel
    {
        public Guid UserId { get; set; }
        public ICollection<Guid> MovieIds { get; set; } = [];
    }
}
