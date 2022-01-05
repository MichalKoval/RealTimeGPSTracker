using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RealtimeGpsTracker.Core.Commands.CoordinateCommands;
using RealtimeGpsTracker.Core.Dtos.Responses.CoordinateResponses;
using RealtimeGpsTracker.Core.Dtos.Responses.Pagination;
using RealtimeGpsTracker.Core.Entities;
using RealtimeGpsTracker.Core.Interfaces.Repositories;
using RealtimeGpsTracker.Core.Queries.CoordinateQueries;

namespace RealtimeGpsTracker.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("coordinate")]
    public class CoordinateController : ControllerExtended
    {
        private readonly ILogger<CoordinateController> _logger;
        private readonly IMediator _mediator;

        public CoordinateController(
            ILogger<CoordinateController> logger,
            IMediator mediator
            )
        {
            _logger = logger;
            _mediator = mediator;
        }
        
        [HttpGet("Dashboard/Coordinate/KickSignalR/{userId}")]
        public async Task<IActionResult> KickSignalR([FromRoute] string userId) {
            string responseMessage;
            try
            {
                //await _deviceHubContext.Clients.Group(userId).UpdateDeviceList(
                //    new DeviceHubMessage(
                //        "UPDATE_COORDINATE_LIST: " + DateTime.UtcNow.ToLongTimeString() + " " + DateTime.UtcNow.ToLongDateString()
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

        [AllowAnonymous]
        [HttpPost("post")]
        public async Task<IActionResult> ReceiveGpsDeviceData([FromBody] InsertCoordinatesCommand insertCoordinatesCommand)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new InsertCoordinatesResponse { Success = false, Errors = GetErrors(ModelState) });
            }

            // Attempt to insert coordinates to a certain device and trip
            var insertCoordinatesResponse = await _mediator.Send(insertCoordinatesCommand);

            return (insertCoordinatesResponse.Success) ? (IActionResult)Ok(insertCoordinatesResponse) : BadRequest(insertCoordinatesResponse);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetGpsCoordnitesForUserDeviceAndTripAsync([FromQuery] CoordinatesQuery coordinatesQuery)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new CoordinatesResponse { Success = false, Errors = GetErrors(ModelState) });
            }

            coordinatesQuery.OwnerId = GetOwnerId();

            // Attempt to get coordinates for a certain trip
            var coordinatesResponse = await _mediator.Send(coordinatesQuery);

            return (coordinatesResponse.Success) ? (IActionResult)Ok(coordinatesResponse) : BadRequest(coordinatesResponse);
        }
    }
}