using UserServiceDataAccess.Models;

namespace UserServiceApplication.Interfaces.Services
{
    public interface ITokenService
    {
        Task<(string, string)> GenerateAuthTokensAsync(User user, CancellationToken cancellationToken);
        Task<Token?> GetTokenAsync(Guid tokenId, CancellationToken cancellationToken);
        Task<Guid> AddTokenAsync(Token token, CancellationToken cancellationToken);
        Task DeleteTokenAsync(Guid tokenId, CancellationToken cancellationToken);
        void UpdateToken(Token token, CancellationToken cancellationToken);
        Task<string> RefreshTokenAsync(string? inputToken, CancellationToken cancellationToken);
    }
}
