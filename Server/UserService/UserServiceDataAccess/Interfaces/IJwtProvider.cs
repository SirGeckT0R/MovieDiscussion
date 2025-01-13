using System.Security.Claims;
using UserServiceDataAccess.Enums;
using UserServiceDataAccess.Models;

namespace UserServiceDataAccess.Interfaces
{
    public interface IJwtProvider
    {
        (string, DateTime) GenerateToken(Guid userId, E_TokenType tokenType, Guid tokenId = default);
        ClaimsPrincipal GetPrincipalFromToken(string token);
    }
}
