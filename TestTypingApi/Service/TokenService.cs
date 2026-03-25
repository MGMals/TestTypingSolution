using System.Security.Claims;
using System.Text;
using TestTypingApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using TestTypingApi.Models.DB;


namespace TestTypingApi.Service
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration config)
        {
            _configuration = config;
        }

        public string GenerateJwtToken(TestTypeUser user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var expireHours = int.Parse(jwtSettings["ExpireHours"]);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim("id", user.Id.ToString()),
                new Claim("Email", user.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(expireHours),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
