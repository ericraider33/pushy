using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace PushyCommon
{
    public interface ICreateTokenService
    {
        String createToken(String userName);
        UserInfo getUserInfo(ClaimsPrincipal user);
    }

    public class CreateTokenService : ICreateTokenService
    {
        private readonly IConfiguration configuration;

        public CreateTokenService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        
        public String createToken(String userName)
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
            return tokenHandler.WriteToken(token);            
        }

        public UserInfo getUserInfo(ClaimsPrincipal user)
        {
            if (user == null)
                return null;

            return new UserInfo
            {
                UserName = user.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sub)?.Value,
                Email = user.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Email)?.Value
            };
        }
    }
}