namespace DiscussionServiceApplication.Dto
{
    public record DiscussionDto(Guid Id, string Title, string Description, DateTime StartDateTime, bool IsActive, ICollection<Guid> Participants, ICollection<MessageDto> Messages)
    {
        public DiscussionDto() : this(Guid.NewGuid(), "", "", DateTime.UtcNow, false, new List<Guid>(), new List<MessageDto>()) { }
    }
}
