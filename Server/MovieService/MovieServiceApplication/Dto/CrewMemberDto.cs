using MovieServiceDomain.Enums;

namespace MovieServiceApplication.Dto
{
    public record CrewMemberDto
    {
        public Guid PersonId { get; set; }
        public int Role { get; set; }
    }
}
