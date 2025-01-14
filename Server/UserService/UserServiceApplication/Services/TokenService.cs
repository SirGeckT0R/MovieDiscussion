using FluentValidation;
using UserServiceApplication.Interfaces.Services;
using UserServiceDataAccess.Enums;
using UserServiceDataAccess.Exceptions;
using UserServiceDataAccess.Interfaces;
using UserServiceDataAccess.Models;

namespace UserServiceApplication.Services
{
    public class TokenService(IUserUnitOfWork unitOfWork, 
                            IValidator<Token> validator,
                            IJwtProvider jwtProvider) : BaseService<Token>(validator), ITokenService
    {
        private readonly IUserUnitOfWork _unitOfWork = unitOfWork;
        private readonly IJwtProvider _jwtProvider = jwtProvider;

        public async Task<(string, string)> GenerateAuthTokensAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var refreshTokenId = Guid.NewGuid();
            var (accessToken, _) = _jwtProvider.GenerateToken(user.Id,user.Role.ToString(), E_TokenType.Access);
            var (refreshToken, expiresRefresh) = _jwtProvider.GenerateToken(user.Id, user.Role.ToString(), E_TokenType.Refresh, refreshTokenId);

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.TokenRepository.AddAsync(new Token(refreshTokenId, E_TokenType.Refresh, user.Id, refreshToken, expiresRefresh), cancellationToken);

            return (accessToken, refreshToken);
        }

        public async Task<Token?> GetTokenAsync(Guid tokenId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _unitOfWork.TokenRepository.GetByIdAsync(tokenId, cancellationToken);
        }

        public async Task<Guid> AddTokenAsync(Token token, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Validate(token);
            return await _unitOfWork.TokenRepository.AddAsync(token, cancellationToken);
        }

        public async Task DeleteTokenAsync(Guid tokenId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.TokenRepository.DeleteAsync(tokenId, cancellationToken);
        }

        public void UpdateToken(Token token, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Validate(token);
            _unitOfWork.TokenRepository.Update(token, cancellationToken);
        }

        public async Task<string> RefreshTokenAsync(string? inputToken, CancellationToken cancellationToken)
        {
            if(inputToken == null)
            {
                throw new TokenException("Token is null");
            }
            var (tokenId, userId, role) = ExtractClaims(inputToken);

            cancellationToken.ThrowIfCancellationRequested();
            var token = await _unitOfWork.TokenRepository.GetByIdAsync(tokenId, cancellationToken);

            ValidateToken(token, inputToken);
            cancellationToken.ThrowIfCancellationRequested();
            var (accessToken, _) = _jwtProvider.GenerateToken(userId, role, E_TokenType.Access);
            return accessToken;
        }

        private (Guid, Guid, string) ExtractClaims(string token)
        {
            var principal = _jwtProvider.GetPrincipalFromToken(token);
            var userId = principal.Claims.FirstOrDefault(c => c.Type.Equals(E_ClaimType.UserId.ToString(), StringComparison.CurrentCultureIgnoreCase))?.Value;
            var tokenId = principal.Claims.FirstOrDefault(c => c.Type.Equals(E_ClaimType.Id.ToString(), StringComparison.CurrentCultureIgnoreCase))?.Value;
            var role = principal.Claims.FirstOrDefault(c => c.Type.Equals(E_ClaimType.Role.ToString(), StringComparison.CurrentCultureIgnoreCase))?.Value;
            if (tokenId == null || userId == null || role == null)
            {
                throw new TokenException("Invalid token");
            }
            return (Guid.Parse(tokenId), Guid.Parse(userId), role);
        }

        private static void ValidateToken(Token? token, string inputToken)
        {
            if(token == null && token?.TokenValue != inputToken)
            {
                throw new TokenException("Token is not valid");
            }

            if(token.ExpiresAt <= DateTime.UtcNow)
            {
                throw new TokenException("Refresh token is expired");
            }
        }
    }
}
