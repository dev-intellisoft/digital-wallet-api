using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DigitalWalletAPI.Auth
{
    public class JwtService
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;

        public JwtService(IConfiguration configuration)
        {
            _secretKey = configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key", "JWT secret key is missing in configuration.");
            _issuer = configuration["Jwt:Issuer"] ?? throw new ArgumentNullException("Jwt:Issuer", "JWT issuer is missing in configuration.");
            _audience = configuration["Jwt:Audience"] ?? throw new ArgumentNullException("Jwt:Audience", "JWT audience is missing in configuration.");

            if (Encoding.UTF8.GetBytes(_secretKey).Length < 32)
            {
                throw new ArgumentException("JWT secret key must be at least 32 bytes (256 bits) long.");
            }
        }

        public string GenerateToken(Guid userId, string email)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
