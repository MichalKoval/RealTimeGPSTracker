using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealtimeGpsTracker.Core.Entities;
using RealtimeGpsTracker.Core.Dtos.Responses.Pagination;
using RealtimeGpsTracker.Core.Interfaces.Hubs;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using RealtimeGpsTracker.Core.Commands.DeviceCommands;
using RealtimeGpsTracker.Core.Queries.DeviceQueries;
using Microsoft.Extensions.Logging;
using RealtimeGpsTracker.Core.Dtos.Responses.DeviceResponses;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace RealtimeGpsTracker.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("dashboard/device")]
    public class DeviceController : ControllerExtended
    {
        private readonly ILogger<DeviceController> _logger;
        private readonly IDeviceHubService _deviceHubService;
        private readonly IMediator _mediator;

        public DeviceController(
            ILogger<DeviceController> logger,
            IDeviceHubService deviceHubService,
            IMediator mediator
        )
        {
            _logger = logger;
            _deviceHubService = deviceHubService;
            _mediator = mediator;
        }

        [HttpGet("kickSignalR/{userId}")]
        public async Task<IActionResult> KickSignalR([FromRoute] string userId) {
            string responseMessage;

            try
            {
                await _deviceHubService.SendUpdateMessageToUserGroup(userId);

                responseMessage = "Success";
            }
            catch (Exception e)
            {

                responseMessage = e.ToString();
            }

            return Ok(responseMessage);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetGpsDevicesForUserAsync([FromQuery] DevicePaginationParameters paginationParameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new DevicesResponse { Success = false, Errors = GetErrors(ModelState)});
            }

            // Attempt to get paginated list of devices based on pagination paramaters for a user that own's those devices
            var devicesResponse = await _mediator.Send(new DevicesQuery { OwnerId = GetOwnerId(), PaginationParameters = paginationParameters});
            Response.Headers.Add("X-Pagination", devicesResponse.PaginationHeader.GenerateJson());

            return (devicesResponse.Success) ? (IActionResult)Ok(devicesResponse) : BadRequest(devicesResponse);
        }
        
        [HttpPost("update")]
        public async Task<IActionResult> UpdateGpsDevice([FromBody] UpdateDeviceCommand updateDeviceCommand)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new UpdateDeviceResponse { Success = false, Errors = GetErrors(ModelState)});
            }

            updateDeviceCommand.OwnerId = GetOwnerId();
            
            // Attempt to update device owned by user
            var updateDeviceResponse = await _mediator.Send(updateDeviceCommand);

            // Log result of the device update request
            _logger.LogInformation("");

            return (updateDeviceResponse.Success) ? (IActionResult)Ok(updateDeviceResponse) : BadRequest(updateDeviceResponse);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateGpsDevice([FromBody] CreateDeviceCommand createDeviceCommand)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new CreateDeviceResponse { Success = false, Errors = GetErrors(ModelState)});
            }

            createDeviceCommand.OwnerId = GetOwnerId();

            // Attempt to create device owned by user
            var createDeviceResponse = await _mediator.Send(createDeviceCommand);

            // Log result of the device creation request
            _logger.LogInformation("");

            return (createDeviceResponse.Success) ? (IActionResult)Ok(createDeviceResponse) : BadRequest(createDeviceResponse);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteMultipleGpsDevices([FromBody] DeleteMultipleDevicesCommand deleteMultipleDevicesCommand)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new DeleteMultipleDevicesResponse { Success = false, Errors = GetErrors(ModelState) });
            }

            deleteMultipleDevicesCommand.OwnerId = GetOwnerId();

            // Attempt to delete multiple devices owned by user
            var deleteMultipleDevicesResponse = await _mediator.Send(deleteMultipleDevicesCommand);

            // Log result of the device deletion request
            _logger.LogInformation("");

            return (deleteMultipleDevicesResponse.Success) ? (IActionResult)Ok(deleteMultipleDevicesResponse) : BadRequest(deleteMultipleDevicesResponse);

        }
    }
}