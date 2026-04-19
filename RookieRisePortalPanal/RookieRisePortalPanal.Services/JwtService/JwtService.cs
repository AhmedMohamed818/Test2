using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RookieRisePortalPanal.Data.Entities;
using RookieRisePortalPanal.Services.AppConfigration;
using RookieRisePortalPanal.Services.AppConfigration.RookieRisePortalPanal.Services.AppConfigration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RookieRisePortalPanal.Services.JwtService
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwt;

        public JwtService(IOptions<JwtSettings> jwt)
        {
            _jwt = jwt.Value;
        }

        public Task<string> GenerateTokenAsync(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("NameEn", user.Company?.NameEn ?? ""),
                new Claim("NameAr", user.Company?.NameAr ?? "")
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwt.SecretKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(_jwt.DurationInDays),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Task.FromResult(tokenString);
        }
    }
}