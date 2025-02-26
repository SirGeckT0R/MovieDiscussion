namespace DiscussionServiceApplication.Dto
{
    public record MessageDto(string Text, string Username, DateTime SentAt)
    {
        public MessageDto() : this(string.Empty, string.Empty, DateTime.UtcNow) 
        {

        }
    }
}
