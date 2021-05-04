using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StudyTestingEnvironment.Models.Options;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace StudyTestingEnvironment.Services.Identity
{
    public class JwtTokenHelper : IJwtTokenHelper
    {
        private readonly IOptions<JwtOptions> _options;

        public JwtTokenHelper(IOptions<JwtOptions> options)
        {
            _options = options;
        }

        public string CreateJwtToken(Claim[] claims)
        {
            var securityKey = _options.Value.GetSymmetricSecurityKey();
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_options.Value.Issuer,
                _options.Value.Audience,
                claims,
                expires: DateTime.Now.AddSeconds(_options.Value.TokenLifeTime),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}