using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Authentication;

namespace MvcImplicit.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Secure()
        {
            return View();
        }

        public IActionResult Logout()
        {
            return new SignOutResult(new string[] { "oidc", "cookies" }, new AuthenticationProperties { RedirectUri = "/" });
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}