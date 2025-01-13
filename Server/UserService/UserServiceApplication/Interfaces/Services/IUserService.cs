using UserServiceApplication.Dto;

namespace UserServiceApplication.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserDto> GetUserByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<UserDto> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
        Task<(string, string)> RegisterAsync(RegisterRequest registerRequest, CancellationToken cancellationToken);
        Task<(string ,string)> LoginAsync(LoginRequest loginRequest, CancellationToken cancellationToken);
        Task UpdateUserAsync(UpdateUserRequest updateUserRequest, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
