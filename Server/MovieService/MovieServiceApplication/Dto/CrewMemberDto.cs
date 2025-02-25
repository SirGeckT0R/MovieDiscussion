namespace MovieServiceApplication.Dto
{
    public record CrewMemberDto
    {
        public Guid PersonId { get; set; }
        public string? FullName { get; set; }
        public int Role { get; set; }

        public CrewMemberDto() { }
    }
}
