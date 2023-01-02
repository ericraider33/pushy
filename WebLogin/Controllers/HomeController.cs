using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PushyCommon;
using WebLogin.Models;

namespace WebLogin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            HomeModel model = new HomeModel();
            LoginCookieInfo info = LoginCookieInfo.getLoginCookieInfo(HttpContext.User);
            model.UserName = info?.userName;
            
            return View(model);
        }

        public async Task<IActionResult> login()
        {
            LoginCookieInfo info = LoginCookieInfo.getLoginCookieInfo(HttpContext.User);
            if (info != null)
                throw new Exception($"User already logged in: {info.userName}");

            info = new LoginCookieInfo
            {
                userName = "ee",
                ticketTimeoutMinutes = 60,
                timeZoneId = "US/Eastern"
            };
            await setAuthCookie(HttpContext, info);
            
            return Redirect("/");
        }

        public async Task<IActionResult> logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
        
        public static async Task setAuthCookie(HttpContext context, LoginCookieInfo userObject, bool persistCookie = true)
        {
            DateTime now = DateTime.Now;

            // Logins user
            ClaimsIdentity identity = userObject.generateIdentity();
            AuthenticationProperties properties = new AuthenticationProperties
            {
                IssuedUtc = now,
                ExpiresUtc = now + TimeSpan.FromMinutes(userObject.ticketTimeoutMinutes),
                IsPersistent = persistCookie,
                AllowRefresh = false
            };
            
            await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), properties);
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