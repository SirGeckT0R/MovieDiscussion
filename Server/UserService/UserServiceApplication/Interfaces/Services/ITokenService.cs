using UserServiceDataAccess.Dto;
using UserServiceDataAccess.Enums;
using UserServiceDataAccess.Models;

namespace UserServiceApplication.Interfaces.Services
{
    public interface ITokenService
    {
        Task<(string, string)> GenerateAuthTokensAsync(UserClaimsDto userClaims, CancellationToken cancellationToken);
        Task<(string, string)> GenerateTokenAndExtractEmailAsync(string? accessToken, TokenType tokenType, CancellationToken cancellationToken, bool isAuth = false);
        Task<string> GenerateResetTokenAsync(string email, CancellationToken cancellationToken);
        Task FindAndDeleteTokenAsync(string? confirmToken, TokenType tokenType, CancellationToken cancellationToken);
        Task<Token?> GetTokenAsync(Guid tokenId, CancellationToken cancellationToken);
        Task<Guid> AddTokenAsync(Token token, CancellationToken cancellationToken);
        Task DeleteTokenAsync(Guid tokenId, CancellationToken cancellationToken);
        void UpdateToken(Token token, CancellationToken cancellationToken);
        Task<string> RefreshTokenAsync(string? inputToken, CancellationToken cancellationToken);
        (Guid, UserClaimsDto) ExtractClaims(string token);
    }
}
