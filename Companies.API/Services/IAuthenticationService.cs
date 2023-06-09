using Companies.API.DataTransferObjects;
using Microsoft.AspNetCore.Identity;

namespace Companies.API.Services
{
    public interface IAuthenticationService
    {
        Task<TokenDto> CreateTokenAsync(bool expTime);
        Task<TokenDto> RefreshTokenAsync(TokenDto token);
        Task<IdentityResult> RegisterUserAsync(EmployeeForCreationDto creationDto, string role, Guid id = default);
        Task<bool> ValidateUserAsync(UserForAuthenticationDto userDto);
    }
}