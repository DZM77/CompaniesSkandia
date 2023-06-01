using Companies.API.DataTransferObjects;
using Companies.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Companies.API.Controllers
{
    [Route("api/demo")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        private readonly IServiceManager serviceManager;

        public DemoController(IServiceManager serviceManager)
        {
            this.serviceManager = serviceManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            return Ok("Working");
        }

        [HttpGet("dto")]
        [AllowAnonymous]
        public ActionResult Index2()
        {
            var dto = new CompanyDto { Name = "Working" };
            return Ok(dto);
        }

        [HttpGet("getonefromservice")]
        [AllowAnonymous]
        public async Task<ActionResult> Get()
        {
            var dto = (await serviceManager.CompanyService.GetCompaniesAsync(true)).First();
            return Ok(dto);
        }

        [HttpGet("getall")]
        [AllowAnonymous]
        public async Task<ActionResult> GetAll()
        {
            var dtos = await serviceManager.CompanyService.GetCompaniesAsync(false);
            return Ok(dtos);
        }

    }
}
