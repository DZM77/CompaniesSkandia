using Companies.API.DataTransferObjects;
using Microsoft.AspNetCore.Identity;

namespace Companies.API.Services
{
    public interface IAuthenticationService
    {
        Task<TokenDto> CreateTokenAsync(bool expTime);
        Task<IdentityResult> RegisterUserAsync(EmployeeForCreationDto creationDto, string role);
        Task<bool> ValidateUserAsync(UserForAuthenticationDto userDto);
    }
}