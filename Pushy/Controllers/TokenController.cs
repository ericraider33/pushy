using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Pushy.Controllers
{
    public class TokenController : Controller
    {
        private IConfiguration configuration { get; }

        public TokenController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        
        public IActionResult index(String userName)
        {
            String issuer = configuration["Jwt:Issuer"];
            String audience = configuration["Jwt:Audience"];
            byte[] key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, userName),
                    new Claim(JwtRegisteredClaimNames.Email, "eeschenbach@chroniccareiq.com"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };
            
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            String stringToken = tokenHandler.WriteToken(token);            
            return Content(stringToken, "text/text");
        }
    }
}