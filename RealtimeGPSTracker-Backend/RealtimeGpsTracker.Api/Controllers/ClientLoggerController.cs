using System;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RealtimeGpsTracker.Core.Dtos.Requests;
using RealtimeGpsTracker.Core.Dtos.Responses.UserResponses;

namespace RealtimeGpsTracker.Api.Controllers
{
    [Route("logs")]
    public class ClientLoggerController : ControllerExtended
    {
        private readonly ILogger<ClientLoggerController> _logger;
        private readonly IMediator _mediator;

        public ClientLoggerController(
            ILogger<ClientLoggerController> logger,
            IMediator mediator
            )
        {
            _logger = logger;
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> PostLogs([FromBody] LoggerDto loggerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ClientLoggerResponse { Success = false, Errors = GetErrors(ModelState) });
            }

            _logger.LogInformation(
                $"\nMessage: {loggerDto.Message}\n" +
                $"File: {loggerDto.FileName}\n" +
                $"LineNumber: {loggerDto.LineNumber}\n" +
                $"Timestamp: {loggerDto.Timestamp:F}\n" +
                $"User: {User.Identity.Name}\n"
            );

            return Ok();
        }
    }
}