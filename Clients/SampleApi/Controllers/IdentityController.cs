using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using System.Linq;
using System.Security.Claims;

namespace SampleApi.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class IdentityController
    {
        private readonly ClaimsPrincipal _caller;

        public IdentityController(ClaimsPrincipal caller)
        {
            _caller = caller;
        }

        [HttpGet]
        public ActionResult Get()
        {
            return new JsonResult(_caller.Claims.Select(
                c => new { c.Type, c.Value }));
        }
    }
}