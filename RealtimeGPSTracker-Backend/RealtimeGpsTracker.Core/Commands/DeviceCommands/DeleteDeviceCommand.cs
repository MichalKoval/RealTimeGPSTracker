using Newtonsoft.Json;
using RealtimeGpsTracker.Core.Dtos.Responses.DeviceResponses;
using System.ComponentModel.DataAnnotations;

namespace RealtimeGpsTracker.Core.Commands.DeviceCommands
{
    public class DeleteDeviceCommand : AuthorizedCommand<DeleteDeviceResponse>
    {
        [Required]
        public string DeviceId { get; set; } = null;
    }
}