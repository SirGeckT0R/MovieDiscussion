using UserServiceDataAccess.Enums;
using UserServiceDataAccess.Models;

namespace UserServiceDataAccess.Dto
{
    public class UserClaimsDto : IdModel
    {
        public string Email { get; } = string.Empty;
        public Role Role { get; } = Role.Guest;
        public UserClaimsDto()
        {

        }

        public UserClaimsDto(Guid id, string email, Role role)
        {
            Id = id;
            Email = email;
            Role = role;
        }
    }
}
