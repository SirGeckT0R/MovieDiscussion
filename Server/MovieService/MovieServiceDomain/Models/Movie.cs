namespace MovieServiceDomain.Models
{
    public class Movie : IdModel
    {
        public string Title { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public DateTime ReleaseDate { get; private set; } = DateTime.UtcNow;
        public double Rating { get; private set; } = 0; 
        public bool IsApproved { get; set; } = false;
        public Guid SubmittedBy { get; set; }
        public ICollection<Guid> Genres { get; private set; } = [];
        public ICollection<CrewMember> CrewMembers { get; private set; } = [];
    }
}
