using API.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Services
{
    public class JwtServices
    {
        private readonly IConfiguration _configuration;
        private readonly SymmetricSecurityKey _key;
        public JwtServices(IConfiguration configuration)
        {
            _configuration = configuration;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));
        }

        public string CreateJWT(User user)
        {
            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Email,user.FirstName),
                new Claim(ClaimTypes.Surname,user.LastName),
                new Claim("salamu","ddin"),

            };
            var creadentials = new SigningCredentials(_key,SecurityAlgorithms.HmacSha256Signature);
            var tokenDescripter = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(userClaims),
                Expires = DateTime.UtcNow.AddDays(int.Parse(_configuration["Jwt:ExpireInDays"])),
                SigningCredentials = creadentials,
                Issuer = _configuration["Jwt:Issuer"]
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwt = tokenHandler.CreateToken(tokenDescripter);
            return tokenHandler.WriteToken(jwt);
        }
    }
}
