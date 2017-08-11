using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using IdentityModel.Client;
using System.Security.Claims;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using IdentityModel;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using Clients;

namespace MvcImplicit.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Secure()
        {
            if (User.Identity.IsAuthenticated) return View();

            return await StartAuthentication();
        }

        public async Task <IActionResult> Logout()
        {
            await HttpContext.Authentication.SignOutAsync("Cookies");

            var disco = await DiscoveryClient.GetAsync(Constants.Authority);
            return Redirect(disco.EndSessionEndpoint);
        }

        public IActionResult Error()
        {
            return View();
        }

        private async Task<IActionResult> StartAuthentication()
        {
            // read discovery document to find authorize endpoint
            var disco = await DiscoveryClient.GetAsync(Constants.Authority);

            var authorizeUrl = new AuthorizeRequest(disco.AuthorizeEndpoint).CreateAuthorizeUrl(
                clientId: "mvc.manual",
                responseType: "id_token",
                scope: "openid",
                redirectUri: "http://localhost:44077/home/callback",
                state: "random_state",
                nonce: "random_nonce",
                responseMode: "form_post");

            return Redirect(authorizeUrl);
        }

        public async Task<IActionResult> Callback()
        {
            var state = Request.Form["state"].FirstOrDefault();
            var idToken = Request.Form["id_token"].FirstOrDefault();
            var error = Request.Form["error"].FirstOrDefault();

            if (!string.IsNullOrEmpty(error)) throw new Exception(error);
            if (!string.Equals(state, "random_state")) throw new Exception("invalid state");

            var user = await ValidateIdentityToken(idToken);

            await HttpContext.Authentication.SignInAsync("Cookies", user);
            return Redirect("/home/secure");
        }

        private async Task<ClaimsPrincipal> ValidateIdentityToken(string idToken)
        {
            // read discovery document to find issuer and key material
            var disco = await DiscoveryClient.GetAsync(Constants.Authority);

            var keys = new List<SecurityKey>();
            foreach (var webKey in disco.KeySet.Keys)
            {
                var e = Base64Url.Decode(webKey.E);
                var n = Base64Url.Decode(webKey.N);

                var key = new RsaSecurityKey(new RSAParameters { Exponent = e, Modulus = n })
                {
                    KeyId = webKey.Kid
                };

                keys.Add(key);
            }

            var parameters = new TokenValidationParameters
            {
                ValidIssuer = disco.Issuer,
                ValidAudience = "mvc.manual",
                IssuerSigningKeys = keys,

                NameClaimType = JwtClaimTypes.Name,
                RoleClaimType = JwtClaimTypes.Role
            };

            var handler = new JwtSecurityTokenHandler();
            handler.InboundClaimTypeMap.Clear();

            var user = handler.ValidateToken(idToken, parameters, out var _);

            var nonce = user.FindFirst("nonce")?.Value ?? "";
            if (!string.Equals(nonce, "random_nonce")) throw new Exception("invalid nonce");

            return user;
        }
    }
}