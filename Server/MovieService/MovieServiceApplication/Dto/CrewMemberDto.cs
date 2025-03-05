using MovieServiceDomain.Enums;

namespace MovieServiceApplication.Dto
{
    public record CrewMemberDto
    {
        public Guid PersonId { get; set; }
        public string? FullName { get; set; }
        public CrewRole Role { get; set; }

        public CrewMemberDto() { }
    }
}
