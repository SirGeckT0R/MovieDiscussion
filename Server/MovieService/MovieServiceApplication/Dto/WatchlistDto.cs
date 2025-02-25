
namespace MovieServiceApplication.Dto
{
    public record WatchlistDto
    {
        public Guid Id { get; set; }
        public Guid ProfileId { get; set; }
        public ICollection<MovieDto> Movies { get; set; } = [];

        public WatchlistDto() { }
    }
}
