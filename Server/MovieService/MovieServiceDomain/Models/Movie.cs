using MovieServiceDomain.Interfaces;

namespace MovieServiceDomain.Models
{
    public class Movie : IdModel, ISoftDelete
    {
        public string Title { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public DateTime ReleaseDate { get; private set; } = DateTime.UtcNow;
        public double Rating { get; set; } = 0; 
        public bool IsApproved { get; set; } = false;
        public Guid SubmittedBy { get; set; }
        public ICollection<Guid> Genres { get; private set; } = [];
        public ICollection<CrewMember> CrewMembers { get; private set; } = [];
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        public Movie() { }

        public Movie(string title, string description, DateTime releaseDate, Guid submittedBy, ICollection<Guid> genres, ICollection<CrewMember> crewMembers)
        {
            Title = title;
            Description = description;
            ReleaseDate = releaseDate;
            SubmittedBy = submittedBy;
            Genres = genres;
            CrewMembers = crewMembers;
        }
    }
}
