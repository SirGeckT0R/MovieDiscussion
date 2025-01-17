using FluentValidation;
using UserServiceApplication.Interfaces.Services;
using UserServiceDataAccess.DatabaseHandlers.Specifications;
using UserServiceDataAccess.Dto;
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

        public async Task<(string, string)> GenerateAuthTokensAsync(UserClaimsDto userClaims, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var candidate = await _unitOfWork.TokenRepository.GetWithSpecificationAsync(new UserIdAndTypeSpecification(TokenType.Refresh, userClaims.Id), cancellationToken);
            if (candidate != null)
            {
                _unitOfWork.TokenRepository.Delete(candidate, cancellationToken);
            }

            cancellationToken.ThrowIfCancellationRequested();
            var refreshTokenId = Guid.NewGuid();
            var (accessToken, _) = _jwtProvider.GenerateToken(userClaims, TokenType.Access);
            var (refreshToken, expiresRefresh) = _jwtProvider.GenerateToken(userClaims, TokenType.Refresh, refreshTokenId);

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.TokenRepository.AddAsync(new Token(refreshTokenId, TokenType.Refresh, userClaims.Id, refreshToken, expiresRefresh), cancellationToken);

            return (accessToken, refreshToken);
        }

        public async Task<(string, string)> GenerateTokenAndExtractEmailAsync(string? accessToken, TokenType tokenType, CancellationToken cancellationToken, bool isAuth = false)
        {
            if (accessToken == null && !isAuth)
            {
                throw new TokenException("Token is null");
            }
            var (_, userClaims) = ExtractClaims(accessToken);

            cancellationToken.ThrowIfCancellationRequested();
            var candidate = await _unitOfWork.TokenRepository.GetWithSpecificationAsync(new UserIdAndTypeSpecification(tokenType, userClaims.Id), cancellationToken);
            if (candidate != null)
            {
                _unitOfWork.TokenRepository.Delete(candidate, cancellationToken);
            }

            cancellationToken.ThrowIfCancellationRequested();
            var tokenId = Guid.NewGuid();
            var (tokenValue, expiresTime) = _jwtProvider.GenerateToken(userClaims, tokenType, tokenId);

            await _unitOfWork.TokenRepository.AddAsync(new Token(tokenId, tokenType, userClaims.Id, tokenValue, expiresTime), cancellationToken);

            return (tokenValue, userClaims.Email);
        }

        public async Task FindAndDeleteTokenAsync(string? confirmToken, TokenType tokenType, CancellationToken cancellationToken)
        {
            if (confirmToken == null)
            {
                throw new TokenException("Token is null");
            }
            var (_, userClaims) = ExtractClaims(confirmToken);

            var candidate = await _unitOfWork.TokenRepository.GetWithSpecificationAsync(new UserIdAndTypeSpecification(tokenType, userClaims.Id), cancellationToken) ?? throw new NotFoundException("Token not found");
            _unitOfWork.TokenRepository.Delete(candidate, cancellationToken);
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
            var candidate = await GetTokenAsync(tokenId, cancellationToken) ?? throw new NotFoundException("No user was found");
            _unitOfWork.TokenRepository.Delete(candidate, cancellationToken);
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
            var (tokenId, userClaims) = ExtractClaims(inputToken);

            cancellationToken.ThrowIfCancellationRequested();
            var candidate = await _unitOfWork.TokenRepository.GetByIdAsync(tokenId, cancellationToken);

            CompareWithEntity(candidate, inputToken);
            cancellationToken.ThrowIfCancellationRequested();
            var (accessToken, _) = _jwtProvider.GenerateToken(userClaims, TokenType.Access);

            return accessToken;
        }

        public (Guid, UserClaimsDto) ExtractClaims(string token)
        {
            var principal = _jwtProvider.GetPrincipalFromToken(token);

            var tokenId = principal.Claims.FirstOrDefault(c => c.Type.Equals(ClaimType.Id.ToString(), StringComparison.CurrentCultureIgnoreCase))?.Value;
            var userId = principal.Claims.FirstOrDefault(c => c.Type.Equals(ClaimType.UserId.ToString(), StringComparison.CurrentCultureIgnoreCase))?.Value;
            var email = principal.Claims.FirstOrDefault(c => c.Type.Equals(ClaimType.Email.ToString(), StringComparison.CurrentCultureIgnoreCase))?.Value;
            var role = principal.Claims.FirstOrDefault(c => c.Type.Equals(ClaimType.Role.ToString(), StringComparison.CurrentCultureIgnoreCase))?.Value;
            if (userId == null|| email == null || role == null)
            {
                throw new TokenException("Invalid token");
            }

            _ = Guid.TryParse(tokenId, out Guid tokenIdGuid);

            return (tokenIdGuid, new UserClaimsDto(Guid.Parse(userId), email, (Role)Enum.Parse(typeof(Role), role)));
        }

        private void CompareWithEntity(Token? token, string inputToken)
        {
            if(token == null && token?.TokenValue != inputToken)
            {
                throw new TokenException("Token is not valid");
            }

            if(token.ExpiresAt <= DateTime.UtcNow)
            {
                throw new TokenException("Token is expired");
            }
        }
    }
}
