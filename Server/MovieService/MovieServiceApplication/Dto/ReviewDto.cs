namespace MovieServiceApplication.Dto
{
    public record ReviewDto
    {
        public Guid Id { get;  set; }
        public Guid MovieId { get;  set; }
        public Guid ProfileId { get;  set; }
        public int Value { get;  set; } = 0;
        public string Text { get;  set; } = string.Empty;

        public ReviewDto() { }
    }
}
