using System;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RealtimeGpsTracker.Core.Dtos.Requests;
using RealtimeGpsTracker.Core.Interfaces.Services;
using RealtimeGpsTracker.Core.Queries.SslQueries;

namespace RealtimeGpsTracker.Api.Controllers
{
    [Route(".well-known/acme-challenge")]
    public class SslController : ControllerBase
    {
        private readonly ILogger<SslController> _logger;
        private readonly IMediator _mediator;

        public SslController(
            ILogger<SslController> logger,
            IMediator mediator
            )
        {
            _logger = logger;
            _mediator = mediator;
        }


        // GET: SSLFree
        [AllowAnonymous]
        [HttpGet]
        [Route("{fileName}")]
        public async Task<IActionResult> GetSsl(String fileName)
        {
            // Attempt to get ssl public keys
            var detailSslResponse = await _mediator.Send(new DetailSslQuery { FileName = fileName });

            // Log result of requesting ssl public key file
            _logger.LogInformation("");

            return (detailSslResponse.Success) ? 
                (IActionResult)File(
                    Encoding.UTF8.GetBytes(detailSslResponse.FileContent),
                    "text/plain",
                    detailSslResponse.FileDownloadName
                ) : 
                BadRequest(detailSslResponse);
        }
    }
}