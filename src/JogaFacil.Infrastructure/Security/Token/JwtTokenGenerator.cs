﻿using JogaFacil.Domain.Entities;
using JogaFacil.Domain.Token;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JogaFacil.Infrastructure.Security.Token
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly uint _expirationTimeMinutes;
        private readonly string _signinKey;

        public JwtTokenGenerator(uint expirationTimeMinutes, string signinKey)
        {
            _expirationTimeMinutes = expirationTimeMinutes;
            _signinKey = signinKey;
        }

        public string Generate(User user)
        {
            var claims = new List<Claim>()
            {
                new(ClaimTypes.Name, user.Name),
                new(ClaimTypes.Role, user.Role),
                new(ClaimTypes.Sid, user.Id.ToString()),
            };

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Expires = DateTime.UtcNow.AddMinutes(_expirationTimeMinutes),
                SigningCredentials = new SigningCredentials(SecurityKey(), SecurityAlgorithms.HmacSha256Signature),
                Subject = new ClaimsIdentity(claims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(securityToken);
        }

        private SymmetricSecurityKey SecurityKey()
        {
            var key = Encoding.UTF8.GetBytes(_signinKey);
            return new SymmetricSecurityKey(key);
        }
    }
}
