using MovieServiceDomain.Enums;

namespace MovieServiceDomain.Models
{
    public class CrewMember
    {
        public Guid PersonId { get; set; }
        public Role Role { get; set; }
    }
}
