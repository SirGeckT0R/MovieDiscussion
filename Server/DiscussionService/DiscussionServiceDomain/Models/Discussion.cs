namespace DiscussionServiceDomain.Models
{
    public class Discussion : IdModel
    {
        public string Title { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public DateTime StartDateTime { get; private set; } = DateTime.UtcNow;
        public Guid CreatorProfileId { get; private set; }
        public bool IsActive { get;  set; } = false;
        public ICollection<Guid> Subscribers { get; private set; } = [];
        public ICollection<Message> Messages { get; private set; } = [];

        public Discussion() { }

        public Discussion(string title, string description, DateTime startDateTime, Guid creatorProfileId, bool isActive, ICollection<Guid> subscribers, ICollection<Message> messages)
        {
            Title = title;
            Description = description;
            StartDateTime = startDateTime;
            CreatorProfileId = creatorProfileId;
            IsActive = isActive;
            Subscribers = subscribers;
            Messages = messages;
        }
    }
}
