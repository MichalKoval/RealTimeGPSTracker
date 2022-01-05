using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MediatR;
using RealtimeGpsTracker.Core.Commands.UserCommands;
using RealtimeGpsTracker.Core.Queries.UserQueries;
using Microsoft.Extensions.Logging;
using RealtimeGpsTracker.Core.Dtos.Responses.UserResponses;
using System.Collections.Generic;

namespace RealtimeGpsTracker.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("dashboard/user")]
    public class UserController : ControllerExtended
    {
        private readonly ILogger<UserController> _logger;
        private readonly IMediator _mediator;

        public UserController(
            ILogger<UserController> logger,
            IMediator mediator
            )
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetById()
        {
            // Attempt to get user detail
            var userDetailResponse = await _mediator.Send(new DetailUserQuery { OwnerId = GetOwnerId() });

            // Log result of the user detail request
            _logger.LogInformation("");

            return (userDetailResponse.Success) ? (IActionResult)Ok(userDetailResponse) : BadRequest(userDetailResponse);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody]UpdateUserCommand userUpdateCommand)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new UpdateUserResponse { Success = false, Errors = GetErrors(ModelState)});
            }

            userUpdateCommand.OwnerId = GetOwnerId();

            // Attempt to update user data
            var userUpdateResponse = await _mediator.Send(userUpdateCommand);

            // Log result of the user update request
            _logger.LogInformation("");

            return (userUpdateResponse.Success) ? (IActionResult)Ok(userUpdateResponse) : BadRequest(userUpdateResponse);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete()
        {
            // Attempt to delete user
            var userDeleteResponse = await _mediator.Send(new DeleteUserCommand { OwnerId = GetOwnerId() });

            // Log result of the user deletion request
            _logger.LogInformation("");

            return (userDeleteResponse.Success) ? (IActionResult)Ok(userDeleteResponse) : BadRequest(userDeleteResponse);
        }
    }
}
