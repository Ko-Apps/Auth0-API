using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace scheduler.Controllers
{
    [Produces("application/json")]
    [Route("/")]
    public class OAuthController : Controller
    {
        [HttpGet("public")]
        public JsonResult Public ()
        {
            return new JsonResult("Hello from a public API!");
        }

        [HttpGet]
        [Route("private")]
        [Authorize]
        public IActionResult Private()
        {
            return Json(new
            {
                Message = "Hello from a private endpoint! You are authenticated."
            });
        }

        [HttpGet]
        [Route("course")]
        [Authorize("read:courses")]
        public IActionResult Course()
        {
            return Json(new
            {
                Message = "Hello from a private endpoint! You are authenticated. And you can read our Courses."
            });
        }

        [HttpGet]
        [Route("admin")]
        [Authorize(Roles = "admin")]
        public IActionResult Admin()
        { 
            return Json(new
            {
                Message = "Hello from an admin API."
            });
        }
    }
}