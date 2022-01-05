using Newtonsoft.Json;
using RealtimeGpsTracker.Core.Dtos.Responses.DeviceResponses;
using System.ComponentModel.DataAnnotations;

namespace RealtimeGpsTracker.Core.Commands.DeviceCommands
{
    public class DeleteMultipleDevicesCommand : AuthorizedCommand<DeleteMultipleDevicesResponse>
    {
        [Required]
        public string[] DeviceIds { get; set; } = null;
    }
}