using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowApp.Infrastructure.Security
{
    public class JwtAuthenticationService
    {
        private readonly EncryptionService _encryptionService;

        public JwtAuthenticationService()
        {
            _encryptionService = new EncryptionService();
        }
        public string GenerateToken(string userId, string secretKey)
        {          
            var encryptedUserId = _encryptionService.EncryptData(userId);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Name, encryptedUserId)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string ValidateTokenAndGetUserId(string token, string secretKey)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out SecurityToken validatedToken);

                // Recuperar o userId criptografado do token
                var encryptedUserId = principal.FindFirst(ClaimTypes.Name)?.Value;

                // Descriptografar o userId
                var userId = _encryptionService.DecryptData(encryptedUserId);

                return userId;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
