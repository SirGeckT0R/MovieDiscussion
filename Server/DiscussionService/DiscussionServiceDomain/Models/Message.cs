namespace DiscussionServiceDomain.Models
{
    public class Message : IdModel
    {
        public Guid DiscussionId { get; set; }
        public Guid SentBy { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public Message() { }

        public Message(Guid discussionId, string text, Guid sentBy, string username)
        {
            DiscussionId = discussionId;
            Text = text;
            SentBy = sentBy;
            Username = username;
        }
    }
}
