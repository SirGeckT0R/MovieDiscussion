namespace DiscussionServiceApplication.Dto
{
    public record MessageDto(string Text, Guid SentBy, DateTime SentAt)
    {
        public MessageDto() : this(string.Empty, Guid.Empty, DateTime.UtcNow) 
        {

        }
    }
}
