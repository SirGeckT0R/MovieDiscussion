using FluentValidation;
using Microsoft.Extensions.Logging;
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
                            IJwtProvider jwtProvider,
                            ILogger<TokenService> logger) : BaseService<Token>(validator, logger), ITokenService
    {
        private readonly IUserUnitOfWork _unitOfWork = unitOfWork;
        private readonly IJwtProvider _jwtProvider = jwtProvider;
        private readonly ILogger<TokenService> _logger = logger;

        public async Task<(string, string)> GenerateAuthTokensAsync(UserClaimsDto userClaims, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Generate auth tokens attempt started for {Id}", userClaims.Id);

            cancellationToken.ThrowIfCancellationRequested();
            var specification = new UserIdAndTypeSpecification(TokenType.Refresh, userClaims.Id);
            var candidate = await _unitOfWork.TokenRepository.GetWithSpecificationAsync(specification, cancellationToken);

            if (candidate != null)
            {
                _logger.LogInformation("Deleting already existing token from database");

                _unitOfWork.TokenRepository.Delete(candidate, cancellationToken);
            }

            cancellationToken.ThrowIfCancellationRequested();
            var refreshTokenId = Guid.NewGuid();
            var (accessToken, _) = _jwtProvider.GenerateToken(userClaims, TokenType.Access);
            var (refreshToken, expiresRefresh) = _jwtProvider.GenerateToken(userClaims, TokenType.Refresh, refreshTokenId);

            cancellationToken.ThrowIfCancellationRequested();
            var token = new Token(refreshTokenId, TokenType.Refresh, userClaims.Id, refreshToken, expiresRefresh);
            await _unitOfWork.TokenRepository.AddAsync(token, cancellationToken);

            _logger.LogInformation("Generate auth tokens attempt completed successfully for {Id}", userClaims.Id);

            return (accessToken, refreshToken);
        }

        public async Task<(string, string)> GenerateTokenAndExtractEmailAsync(string? accessToken, TokenType tokenType, CancellationToken cancellationToken, bool isAuth = false)
        {
            _logger.LogInformation("Generate token and extract email attempt started");

            if (string.IsNullOrWhiteSpace(accessToken) && !isAuth)
            {
                _logger.LogError("Generate token and extract email attempt failed: token is not valid");

                throw new TokenException("Token is not valid");
            }

            var (_, userClaims) = ExtractClaims(accessToken!);

            cancellationToken.ThrowIfCancellationRequested();
            var specification = new UserIdAndTypeSpecification(tokenType, userClaims.Id);
            var candidate = await _unitOfWork.TokenRepository.GetWithSpecificationAsync(specification, cancellationToken);
            if (candidate != null)
            {
                _logger.LogInformation("Deleting already existing token from database");

                _unitOfWork.TokenRepository.Delete(candidate, cancellationToken);
            }

            cancellationToken.ThrowIfCancellationRequested();
            var tokenId = Guid.NewGuid();
            var (tokenValue, expiresTime) = _jwtProvider.GenerateToken(userClaims, tokenType, tokenId);

            var token = new Token(tokenId, tokenType, userClaims.Id, tokenValue, expiresTime);
            await _unitOfWork.TokenRepository.AddAsync(token, cancellationToken);

            _logger.LogInformation("Generate token and extract email attempt completed successfully");

            return (tokenValue, userClaims.Email);
        }

        public async Task FindAndDeleteTokenAsync(string? confirmToken, TokenType tokenType, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Find and delete token attempt started");

            if (string.IsNullOrWhiteSpace(confirmToken))
            {
                _logger.LogError("Find and delete token attempt failed: token is not valid");

                throw new TokenException("Token is not valid");
            }

            var (_, userClaims) = ExtractClaims(confirmToken);

            var specification = new UserIdAndTypeSpecification(tokenType, userClaims.Id);
            var candidate = await _unitOfWork.TokenRepository.GetWithSpecificationAsync(specification, cancellationToken);

            if(candidate == null)
            {
                _logger.LogError("Find and delete token attempt failed: token not found");

                throw new NotFoundException("Token not found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            _unitOfWork.TokenRepository.Delete(candidate, cancellationToken);

            _logger.LogInformation("Find and delete token attempt completed successfully");
        }

        public async Task<Token?> GetTokenAsync(Guid tokenId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get token attempt started for {Id}", tokenId);

            cancellationToken.ThrowIfCancellationRequested();
            var token = await _unitOfWork.TokenRepository.GetByIdAsync(tokenId, cancellationToken);

            _logger.LogInformation("Get token attempt completed successfully for {Id}", tokenId);

            return token;
        }

        public async Task<Guid> AddTokenAsync(Token token, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Add token attempt started for {Id}", token.Id);

            cancellationToken.ThrowIfCancellationRequested();
            Validate(token);

            var tokenId = await _unitOfWork.TokenRepository.AddAsync(token, cancellationToken);

            _logger.LogInformation("Add token attempt completed successfully for {Id}", token.Id);

            return tokenId;
        }

        public async Task DeleteTokenAsync(Guid tokenId, CancellationToken cancellationToken)
        {
            _logger.LogError("Delete token attempt started for {Id}", tokenId);

            cancellationToken.ThrowIfCancellationRequested();
            var candidate = await GetTokenAsync(tokenId, cancellationToken);

            if(candidate == null)
            {
                _logger.LogError("Delete token attempt failed for {Id}: no token was found", tokenId);

                throw new NotFoundException("No token was found");
            }

            _unitOfWork.TokenRepository.Delete(candidate, cancellationToken);

            _logger.LogError("Delete token attempt completed successfully for {Id}", tokenId);
        }

        public void UpdateToken(Token token, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Update token attempt started for {Id}", token.Id);

            cancellationToken.ThrowIfCancellationRequested();
            Validate(token);
            _unitOfWork.TokenRepository.Update(token, cancellationToken);

            _logger.LogInformation("Update token attempt completed successfully for {Id}", token.Id);
        }

        public async Task<string> RefreshTokenAsync(string? inputToken, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Refresh token attempt started");

            if(string.IsNullOrWhiteSpace(inputToken))
            {
                _logger.LogError("Refresh token attempt failed: token is not valid");

                throw new TokenException("Token is empty");
            }

            var (tokenId, userClaims) = ExtractClaims(inputToken);

            cancellationToken.ThrowIfCancellationRequested();
            var candidate = await _unitOfWork.TokenRepository.GetByIdAsync(tokenId, cancellationToken);

            CompareWithEntity(candidate, inputToken);

            cancellationToken.ThrowIfCancellationRequested();
            var (accessToken, _) = _jwtProvider.GenerateToken(userClaims, TokenType.Access);

            _logger.LogInformation("Refresh token attempt completed successfully");

            return accessToken;
        }

        public (Guid, UserClaimsDto) ExtractClaims(string token)
        {
            _logger.LogInformation("Extract claims from token attempt started");

            var principal = _jwtProvider.GetPrincipalFromToken(token);

            var tokenId = principal.Claims.FirstOrDefault(c => c.Type.Equals(ClaimType.Id.ToString(), StringComparison.CurrentCultureIgnoreCase))?.Value;
            var userId = principal.Claims.FirstOrDefault(c => c.Type.Equals(ClaimType.UserId.ToString(), StringComparison.CurrentCultureIgnoreCase))?.Value;
            var email = principal.Claims.FirstOrDefault(c => c.Type.Equals(ClaimType.Email.ToString(), StringComparison.CurrentCultureIgnoreCase))?.Value;
            var role = principal.Claims.FirstOrDefault(c => c.Type.Equals(ClaimType.Role.ToString(), StringComparison.CurrentCultureIgnoreCase))?.Value;

            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(role))
            {
                _logger.LogError("Extract claims from token attempt failed: empty claims");

                throw new TokenException("Invalid token");
            }

            _ = Guid.TryParse(tokenId, out Guid tokenIdGuid);

            _logger.LogInformation("Extract claims from token attempt completed successfully");

            return (tokenIdGuid, new UserClaimsDto(Guid.Parse(userId), email, (Role)Enum.Parse(typeof(Role), role)));
        }

        private void CompareWithEntity(Token? token, string inputToken)
        {
            if(token == null && token?.TokenValue != inputToken)
            {
                _logger.LogError("Extract claims from token attempt failed: token is not valid");

                throw new TokenException("Token is not valid");
            }

            if(token.ExpiresAt <= DateTime.UtcNow)
            {
                _logger.LogError("Extract claims from token attempt failed: token is expired");

                throw new TokenException("Token is expired");
            }
        }
    }
}
