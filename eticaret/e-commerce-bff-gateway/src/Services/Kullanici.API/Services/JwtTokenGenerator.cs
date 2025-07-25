using Kullanici.API.Models;
using Kullanici.API.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Kullanici.API.Services
{
    public class JwtTokenGenerator 
    {
        private readonly JwtSettings _jwtSettings;

        public JwtTokenGenerator(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public string GenerateToken(int userId, string userName, string email, string role)
        {
            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
        new Claim(ClaimTypes.NameIdentifier, userId.ToString()), 
        new Claim(ClaimTypes.Name, userName),                     
        new Claim(ClaimTypes.Email, email) ,
        new Claim(ClaimTypes.Role, role)
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
