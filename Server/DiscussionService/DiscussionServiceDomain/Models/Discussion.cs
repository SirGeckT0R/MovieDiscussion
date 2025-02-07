namespace DiscussionServiceDomain.Models
{
    public class Discussion : IdModel
    {
        public string Title { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public DateTime StartAt { get; private set; } = DateTime.UtcNow;
        public Guid CreatedBy { get; private set; } 
        public ICollection<Guid> Subscribers { get; private set; } = [];
        public bool IsActive { get; set; } = false;

        public Discussion() { }

        public Discussion(string title, string description, DateTime startAt, Guid createdBy, bool isActive, ICollection<Guid> subscribers)
        {
            Title = title;
            Description = description;
            StartAt = startAt;
            CreatedBy = createdBy;
            IsActive = isActive;
            Subscribers = subscribers;
        }
    }
}
