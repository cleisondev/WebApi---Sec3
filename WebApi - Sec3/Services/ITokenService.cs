using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebApi___Sec3.Services
{
    public interface ITokenService
    {
        JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims, IConfiguration _config);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalExpiredToken(string token, IConfiguration _config);
    }
}
