using JoggingTimeTracker.API.DTOs.Auth;
using JoggingTimeTracker.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoggingTimeTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {

        private readonly IAuthService _authService;

        public AuthenticationController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token.
        /// </summary>
        /// <param name="model">The login request details.</param>
        /// <returns>The JWT token.</returns>
        /// <response code="200">Returns the JWT token.</response>
        /// <response code="401">Invalid credentials.</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequestDto model)
        {
            var token = await _authService.Login(model.Username, model.Password);

            if (token == null)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            return Ok(new { token });
        }

        /// <summary>
        /// Logs out the current user.
        /// </summary>
        /// <returns>A message indicating that the user has been logged out.</returns>
        /// <response code="200">Returns a message indicating that the user has been logged out.</response>
        /// <response code="401">Unauthorized request.</response>
        [HttpPost("logout")]
        [Authorize]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Logout()
        {
            await _authService.Logout();

            return Ok(new { message = "Logged out successfully" });
        }
    }
}
