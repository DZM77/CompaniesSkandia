using AutoMapper;
using Companies.API.DataTransferObjects;
using Companies.API.Entities;
using Companies.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Companies.API.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService autheniticationService;
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;

        public AuthenticationController(IMapper mapper, UserManager<User> userManager, IAuthenticationService authenticationService)
        {
            autheniticationService = authenticationService;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser(EmployeeForCreationDto employeeForCreationDto)
        {

            var result = await autheniticationService.RegisterUserAsync(employeeForCreationDto, "Admin");
            return result.Succeeded ? StatusCode(StatusCodes.Status201Created) : BadRequest(result.Errors);

        } 
        
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<TokenDto>> Authenticate(UserForAuthenticationDto userDto)
        {
            if (!await autheniticationService.ValidateUserAsync(userDto))
                return Unauthorized();


            var tokenDto = await autheniticationService.CreateTokenAsync(expTime: true);
            
            return Ok(tokenDto);
         
        }


    }
}
