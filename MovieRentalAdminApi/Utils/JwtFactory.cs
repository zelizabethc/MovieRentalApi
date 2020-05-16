using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MovieRentalAdminApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace MovieRentalAdminApi.Utils
{
    public class JwtFactory : ITokenFactory
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public string UserIdClaim => "userId";
        public string RoleClaim => "role";

        public JwtFactory(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public string GenerateToken(string userId, Role role)
        {
            var signingKey = Convert.FromBase64String(_configuration["AuthJwt:SigningSecret"]);
            var expiryDuration = int.Parse(_configuration["AuthJwt:ExpiryDuration"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = null,
                Audience = null,
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(expiryDuration),
                Subject = new ClaimsIdentity(new List<Claim>
                {
                    new Claim(UserIdClaim, userId.ToString()),
                    new Claim(RoleClaim, role.ToString())
                }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(signingKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtTokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(jwtToken);
        }

        public string GetUser()
        {
            return _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(a => a.Type == UserIdClaim)?.Value;
        }

        public string GetRole()
        {
            return _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(a => a.Type == RoleClaim)?.Value;
        }
    }
}