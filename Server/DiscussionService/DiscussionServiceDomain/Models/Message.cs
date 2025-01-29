namespace DiscussionServiceDomain.Models
{
    public class Message
    {
        public string Text { get; set; } = string.Empty;
        public Guid SentBy { get; set; } 
        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public Message() { }

        public Message(string text, Guid sentBy, DateTime sentAt)
        {
            Text = text;
            SentBy = sentBy;
            SentAt = sentAt;
        }
    }
}
