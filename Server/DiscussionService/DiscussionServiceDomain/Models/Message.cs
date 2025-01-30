namespace DiscussionServiceDomain.Models
{
    public class Message
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid SentBy { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public Message() { }

        public Message(string text, Guid sentBy)
        {
            Text = text;
            SentBy = sentBy;
        }
    }
}
