using UserServiceDataAccess.Enums;
using UserServiceDataAccess.Models;

namespace UserServiceDataAccess.DatabaseHandlers.Specifications
{
    public class UserIdAndTypeSpecification : Specification<Token>
    {
        public UserIdAndTypeSpecification(ETokenType type, Guid userId) : base(x => x.TokenType.Equals(type) && x.UserId.Equals(userId))
        {
        }
    }
}
