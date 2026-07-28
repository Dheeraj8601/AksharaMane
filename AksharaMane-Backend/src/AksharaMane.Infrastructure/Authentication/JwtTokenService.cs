using AksharaMane.Application.Common.Models;
using AksharaMane.Application.Interfaces.Authentication;
using AksharaMane.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AksharaMane.Infrastructure.Authentication
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtSettings _jwtSettings;

        public JwtTokenService(
            IOptions<JwtSettings> jwtOptions)
        {
            _jwtSettings = jwtOptions.Value;
        }

        public JwtTokenResult GenerateToken(
            AdminUser adminUser)
        {
            var expiresAt = DateTime.UtcNow.AddMinutes(
                _jwtSettings.ExpiryMinutes);

            var claims = new List<Claim>
        {
            new(
                JwtRegisteredClaimNames.Sub,
                adminUser.Id.ToString()),

            new(
                ClaimTypes.NameIdentifier,
                adminUser.Id.ToString()),

            new(
                ClaimTypes.Name,
                adminUser.Name),

            new(
                ClaimTypes.Email,
                adminUser.Email),

            new(
                ClaimTypes.Role,
                adminUser.Role),

            new(
                JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
        };

            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _jwtSettings.Key));

            var credentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials);

            var tokenValue =
                new JwtSecurityTokenHandler()
                    .WriteToken(token);

            return new JwtTokenResult
            {
                AccessToken = tokenValue,
                ExpiresAt = expiresAt
            };
        }
    }
}


