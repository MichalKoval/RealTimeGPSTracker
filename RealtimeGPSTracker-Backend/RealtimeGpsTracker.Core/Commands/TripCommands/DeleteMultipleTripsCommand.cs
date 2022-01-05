using Newtonsoft.Json;
using RealtimeGpsTracker.Core.Dtos.Responses.DeviceResponses;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RealtimeGpsTracker.Core.Commands.TripCommands
{
    public class DeleteMultipleTripsCommand : AuthorizedCommand<DeleteMultipleTripsResponse>
    {
        [Required]
        public string[] DeviceId { get; set; } = null;

        [Required]
        public string[] TripIds { get; set; } = null;
    }
}