using AutoMapper;
using Companies.API.Configuration;
using Companies.API.DataTransferObjects;
using Companies.API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
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
        private readonly JwtConfigurations jwtConfigurations;
        private User? user;

        public AuthenticationService(IMapper mapper, UserManager<User> userManager, IConfiguration configuration, IOptions<JwtConfigurations> options)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this.configuration = configuration;
            this.jwtConfigurations = options.Value;
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
            //var jwtSettings = configuration.GetSection("JwtSettings");

            var tokenOptions = new JwtSecurityToken(
                                        issuer: jwtConfigurations.ValidIssuer,
                                        audience: jwtConfigurations.ValidAudience,
                                        claims: claims,
                                        expires: DateTime.Now.AddDays(5),//DateTime.Now.AddMinutes(Convert.ToDouble(jwtConfigurations.Expires)),
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

        public async Task<TokenDto> RefreshTokenAsync(TokenDto token)
        {
            var principal = GetPrincipalFromExpiredToken(token.AccessToken);

            User? user = await userManager.FindByNameAsync(principal.Identity?.Name);

            if (user == null || user.RefreshToken != token.RefreshToken || user.RefreshTokenExpireTime <= DateTime.Now)
                //ToDo: Handle with middleware
                throw new BadHttpRequestException("The TokenDto has som invalid values");

            this.user = user;

            return await CreateTokenAsync(expTime: false);

        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");

            var secretKey = Environment.GetEnvironmentVariable("SECRET");
            ArgumentNullException.ThrowIfNull(nameof(secretKey));

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["validIssuer"],
                ValidAudience = jwtSettings["validAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
    }
}
