namespace MovieServiceApplication.Dto
{
    public record MovieDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; } = DateTime.UtcNow;
        public double Rating { get;  set; } = 0;
        public IList<Guid> Genres { get;  set; } = [];
        public IList<CrewMemberDto> CrewMembers { get;  set; } = [];
        public string? Image { get; set; }
        public MovieDto() { }
    }
}
