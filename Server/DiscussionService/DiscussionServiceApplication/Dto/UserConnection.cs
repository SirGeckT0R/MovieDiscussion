namespace DiscussionServiceApplication.Dto
{
    public record UserConnection
    {
        public Guid DiscussionId { get; set; }
        public Guid AccountId { get; set; }

        public UserConnection(Guid discussionId, Guid accountId)
        {
            DiscussionId = discussionId;
            AccountId = accountId;
        }
    }
}
