using MovieServiceDomain.Models;

namespace MovieServiceDataAccess.Specifications.UserProfileSpecifications
{
    public class UserProfileByAccountIdSpecification : Specification<UserProfile>
    {
        public UserProfileByAccountIdSpecification(Guid accountId) : base(x => x.AccountId.Equals(accountId)) 
        {
        }
    }
}
