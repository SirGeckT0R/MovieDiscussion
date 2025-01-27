using MovieServiceDomain.Interfaces;

namespace MovieServiceDomain.Models
{
    public class Genre : IdModel, ISoftDelete
    {
        public string Name { get; private set; } = string.Empty;
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        public Genre() { }

        public Genre(string name)
        {
            Name = name;
        }
    }
}
