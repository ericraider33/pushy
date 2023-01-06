using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Pushy.Controllers
{
    [Authorize]
    public class PrivateController : Controller
    {
        public IActionResult index()
        {
            String userName = User.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sub)?.Value;
            String email = User.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Email)?.Value;
            
            Console.WriteLine(JwtRegisteredClaimNames.Sub);
            Console.WriteLine(JwtRegisteredClaimNames.Email);
            foreach (Claim claim in User.Claims)
            {
                Console.WriteLine(claim.Type + " = " + claim.Value);
            }
            
            return Content($"Hi there {userName} with email={email}", "text/html");
        }
    }
}