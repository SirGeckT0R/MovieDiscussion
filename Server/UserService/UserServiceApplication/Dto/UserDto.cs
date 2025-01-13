using UserServiceDataAccess.Enums;
using UserServiceDataAccess.Models;

namespace UserServiceApplication.Dto
{
    public class UserDto
    {
        public string Email { get; } = string.Empty;
        public string Username { get; } = string.Empty;
        public E_Role Role { get; } = E_Role.Guest;
        public UserDto()
        {

        }

        public UserDto(string email, string username, E_Role role)
        {
            Email = email;
            Username = username;
            Role = role;
        }
    }
}
