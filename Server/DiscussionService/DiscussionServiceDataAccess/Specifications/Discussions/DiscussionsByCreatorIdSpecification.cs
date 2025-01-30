using DiscussionServiceDomain.Models;

namespace DiscussionServiceDataAccess.Specifications.Discussions
{
    public class DiscussionsByCreatorIdSpecification : Specification<Discussion>
    {
        public DiscussionsByCreatorIdSpecification(Guid id) : base(x => x.CreatedBy.Equals(id))
        {
        }
    }
}
