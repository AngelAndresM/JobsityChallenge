using System;
using System.Text;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using Microsoft.IdentityModel.Tokens;

using JobsityChat.Core.Contracts;
using JobsityChat.Core.Helpers;

namespace JobsityChat.Infraestructure.Services
{
    public class TokenService : ITokenService
    {
        public string GetNewToken(string authenticatedUserName)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(ApplicationConstants.IdentityTokenKey);

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, authenticatedUserName) };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims.ToArray()),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var result = tokenHandler.WriteToken(token);

            return result;
        }
    }
}
