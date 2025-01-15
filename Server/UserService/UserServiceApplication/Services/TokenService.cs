using FluentValidation;
using UserServiceApplication.Interfaces.Services;
using UserServiceDataAccess.Dto;
using UserServiceDataAccess.Enums;
using UserServiceDataAccess.Exceptions;
using UserServiceDataAccess.Interfaces;
using UserServiceDataAccess.Models;
using UserServiceDataAccess.Specifications;

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
            var refreshTokenId = Guid.NewGuid();
            var (accessToken, _) = _jwtProvider.GenerateToken(userClaims, E_TokenType.Access);
            var (refreshToken, expiresRefresh) = _jwtProvider.GenerateToken(userClaims, E_TokenType.Refresh, refreshTokenId);

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.TokenRepository.AddAsync(new Token(refreshTokenId, E_TokenType.Refresh, userClaims.Id, refreshToken, expiresRefresh), cancellationToken);

            return (accessToken, refreshToken);
        }

        public async Task<(string,string)> GenerateConfirmEmailTokenAsync(string? accessToken, CancellationToken cancellationToken)
        {
            if (accessToken == null)
            {
                throw new TokenException("Token is null");
            }
            var (_, userClaims) = ExtractClaims(accessToken);

            var candidate = await _unitOfWork.TokenRepository.GetWithSpecificationAsync(new UserIdAndTypeSpecification(E_TokenType.ConfirmEmail, userClaims.Id), cancellationToken);
            if (candidate != null)
            {
                await _unitOfWork.TokenRepository.DeleteAsync(candidate.Id, cancellationToken);
            }
            var confirmTokenId = Guid.NewGuid();
            cancellationToken.ThrowIfCancellationRequested();
            
            cancellationToken.ThrowIfCancellationRequested();
            var (confirmEmailToken, expiresConfirm) = _jwtProvider.GenerateToken(userClaims, E_TokenType.ConfirmEmail, confirmTokenId);

            await _unitOfWork.TokenRepository.AddAsync(new Token(confirmTokenId, E_TokenType.ConfirmEmail, userClaims.Id, confirmEmailToken, expiresConfirm), cancellationToken);
            return (confirmEmailToken, userClaims.Email);
        }

        public async Task<(string, string)> GenerateResetPasswordTokenAsync(string? accessToken, CancellationToken cancellationToken)
        {
            if (accessToken == null)
            {
                throw new TokenException("Token is null");
            }
            var (_, userClaims) = ExtractClaims(accessToken);

            var candidate = await _unitOfWork.TokenRepository.GetWithSpecificationAsync(new UserIdAndTypeSpecification(E_TokenType.ResetPassword, userClaims.Id), cancellationToken);
            if (candidate != null)
            {
                await _unitOfWork.TokenRepository.DeleteAsync(candidate.Id, cancellationToken);
            }
            var confirmTokenId = Guid.NewGuid();
            cancellationToken.ThrowIfCancellationRequested();

            cancellationToken.ThrowIfCancellationRequested();
            var (confirmEmailToken, expiresConfirm) = _jwtProvider.GenerateToken(userClaims, E_TokenType.ResetPassword, confirmTokenId);

            await _unitOfWork.TokenRepository.AddAsync(new Token(confirmTokenId, E_TokenType.ResetPassword, userClaims.Id, confirmEmailToken, expiresConfirm), cancellationToken);
            return (confirmEmailToken, userClaims.Email);
        }

        public async Task ValidateConfirmTokenAsync(string? confirmToken, CancellationToken cancellationToken)
        {
            if (confirmToken == null)
            {
                throw new TokenException("Token is null");
            }
            var (_, userClaims) = ExtractClaims(confirmToken);


            var candidate = await _unitOfWork.TokenRepository.GetWithSpecificationAsync(new UserIdAndTypeSpecification(E_TokenType.ConfirmEmail, userClaims.Id), cancellationToken);
            if (candidate == null)
            {
                throw new NotFoundException("Confirm token not found");
            }

            await _unitOfWork.TokenRepository.DeleteAsync(candidate.Id, cancellationToken);
        }

        public async Task ValidateResetTokenAsync(string? resetToken, CancellationToken cancellationToken)
        {
            if (resetToken == null)
            {
                throw new TokenException("Token is null");
            }
            var (_, userClaims) = ExtractClaims(resetToken);


            var candidate = await _unitOfWork.TokenRepository.GetWithSpecificationAsync(new UserIdAndTypeSpecification(E_TokenType.ResetPassword, userClaims.Id), cancellationToken);
            if (candidate == null)
            {
                throw new NotFoundException("Reset token not found");
            }

            await _unitOfWork.TokenRepository.DeleteAsync(candidate.Id, cancellationToken);
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
            var (tokenId, userClaims) = ExtractClaims(inputToken);

            cancellationToken.ThrowIfCancellationRequested();
            var candidate = await _unitOfWork.TokenRepository.GetByIdAsync(tokenId, cancellationToken);

            ValidateToken(candidate, inputToken);
            cancellationToken.ThrowIfCancellationRequested();
            var (accessToken, _) = _jwtProvider.GenerateToken(userClaims, E_TokenType.Access);
            await _unitOfWork.TokenRepository.DeleteAsync(tokenId, cancellationToken);
            return accessToken;
        }

        private (Guid, UserClaimsDto) ExtractClaims(string token)
        {
            var principal = _jwtProvider.GetPrincipalFromToken(token);

            var tokenId = principal.Claims.FirstOrDefault(c => c.Type.Equals(E_ClaimType.Id.ToString(), StringComparison.CurrentCultureIgnoreCase))?.Value;
            var userId = principal.Claims.FirstOrDefault(c => c.Type.Equals(E_ClaimType.UserId.ToString(), StringComparison.CurrentCultureIgnoreCase))?.Value;
            var email = principal.Claims.FirstOrDefault(c => c.Type.Equals(E_ClaimType.Email.ToString(), StringComparison.CurrentCultureIgnoreCase))?.Value;
            var role = principal.Claims.FirstOrDefault(c => c.Type.Equals(E_ClaimType.Role.ToString(), StringComparison.CurrentCultureIgnoreCase))?.Value;
            if (userId == null|| email == null || role == null)
            {
                throw new TokenException("Invalid token");
            }
            var tokenIdGuid = Guid.Empty;
            _ = Guid.TryParse(tokenId, out tokenIdGuid);
            return (tokenIdGuid, new UserClaimsDto(Guid.Parse(userId), email, (E_Role)Enum.Parse(typeof(E_Role), role)));
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
