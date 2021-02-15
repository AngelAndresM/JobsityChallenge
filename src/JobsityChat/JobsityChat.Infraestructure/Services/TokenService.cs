using System;
using System.Text;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

using JobsityChat.Core.Contracts;
using JobsityChat.Core.Helpers;

namespace JobsityChat.Infraestructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtTokenConfig _jwtTokenConfig;

        public TokenService(JwtTokenConfig jwtTokenConfig)
        {
            _jwtTokenConfig = jwtTokenConfig;
        }

        public string GetNewToken(string authenticatedUserName)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtTokenConfig.Secret);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, authenticatedUserName)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims.ToArray()),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = _jwtTokenConfig.Audience,
                Issuer = _jwtTokenConfig.Issuer
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var result = tokenHandler.WriteToken(token);

            return result;
        }
    }

    #region Models
    public class JwtTokenConfig
    {
        [JsonProperty("secret")]
        public string Secret { get; set; }

        [JsonProperty("issuer")]
        public string Issuer { get; set; }

        [JsonProperty("audience")]
        public string Audience { get; set; }
    }
    #endregion
}
