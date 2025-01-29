using MovieServiceDomain.Enums;

namespace MovieServiceDomain.Models
{
    public class CrewMember
    {
        public Guid PersonId { get; set; }
        public CrewRole Role { get; set; }

        public CrewMember() { }

        public CrewMember(Guid personId, CrewRole role)
        {
            PersonId = personId;
            Role = role;
        }
    }
}
