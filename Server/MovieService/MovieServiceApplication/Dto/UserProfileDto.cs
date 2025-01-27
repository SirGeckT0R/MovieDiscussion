namespace MovieServiceApplication.Dto
{
    public record UserProfileDto
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public string Username { get; set; } = string.Empty;

        public UserProfileDto() { }
    }
}
