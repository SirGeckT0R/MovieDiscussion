using Microsoft.AspNetCore.Authorization;

namespace UserServiceWebAPI.RoleAuthorization
{
    public class RoleRequirement(string role) : IAuthorizationRequirement
    {
        public string Role { get; } = role;
    }
}
