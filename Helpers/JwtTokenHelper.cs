using CourseProject_backend.Entities;
using CourseProject_backend.Interfaces.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CourseProject_backend.Helpers
{
    public class JwtTokenHelper : IJwtTokenHelper
    {
        public string GenerateToken(IEnumerable<Claim> claims, string key, int experiseHours)
        {
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                                                  SecurityAlgorithms.HmacSha256);

            JwtSecurityToken? token = new JwtSecurityToken(signingCredentials: signingCredentials,
                                             expires: DateTime.Now.AddHours(experiseHours),
                                             claims: claims);

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
        }

        public IEnumerable<Claim> DeserializeToken(string token, string key)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken(token);

            var result = jsonToken.Claims;

            return result;
        }
    }
}
