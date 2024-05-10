using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace WebApi___Sec3.Services
{
    public class TokenService : ITokenService
    {
        public JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims, IConfiguration _config)
        {
            var key = _config.GetSection("JWT").GetValue<string>("SecretKey") ??
                throw new InvalidOperationException("Invalid secret key"); // Obter o valor da secret key

            var privateKey = Encoding.UTF8.GetBytes(key); //Codificar a chave secreta

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(privateKey),
                SecurityAlgorithms.HmacSha256Signature); //Criar as credenciais para assinar os tokens

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims), //As claims 
                Expires = DateTime.UtcNow.AddMinutes(_config.GetSection("JWT").GetValue<double>("TokenValidityInMinutes")),//Tempo de expirar
                Audience = _config.GetSection("JWT")
                .GetValue<string>("ValidAudience"),
                Issuer = _config.GetSection("JWT").GetValue<string>("ValidIssuer"),//Emissor
                SigningCredentials = signingCredentials
            };

            var tokenHandler  = new JwtSecurityTokenHandler();
            var token =  tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            return token;

        }

        public string GenerateRefreshToken()
        {
            var secureRandomBytes = new byte[128]; // Armazenar bites aleatorios

            using var randomNuberGenerator = RandomNumberGenerator.Create();//Criara numeros aleatorios

            randomNuberGenerator.GetBytes(secureRandomBytes); //Preenche os numeros aleatorios

            var refreshToken = Convert.ToBase64String(secureRandomBytes); //Converte pra base64
            return refreshToken;

        }

        public ClaimsPrincipal GetPrincipalExpiredToken(string token, IConfiguration _config)
        {
            var secretKey = _config["JWT:SecretKey"] ?? throw new InvalidOperationException("Invalid key");//Obter a chave

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ValidateLifetime = false,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if(securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(
                    SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
    }
}
