namespace MovieServiceApplication.Dto
{
    public record GenreDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public GenreDto() { }
    }
}
