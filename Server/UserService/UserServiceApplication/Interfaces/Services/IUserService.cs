using UserServiceApplication.Dto;
using UserServiceDataAccess.Enums;

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
        Task<string> ConfirmEmailSendAsync(string? accessToken, string callbackUrl, CancellationToken cancellationToken);
        Task<string> ConfirmEmailRecieveAsync(ConfirmEmailRequest confirmEmailRequest, CancellationToken cancellationToken);
        Task<string> ChangePasswordAsync(string? accessToken, string callbackUrl, CancellationToken cancellationToken);
        Task<string> ForgotPasswordAsync(string email, string callbackUrl, CancellationToken cancellationToken);
        Task<string> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest, CancellationToken cancellationToken);
        Task<ICollection<UserDto>> GetFromCollectionAsync(ICollection<Guid> ids, CancellationToken cancellationToken);
        Task<Role> GetUserRoleByTokenAsync(string? accessToken, CancellationToken cancellationToken);
    }
}
