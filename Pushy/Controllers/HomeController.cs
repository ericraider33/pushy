using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pushy.Models;
using PushyCommon;

namespace Pushy.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICreateTokenService tokenService;
        private readonly ILogger<HomeController> logger;

        public HomeController(ICreateTokenService tokenService, ILogger<HomeController> logger)
        {
            this.tokenService = tokenService;
            this.logger = logger;
        }

        public IActionResult Index()
        {
            HomeModel model = new HomeModel();
            model.JwtToken = tokenService.createToken("epeel");
            
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
