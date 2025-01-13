using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserServiceDataAccess.Enums;
using UserServiceDataAccess.Exceptions;
using UserServiceDataAccess.Interfaces;

namespace UserServiceDataAccess.Handlers
{
    public class JwtProvider(IConfiguration configuration, IOptions<JwtOptions> options) : IJwtProvider
    {
        private readonly JwtOptions _options = options.Value;
        private readonly string secretKey = configuration["JWTSecretKey"]!;

        public (string, DateTime) GenerateToken(Guid userId, E_TokenType tokenType, Guid tokenId = default)
        {
            Claim[] claims = tokenId == Guid.Empty ?
                [new Claim(E_ClaimType.UserId.ToString(), userId.ToString())] 
                :
                [new Claim(E_ClaimType.Id.ToString().ToLower(), tokenId.ToString()),
                new Claim(E_ClaimType.UserId.ToString().ToLower(), userId.ToString())];

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                SecurityAlgorithms.HmacSha256);

            var expiresTime = tokenType switch
            {
                E_TokenType.Access => DateTime.UtcNow.AddMinutes(_options.ExpiresMinutesAccess),
                E_TokenType.Refresh => DateTime.UtcNow.AddHours(_options.ExpiresHoursRefresh),
                E_TokenType.ConfirmEmail => DateTime.UtcNow.AddHours(_options.ExpiresHoursConfirm),
                E_TokenType.ResetPassword => DateTime.UtcNow.AddHours(_options.ExpiresMinutesReset),
                _ => throw new TokenException("Invalid token type for token generation was provided")
            };

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: expiresTime
                );

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return (tokenValue, expiresTime);
        }

        public ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
    }
}
