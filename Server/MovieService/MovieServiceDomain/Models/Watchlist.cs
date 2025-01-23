namespace MovieServiceDomain.Models
{
    public class Watchlist : IdModel
    {
        public Guid ProfileId { get; set; }
        public ICollection<Guid> MovieIds { get; set; } = [];
    }
}
