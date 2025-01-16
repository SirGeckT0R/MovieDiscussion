using System.Security.Claims;
using UserServiceDataAccess.Dto;
using UserServiceDataAccess.Enums;

namespace UserServiceDataAccess.Interfaces
{
    public interface IJwtProvider
    {
        (string, DateTime) GenerateToken(UserClaimsDto userClaims, ETokenType tokenType, Guid tokenId = default);
        ClaimsPrincipal GetPrincipalFromToken(string token);
    }
}
