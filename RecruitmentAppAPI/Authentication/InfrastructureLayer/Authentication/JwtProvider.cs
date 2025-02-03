using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RecruitmentApp.Entities;
using RecruitmentAppAPI.Authentication.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RecruitmentAppAPI.Authentication.InfrastructureLayer.Authentication
{
    public class JwtProvider : IJwtProvider
    {
        private readonly JwtOptions _jwtOptions;

        public JwtProvider(IOptions<JwtOptions> options)
        {
            _jwtOptions = options.Value;
        }

        public string Generate(Member member)
        {
            var claims = new Claim[]
            {
                new (JwtRegisteredClaimNames.Sub, member.Id.ToString()),
                new (JwtRegisteredClaimNames.Email, member.Email),
            };

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _jwtOptions.Issuer,
                _jwtOptions.Audience,
                claims,
                null,
                DateTime.UtcNow.AddHours(1),
                signingCredentials);

            string tokenValue = new JwtSecurityTokenHandler()
                .WriteToken(token);

            return tokenValue;
        }
    }
}
