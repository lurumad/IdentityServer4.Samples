using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IdentityServer4.Services;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authorization;
using SpaWithAspId3AndIdSvr4.Models;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Builder;
using System.Text.Encodings.Web;
using Microsoft.Extensions.DependencyInjection;

namespace SpaWithAspId3AndIdSvr4.Controllers
{
    public class IdentityServerController : Controller
    {
        private readonly SignInInteraction _signInInteraction;
        private readonly ErrorInteraction _errorInteraction;
        private readonly IOptions<CookieAuthenticationOptions> _cookieOptions;

        public IdentityServerController(
            SignInInteraction signInInteraction,
            ErrorInteraction errorInteraction,
            IOptions<CookieAuthenticationOptions> cookieOptions)
        {
            _signInInteraction = signInInteraction;
            _errorInteraction = errorInteraction;
            _cookieOptions = cookieOptions;
        }

        [HttpGet(Constants.RoutePaths.Login)]
        public IActionResult Login(string id)
        {
            // no easy way to find out the path to login page (Challenge doesn't always work)
            var url = _cookieOptions.Value.LoginPath.ToString();
            if (String.IsNullOrWhiteSpace(url)) url = "/Account/Login";
            url += "?" + _cookieOptions.Value.ReturnUrlParameter + "=" + UrlEncoder.Default.Encode("/IdentityServer/LoginComplete?id=" + id);
            return Redirect(url);
        }

        // this ensures we're only allowing authenticted users via normal login cookie/ui
        [Authorize(ActiveAuthenticationSchemes = "Cookies")]
        public IActionResult LoginComplete(string id)
        {
            // this issues the redirect back to IdSvr4's authorization endpoint to complete the token issuance
            return new IdentityServerSignInResult(id);
        }

        [Route(Constants.RoutePaths.Error)]
        public async Task<IActionResult> Error(string id)
        {
            ErrorMessage error = null;

            if (id != null)
            {
                error = await _errorInteraction.GetRequestAsync(id);
            }

            return View("OidcError", error);
        }
    }

    public class IdentityServerSignInResult : IActionResult
    {
        private readonly string _requestId;

        public IdentityServerSignInResult(string requestId)
        {
            _requestId = requestId;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var interaction = context.HttpContext.RequestServices.GetRequiredService<SignInInteraction>();
            await interaction.ProcessResponseAsync(_requestId, new IdentityServer4.Models.SignInResponse());
        }
    }
}
