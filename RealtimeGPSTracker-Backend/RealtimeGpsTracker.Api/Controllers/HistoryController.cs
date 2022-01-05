using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RealtimeGpsTracker.Api.Controllers;
using RealtimeGpsTracker.Core.Commands.TripCommands;
using RealtimeGpsTracker.Core.Dtos.Responses.DeviceResponses;
using RealtimeGpsTracker.Core.Dtos.Responses.Pagination;
using RealtimeGpsTracker.Core.Dtos.Responses.TripResponses;
using RealtimeGpsTracker.Core.Entities;
using RealtimeGpsTracker.Core.Interfaces.Repositories;
using RealtimeGpsTracker.Core.Queries.TripQueries;

namespace RealtimeGpsTracker.ApiControllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("dashboard/history")]
    public class HistoryController : ControllerExtended
    {
        private readonly ILogger<HistoryController> _logger;
        private readonly IMediator _mediator;

        public HistoryController(
            ILogger<HistoryController> logger,
            IMediator mediator
        )
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("kickSignalR/{userId}")]
        public async Task<IActionResult> KickSignalR([FromRoute] string userId) {
            string responseMessage = string.Empty;

            try
            {
                //await _tripHubContext.Clients.Group(userId).UpdateTripList(
                //    new TripHubMessage(
                //        "UPDATE_TRIP_LIST: " + DateTime.UtcNow.ToLongTimeString() + " " + DateTime.UtcNow.ToLongDateString()
                //    )
                //);

                responseMessage = "Success";
            }
            catch (Exception e)
            {

                responseMessage = e.ToString();
            }

            return Ok(responseMessage);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetTripDeviceListForUserAsync([FromQuery] TripPaginationParameters paginationParameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new TripsResponse { Success = false, Errors = GetErrors(ModelState)});
            }

            // Attempt to get paginated list of devices based on pagination paramaters.
            var tripsResponse = await _mediator.Send(new TripsQuery { OwnerId = GetOwnerId(), PaginationParameters = paginationParameters });
            Response.Headers.Add("X-Pagination", tripsResponse.PaginationHeader.GenerateJson());

            // Log result of requesting list of trips
            _logger.LogInformation("");

            return (tripsResponse.Success) ? (IActionResult)Ok(tripsResponse) : BadRequest(tripsResponse);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteMultipleTrips([FromBody] DeleteMultipleTripsCommand deleteMultipleTripsCommand)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new DeleteMultipleDevicesResponse { Success = false, Errors = GetErrors(ModelState)});
            }

            deleteMultipleTripsCommand.OwnerId = GetOwnerId();

            // Attempt to delete multiple trips owned by user and collected by owner's device
            var deleteMultipleTripsResponse = await _mediator.Send(deleteMultipleTripsCommand);

            // Log result of the trips deletion request
            _logger.LogInformation("");

            return (deleteMultipleTripsResponse.Success) ? (IActionResult)Ok(deleteMultipleTripsResponse) : BadRequest(deleteMultipleTripsResponse);
        }
    }
}