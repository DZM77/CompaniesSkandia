using AutoMapper;
using Companies.API.DataTransferObjects;
using Companies.API.Services;
using Companies.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Companies.API.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthenticationService autheniticationService;
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;

        public AuthenticationController(IMapper mapper, UserManager<User> userManager)
        {
            autheniticationService = new AuthenticationService(mapper, userManager);
            this.mapper = mapper;
            this.userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(EmployeeForCreationDto employeeForCreationDto)
        {

            var result = await autheniticationService.RegisterUserAsync(employeeForCreationDto, "Admin");
            return BadRequest(result.Errors);

        }
    }
}
