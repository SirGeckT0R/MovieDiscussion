using UserServiceDataAccess.Enums;
using UserServiceDataAccess.Models;

namespace UserServiceDataAccess.DatabaseHandlers.Specifications.TokenSpecifications
{
    public class UserIdAndTypeSpecification : Specification<Token>
    {
        public UserIdAndTypeSpecification(TokenType type, Guid userId) : base(x => x.TokenType.Equals(type) && x.UserId.Equals(userId))
        {
        }
    }
}
