using UserServiceDataAccess.Models;

namespace UserServiceDataAccess.DatabaseHandlers.Specifications.UserSpecifications
{
    public class UsersFromCollectionSpecification : Specification<User>
    {
        public UsersFromCollectionSpecification(ICollection<Guid> ids) : base(x => ids.Contains(x.Id))
        {
        }
    }
}
