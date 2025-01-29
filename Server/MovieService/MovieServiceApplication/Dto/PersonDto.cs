namespace MovieServiceApplication.Dto
{
    public record PersonDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; } = DateTime.UtcNow;

        public PersonDto() { }
    }
}
