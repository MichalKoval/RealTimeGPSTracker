using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using RealtimeGpsTracker.Core.Dtos.Requests;
using RealtimeGpsTracker.Core.Interfaces.Services;
using MediatR;
using RealtimeGpsTracker.Core.Commands.AuthCommands;

namespace RealtimeGpsTracker.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IMediator _mediator;

        public AuthController(
            ILogger<AuthController> logger,
            IMediator mediator
            )
        {
            _logger = logger;
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginUserCommand loginUserCommand)
        {
            // Attempt to login user
            var loginUserResponse = await _mediator.Send(loginUserCommand);

            // Log result of the login
            _logger.LogInformation("");

            return (loginUserResponse.Success) ? (IActionResult)Ok(loginUserResponse) : Unauthorized(loginUserResponse);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterUserCommand registerUserCommand)
        {
            // Attempt to register new user
            var registerUserResponse = await _mediator.Send(registerUserCommand);

            // Log result of the registration
            _logger.LogInformation("");

            return (registerUserResponse.Success) ? (IActionResult)Ok(registerUserResponse) : BadRequest(registerUserResponse);
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody]RefreshTokenCommand refreshTokenCommand)
        {
            // Attempt to register new user
            var refreshTokenResponse = await _mediator.Send(refreshTokenCommand);

            // Log result of the registration
            _logger.LogInformation("");

            return (refreshTokenResponse.Success) ? (IActionResult)Ok(refreshTokenResponse) : BadRequest(refreshTokenResponse);
        }
    }
}