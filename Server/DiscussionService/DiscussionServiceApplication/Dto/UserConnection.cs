namespace DiscussionServiceApplication.Dto
{
    public record UserConnection
    {
        public Guid DiscussionId { get; set; }
        public Guid AccountId { get; set; }
        public string Username { get; set; }

        public UserConnection(Guid discussionId, Guid accountId, string username)
        {
            DiscussionId = discussionId;
            AccountId = accountId;
            Username = username;
        }
    }
}
