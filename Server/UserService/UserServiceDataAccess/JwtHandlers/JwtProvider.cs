using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserServiceDataAccess.Dto;
using UserServiceDataAccess.Enums;
using UserServiceDataAccess.Exceptions;
using UserServiceDataAccess.Interfaces;

namespace UserServiceDataAccess.Handlers
{
    public class JwtProvider(IConfiguration configuration, IOptions<JwtOptions> options) : IJwtProvider
    {
        private readonly JwtOptions _options = options.Value;
        private readonly string secretKey = configuration["JWTSecretKey"]!;

        public (string, DateTime) GenerateToken(UserClaimsDto userClaims, ETokenType tokenType, Guid tokenId = default)
        {
            Claim[] claims = tokenId == Guid.Empty ?
                [new Claim(EClaimType.UserId.ToString(), userClaims.Id.ToString()),
                new Claim(EClaimType.Email.ToString(), userClaims.Email.ToString()),
                new Claim(EClaimType.Role.ToString(), userClaims.Role.ToString())] 
                :
                [new Claim(EClaimType.Id.ToString(), tokenId.ToString()),
                new Claim(EClaimType.UserId.ToString(), userClaims.Id.ToString()),
                new Claim(EClaimType.Email.ToString(), userClaims.Email.ToString()),
                new Claim(EClaimType.Role.ToString(), userClaims.Role.ToString())];

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                SecurityAlgorithms.HmacSha256);

            var expiresTime = tokenType switch
            {
                ETokenType.Access => DateTime.UtcNow.AddMinutes(_options.ExpiresMinutesAccess),
                ETokenType.Refresh => DateTime.UtcNow.AddHours(_options.ExpiresHoursRefresh),
                ETokenType.ConfirmEmail => DateTime.UtcNow.AddHours(_options.ExpiresHoursConfirm),
                ETokenType.ResetPassword => DateTime.UtcNow.AddHours(_options.ExpiresMinutesReset),
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
