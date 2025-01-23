namespace MovieServiceDomain.Models
{
    public class UserProfile : IdModel
    {
        public Guid AccountId { get; set; }
        public string Username { get; set; } = string.Empty;
    }
}
