using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Net.Web.Admin
{
    [Route("api/[controller]/[action]")]
    [AllowAnonymous]
    public class TestController : Controller
    {
        [HttpPost]
        public IActionResult Test()
        {
            return Ok("test");
        }
    }
}
