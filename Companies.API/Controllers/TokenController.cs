using Companies.API.DataTransferObjects;
using Companies.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Companies.API.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IAuthenticationService authenticationService;

        public TokenController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<TokenDto>> RefreshToken(TokenDto token)
        {
            var tokenDto = await authenticationService.RefreshTokenAsync(token);

            return Ok(tokenDto);
        }
    }
}
