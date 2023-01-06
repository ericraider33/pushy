using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PushyCommon;

namespace Pushy.Controllers
{
    [Authorize]
    public class PrivateController : Controller
    {
        private readonly ICreateTokenService tokenService;

        public PrivateController(ICreateTokenService tokenService)
        {
            this.tokenService = tokenService;
        }

        public IActionResult index()
        {
            UserInfo userInfo = tokenService.getUserInfo(User);
            
            return Content($"Hi there {userInfo?.UserName} with email={userInfo?.Email}", "text/html");
        }
    }
}