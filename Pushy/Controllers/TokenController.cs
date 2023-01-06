using System;
using Microsoft.AspNetCore.Mvc;
using PushyCommon;

namespace Pushy.Controllers
{
    public class TokenController : Controller
    {
        private readonly ICreateTokenService tokenService;

        public TokenController(ICreateTokenService tokenService)
        {
            this.tokenService = tokenService;
        }

        public IActionResult index(String userName)
        {
            String stringToken = tokenService.createToken(userName);            
            return Content(stringToken, "text/text");
        }
    }
}