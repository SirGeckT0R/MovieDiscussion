using UserServiceDataAccess.Enums;
using UserServiceDataAccess.Interfaces;

namespace UserServiceDataAccess.Models
{
    public class User : IdModel, ISoftDelete
    {
        public string Email { get; private set;} = string.Empty;
        public string Username { get; private set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public ERole Role { get; set; } = ERole.Guest;
        public bool IsEmailConfirmed { get; set; } = false;
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        public User() { 

        }

        public User(string email, string username, string password, ERole role, bool isEmailConfirmed = false)
        {
            Email = email;
            Username = username;
            Password = password;
            Role = role;
            IsEmailConfirmed = isEmailConfirmed;
        }
    }
}
