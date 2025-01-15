using UserServiceDataAccess.Enums;
using UserServiceDataAccess.Models;

namespace UserServiceDataAccess.Specifications
{
    public class UserIdAndTypeSpecification : Specification<Token>
    {
        public UserIdAndTypeSpecification(E_TokenType type, Guid userId) : base(x=> x.TokenType.Equals(type) && x.UserId.Equals(userId))
        {
        }
    }
}
