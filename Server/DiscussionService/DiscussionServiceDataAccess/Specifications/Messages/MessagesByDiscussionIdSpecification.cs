using DiscussionServiceDomain.Models;

namespace DiscussionServiceDataAccess.Specifications.Messages
{
    public class MessagesByDiscussionIdSpecification : Specification<Message>
    {
        public MessagesByDiscussionIdSpecification(Guid discussionId) : base(x => x.DiscussionId.Equals(discussionId))
        {
        }
    }
}
