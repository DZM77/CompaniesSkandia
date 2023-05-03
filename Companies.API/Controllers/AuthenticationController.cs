﻿using AutoMapper;
using Companies.API.DataTransferObjects;
using Companies.API.Services;
using Companies.Core.Entities;
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
        public async Task<IActionResult> RegisterUser(EmployeeForCreationDto employeeForCreationDto)
        {

            var result = await autheniticationService.RegisterUserAsync(employeeForCreationDto, "Admin");
            return result.Succeeded ? StatusCode(StatusCodes.Status201Created) : BadRequest(result.Errors);

        } 
        
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate(UserForAuthenticationDto userDto)
        {
            if (!await autheniticationService.ValidateUserAsync(userDto))
                return Unauthorized();

            return Ok(new
            {
                Token = await autheniticationService.CreateTokenAsync()
            });
        }


    }
}
