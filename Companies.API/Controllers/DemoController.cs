using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Companies.API.Controllers
{
    [Route("api/demo")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            return Ok("Working");
        }
    }
}
