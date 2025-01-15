using UserServiceDataAccess.Dto;
using UserServiceDataAccess.Models;

namespace UserServiceApplication.Interfaces.Services
{
    public interface ITokenService
    {
        Task<(string, string)> GenerateAuthTokensAsync(UserClaimsDto userClaims, CancellationToken cancellationToken);
        Task<(string, string)> GenerateConfirmEmailTokenAsync(string? accessToken, CancellationToken cancellationToken);
        Task<(string, string)> GenerateResetPasswordTokenAsync(string? accessToken, CancellationToken cancellationToken);
        Task ValidateConfirmTokenAsync(string? confirmToken, CancellationToken cancellationToken);
        Task ValidateResetTokenAsync(string? resetToken, CancellationToken cancellationToken);
        Task<Token?> GetTokenAsync(Guid tokenId, CancellationToken cancellationToken);
        Task<Guid> AddTokenAsync(Token token, CancellationToken cancellationToken);
        Task DeleteTokenAsync(Guid tokenId, CancellationToken cancellationToken);
        void UpdateToken(Token token, CancellationToken cancellationToken);
        Task<string> RefreshTokenAsync(string? inputToken, CancellationToken cancellationToken);
    }
}
