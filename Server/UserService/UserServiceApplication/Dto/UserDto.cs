using UserServiceDataAccess.Enums;
using UserServiceDataAccess.Models;

namespace UserServiceApplication.Dto
{
    public class UserDto : IdModel
    {
        public string Email { get; } = string.Empty;
        public string Username { get; } = string.Empty;
        public E_Role Role { get; } = E_Role.Guest;
        public bool IsEmailConfirmed { get; } = false;
        public UserDto()
        {

        }

        public UserDto(Guid id, string email, string username, E_Role role, bool isEmailConfirmed)
        {
            Id = id;
            Email = email;
            Username = username;
            Role = role;
            IsEmailConfirmed = isEmailConfirmed;
        }
    }
}
