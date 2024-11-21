using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ExpertCentreTechnicalTask.BaseTests
{
    public static class JwtTokenGenerator
    {
        private const string SecretKey = "MySecretKey123456789MySecretKey1"; // Убедитесь, что ключ достаточно длинный

        public static string GenerateToken(string username, int userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(SecretKey);

            // Установка claims (данных) в токен
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim("userId", userId.ToString()),
                new Claim("username", username)
            }),
                Expires = DateTime.UtcNow.AddHours(1), // Настройка срока действия токена
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
