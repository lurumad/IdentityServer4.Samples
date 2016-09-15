using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System;

namespace AspNetCoreAuthentication.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOptions<ApiSettings> _apiSettings;

        public HomeController (IOptions<ApiSettings> apiSettings)
        {
            _apiSettings = apiSettings;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Secure()
        {
            ViewBag.IdentityToken = await HttpContext.Authentication.GetTokenAsync("id_token");
            ViewBag.AccessToken = await HttpContext.Authentication.GetTokenAsync("access_token");
            
            return View();
        }

        [Authorize]
        public async Task<IActionResult> CallApi()
        {
            var accessToken = await HttpContext.Authentication.GetTokenAsync("access_token");
            var client = new HttpClient 
            {
                BaseAddress = new Uri(_apiSettings.Value.BaseAddress)
            };
            
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.GetStringAsync("/claims");
            ViewBag.Json = JArray.Parse(response).ToString();

            return View();
        }
    }
}