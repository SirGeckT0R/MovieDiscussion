using UserServiceDataAccess.Enums;
using UserServiceDataAccess.Models;

namespace UserServiceDataAccess.Dto
{
    public class UserClaimsDto : IdModel
    {
        public string Email { get; } = string.Empty;
        public ERole Role { get; } = ERole.Guest;
        public UserClaimsDto()
        {

        }

        public UserClaimsDto(Guid id, string email, ERole role)
        {
            Id = id;
            Email = email;
            Role = role;
        }
    }
}
