using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace SpaWithAspId3AndIdSvr4.Controllers
{
    [Produces("application/json")]
    [Route("api/Test")]
    [Authorize(ActiveAuthenticationSchemes = "Bearer")]
    public class TestController : Controller
    {
        public IActionResult Get()
        {
            var claims = User.Claims.Select(x => new { x.Type, x.Value });
            return Ok(claims.ToArray());
        }
    }
}