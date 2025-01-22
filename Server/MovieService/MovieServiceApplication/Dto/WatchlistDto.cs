namespace MovieServiceApplication.Dto
{
    public record WatchlistDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public ICollection<Guid> MovieIds { get; set; } = [];

        public WatchlistDto()
        {
        }

        public WatchlistDto(Guid id, Guid userId, ICollection<Guid> movieIds)
        {
            Id = id;
            UserId = userId;
            MovieIds = movieIds;
        }
    }
}
