using AutoMapper;
using Companies.API.DataTransferObjects;
using Companies.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Companies.API.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;
        private readonly IConfiguration configuration;
        private User? user;

        public AuthenticationService(IMapper mapper, UserManager<User> userManager, IConfiguration configuration)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this.configuration = configuration;
        }

        public async Task<TokenDto> CreateTokenAsync(bool expTime)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaimsAsync();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            user.RefreshToken = GenerateRefreshToken();

            if(expTime)
                user.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(1);

            //ToDo: Validate res!
            await userManager.UpdateAsync(user);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return new TokenDto(accessToken, user.RefreshToken);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");

            var tokenOptions = new JwtSecurityToken(
                                        issuer: jwtSettings["validIssuer"],
                                        audience: jwtSettings["validAudience"],
                                        claims: claims,
                                        expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expires"])),
                                        signingCredentials: signingCredentials);

            return tokenOptions;
        }

        private async Task<List<Claim>> GetClaimsAsync()
        {
            ArgumentNullException.ThrowIfNull(nameof(user));

            var claims = new List<Claim>
                             {
                                  new Claim(ClaimTypes.Name, user?.UserName!),
                                  new Claim("Age", user!.Age.ToString()!),
                                  new Claim(ClaimTypes.NameIdentifier, user.Id)
                             };

            var roles = await userManager.GetRolesAsync(user!);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var keyFromEnviroment = Environment.GetEnvironmentVariable("SECRET");
            ArgumentNullException.ThrowIfNull(nameof(keyFromEnviroment));

            var key = Encoding.UTF8.GetBytes(keyFromEnviroment!);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
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

        public async Task<bool> ValidateUserAsync(UserForAuthenticationDto userDto)
        {
            if (userDto is null)
            {
                throw new ArgumentNullException(nameof(userDto));
            }

            user = await userManager.FindByNameAsync(userDto.UserName!);

            var result = (user != null && await userManager.CheckPasswordAsync(user, userDto.Password!));

            return result;
        }
    }
}
