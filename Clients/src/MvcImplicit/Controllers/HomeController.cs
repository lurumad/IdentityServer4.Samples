using System.Threading.Tasks;
using System.Net.Http;
using Clients;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features.Authentication;

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

        [Authorize]
        public async Task<IActionResult> CallApi()
        {
            var authCtx = new AuthenticateContext("cookies");
            await HttpContext.Authentication.AuthenticateAsync(authCtx);
            var token = authCtx.Properties[".Token.access_token"];

            var client = new HttpClient();
            client.SetBearerToken(token);

            var response = await client.GetStringAsync(Constants.AspNetWebApiSampleApi + "identity");
            ViewBag.Json = JArray.Parse(response).ToString();

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.Authentication.SignOutAsync("cookies");
            return Redirect("~/");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
