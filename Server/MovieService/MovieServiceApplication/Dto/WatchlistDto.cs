namespace MovieServiceApplication.Dto
{
    public record WatchlistDto
    {
        public Guid Id { get; set; }
        public Guid ProfileId { get; set; }
        public ICollection<Guid> MovieIds { get; set; } = [];

        public WatchlistDto() { }
    }
}
