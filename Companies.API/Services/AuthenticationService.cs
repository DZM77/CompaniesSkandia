using AutoMapper;
using Companies.API.DataTransferObjects;
using Companies.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Companies.API.Services
{
    public class AuthenticationService
    {
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;

        public AuthenticationService(IMapper mapper, UserManager<User> userManager)
        {
            this.mapper = mapper;
            this.userManager = userManager;
        }

        public async Task<IdentityResult> RegisterUserAsync(EmployeeForCreationDto creationDto, string role)
        {
            if (creationDto is null)
            {
                throw new ArgumentNullException(nameof(creationDto));
            }

            if (string.IsNullOrEmpty(role))
            {
                throw new ArgumentException($"'{nameof(role)}' cannot be null or empty.", nameof(role));
            }

            var user = mapper.Map<User>(creationDto);

            var result = await userManager.CreateAsync(user, creationDto.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Admin");

            }

            return result;

        }
    }
}
