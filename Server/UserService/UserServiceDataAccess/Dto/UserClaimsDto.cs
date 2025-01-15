using UserServiceDataAccess.Enums;
using UserServiceDataAccess.Models;

namespace UserServiceDataAccess.Dto
{
    public class UserClaimsDto : IdModel
    {
        public string Email { get; } = string.Empty;
        public E_Role Role { get; } = E_Role.Guest;
        public UserClaimsDto()
        {

        }

        public UserClaimsDto(Guid id, string email, E_Role role)
        {
            Id = id;
            Email = email;
            Role = role;
        }
    }
}
