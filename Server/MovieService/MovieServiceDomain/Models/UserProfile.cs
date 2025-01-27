using MovieServiceDomain.Interfaces;

namespace MovieServiceDomain.Models
{
    public class UserProfile : IdModel, ISoftDelete
    {
        public Guid AccountId { get; set; }
        public string Username { get; private set; } = string.Empty;
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        public UserProfile() { }

        public UserProfile(Guid accountId, string username)
        {
            AccountId = accountId;
            Username = username;
        }
    }
}
