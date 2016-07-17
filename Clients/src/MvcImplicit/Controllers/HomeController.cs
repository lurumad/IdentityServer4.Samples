using System.Threading.Tasks;
using System.Net.Http;
using Clients;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features.Authentication;
using IdentityModel.Client;
using System.Collections.Generic;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Globalization;
using Microsoft.AspNetCore.Http.Authentication;

namespace MvcImplicit.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
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
