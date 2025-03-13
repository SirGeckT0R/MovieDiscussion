namespace DiscussionServiceApplication.Dto
{
    public record MessageDto(Guid UserId, string Text, string Username, DateTime SentAt)
    {
        public MessageDto() : this(Guid.Empty, string.Empty, string.Empty, DateTime.UtcNow) 
        {

        }
    }
}
